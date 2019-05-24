using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProjectTaskManagement.Models.DataAccess.Entity
{
    public class CustomerDemand
    {
        public int CustomerDemandId { get; set; }
        [Display(Name = "Proje")]
        public int ProjectId { get; set; }
        [Display(Name = "Müşteri Talebi")]
        public string Demand { get; set; }
        [Display(Name = "İstek Türü")]
        public DemandType DemandType { get; set; }
        public bool IsActive { get; set; } = true;

        public Project Project { get; set; }
    }
}