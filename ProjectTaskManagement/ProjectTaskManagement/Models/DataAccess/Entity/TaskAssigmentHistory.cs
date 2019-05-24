using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProjectTaskManagement.Models.DataAccess.Entity
{
    public class TaskAssigmentHistory
    {
        public int TaskAssigmentHistoryId { get; set; }
        public int TaskId { get; set; }
        [DataType("varchar"), MaxLength(50),Required]
        public string UserName { get; set; }
        public Nullable<DateTime> StartedDate { get; set; }
        public Nullable<DateTime> EndedDate { get; set; }

        public Task Task { get; set; }
    }
}