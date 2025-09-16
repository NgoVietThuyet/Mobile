namespace Common
{
    public class BaseDto
    {
        public string CreateBy { get; set; }

        public string UpdateBy { get; set; }

        public virtual DateTime? CreateDate { get; set; }

        public DateTime? UpdateDate { get; set; }
    }
}
