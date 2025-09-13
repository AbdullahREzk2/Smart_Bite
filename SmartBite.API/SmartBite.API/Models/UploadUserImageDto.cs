using Microsoft.AspNetCore.Http;

namespace SmartBite.API.Models
{
    public class UploadUserImageDto
    {
        public int UserId { get; set; }
        public IFormFile? File { get; set; }
    }
}
