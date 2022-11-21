using HrSystem.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HrSystem.Interfaces
{
    public interface IEmployeeRepo
    {
        List<EmployeeVM> GetAllEmployees();
        EmployeeVM GetEmployeeById(int id);
        void CreateEmployee(EmployeeVM employee);
        void UpdateEmployee(EmployeeVM employee);
        void DeleteEmployee(EmployeeVM employee , string localPath);
        void AddEmpFile(IFormFile File, string LocalPath, int EmployeeId);
        void DeleteEmpFile(string LocalPath, string FileName);
        List<EmployeeFilesVM> GetAllFiles();
    }
}
