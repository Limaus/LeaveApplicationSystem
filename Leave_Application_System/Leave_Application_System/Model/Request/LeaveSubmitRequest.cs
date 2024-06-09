namespace Leave_Application_System.Model.Request
{
    public class LeaveSubmitRequest
    {
        public string EmployeeName { get; set; }
        public int LeaveTypeID { get; set; }
        public int LeaveDays { get; set; }
        public string LeaveReason { get; set; }
    }
}
