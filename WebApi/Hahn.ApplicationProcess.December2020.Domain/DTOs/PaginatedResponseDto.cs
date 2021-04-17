namespace Hahn.ApplicationProcess.December2020.Domain.DTOs
{
    public class PaginatedResponseDto<T>
    {
        public string Name { get; set; }
        public int Count { get; set; }
        public int PerPage { get; set; }
        public int PageNumber { get; set; }
        public T Data { get; set; }

    }
}
