using ECommerce.Business.Abstract;
using ECommerce.Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.API.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public IEnumerable<Product> Get()
        {
            return _productService.GetAllForApi();//bu fonksiyonu yazmamın sebebi ilişkisel ola nveriyi getirtmek istemem.Yoksa dallanma çok oluyor hata veriyor
        }
    }
}
