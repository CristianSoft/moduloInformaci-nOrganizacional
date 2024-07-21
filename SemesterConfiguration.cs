using backendfepon.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace backendfepon.ModelConfigurations
{
    public class SemesterConfiguration : IEntityTypeConfiguration<Semester>

    {
        public void Configure(EntityTypeBuilder<Semester> modelBuilder)
        {
            modelBuilder.ToTable("SEMESTER");
            modelBuilder.HasKey(p => p.Semester_Id);
        }
    }
}
