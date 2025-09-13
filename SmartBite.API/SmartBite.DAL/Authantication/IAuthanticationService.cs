using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartBite.DAL.Models;

namespace SmartBite.DAL.Authantication
{
    public interface IAuthanticationService
    {
        public TinyPersonModel Login(string Email,string Password);

        public TinyPersonModel SignUP(string name, string email, string password);

        public bool UpdatePassword(string Email, string NewPassword);

        public bool FindUser(string Email);

        public bool DeleteProfile(int ID);

        public string GetName(int UserID);

    }
}
