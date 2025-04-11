using AutoMapper;
using Microsoft.EntityFrameworkCore;
using pv311_web_api.BLL.DTOs.Laptops;
using pv311_web_api.BLL.Services.Image;
using pv311_web_api.DAL.Entities;
using pv311_web_api.DAL.Repositories.Laptops;
using pv311_web_api.DAL.Repositories.Manufactures;

namespace pv311_web_api.BLL.Services.Laptops
{
    public class LaptopService : ILaptopService
    {
        private readonly IMapper _mapper;
        private readonly ILaptopRepository _laptopRepository;
        private readonly IManufactureRepository _manufactureRepository;
        private readonly IImageService _imageService;

        public LaptopService(IMapper mapper, ILaptopRepository laptopRepository, IManufactureRepository manufactureRepository, IImageService imageService)
        {
            _mapper = mapper;
            _laptopRepository = laptopRepository;
            _manufactureRepository = manufactureRepository;
            _imageService = imageService;
        }

        public async Task<ServiceResponse> CreateAsync(CreateLaptopDto dto)
        {
            var entity = _mapper.Map<Laptop>(dto);

            if(!string.IsNullOrEmpty(dto.Manufacture))
            {
                entity.Manufacture = await _manufactureRepository
                    .GetByNameAsync(dto.Manufacture);
            }

            if(dto.Images.Count() > 0)
            {
                string path = Path.Combine(Settings.LaptopsPath, entity.Id);
                _imageService.CreateDirectory(path);
                var laptopImages = await _imageService.SaveLaptopImagesAsync(dto.Images, path);
                entity.Images = laptopImages;
            
            }

            var result = await _laptopRepository.CreateAsync(entity);

            if(!result)
            {
                return new ServiceResponse("Не вдалося збрегети ноутбук");
            }

            return new ServiceResponse($"Ноутбук '{entity.Brand} {entity.Model}' збережено", true);
        }

        public async Task<ServiceResponse> GetAllAsync(int page = 1, int pageSize = Settings.PageSize, string? manufacture = null)
        {
            pageSize = pageSize < 1 ? Settings.PageSize : pageSize;

            var laptops = string.IsNullOrEmpty(manufacture)
                ? _laptopRepository.GetLaptops()
                : _laptopRepository.GetLaptops(l => l.Manufacture == null ? false : l.Manufacture.Name.ToLower() == manufacture.ToLower());

            int count = laptops.Count();
            int pageCount = (int)Math.Ceiling((double)count / pageSize);

            page = page < 1 || page > pageCount ? 1 : page;

            laptops = laptops
                .Skip(pageSize * (page - 1))
                .Take(pageSize);

            var list = await laptops.ToListAsync();

            var dtos = _mapper.Map<List<LaptopDto>>(list);

            var dtoList = new LaptopListDto
            {
                Cars = dtos,
                Page = page,
                PageCount = pageCount,
                TotalCount = count
            };

            return new ServiceResponse("Ноутбуки отримано", true, dtoList);
        }

        public async Task<ServiceResponse> GetByPriceAsync(Range range, int page = 1, int pageSize = Settings.PageSize)
        {
            var laptops = await _laptopRepository
                .GetLaptops(c => c.Price >= range.Start.Value && c.Price <= range.End.Value)
                .Include(c => c.Images)
                .Include(c => c.Manufacture)
                .ToListAsync();

            var dtos = _mapper.Map<List<LaptopDto>>(cars);

            return new ServiceResponse("Ноутбуки отримано", true, dtos);
        }
    }
}
