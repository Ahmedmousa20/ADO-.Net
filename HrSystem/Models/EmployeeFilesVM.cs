using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HrSystem.Models
{
    public class EmployeeFilesVM
    {
        public int EmpId { get; set; }
        public String FileName { get; set; }
        public float FileSize { get; set; }

    }
}
