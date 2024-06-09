using Leave_Application_System.Model.Request;
using Leave_Application_System.Model.Response;
using System.Data;
using System.Data.SqlClient;

namespace Leave_Application_System.Database
{
    public interface IDataAccess
    {
        string LeaveSubmit(LeaveSubmitRequest request);
        List<LeaveListResponse> leave_list();
        string UserLogin(LoginRequest request);
        List<LeaveRequestingDetailsResponse> LeaveRequestingDetails(LeaveRequestingDetailsRequest request);
        string LeaveReject(LeaveApprovalRequest request);
        string LeaveApproval(LeaveApprovalRequest request);
        List<EmployeesLeaveDetailsResponse> EmployeesLeaveDetails(LeaveRequestingDetailsRequest request);
    }
    public class DataAccess : IDataAccess
    {
        private string _connectionString = string.Empty;
        private IConfiguration _configuration { get; set; }

        public DataAccess(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration["ConnectionStrings"];
        }
        public string UserLogin(LoginRequest request)
        {
            string str = "";
            int count = 0;
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                { 
                    string query = "select count(*) as count from Employee e where e.EmployeeName=@EmployeeName and  Password = @Password;";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@EmployeeName", request.Username);
                        command.Parameters.AddWithValue("@Password", request.Password);



                        connection.Open();
                        // Execute the query
                        using (SqlDataReader reader = command.ExecuteReader())
                        {


                            // Read the data and do something with it
                            while (reader.Read())
                            {
                                string data = reader["count"].ToString();
                                
                                count = int.Parse(data);
                                if (count > 0)
                                {

                                    str = "Login Suceessfull..";
                                }
                                else
                                {
                                    str = "Invalid Username or Password";
                                }
                            }
                            reader.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    str = "Error: " + ex.ToString;
                }
                return str;
            }
        }

        public List<LeaveListResponse> leave_list()
        {
            string query = "select * from LeaveType";
            List<LeaveListResponse> LeaveTypelist = new List<LeaveListResponse>();


            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    LeaveTypelist.Add(new LeaveListResponse
                    {
                        ID = Convert.ToInt32(reader["LeaveType_ID"]),
                        LeaveName = reader["LeaveName"].ToString(),
                        LeaveLimit = Convert.ToInt32(reader["Leave_Limit"]),
                    });
                }
            }

            return LeaveTypelist;
        }

        public string LeaveSubmit(LeaveSubmitRequest request)
        {
            string message = "";
            using (SqlConnection Sqlcon = new SqlConnection(_connectionString))
            {
                using (SqlCommand sqlcmd = new SqlCommand(_connectionString))
                {
                    sqlcmd.Connection = Sqlcon;
                    sqlcmd.CommandType = CommandType.Text;
                    sqlcmd.CommandText = "INSERT INTO Leave(EmployeeCode,LeaveType_ID,Leave_Duration,Leave_Reason,Leave_subittedDate,Leave_Status) " +
                        "VALUES((select EmployeeCode from Employee where EmployeeName=@EmployeeName),@LeaveType_ID,@Leave_Duration,@Leave_Reason,(CAST(GETDATE() AS DATE)),1)";

                    sqlcmd.Parameters.AddWithValue("@EmployeeName", request.EmployeeName);
                    sqlcmd.Parameters.AddWithValue("@LeaveType_ID", request.LeaveTypeID);
                    sqlcmd.Parameters.AddWithValue("@Leave_Duration", request.LeaveDays);
                    sqlcmd.Parameters.AddWithValue("@Leave_Reason", request.LeaveReason);
                    Sqlcon.Open();
                    int rowsAffected = sqlcmd.ExecuteNonQuery();
                    // Sqlcon.Close();
                    if (rowsAffected > 0)
                    {
                        message = "Successfully Submitted Leave";
                    }

                    return message;

                }
            }
        }

            public List<LeaveRequestingDetailsResponse> LeaveRequestingDetails(LeaveRequestingDetailsRequest request)
            {
                string query = "select e.EmployeeName,l.Leave_Duration,lt.LeaveName,l.Leave_Reason,l.Leave_subittedDate as TakingDate from Leave l " +
                    "INNER JOIN LeaveType lt ON l.LeaveType_ID=lt.LeaveType_ID " +
                    "INNER JOIN Employee e ON e.EmployeeCode=l.EmployeeCode " +
                    "where e.Department_ID=(select e.Department_ID from Manager m,Employee e where e.EmployeeName=@EmployeeName and e.Department_ID=m.Department_ID)";
                List<LeaveRequestingDetailsResponse> LeaveDetails = new List<LeaveRequestingDetailsResponse>();


                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@EmployeeName", request.EmployeeName);

                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        LeaveDetails.Add(new LeaveRequestingDetailsResponse
                        {
                        
                            EmployeeName = reader["EmployeeName"].ToString(),
                            Leave_Duration = Convert.ToInt32(reader["Leave_Duration"]),
                            LeaveName = reader["LeaveName"].ToString(),
                            Leave_Reason = reader["Leave_Reason"].ToString(),
                            TakingDate = reader.GetDateTime(reader.GetOrdinal("TakingDate"))
                        });
                    }
                }

                return LeaveDetails;
            }

        public string LeaveReject(LeaveApprovalRequest request)
        {
            string message = "";
            using (SqlConnection Sqlcon = new SqlConnection(_connectionString))
            {
                using (SqlCommand sqlcmd = new SqlCommand(_connectionString))
                {
                    sqlcmd.Connection = Sqlcon;
                    sqlcmd.CommandType = CommandType.Text;
                    sqlcmd.CommandText = "update Leave  set Leave_Verifiedby=(select e.EmployeeCode from employee e where e.EmployeeName=@Leave_Verifiedby) ,Leave_VerifiedDate=CAST(GETDATE() AS DATE), Leave_Status=0 where Leave_ID=@LeaveID";

                    sqlcmd.Parameters.AddWithValue("@LeaveID", request.LeaveID);
                    sqlcmd.Parameters.AddWithValue("@Leave_Verifiedby", request.Verifiedby);
                    Sqlcon.Open();
                    int rowsAffected = sqlcmd.ExecuteNonQuery();
                     Sqlcon.Close();
                    if (rowsAffected > 0)
                    {
                        message = "The Requested Leave is Rejected";
                    }

                    return message;

                }
            }
        }

        public string LeaveApproval(LeaveApprovalRequest request)
        {
            string message = "";
            using (SqlConnection Sqlcon = new SqlConnection(_connectionString))
            {
                using (SqlCommand sqlcmd = new SqlCommand(_connectionString))
                {
                    sqlcmd.Connection = Sqlcon;
                    sqlcmd.CommandType = CommandType.Text;
                    sqlcmd.CommandText = "update Leave  set Leave_Verifiedby=(select e.EmployeeCode from employee e where e.EmployeeName=@Leave_Verifiedby),Leave_VerifiedDate=CAST(GETDATE() AS DATE), Leave_Status=2 where Leave_ID=@LeaveID";

                    sqlcmd.Parameters.AddWithValue("@LeaveID", request.LeaveID);
                    sqlcmd.Parameters.AddWithValue("@Leave_Verifiedby", request.Verifiedby);
                    Sqlcon.Open();
                    int rowsAffected = sqlcmd.ExecuteNonQuery();
                    Sqlcon.Close();
                    if (rowsAffected > 0)
                    {
                        message = "The Requested Leave is Approved";
                    }

                    return message;

                }
            }
        }

        public List<EmployeesLeaveDetailsResponse> EmployeesLeaveDetails(LeaveRequestingDetailsRequest request)
        {
            List<EmployeesLeaveDetailsResponse> LeaveDetails = new List<EmployeesLeaveDetailsResponse>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand("EmployeeLeaveDetails", connection);
                command.CommandType = CommandType.StoredProcedure;

                // Add parameters
                command.Parameters.AddWithValue("@EmployeeName", request.EmployeeName);

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    LeaveDetails.Add(new EmployeesLeaveDetailsResponse
                    {
                        LeaveType_ID = Convert.ToInt32(reader["LeaveType_ID"]),
                        LeaveName = reader["LeaveName"].ToString(),
                        AvailableLeave = Convert.ToInt32(reader["AvailableLeave"]),
                        LeaveTakenCount = Convert.ToInt32(reader["LeaveTakenCount"]),
      
                    });
                }
            }

            return LeaveDetails;
        }
    }
}
