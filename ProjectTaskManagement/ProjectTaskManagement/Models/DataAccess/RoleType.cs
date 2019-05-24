using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProjectTaskManagement.Models.DataAccess
{
    public enum RoleType
    {
        [Display(Name = "Seçiniz")]
        None = 0,
        [Display(Name = "Takım Lideri")]
        TeamLeader,
        [Display(Name = "Proje Yöneticisi")]
        ProjectManager,
        [Display(Name = "İş Analisti")]
        BusinessAnalyst,
        [Display(Name = "Yazılım Geliştirici")]
        SoftwareDeveloper,
        [Display(Name = "Test Uzmanı")]
        TestSpecialist
    }
}