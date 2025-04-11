using pv311_web_api.BLL.DTOs.Laptops;

namespace pv311_web_api.BLL.DTOs.Laptops
{
    public class LaptopListDto
    {
        public List<LaptopDto> Laptops { get; set; } = [];
        public int TotalCount { get; set; } = 0;
        public int Page { get; set; } = 1;
        public int PageCount { get; set; } = 3;
    }
}
