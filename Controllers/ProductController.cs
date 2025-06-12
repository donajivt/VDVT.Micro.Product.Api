using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VDVT.Micro.Product.Api.Data;
using VDVT.Micro.Product.Api.Models.Dto;

namespace VDVT.Micro.Product.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly AppDbContext _db;
        private ResponseDto _response;
        private IMapper _mapper;

        public ProductController(AppDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
            _response = new ResponseDto();
        }

        [HttpGet]
        public ResponseDto Get()
        {
            try
            {
                IEnumerable<Models.Product> objList = _db.Productos.ToList();
                _response.Result = _mapper.Map<IEnumerable<ProductDto>>(objList);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }
        [HttpPost]
        [Authorize(Roles = "ADMINISTRATOR")]
        public ResponseDto Post(ProductDto productDto)
        {
            try
            {
                Models.Product product = _mapper.Map<Product.Api.Models.Product>(productDto);
                _db.Productos.Add(product);
                _db.SaveChanges();
                if(productDto.Image != null)
                {
                    string fileName = product.ProductId + Path.GetExtension(productDto.Image.FileName);
                    var filePath = @"wwwroot\Images\" + fileName;
                    var filePathDirectory = Path.Combine(Directory.GetCurrentDirectory(), filePath);
                    using (var fileStream = new FileStream(filePathDirectory, FileMode.Create))
                    {
                        productDto.Image.CopyTo(fileStream);
                    }
                    var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}{HttpContext.Request.PathBase.Value}";
                    product.ImageUrl = baseUrl + "/Images/" + fileName;
                    product.ImageLocalPath = filePath;
                }
                else
                {
                    product.ImageUrl = "https://placehold.co/600x400";
                }
                _db.Productos.Update(product);
                _db.SaveChanges();
                _response.Result = _mapper.Map<ProductDto>(product);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpGet]
        [Route("{id:int}")]
        public ResponseDto Get(int id)
        {
            try
            {
                Models.Product obj = _db.Productos.First(u => u.ProductId == id);
                _response.Result = _mapper.Map<ProductDto>(obj);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpPut]
        [Authorize(Roles = "ADMINISTRATOR")]
        public ResponseDto Put(ProductDto productDto)
        {
            try
            {
                Models.Product product = _mapper.Map<Product.Api.Models.Product>(productDto);
                if(productDto.Image != null)
                {
                    if (!string.IsNullOrEmpty(product.ImageLocalPath))
                    {
                        var oldFilePathDirectory = Path.Combine(Directory.GetCurrentDirectory(), product.ImageLocalPath);
                        FileInfo archivo = new FileInfo(oldFilePathDirectory);
                        if(archivo.Exists)
                        {
                            //eliminamos la imagen anterior antes de actualizar la nueva imagen
                            archivo.Delete();
                        }
                    }
                    string fileName = product.ProductId + Path.GetExtension(productDto.Image.FileName);
                    string archivoPath = @"wwwroot\Images\" + fileName;
                    var archivoPathDirectory = Path.Combine(Directory.GetCurrentDirectory(), archivoPath);
                    using(var fileStream = new FileStream(archivoPathDirectory, FileMode.Create))
                    {
                        productDto.Image.CopyTo(fileStream);
                    }
                    var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}{HttpContext.Request.PathBase.Value}";
                    product.ImageLocalPath = baseUrl + "/Images/" +fileName;
                    //guardamos la url o path de la imagen
                    product.ImageLocalPath = archivoPath;
                }
                _db.Productos.Update(product);
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }
        [HttpDelete]
        [Route("{id:int}")]
        [Authorize(Roles = "ADMINISTRATOR")]
        public ResponseDto Delete(int id)
        {
            try
            {
                //consultamos el producto que sera eliminado
                Product.Api.Models.Product product = _db.Productos.First(p => p.ProductId == id);
                if (!string.IsNullOrEmpty(product.ImageLocalPath))
                {
                    //obtenemos la ruta o direccion que contiene la imagen de forma local
                    var oldPathDirectory = Path.Combine(Directory.GetCurrentDirectory(), product.ImageLocalPath);
                    FileInfo archivo = new FileInfo(oldPathDirectory);
                    if (archivo.Exists)
                    {
                        archivo.Delete();
                    }
                }
                _db.Productos.Remove(product);
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }
    }
}
