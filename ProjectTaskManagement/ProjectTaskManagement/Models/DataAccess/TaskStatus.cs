using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProjectTaskManagement.Models.DataAccess
{
    public enum TaskStatus
    {
        [Display(Name = "Seçiniz")]
        None = 0,
        [Display(Name = "Başlanmadı")]
        Pending,
        [Display(Name = "Başladı")]
        Started,
        [Display(Name = "Geliştirilecek")]
        WillBeDeveloped,
        [Display(Name = "Test Edilecek")]
        WillBeTested,
        [Display(Name = "Analiz Edilecek")]
        WillBeAnalized,
        [Display(Name = "Tamamlandı")]
        Completed
    }
}