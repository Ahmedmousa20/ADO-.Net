using HrSystem.Interfaces;
using HrSystem.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HrSystem.Repository
{
    public class EmployeeRepo : IEmployeeRepo
    {
        private readonly IConfiguration configuration;

        public EmployeeRepo(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public List<EmployeeVM> GetAllEmployees()
        {
            List<EmployeeVM> Emps = new List<EmployeeVM>();
            string connectionString = configuration["ConnectionStrings:DefaultConnection"];
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                
                connection.Open();

                string sql = "Select * From Employee";
                SqlCommand command = new SqlCommand(sql, connection);

                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        EmployeeVM employee = new EmployeeVM();
                        employee.Id = Convert.ToInt32(dataReader["Id"]);
                        employee.Name = Convert.ToString(dataReader["Name"]);
                        employee.Department = Convert.ToInt32(dataReader["Department"]);
                        employee.DatOfBirth = Convert.ToString(dataReader["DateOfBirth"]);
                        employee.Address = Convert.ToString(dataReader["Address"]);

                        Emps.Add(employee);
                    }
                }
                connection.Close();

            }
            return Emps;

        }

        public EmployeeVM GetEmployeeById(int id)
        {
            string connectionString = configuration["ConnectionStrings:DefaultConnection"];
            EmployeeVM employee = new EmployeeVM();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                
                connection.Open();

                string sql = $"Select * From Employee Where Id='{id}'";

                SqlCommand command = new SqlCommand(sql, connection);

                using (SqlDataReader dataReader = command.ExecuteReader())
                {

                    employee.Id = Convert.ToInt32(dataReader["Id"]);
                    employee.Name = Convert.ToString(dataReader["Name"]);
                    employee.Department = Convert.ToInt32(dataReader["Department"]);
                    employee.DatOfBirth = Convert.ToString(dataReader["DateOfBirth"]);
                    employee.Address = Convert.ToString(dataReader["Address"]);


                }
                connection.Close();

            }
            return employee;

        }

        public void CreateEmployee(EmployeeVM employee)
        {
            string connectionString = configuration["ConnectionStrings:DefaultConnection"];
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"Insert Into Employee ( Name, Department, DateOfBirth, Address) Values ('{employee.Name}', '{employee.Department}','{employee.DatOfBirth}','{employee.Address}')";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }

            }
        }

        public void UpdateEmployee(EmployeeVM employee)
        {
            string connectionString = configuration["ConnectionStrings:DefaultConnection"];
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"Update Employee SET Name='{employee.Name}', Department='{employee.Department}', DateOfBirth='{employee.DatOfBirth}', Address='{employee.Address}' Where Id='{employee.Id}'";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        public void DeleteEmployee(EmployeeVM employee ,string localPath)
        {
            #region Delete All Employees Files From DB & LoclStorage
            DeleteAllEmployeeFiles(localPath, employee.Id);
            #endregion
           
            #region Delete Employee From Databases
            string connectionString = configuration["ConnectionStrings:DefaultConnection"];
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"Delete From Employee Where Id='{employee.Id}'";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            #endregion


        }

        public void AddEmpFile(IFormFile File, string LocalPath, int EmployeeId)
        {
            float filesize = File.Length;
            string FilePath = Directory.GetCurrentDirectory() + LocalPath;
            string FileName = Guid.NewGuid() + Path.GetFileName(File.FileName);
            string FinalPath = Path.Combine(FilePath, FileName);
            using (var Stream = new FileStream(FinalPath, FileMode.Create))
            {
                File.CopyTo(Stream);
            }

            string connectionString = configuration["ConnectionStrings:DefaultConnection"];
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"Insert Into EmployeeFiles ( Id, FileName, FileSize) Values ('{EmployeeId}', '{FileName}','{filesize}')";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }

            }
        }

        public void DeleteEmpFile(string LocalPath, string FileName)
        {
            string DeletedPath = Directory.GetCurrentDirectory() + LocalPath + FileName;
            if (System.IO.File.Exists(DeletedPath))
            {
                System.IO.File.Delete(DeletedPath);
            }
            string connectionString = configuration["ConnectionStrings:DefaultConnection"];
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"Delete From EmployeeFiles Where FileName='{FileName}'";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        public void DeleteAllEmployeeFiles(string LocalPath, int EmpId)
        {
            List<string> FilesName = new List<string>();
            int i=0;
            string connectionString = configuration["ConnectionStrings:DefaultConnection"];
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                #region Get Files Name I Want To Delete
                string sql = "Select FileName From EmployeeFiles";
                SqlCommand command = new SqlCommand(sql, connection);

                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        FilesName[i] = Convert.ToString(dataReader["FileName"]);
                        i++;
                    }
                }
                #endregion

                #region  Delete files from Database
                string Sql = $"Delete From EmployeeFiles Where Id='{EmpId}'";
                using (SqlCommand Command = new SqlCommand(Sql, connection))
                {
                    command.ExecuteNonQuery();
                }
                #endregion

                #region Delete Files From LocalStorage
                string DeletedPath;
                foreach (string FileName in FilesName)
                {
                    DeletedPath = Directory.GetCurrentDirectory() + LocalPath + FileName;
                    if (System.IO.File.Exists(DeletedPath))
                    {
                        System.IO.File.Delete(DeletedPath);
                    }

                }
                #endregion

                connection.Close();

            }


        }

        public List<EmployeeFilesVM> GetAllFiles()
        {
            List<EmployeeFilesVM> EmpsFiles = new List<EmployeeFilesVM>();
            string connectionString = configuration["ConnectionStrings:DefaultConnection"];
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string sql = "Select * From EmployeeFiles";
                SqlCommand command = new SqlCommand(sql, connection);

                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        EmployeeFilesVM FileData = new EmployeeFilesVM();
                        FileData.EmpId = Convert.ToInt32(dataReader["Id"]);
                        FileData.FileName = Convert.ToString(dataReader["FileName"]);
                        FileData.FileSize = Convert.ToInt32(dataReader["FileSize"]);

                        EmpsFiles.Add(FileData);
                    }
                }
                connection.Close();

            }
            return EmpsFiles;

        }




    }
}
