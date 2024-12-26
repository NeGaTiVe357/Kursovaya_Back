using Kursovoy_project_electronic_shop.Contracts;
using Kursovoy_project_electronic_shop.Interfaces;
using Kursovoy_project_electronic_shop.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Kursovoy_project_electronic_shop.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult CreateProduct(ProductInfo productInfo)
        {
            if (productInfo.Name == null || productInfo.Price <= 0)
            {
                return BadRequest();
            }

            if (!_productService.CheckRegex(productInfo.Name))
            {
                ModelState.AddModelError("", "Invalid product name format");

                return BadRequest(ModelState);
            }

            if (!_productService.CheckRegexList(productInfo.Manufacturers))
            {
                ModelState.AddModelError("", "Invalid manufacturer name format");

                return BadRequest(ModelState);
            }

            if (!_productService.CheckRegexList(productInfo.Types))
            {
                ModelState.AddModelError("", "Invalid type name format");

                return BadRequest(ModelState);
            }


            if (_productService.CheckProductInfo(productInfo))
            {
                ModelState.AddModelError("", "Product already exists");

                return BadRequest(ModelState);
            }

            if (!_productService.CreateProduct(productInfo))
            {
                ModelState.AddModelError("", "Failed to create product");

                return BadRequest(ModelState);
            }

            return Ok("Product created");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult<List<Product>> GetAllProducts()
        {
            var products = _productService.GetAllProducts();

            if (products == null)
            {
                return NotFound("No products found");
            }

            return Ok(products);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult<Product> GetSingleProduct(string productName)
        {
            var product = _productService.GetSingleProduct(productName);

            if (product == null)
            {
                return NotFound("Product not found");
            }

            return Ok(product);
        }

        [HttpGet]
        [Authorize(Roles = "Admin, User")]
        public ActionResult<List<ProductInfo>> GetProductsInfo()
        {
            var products = _productService.GetProductsInfo();

            if (products == null)
            {
                return NotFound("No products found");
            }

            return Ok(products);
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public ActionResult UpdateProduct(Guid productUid, ProductInfo productInfo)
        {
            if (productInfo.Name == null || productInfo.Price <= 0 )
            {
                return BadRequest();
            }

            if (!_productService.CheckRegex(productInfo.Name))
            {
                ModelState.AddModelError("", "Invalid product name format");

                return BadRequest(ModelState);
            }

            if (!_productService.CheckRegexList(productInfo.Manufacturers))
            {
                ModelState.AddModelError("", "Invalid manufacturer name format");

                return BadRequest(ModelState);
            }

            if (!_productService.CheckRegexList(productInfo.Types))
            {
                ModelState.AddModelError("", "Invalid type name format");

                return BadRequest(ModelState);
            }

            if (_productService.CheckProductInfo(productUid, productInfo))
            {
                ModelState.AddModelError("", "Product already exists");

                return BadRequest(ModelState);
            }

            if (!_productService.UpdateProduct(productUid, productInfo))
            {
                ModelState.AddModelError("", "Failed to update product");

                return BadRequest(ModelState);
            }

            return Ok("Product updated");
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteProduct(Guid productUid)
        {
            if (!_productService.DeleteProduct(productUid))
            {
                ModelState.AddModelError("", "Failed to delete product");

                return BadRequest(ModelState);
            }

            return Ok("Product deleted");
        }
    }
}
