using System.ComponentModel.DataAnnotations;

namespace Leave_Application_System.Model.Response
{
    public class LoginResponse
    {
        public string message { get; set; }
        public string token { get; set; }
    }
}
