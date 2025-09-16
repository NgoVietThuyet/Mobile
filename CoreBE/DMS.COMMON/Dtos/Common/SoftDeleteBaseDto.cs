namespace Common
{
    public class SoftDeleteBaseDto : BaseDto
    {
        public bool? IsDeleted { get; set; }
    }
}
