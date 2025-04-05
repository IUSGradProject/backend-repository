using APIs;
using APIs.Contracts.Product;
using BLL.ActionFilter;
using BLL.Services.Interface;
using DTOs.Contracts;
using DTOs.Contracts.Product;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    // PUT: /products/all
    [HttpPut("All")]
    public async Task<IActionResult> GetProducts([FromBody] FilterRequest filters, [AsParameters] int pageNumber = 1, int pageSize = 20)
    {
        var products = await _productService.GetProductsAsync(filters, pageNumber, pageSize);
        return Ok(products);
    }

    // GET: /products/all
    [HttpGet("All")]
    public async Task<IActionResult> GetAllProducts([AsParameters] int pageNumber = 1, int pageSize = 20)
    {
        var products = await _productService.GetAllProductsAsync(pageNumber, pageSize);
        return Ok(products);
    }

    // POST: /products
    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody] UpdateProductContract createProduct)
    {
        await _productService.AddProductAsync(createProduct);

        return Ok();
    }

    // GET: /products/id
    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductById(Guid id)
    {
        var products = await _productService.GetProductAsync(id);
        return Ok(products);
    }


    // DELETE: /products/id
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(Guid id)
    {
        await _productService.DeleteAsync(id);
        return Ok();
    }

    // GET: /products/deleted
    [HttpGet("deleted")]
    public async Task<ActionResult<List<Product>>> GetDeletedProducts()
    {
        var deletedProducts = await _productService.GetAllDeletedProductsAsync();
        return Ok(deletedProducts);
    }

    // PUT: /products/restore/{id}
    [HttpPut("restore/{id}")]
    public async Task<IActionResult> RestoreProduct(Guid id)
    {
        await _productService.RestoreAsync(id);
        return Ok();
    }


    // GET: /products/edit/id
    [HttpGet("Edit/{id}")]
    public async Task<IActionResult> GetEditProduct(Guid id)
    {
        return Ok(await _productService.GetProductForEdit(id));
    }

    //POST: /products/
    [HttpPut]
    public async Task<IActionResult> UpdateProduct([FromBody]UpdateProductContract product)
    {
        await _productService.UpdateProductAsync(product);
        return Ok();
    }

    // GET: /products/soldout
    [HttpGet("soldout")]
    public async Task<IActionResult> GetSoldOutProducts()
    {
        var soldOutProducts = await _productService.GetSoldOutProductsAsync();
        return Ok(soldOutProducts);
    }

}
