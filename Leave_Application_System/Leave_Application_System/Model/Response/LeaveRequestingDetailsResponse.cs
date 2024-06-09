namespace Leave_Application_System.Model.Response
{
    public class LeaveRequestingDetailsResponse
    {
        public string EmployeeName { get; set; }
        public int Leave_Duration { get; set; }
        public string LeaveName { get; set; }
        public string Leave_Reason { get; set; }
        public DateTime TakingDate { get; set; }
    }
}
