using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NuGet.Common;
using SmartBite.BAL.Authantication;
using SmartBite.BAL.DTOS;
using static System.Net.WebRequestMethods;

namespace SmartBite.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthanticationController : ControllerBase
    {
        private readonly IAuthanticationManager _manager;

        public AuthanticationController(IAuthanticationManager manager)
        {
            _manager = manager;
        }


        #region SignUp EndPoint
        [HttpPost("SignUp",Name ="SignUp")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public ActionResult SignUp(UserDTO person)
        {
            if(string.IsNullOrEmpty(person.Name) || string.IsNullOrEmpty(person.Email) || string.IsNullOrEmpty(person.Password))
            {
                return BadRequest(new { message = "Invalid data entered!" });
            }

            var Token = _manager.SignUP(person);

            if (Token != null)
            {
                return Ok(new
                {
                    message = "Registration successful!",
                    token = Token
                });
            }
            return Unauthorized(new {message="Failed To Register ! "});
        }
        #endregion


        
        #region Login EndPoint
        [HttpPost("Login",Name ="Login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public ActionResult Login(LoginDTO login)
        {
            if (string.IsNullOrEmpty(login.Email) || string.IsNullOrEmpty(login.Password))
            {
                return BadRequest(new { message = "Invalid data entered!" });
            }

            string Token=_manager.Login(login);          
            if (Token!=null)
            {
                return Ok(new
                {
                    token = Token
                });
            
            }
          
            
            return Unauthorized(new { message = "Invalid credentials or user not found!" });

        }
        #endregion


        #region UpdatePassword Endpoint
        [HttpPut("UpdatePassword",Name ="UpdatePassword")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public ActionResult UpdatePassword(UpdatePasswordDTO updatePassword)
        {
            if(string.IsNullOrEmpty(updatePassword.Email) || string.IsNullOrEmpty(updatePassword.Password))
            {
                return BadRequest("Invalid Data Entered !");
            }

            var PasswordUpdated=_manager.UpdatePassword(updatePassword.Email,updatePassword.Password);
            if (PasswordUpdated)
            {
                return Ok("Updated Successfully !");
            }
            return NotFound("Failed To Update !");
        }
        #endregion



        #region ForgetPassword EndPoint
        [HttpPost("ForgetPassword",Name ="ForgetPassword")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task <ActionResult> ForgetPassword(ForgetPasswordDTO forgetPassword)
        {
            if (string.IsNullOrEmpty(forgetPassword.Email))
            {
                return BadRequest("Invalid Email Entered ! ");
            }

            bool FindUser=_manager.FindUser(forgetPassword.Email);
            if(!FindUser)
            {
                return NotFound("User Not Found !");
            }

            var OTP = new Random().Next(100000, 999999).ToString();
            _manager.StoreOTP(forgetPassword.Email, OTP);

            await _manager.SendOtpEmail(forgetPassword.Email, OTP);
            
            return Ok("The OTP Sended To Email !");

        }

        #endregion


        #region VerifyOTP EndPoint
        [HttpPost("VerifyPassword",Name ="VerifyPassword")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public ActionResult VerifyOTP(VerifyOTPDTO verify)
        {
            if (string.IsNullOrEmpty(verify.Email) || string.IsNullOrEmpty(verify.OTP))
            {
                return BadRequest("Email and OTP are required.");
            }

            var StoredOTP=_manager.GetStoredOtp(verify.Email);

            if (StoredOTP == null || StoredOTP != verify.OTP)
            {
                return NotFound("Invalid OTP.");
            
            }

            return Ok("OTP Verified Successfully !");
        }

        #endregion



    }
}
