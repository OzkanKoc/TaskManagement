using ProjectTaskManagement.Models.Context;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace ProjectTaskManagement.Models.DataAccess.Entity
{
    public class Project
    {
        public int ProjectId { get; set; }
        public int CustomerId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime PlannedStartingDate { get; set; }
        public DateTime PlannedEndingDate { get; set; }
        public Nullable<DateTime> StartingDate { get; set; } = null;
        public Nullable<DateTime> EndingDate { get; set; } = null;

        public Customer Customer { get; set; }
    }
}