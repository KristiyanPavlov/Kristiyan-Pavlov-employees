namespace CSV
{
    public class EmployeePairResult
    {
        public EmployeePairResult(int employeeId1, int employeeId2, int projectId, int workedDays)
        {
            EmployeeId1 = employeeId1;
            EmployeeId2 = employeeId2;
            ProjectId = projectId;
            WorkedDays = workedDays;
        }

        public int EmployeeId1 { get; set; }
        public int EmployeeId2 { get; set; }
        public int ProjectId { get; set; }
        public int WorkedDays { get; set; }

        public override string ToString()
        {
            return $"{EmployeeId1}\t{EmployeeId2}\t{ProjectId}\t{WorkedDays}";
        }
    }
}
