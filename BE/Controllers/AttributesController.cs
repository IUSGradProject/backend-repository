using BLL.Services.Interface;
using DTOs.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APIs.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AttributesController : ControllerBase
    {
        private readonly IAttributeService _attributesService;

        public AttributesController(IAttributeService attributesService)
        {
            _attributesService = attributesService;
        }

        [HttpGet("Categories")]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _attributesService.GetAllCategoriesAsync();
            return Ok(categories);
        }

        [HttpGet("Brands")]
        public async Task<IActionResult> GetAllBrands()
        {
            var brands = await _attributesService.GetAllBrandsAsync();
            return Ok(brands);
        }

        [HttpGet("Styles")]
        public async Task<IActionResult> GetAllStyles()
        {
            var styles = await _attributesService.GetAllStylesAsync();
            return Ok(styles);
        }

        [HttpGet("Powers")]
        public async Task<IActionResult> GetAllPowers()
        {
            var powers = await _attributesService.GetAllPowersAsync();
            return Ok(powers);
        }
    }
}
