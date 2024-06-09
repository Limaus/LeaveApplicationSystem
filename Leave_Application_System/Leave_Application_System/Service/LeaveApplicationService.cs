using Leave_Application_System.Database;
using Leave_Application_System.Model.Request;
using Leave_Application_System.Model.Response;

namespace Leave_Application_System.Service
{
    public interface ILeave
    {
        List<EmployeesLeaveDetailsResponse> EmployeesLeaveDetails(LeaveRequestingDetailsRequest request);
        List<LeaveRequestingDetailsResponse> LeaveRequestingDetails(LeaveRequestingDetailsRequest request);
        LeaveResponse LeaveSubmit(LeaveSubmitRequest request);
        LeaveResponse LeaveApproval(LeaveApprovalRequest request);
        LeaveResponse LeaveReject(LeaveApprovalRequest request);
        List<LeaveListResponse> Leave_list();
        LoginResponse UserLogin(LoginRequest request);

    }
    public class LeaveApplicationService : ILeave
    {
        private readonly IDataAccess _dataAccess;
        public LeaveApplicationService(IDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

      
        public LoginResponse UserLogin(LoginRequest request)
        {
            try
            {
                var login = new LoginResponse();
                if (request != null)
                {
                    var data = _dataAccess.UserLogin(request);
                    login.message = data.ToString();
                   
                    
                }
                return login;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<LeaveListResponse> Leave_list()
        {
            try
            {
                var leavelist = new List<LeaveListResponse>();
                var data = _dataAccess.leave_list();
                leavelist = data.ToList();

                return leavelist;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public LeaveResponse LeaveSubmit(LeaveSubmitRequest request)
        {
            try
            {
                var leaveMessage = new LeaveResponse();
                if (request != null)
                {
                    var data = _dataAccess.LeaveSubmit(request);
                    leaveMessage.Message = data.ToString();


                }
                return leaveMessage;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<LeaveRequestingDetailsResponse> LeaveRequestingDetails(LeaveRequestingDetailsRequest request)
        {
            try
            {
                var deatils = new List<LeaveRequestingDetailsResponse>();
                var data = _dataAccess.LeaveRequestingDetails(request);
                deatils = data.ToList();

                return deatils;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
         public List<EmployeesLeaveDetailsResponse> EmployeesLeaveDetails(LeaveRequestingDetailsRequest request)
        {
            try
            {
                var deatils = new List<EmployeesLeaveDetailsResponse>();
                var data = _dataAccess.EmployeesLeaveDetails(request);
                deatils = data.ToList();

                return deatils;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public LeaveResponse LeaveApproval(LeaveApprovalRequest request)
        {
            try
            {
                var leaveMessage = new LeaveResponse();
                if (request != null)
                {
                    var data = _dataAccess.LeaveApproval(request);
                    leaveMessage.Message = data.ToString();


                }
                return leaveMessage;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public LeaveResponse LeaveReject(LeaveApprovalRequest request)
        {
            try
            {
                var leaveMessage = new LeaveResponse();
                if (request != null)
                {
                    var data = _dataAccess.LeaveReject(request);
                    leaveMessage.Message = data.ToString();


                }
                return leaveMessage;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
