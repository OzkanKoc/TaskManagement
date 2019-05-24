using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectTaskManagement.Models.DataAccess.Entity
{
    public class CustomerContact
    {
        public int CustomerContactId { get; set; }
        public int CustomerId { get; set; }
        public string ContactName { get; set; }
        public string ContactTitle { get; set; }

        public Customer Customer { get; set; }
    }
}