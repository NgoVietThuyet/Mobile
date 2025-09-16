using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DMS.CORE.Entities.AD;

namespace DMS.CORE.Configuration.AD
{
    public class TblAdAccount_AccountGroupConfig : IEntityTypeConfiguration<TblAdAccount_AccountGroup>
    {
        public void Configure(EntityTypeBuilder<TblAdAccount_AccountGroup> builder)
        {
            builder.HasKey(x => new { x.UserName, x.GroupId });
            builder.HasOne(x=>x.Account).WithMany(x=>x.Account_AccountGroups).HasForeignKey(x=>x.UserName).IsRequired().OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x => x.AccountGroup).WithMany(x=>x.Account_AccountGroups).HasForeignKey(x => x.GroupId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        }
    }
}
