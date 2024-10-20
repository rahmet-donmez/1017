using AccountManagment.Core.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountManagment.Service.Mapping
{
    public class Mapper:Profile
    {

        public Mapper(string profileName) : base(profileName)
        {

        }
    }
}
