using Leave_Application_System.Database;
using Leave_Application_System.Model.Request;
using Leave_Application_System.Model.Response;
using Leave_Application_System.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace Leave_Application_System.Controllers
{

    public class LeaveApplicationController : ControllerBase
    {
        private readonly ILogger<LeaveApplicationController> _logger;
        private readonly ILeave _leave;
        private readonly IDataAccess _dataAccess;
        private IConfiguration _config;
        public LeaveApplicationController(ILogger<LeaveApplicationController> logger, ILeave leave, IDataAccess dataAccess)
        {
            _logger = logger;
            _leave = leave;
            _dataAccess = dataAccess;

        }

        private string GenerateToken()
        {

           string secretKey = _config?["JWT:SecretKey"]; 

            if (secretKey != null)
            {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var jwtToken = new JwtSecurityToken(_config["JWT:ValidateIssuer"], _config["JWT:ValidateAudience"], null,
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: credentials
                );

                return new JwtSecurityTokenHandler().WriteToken(jwtToken);
            }
            else
            {
                // Handle the case where the secret key is not available
                // For example, provide a default key, log an error, or throw an exception
                const string defaultSecretKey = "LongerThan-16-Char-SecretKey-THIS IS USED TO SIGN AND VERIFY JWT TOKENrqwertydwerty"; // Provide a default secret key
                const string ValidateIssuer = "http://localhost:5107"; // Provide a default secret key
                const string ValidateAudience = "https://localhost:7197"; // Provide a default secret key
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(defaultSecretKey));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var jwtToken = new JwtSecurityToken(ValidateIssuer, ValidateAudience, null,
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: credentials
                );

              
                return new JwtSecurityTokenHandler().WriteToken(jwtToken);
            }

        }

        [HttpPost]
        [Route("Login")]
        public LoginResponse UserLogin(LoginRequest request)
        {
            try
            {

                var response = _leave.UserLogin(request);
                var mssg = response.message;
                if (mssg == "Login Suceessfull..")
                {
                    var token = GenerateToken();
                    var value = token.ToString();
                    response.message = mssg.ToString();
                    response.token = value;
                }
                else
                {
                    response.message = mssg.ToString();
                }

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Authorize]
        [HttpPost]
        [Route("leavelist")]
        public List<LeaveListResponse> Leave_list()
        {
            try
            {
                var response = _leave.Leave_list();
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Authorize]
        [HttpPost]
        [Route("EmployeesLeaveDetails")]
        public List<EmployeesLeaveDetailsResponse> EmployeesLeaveDetails(LeaveRequestingDetailsRequest request)
        {
            try
            {
                var response = _leave.EmployeesLeaveDetails(request);

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Authorize]
        [HttpPost]
        [Route("LeaveSubmit")]
        public LeaveResponse LeaveSubmit(LeaveSubmitRequest request)
        {
            try
            {
                var response = _leave.LeaveSubmit(request);

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Authorize]
        [HttpPost]
        [Route("LeaveRequestingDetails")]
        public List<LeaveRequestingDetailsResponse> LeaveRequestingDetails(LeaveRequestingDetailsRequest request)
        {
            try
            {
                var response = _leave.LeaveRequestingDetails(request);

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Authorize]
        [HttpPost]
        [Route("LeaveApproval")]
        public LeaveResponse LeaveApproval(LeaveApprovalRequest request)
        {
            try
            {
                var response = _leave.LeaveApproval(request);

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Authorize]
        [HttpPost]
        [Route("LeaveReject")]
        public LeaveResponse LeaveReject(LeaveApprovalRequest request)
        {
            try
            {
                var response = _leave.LeaveReject(request);

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
