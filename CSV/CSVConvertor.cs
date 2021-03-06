using System.Globalization;
using System.IO;

namespace CSV
{
    /// <summary>
    /// CSV helper used for work, aggreagate and map information from csv file.
    /// </summary>
    public sealed class CSVConvertor
    {
        private static CSVConvertor? _instance;
        private CSVConvertor() { }

        /// <summary>
        /// Singleton instance
        /// </summary>
        /// <returns></returns>
        public static CSVConvertor GetInstance()
        {
            if (_instance == null)
            {
                _instance = new CSVConvertor();
            }

            return _instance;
        }

        /// <summary>
        /// Converts information from CSV file to EmployeeAssignment collection
        /// </summary>
        /// <param name="reader"></param>
        /// <returns>IList<EmployeeAssignment></returns>
        /// <exception cref="FormatException"></exception>
        public IList<EmployeeAssignment> ConvertFromFile(IList<string> csvLines,
            string? dateFormat = null, string? twoLetter = null)
        {
            var result = new List<EmployeeAssignment>();

            var lines = csvLines.Select(q => q.Split(',').Select(p => p.Trim()).ToList());
            if (lines.Any(q => q.Count != 4))
                throw new FormatException("CSV file format is wrong! Must be 4 columns on line!");

            var cultureInfo = CultureInfo.InvariantCulture;
            if (!string.IsNullOrEmpty(twoLetter))
                cultureInfo = new CultureInfo(twoLetter);

            result = lines.Select(line =>
                new EmployeeAssignment()
                {
                    EmployeeId = Convert.ToInt32(line[0], cultureInfo),
                    ProjectId = Convert.ToInt32(line[1], cultureInfo),
                    DateFrom = !string.IsNullOrEmpty(dateFormat) ?
                        DateTime.ParseExact(line[2], dateFormat ?? string.Empty, null) :
                        Convert.ToDateTime(line[2], cultureInfo),
                    DateTo = line[3].ToUpperInvariant() == "NULL" ?
                        DateTime.Now :
                        !string.IsNullOrEmpty(dateFormat) ?
                            DateTime.ParseExact(line[3], dateFormat ?? string.Empty, null) :
                            Convert.ToDateTime(line[3], cultureInfo)
                })
                .ToList();

            var invalidRows = result.GroupBy(q => q.EmployeeId.ToString() + '-' + q.ProjectId.ToString()).Where(q => q.Count() > 1).ToList();
            if (invalidRows.Count > 0)
                throw new Exception($"There are duplicated rows. Combination of EmployeeID and ProjectId must be unique! {string.Join(",", invalidRows.Select(q => $"[{q.Key}]"))}");

            return result;
        }

        /// <summary>
        /// Get employee pairs by max overlap between two employee by projectId.
        /// </summary>
        /// <param name="employeeAssignments"></param>
        /// <returns>IList<EmployeePairResult></returns>
        public async Task<IList<EmployeePairResult>> GetEmployeePairs(IList<EmployeeAssignment> employeeAssignments)
        {
            var result = new List<EmployeePairResult>();
            var assignemtsByProjectId = employeeAssignments.GroupBy(q => q.ProjectId).ToList();
            var taskList = new List<Task>();

            foreach (var item in assignemtsByProjectId)
            {
                var list = item.ToList();
                if (list.Count > 1)
                {
                    taskList.Add(Task.Run(() =>
                    {
                        var employeePair = GetEmployeePairForProject(item.Key, list);
                        if (employeePair != null)
                            result.Add(employeePair);
                    }));
                }
            }

            await Task.WhenAll(taskList.ToArray());

            return result.OrderByDescending(q => q.WorkedDays).ToList();
        }

        private EmployeePairResult? GetEmployeePairForProject(int projectId, IList<EmployeeAssignment> list)
        {
            var employeeId1 = 0;
            var employeeId2 = 0;
            var overlapMax = TimeSpan.Zero;

            for (int i = 0; i < list.Count - 1; i++)
            {
                for (int j = i + 1; j < list.Count; j++)
                {
                    var temp = list[i].GetOverlap(list[j]);
                    if (temp > overlapMax)
                    {
                        employeeId1 = list[i].EmployeeId;
                        employeeId2 = list[j].EmployeeId;
                        overlapMax = temp;
                    }
                }
            }

            if (overlapMax > TimeSpan.Zero)
                return new EmployeePairResult(employeeId1, employeeId2, projectId, (int)Math.Ceiling(overlapMax.TotalDays));

            return null;
        }
    }
}
