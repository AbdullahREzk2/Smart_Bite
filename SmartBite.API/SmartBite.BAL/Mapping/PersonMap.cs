using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using SmartBite.BAL.DTOS;
using SmartBite.DAL.Models;

namespace SmartBite.BAL.Mapping
{
    public class PersonMap:Profile
    {
        public PersonMap()
        {
            CreateMap<PersonDTO, PersonModel>().ReverseMap();
        }

    }
}
