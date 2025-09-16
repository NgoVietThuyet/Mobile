using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DMS.CORE.Entities.AD;

namespace DMS.CORE.Configuration.AD
{
    public class TblAdAccountConfig : IEntityTypeConfiguration<TblAdAccount>
    {
        public void Configure(EntityTypeBuilder<TblAdAccount> builder)
        {
            builder.HasMany(x => x.Account_AccountGroups)
                .WithOne(g => g.Account)
                .HasForeignKey(x => x.UserName)
                .OnDelete(DeleteBehavior.Cascade);

           
        }
    }
}
