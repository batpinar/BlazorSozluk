using AutoMapper;
using BlazorSozluk.Api.Domain.Models;
using BlazorSozluk.Common.Models.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorSozluk.Api.Applicaition.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            #region User Mapping
            CreateMap<User, LoginUserViewModel>()
                .ReverseMap();
            #endregion
        }
    }
}
