namespace Leave_Application_System.Model.Response
{
    public class EmployeesLeaveDetailsResponse
    {
        public int LeaveType_ID { get; set; }
        public string LeaveName { get; set; }
        public int AvailableLeave { get; set; }
        public int LeaveTakenCount { get; set; }
    }
}
