using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProjectTaskManagement.Models.DataAccess
{
    public enum DemandType
    {
        [Display(Name = "Default")]
        None = 0,
        [Display(Name = "Yeni")]
        NewDemand,
        [Display(Name = "Hata")]
        Flaw
    }
}