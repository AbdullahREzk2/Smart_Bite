using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartBite.BAL.DTOS;
using SmartBite.DAL.Models;

namespace SmartBite.BAL.Authantication
{
    public interface IAuthanticationManager
    {
        public string Login(LoginDTO login);

        public string SignUP(UserDTO person);

        public bool UpdatePassword(string Email, string NewPassword);

        public bool DeleteProfile(int ID);

        public bool FindUser(string Email);

        public void StoreOTP(string Email, string OTP);

        public Task SendOtpEmail(string email, string otp);

        public string GetStoredOtp(string email);

    }
}
