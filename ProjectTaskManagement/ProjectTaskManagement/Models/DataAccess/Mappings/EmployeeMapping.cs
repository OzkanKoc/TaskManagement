using ProjectTaskManagement.Models.DataAccess.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace ProjectTaskManagement.Models.Mappings
{
    public class EmployeeMapping : EntityTypeConfiguration<Employee>
    {
        public EmployeeMapping()
        {
            HasKey(e => e.EmployeeId);
            Property(e => e.Address)
                .IsOptional()
                .HasMaxLength(2000)
                .HasColumnType("varchar");
            Property(e => e.DateOfBirth).IsOptional();
            Property(e => e.Email).IsRequired();
            Property(e => e.FirstName).IsRequired().HasColumnType("varchar").HasMaxLength(50);
            Property(e => e.LastName).IsRequired().HasColumnType("varchar").HasMaxLength(50);
            Property(e => e.IdentityNumber).IsOptional().HasColumnType("varchar").HasMaxLength(11);
            Property(e => e.HiringDate).IsOptional();
        }
    }
}