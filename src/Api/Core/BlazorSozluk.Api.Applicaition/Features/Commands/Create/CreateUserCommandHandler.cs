using AutoMapper;
using BlazorSozluk.Api.Applicaition.Interfaces.Repositories;
using BlazorSozluk.Api.Domain.Models;
using BlazorSozluk.Common;
using BlazorSozluk.Common.Events.User;
using BlazorSozluk.Common.Infrastructure;
using BlazorSozluk.Common.Infrastructure.Exceptions;
using BlazorSozluk.Common.Models.RequestModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorSozluk.Api.Applicaition.Features.Commands.Create;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Guid>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public CreateUserCommandHandler(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var existUser = await _userRepository.GetSingleAsync(x => x.EmailAddress == request.EmailAddress);


        if (existUser != null)
            throw new DatabaseValidationException("User already exists");

        var dbUser = _mapper.Map<Domain.Models.User>(request);
        var rows = await _userRepository.AddAsync(dbUser);

        if (rows > 0)
        {
            var @event = new UserEmailChangedEvent()
            {
                OldEmailAddress = null,
                NewEmailAddress = dbUser.EmailAddress
            };
            QueueFactory.SendMessageToExchange(exchangeName :  SozlukConstants.UserExchangeName,
                                                exchangeType: SozlukConstants.DefaultExchangeType,
                                                queueName: SozlukConstants.UserEmailChangedQueueName,
                                                obj: @event);

        }
        return dbUser.Id;
    }
}
