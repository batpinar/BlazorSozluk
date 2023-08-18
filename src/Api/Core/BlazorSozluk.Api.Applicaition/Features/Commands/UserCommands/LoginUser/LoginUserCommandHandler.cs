using AutoMapper;
using BlazorSozluk.Api.Applicaition.Interfaces.Repositories;
using BlazorSozluk.Common.Infrastructure;
using BlazorSozluk.Common.Infrastructure.Exceptions;
using BlazorSozluk.Common.Models.Queries;
using BlazorSozluk.Common.Models.RequestModels;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BlazorSozluk.Api.Applicaition.Features.Commands.UserCommands.LoginUser;

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, LoginUserViewModel>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;
    //JWT Token generate ederken configuration içerisinde key'e ihtiyacımız var
    //When we generate JWT Token we need a key inside the configuration

    public LoginUserCommandHandler(IUserRepository userRepository, IMapper mapper, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _configuration = configuration;
    }

    public async Task<LoginUserViewModel> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var dbUser = await _userRepository.GetSingleAsync(i => i.EmailAddress == request.EmailAddress);

        if (dbUser == null)
            throw new DatabaseValidationException("User not found!");

        var pass = PasswordEncryptor.Encrpt(request.Password);
        if (dbUser.Password != pass)
            throw new DatabaseValidationException("Entered password is wrong!");

        if (!dbUser.EmailConfirmed)
            throw new DatabaseValidationException("Email address is not confirm yet!");

        var result = _mapper.Map<LoginUserViewModel>(dbUser);

        var claims = new Claim[]
        {
            new Claim(ClaimTypes.NameIdentifier, dbUser.Id.ToString()),
            new Claim(ClaimTypes.Email, dbUser.EmailAddress),
            new Claim(ClaimTypes.Name, dbUser.UserName),
            new Claim(ClaimTypes.GivenName, dbUser.FirstName),
            new Claim(ClaimTypes.Surname, dbUser.LastName)
        };

        result.Token = GenerateToken(claims);
        return result;
    }
    
    private string GenerateToken(Claim[] claims)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["AuthConfig:Secret"]));
        var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expiry = DateTime.Now.AddDays(1);

        var token = new JwtSecurityToken(claims: claims,
                                         expires: expiry,
                                         signingCredentials: cred,
                                         notBefore: DateTime.Now);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

}
