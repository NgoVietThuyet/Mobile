namespace Common
{
    public class PagedResponseDto
    {
        public int CurrentPage { get; set; }

        public int PageSize { get; set; }

        public int TotalPage { get; set; }

        public int TotalRecord { get; set; }

        public object Data { get; set; }
    }
}
