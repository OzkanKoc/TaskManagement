using ProjectTaskManagement.Models.DataAccess.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace ProjectTaskManagement.Models.Mappings
{
    public class TaskMapping : EntityTypeConfiguration<Task>
    {
        public TaskMapping()
        {
            Property(t => t.Name)
                .IsRequired()
                .HasColumnType("varchar")
                .HasMaxLength(300);

            Property(t => t.Description)
                .IsRequired()
                .HasColumnType("varchar")
                .HasMaxLength(2000);
            
            Property(t => t.CreationDate).IsRequired();
            Property(t => t.Status).IsRequired();

            HasRequired(t => t.Project).WithMany().HasForeignKey(t => t.ProjectId);
        }
    }
}