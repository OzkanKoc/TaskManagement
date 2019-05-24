using ProjectTaskManagement.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProjectTaskManagement.Models.DataAccess.Entity
{
    public class Employee
    {
        [DataType("varchar")]
        public string EmployeeId { get; set; } = Guid.NewGuid().ToString();
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public string Address { get; set; }
        public string IdentityNumber { get; set; }
        public Nullable<DateTime> DateOfBirth { get; set; }
        public Nullable<DateTime> HiringDate { get; set; }

        public ApplicationUser User { get; set; }
        public List<Task> Tasks { get; set; }
    }
}