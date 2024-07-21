using backendfepon.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace backendfepon.ModelConfigurations
{
    public class FacultyConfiguration : IEntityTypeConfiguration<Faculty>
    {
        public void Configure(EntityTypeBuilder<Faculty> modelBuilder)
        {
            modelBuilder.ToTable("FACULTY");
            modelBuilder.HasKey(p => p.Faculty_Id);
        }
    }
}
