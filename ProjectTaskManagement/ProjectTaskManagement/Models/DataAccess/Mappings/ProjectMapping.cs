using ProjectTaskManagement.Models.DataAccess.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace ProjectTaskManagement.Models.Mappings
{
    public class ProjectMapping : EntityTypeConfiguration<Project>
    {
        public ProjectMapping()
        {
            Property(p => p.Description)
                .IsRequired()
                .HasColumnType("varchar")
                .HasMaxLength(2000);
            Property(p => p.Name)
                .IsRequired()
                .HasColumnType("varchar")
                .HasMaxLength(300);
            Property(p => p.PlannedEndingDate).IsRequired();
            Property(p => p.PlannedStartingDate).IsRequired();
            Property(p => p.StartingDate).IsOptional();
            Property(p => p.EndingDate).IsOptional();
        }
    }
}