namespace DMS.CORE.Common
{
    public interface IBaseEntity
    {
        string? CreateBy { get; set; }
        string? UpdateBy { get; set; }
        DateTime? CreateDate { get; set; }
        DateTime? UpdateDate { get; set; }
    }
}
