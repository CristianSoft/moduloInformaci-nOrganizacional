using backendfepon.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace backendfepon.ModelConfigurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> modelBuilder)
        {
            modelBuilder.ToTable("ADMINISTRATIVE_ROLE");
            modelBuilder.HasKey(p => p.Role_Id);
        }
    }
}
