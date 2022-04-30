using CSV;
using System.Globalization;

IList<EmployeeAssignment> csvResult = new List<EmployeeAssignment>();
var lines = new List<string>();
using (var reader = new StreamReader(@"Test.csv"))
{
    lines = reader.ReadToEnd().Split('\n').ToList();
}

csvResult = CSVConvertor.GetInstance().ConvertFromFile(lines);

var result = await CSVConvertor.GetInstance().GetEmployeePairs(csvResult);
Console.WriteLine("EmployeeId1\tEmployeeId2\tProjectId\tWorkedDays");
foreach (var item in result)
{
    Console.WriteLine(item);
}