using Microsoft.AspNetCore.Mvc;
using pv311_web_api.BLL;
using pv311_web_api.BLL.DTOs.Laptops;
using pv311_web_api.BLL.Services.Laptops;

namespace pv311_web_api.Controllers
{
    [ApiController]
    [Route("api/laptop")]
    public class LaptopController : AppController
    {
        private readonly ILaptopService _laptopService;

        public LaptopController(ILaptopService laptopService)
        {
            _laptopService = laptopService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreateLaptopDto dto)
        {
            var response = await _laptopService.CreateAsync(dto);
            return CreateActionResult(response);
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetAllAsync(int page = 1, int pageSize = Settings.PageSize, string? manufacture = null)
        {
            var response = await _laptopService.GetAllAsync(page, pageSize, manufacture);
            return CreateActionResult(response);
        }
    }
}
