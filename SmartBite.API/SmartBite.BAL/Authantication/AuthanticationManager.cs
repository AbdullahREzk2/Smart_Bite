using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Data.SqlClient;
using SmartBite.BAL.CalorieCalculator;
using SmartBite.BAL.DTOS;
using SmartBite.BAL.JWTHelper;
using SmartBite.DAL;
using SmartBite.DAL.Authantication;
using SmartBite.DAL.Calories;
using SmartBite.DAL.Models;

namespace SmartBite.BAL.Authantication
{
    public class AuthanticationManager : IAuthanticationManager
    {
        private readonly IAuthanticationService _service;
        private readonly IMapper _mapper;
        private readonly IJWTService _jwtservice;
        private readonly ICalorieCalculatorManager _calorieCalculator;
        private readonly ICaloriesService _caloriesService;

        public AuthanticationManager(IAuthanticationService service,IMapper mapper,IJWTService jwtservice ,ICalorieCalculatorManager calorieCalculator,ICaloriesService caloriesService)
        {
            _service = service;
            _mapper = mapper;
            _jwtservice = jwtservice;
            _calorieCalculator = calorieCalculator;
            _caloriesService = caloriesService;
        }


        public bool DeleteProfile(int ID)
        {
            return _service.DeleteProfile(ID);
        }

        public bool FindUser(string Email)
        {
            return _service.FindUser(Email);
        }
        


        public string Login(LoginDTO login)
        {
            bool founded = FindUser(login.Email!);
            if(!founded)
            {
                return "User Not Found !";
            }
            var Data=_service.Login(login.Email!, login.Password!);

            if (Data == null)
            {
                return "Invalid email or password!";
            }

            var Claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier,Data.UserID.ToString()),
                new Claim(ClaimTypes.Name,Data.Name!),
                new Claim(ClaimTypes.Email,Data.Email!)
            };
            var Token = _jwtservice.GenerateToken(Claims);
            return Token;

        }


        public string SignUP(UserDTO person)
        {
            bool Founded = FindUser(person.Email!);
            if (Founded)
            {
                return "User Already Exist !! ";
            }

            var Data = _service.SignUP(person.Name!, person.Email!, person.Password!);
            var claims = new List<Claim>

            {
              new Claim(ClaimTypes.NameIdentifier, Data.UserID.ToString()),
              new Claim(ClaimTypes.Name, Data.Name!),
              new Claim(ClaimTypes.Email, Data.Email!)
             };


            var token = _jwtservice.GenerateToken(claims);

            return token;

        }


        public bool UpdatePassword(string Email, string NewPassword)
        {
            return _service.UpdatePassword(Email, NewPassword);
        }

       
        private static Dictionary<string, string> OTPStorage = new Dictionary<string, string>();
        public void StoreOTP(string Email, string OTP)
        {
            OTPStorage[Email] = OTP;
        }


        public async Task SendOtpEmail(string email, string otp)
        {
            var emailService = new EmailService("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");

            var subject = "Password Reset OTP";
            var body = $"Your OTP for password reset is: {otp}";

            await emailService.SendEmail(email, subject, body);
        }



        public string GetStoredOtp(string email)
        {
            return OTPStorage.ContainsKey(email) ? OTPStorage[email] : null!;
        }



    }
}
