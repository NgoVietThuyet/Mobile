namespace Common
{
    public class BaseMdFilter
    {
        public bool? IsActive { get; set; }

        public string? KeyWord { get; set; }

        public string? SortColumn { get; set; }

        public bool? IsDescending { get; set; }

        public List<string>? Fields { get; set; }
    }
}
