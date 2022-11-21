using HrSystem.Interfaces;
using HrSystem.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Octokit.Internal;
using Refit;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;


namespace HrSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepo employee;

        public IConfiguration Configuration { get; }

        public EmployeeController(IConfiguration configuration , IEmployeeRepo Employee )
        {
            Configuration = configuration;
            employee = Employee;
        }


        [HttpGet]
        [Route("~/GetEmployees")]
        public IActionResult GetEmployees()
        {
            try
            {
               List<EmployeeVM> employees = employee.GetAllEmployees();
               return Ok(employees);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
           

        }


        [HttpGet]
        [Route("~/GetEmployeeById")]
        public IActionResult GetEmployeeById(int id)
        {
            try
            {
                EmployeeVM Employee = employee.GetEmployeeById(id);
                return Ok(Employee);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }


        }


        [HttpPost]
        [Route("~/AddEmployee")]
        public IActionResult Create(EmployeeVM Employee)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    employee.CreateEmployee(Employee);
                }
                return Ok();

            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
            
            
        }


        [HttpPut]
        [Route("~/UpdateEmployee")]
        public IActionResult Update(EmployeeVM Employee)
        {
            try
            {
                employee.UpdateEmployee(Employee);
                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
           
        }


        [HttpDelete]
        [Route("~/DeleteEmployee")]
        public IActionResult Delete(EmployeeVM Employee , string localpath)
        {
            try
            {
                employee.DeleteEmployee(Employee, localpath);
                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
           

        }


        [HttpPost]
        [Route("~/AddEmployeeFile")]
        public IActionResult AddFile(IFormFile File, string LocalPath , int EmployeeId)
        {
            try
            {
                employee.AddEmpFile(File, LocalPath, EmployeeId);
                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
            
        }


        [HttpDelete]
        [Route("~/DeleteEmployeeFile")]
        public IActionResult RemoveFile(string LocalPath, string FileName)
        {
            try
            {
                employee.DeleteEmpFile(LocalPath , FileName);
                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
            

        }

        [HttpGet]
        [Route("~/GetEmpsFiles")]
        public IActionResult GetEmployeesFiles()
        {
            try
            {
                List<EmployeeFilesVM> EmpsFiles = employee.GetAllFiles();
                return Ok(EmpsFiles);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }


        }
    }
}
