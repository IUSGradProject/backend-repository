using APIs.Contracts;
using APIs;
using APIs.Repository.Interface;
using AutoMapper;
using BLL.Services.Interface;

namespace BLL.Services
{
    public class AttributesService : IAttributeService
    {

        private readonly IAttributeRepository _attributeRepository;
        private readonly IMapper _mapper;
        public AttributesService(IAttributeRepository attributeRepository, IMapper mapper)
        {
            _attributeRepository = attributeRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<LoV>> GetAllCategoriesAsync()
        {
            var categories = await _attributeRepository.GetAllCategoriesAsync();
            var models = _mapper.Map<IEnumerable<LoV>>(categories);
            return models;
        }

        public async Task<IEnumerable<LoV>> GetAllBrandsAsync()
        {
            var colors = await _attributeRepository.GetAllBrandsAsync();

            var models = _mapper.Map<IEnumerable<LoV>>(colors);
            return models;
        }

        public async Task<IEnumerable<LoV>> GetAllStylesAsync()
        {
            var styles = await _attributeRepository.GetAllStylesAsync();

            var models = _mapper.Map<IEnumerable<LoV>>(styles);
            return models;
        }

        public async Task<IEnumerable<LoV>> GetAllPowersAsync()
        {
            var powers = await _attributeRepository.GetAllPowersAsync();

            var models = _mapper.Map<IEnumerable<LoV>>(powers);
            return models;
        }
    }
}
