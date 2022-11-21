using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HrSystem.Models
{
    public class EmployeeVM
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Department { get; set; }

        public string DatOfBirth { get; set; }

        public string Address { get; set; }
        

    }
}
