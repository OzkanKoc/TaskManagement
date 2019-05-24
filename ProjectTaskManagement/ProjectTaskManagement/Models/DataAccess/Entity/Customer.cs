using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProjectTaskManagement.Models.DataAccess.Entity
{
    public class Customer
    {
        [Display(Name = "Müşteriler")]
        public int CustomerId { get; set; }
        public string CompanyName { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }
        public string Address { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public List<Project> Projects { get; set; }
        public List<CustomerContact> CustomerContacts { get; set; }
    }
}