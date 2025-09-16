using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DMS.CORE.Entities.AD;

namespace DMS.CORE.Configuration.AD
{
    public class TblAdMenuRightConfig : IEntityTypeConfiguration<TblAdMenuRight>
    {
        public void Configure(EntityTypeBuilder<TblAdMenuRight> builder)
        {
            builder.HasKey(x => new { x.MenuId, x.RightId });
        }
    }
}
