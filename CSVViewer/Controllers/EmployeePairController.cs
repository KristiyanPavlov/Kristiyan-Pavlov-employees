﻿using CSV;
using Microsoft.AspNetCore.Mvc;
using CSVViewer.Models;

namespace CSVViewer.Controllers
{
    public class EmployeePairController : Controller
    {
        public IActionResult Index()
        {
            return View(new List<EmployeePairResult>());
        }

        [HttpPost]
        public async Task<IActionResult> Index(IFormCollection csvFile)
        {
            if (csvFile != null && csvFile.Files.Any())
            {
                try
                {
                    var lines = csvFile.Files[0].ReadAsList();
                    var employeeAssignments = CSVConvertor.GetInstance().ConvertFromFile(await lines);
                    var employeePairResults = await CSVConvertor.GetInstance().GetEmployeePairs(employeeAssignments);

                    return View(employeePairResults);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }

            return View(new List<EmployeePairResult>());
        }
    }
}
