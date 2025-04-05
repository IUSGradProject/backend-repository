using APIs.Contracts;
using APIs.Contracts.Product;
using APIs;
using APIs.Repository.Interface;
using AutoMapper;
using BLL.Services.Interface;
using DTOs.Contracts.Product;
using DTOs.Contracts;

namespace BLL.Services
{
    public class ProductsService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMaterialRepository _materialRepository;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public ProductsService(IProductRepository productRepository, IMapper mapper, IMaterialRepository materialRepository, ITokenService tokenService)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _materialRepository = materialRepository;
            _tokenService = tokenService;
        }

        public async Task<PaginatedDataContract<IEnumerable<ProductContract>>> GetProductsAsync(FilterRequest filters, int pageNumber = 1, int pageSize = 20)
        {
            var role = _tokenService.GetRole();
            var products = await _productRepository.GetAllProductsAsync(filters);

            var paginatedProducts = products.Skip((pageNumber - 1) * pageSize).Take(pageSize);
            var models = _mapper.Map<IEnumerable<ProductContract>>(paginatedProducts);
            var paginatedData = new PaginatedDataContract<IEnumerable<ProductContract>>(models, pageNumber, pageSize, products.Count());

            return paginatedData;
        }

        public async Task<PaginatedDataContract<IEnumerable<ProductBaseContract>>> GetAllProductsAsync(int pageNumber = 1, int pageSize = 20)
        {
            var role = _tokenService.GetRole();
            var products = await _productRepository.GetAllProductsAsync();

            var paginatedProducts = products.Skip((pageNumber - 1) * pageSize).Take(pageSize);
            var models = _mapper.Map<IEnumerable<ProductBaseContract>>(paginatedProducts);
            var paginatedData = new PaginatedDataContract<IEnumerable<ProductBaseContract>>(models, pageNumber, pageSize, products.Count());

            return paginatedData;
        }

        public async Task<IEnumerable<Product>> GetSoldOutProductsAsync()
        {
            return await _productRepository.GetSoldOutProductsAsync();
        }

        public async Task AddProductAsync(UpdateProductContract product)
        {
            var model = _mapper.Map<Product>(product);
            model.Version = 1;
            model.ProductId = Guid.NewGuid();
            await _productRepository.AddProductAsync(model);
        }

        public async Task<ProductOverviewContract> GetProductAsync(Guid productId)
        {
            var product = await _productRepository.GetProductByIdAsync(productId);
            var materials = await _materialRepository.GetMaterialsByProductIdAsync(productId);

            var model = _mapper.Map<ProductOverviewContract>(product);
            model.Materials = _mapper.Map<IEnumerable<LoV>>(materials);
            return model;
        }

        public async Task DeleteAsync(Guid id)
        {
            await _productRepository.DeleteProductAsync(id);
        }

        public async Task<List<Product>> GetAllDeletedProductsAsync()
        {
            return await _productRepository.GetAllDeletedProductsAsync();
        }

        public async Task RestoreAsync(Guid id)
        {
            await _productRepository.RestoreProductAsync(id);
        }

        public async Task<UpdateProductContract> GetProductForEdit(Guid productId)
        {
            var product = await _productRepository.GetProductForEdit(productId);
            return _mapper.Map<UpdateProductContract>(product);
        }

        public async Task UpdateProductAsync(UpdateProductContract product)
        {
            var model = _mapper.Map<Product>(product);
            await _productRepository.UpdateProductAsync(model);
        }
    }

}
