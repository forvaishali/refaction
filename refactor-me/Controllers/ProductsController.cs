using AutoMapper;
using refactor_me.Dto;
using refactor_me.Models;
using refactor_me.Repository;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;

namespace refactor_me.Controllers
{
    /// <summary>
    /// Contains the logic to perform Get, Insert, Update, Delete operations on Products and ProductOptions
    /// </summary>
    [RoutePrefix("products")]
    public class ProductsController : ApiController
    {
        private IProductRepository _productRepository;
        private IProductOptionRepository _productOptionRepository;

        public ProductsController(IProductRepository productRepository, IProductOptionRepository productOptionRepository)
        {
            _productRepository = productRepository;
            _productOptionRepository = productOptionRepository;
        }

        #region Product methods
        /// <summary>
        /// Get Products
        /// </summary>
        /// <param name="name">The Product with the specified name</param>
        /// <returns>Returns all the products if name is not supplied by the user otherwise returns products matching the specified name.</returns>
        [HttpGet()]
        public IHttpActionResult Get(string name = "")
        {
            ProductsDto productResult = new ProductsDto();

            if (string.IsNullOrWhiteSpace(name))
            {
                var products = _productRepository.GetAll();
                productResult.Items =
                   Mapper.Map<List<ProductDto>>(products);
            }
            else
            {
                var products = _productRepository.GetProducts(name);
                if (products == null)
                {
                    return NotFound();
                }
                productResult.Items = Mapper.Map<List<ProductDto>>(products);

            }
            if (productResult == null)
            {
                return NotFound();
            }

            return Ok(productResult);
        }

        /// <summary>
        /// Get Products with the specified product id
        /// </summary>
        /// <param name="id">The Product with the specified id</param>
        /// <returns>Returns all the products matching the specified id.</returns>
        [Route("{id:guid}")]
        [HttpGet()]
        public IHttpActionResult GetProduct(Guid id)
        {
            var products = _productRepository.GetByID(id);
            if (products == null)
            {
                return NotFound();
            }

            var results = Mapper.Map<ProductDto>(products);
            return Ok(results);
        }

        /// <summary>
        /// Creates the product
        /// </summary>
        /// <param name="product">The product to be inserted into the database</param>
        /// <returns>Returns BadRequest if the product is not specified by the user, returns badrequest if the state of the product is invalid, returns internalservererror
        /// if there is an error while saving the product, returns created response code if the product was successfully created</returns>
        [HttpPost()]
        public IHttpActionResult Post([FromBody] ProductDto product)
        {
            try
            {
                if (product == null)
                {
                    return BadRequest();
                }

                if (_productRepository.ProductExists(product.Id))
                {
                    return new ResponseMessageResult(new HttpResponseMessage(HttpStatusCode.Ambiguous));

                }

                var productToAdd = Mapper.Map<Product>(product);

                Validate(productToAdd);

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                _productRepository.Add(productToAdd);

                if (!_productRepository.Save())
                {
                    return new ResponseMessageResult(new HttpResponseMessage(HttpStatusCode.InternalServerError));

                }

                return new ResponseMessageResult(new HttpResponseMessage(HttpStatusCode.Created));
            }
            catch (Exception ex)
            {
                return new ResponseMessageResult(Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message));
            }
        }

        /// <summary>
        /// Update the specified product
        /// </summary>
        /// <param name="id">The id of the product to be updated into the database</param>
        /// <param name="product">The product to be updated into the database</param>
        /// <returns>Returns BadRequest if the product is not specified by the user or if the state of the product is invalid or returns notfound if the
        /// product is not found in the database or returns internalservererror if there is an error while saving the product or
        /// returns ok response code if the product was successfully created</returns>
        [Route("{id}")]
        [HttpPut()]
        public IHttpActionResult Put(Guid id, [FromBody]ProductDto product)
        {
            try
            {
                if (product == null)
                {
                    return BadRequest();
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (!_productRepository.ProductExists(id))
                {
                    return NotFound();
                }

                var productToUpdate = Mapper.Map<Product>(product);

                _productRepository.Update(productToUpdate);

                if (!_productRepository.Save())
                {
                    return InternalServerError();
                }

                return new ResponseMessageResult(new HttpResponseMessage(HttpStatusCode.OK));
            }
            catch (Exception ex)
            {
                return new ResponseMessageResult(Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message));
            }

        }

        #endregion

        #region ProductOptions methods
        /// <summary>
        /// Gets the productoptions associated with the specified product id
        /// </summary>
        /// <param name="id">The id of the product used to retrieve the productoption from the database</param>
        /// <returns>Returns notfound if the product is not found in the database otherwise returns the productoptions</returns>
        [Route("~/products/{id}/options", Name = "GetProductOptions")]
        [HttpGet()]
        public IHttpActionResult Get([FromUri]Guid id)
        {
            try
            {
                var productOptionsResult = new ProductOptionsDto();

                var productOptions = _productOptionRepository.GetProductOptionsForProduct(id);
                if (productOptions == null)
                {
                    return NotFound();
                }

                productOptionsResult.Items = Mapper.Map<List<ProductOptionDto>>(productOptions);

                return Ok(productOptionsResult);
            }
            catch (Exception ex)
            {
                return new ResponseMessageResult(Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message));
            }
        }

        /// <summary>
        /// Gets the productoptions associated with the specified product id and productoption id
        /// </summary>
        /// <param name="id">The id of the product used to retrieve the productoption from the database</param>
        /// <param name="optionId">The productoptionId of the productoption to retrieve from the database</param>
        /// <returns>Returns notfound if the product is not found in the database otherwise returns the productoptions</returns>
        [Route("{id:guid}/options/{optionId}")]
        [HttpGet()]
        public IHttpActionResult GetProductoptionsForProduct(Guid id, Guid optionId)
        {
            try
            {
                var productOptions = _productOptionRepository.GetProductOptionForProduct(id, optionId);
                if (productOptions == null)
                {
                    return NotFound();
                }

                return Ok(productOptions);
            }
            catch (Exception ex)
            {
                return new ResponseMessageResult(Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message));
            }
        }

        /// <summary>
        /// Creates the specified productoption
        /// </summary>
        /// <param name="id">The id of the product to which the productoption is added</param>
        /// <param name="productOption">The productoption to be inserted into the database</param>
        /// <returns>Returns badrequest if the productoption to be created is not provided or the product id of the productoption is not matching the
        /// specified id or if the validation is not successful else returns internalservererror if there is an error while saving the productoption else returns the
        /// created status code if the productoption was created successfully  </returns>
        [Route("{id:guid}/options")]
        [HttpPost()]
        public IHttpActionResult Post(Guid id, [FromBody]ProductOptionDto productOption)
        {
            try
            {
                if (productOption == null || productOption.ProductId != id)
                {
                    return BadRequest();
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (!_productRepository.ProductExists(id))
                {
                    return NotFound();

                }

                var productOptionToAdd = Mapper.Map<ProductOption>(productOption);
                _productOptionRepository.AddProductOptionToProduct(productOptionToAdd, id);

                if (!_productOptionRepository.Save())
                {
                    return new ResponseMessageResult(new HttpResponseMessage(HttpStatusCode.InternalServerError));

                }

                return new ResponseMessageResult(new HttpResponseMessage(HttpStatusCode.Created));
            }
            catch (Exception ex)
            {
                return new ResponseMessageResult(Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message));
            }
        }

        /// <summary>
        /// Update the specified productoption
        /// </summary>
        /// <param name="id">The product id associated with the specified productoption</param>
        /// <param name="optionId">The option id associated with the specified productoption</param>
        /// <param name="productOption">The productoption to be updated</param>
        /// <returns>Returns badrequest if the productoption or the productid or the productoptionid is not specified or if the product state is not valid otherwise
        /// returns notfound if the productoption to be updated is not found otherwise returns internalservererror if there is error whie saving the productoption
        /// otherwise returns ok statuscode if the productoption was saved successfully </returns>
        [Route("{id}/options/{optionId}")]
        [HttpPut()]
        public IHttpActionResult Put(Guid id, Guid optionId, [FromBody]ProductOptionDto productOption)
        {
            try
            {
                if (productOption == null || productOption.ProductId != id || productOption.Id != optionId)
                {
                    return BadRequest();
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (!_productOptionRepository.ProductOptionExists(optionId))
                {
                    return NotFound();
                }

                var productOptionToUpdate = Mapper.Map<ProductOption>(productOption);
                _productOptionRepository.UpdateProductOption(productOptionToUpdate);

                if (!_productOptionRepository.Save())
                {
                    return InternalServerError();
                }

                return new ResponseMessageResult(new HttpResponseMessage(HttpStatusCode.OK));
            }
            catch (Exception ex)
            {
                return new ResponseMessageResult(Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message));
            }


        }

        /// <summary>
        /// Delete the productoption associated with the specified productid and productoptionid
        /// </summary>
        /// <param name="id">The productid</param>
        /// <param name="optionId">The productoptionid</param>
        /// <returns>Returns notfound if the productoption does not exist or returns internalservererror if there is an error while deleting the productoption otherwise
        /// returns nocontent once the productoption is deleted successfully</returns>
        [Route("{id:Guid}/options/{optionId}")]
        [HttpDelete()]
        public IHttpActionResult Delete(Guid id, Guid optionId)
        {
            try
            {
                if (!_productOptionRepository.ProductOptionExists(optionId))
                {
                    return NotFound();
                }

                var productOption = _productOptionRepository.GetProductOptionForProduct(id, optionId);

                if (productOption == null)
                {
                    return NotFound();
                }

                _productOptionRepository.DeleteProductOption(optionId, id);

                if (!_productOptionRepository.Save())
                {
                    return InternalServerError();
                }
                return new ResponseMessageResult(new HttpResponseMessage(HttpStatusCode.NoContent));
            }
            catch (Exception ex)
            {
                return new ResponseMessageResult(Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message));
            }
        }

        #endregion

        /// <summary>
        ///  Delete the product associated with the specified product id
        /// </summary>
        /// <param name="id">The id of the product to be deleted</param>
        /// <returns>Returns notfound if the product does not exist or returns internalservererror if there is an error while deleting the product otherwise
        /// returns nocontent once the product is deleted successfully</returns>
        [Route("{id:Guid}")]
        [HttpDelete()]
        public IHttpActionResult Delete(Guid id)
        {
            try
            {
                if (!_productRepository.ProductExists(id))
                {
                    return NotFound();
                }
                var product = _productRepository.GetByID(id);
                if (product == null)
                {
                    return NotFound();
                }
                _productRepository.DeleteProductWithOptions(product);

                if (!_productRepository.Save())
                {
                    return InternalServerError();
                }
                return new ResponseMessageResult(new HttpResponseMessage(HttpStatusCode.NoContent));
            }
            catch (Exception ex)
            {
                return new ResponseMessageResult(Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message));
            }
        }

    }
}
