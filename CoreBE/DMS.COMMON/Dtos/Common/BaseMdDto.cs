namespace Common
{
    public class BaseMdDto
    {
        public bool? IsActive { get; set; }
    }
    public class BaseMdTemDto
    {
        public bool? IsActive { get; set; }
        public DateTime? CreateDate { get; set; }
        public string? CreateBy { get; set; }
    }
}
