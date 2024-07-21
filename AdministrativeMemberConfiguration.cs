using backendfepon.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace backendfepon.ModelConfigurations
{
    public class AdministrativeMemberConfiguration : IEntityTypeConfiguration<AdministrativeMember>
    {
        public void Configure(EntityTypeBuilder<AdministrativeMember> modelBuilder)
        {
            modelBuilder.ToTable("ADMINISTRATIVE_MEMBER");
            modelBuilder.HasKey(p => p.Administrative_Member_Id);


            modelBuilder
              .HasOne(p => p.Role)
              .WithMany(s => s.Administrative_Members)
              .HasForeignKey(p => p.Role_Id);

            modelBuilder
              .HasOne(p => p.State)
              .WithMany(s => s.AdministrativesMembers)
              .HasForeignKey(p => p.State_Id);

            modelBuilder
              .HasOne(p => p.Semester)
              .WithMany(s => s.administrativeMembers)
              .HasForeignKey(p => p.Semester_Id);

            modelBuilder
              .HasOne(p => p.Career)
              .WithMany(s => s.administrativeMembers)
              .HasForeignKey(p => p.Career_Id);

            modelBuilder
              .HasOne(p => p.Faculty)
              .WithMany(s => s.administrativeMembers)
              .HasForeignKey(p => p.Faculty_Id);




        }
    }
}
