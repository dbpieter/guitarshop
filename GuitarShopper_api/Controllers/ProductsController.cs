using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using GuitarShopper_api.Models;
using System.Web;

namespace GuitarShopper_api.Controllers
{
    public class ProductsController : ApiController
    {
        // GET api/<controller>
        /// <summary>
        ///  Gets all products
        /// </summary>
        /// <returns></returns>
        public IEnumerable<MyProduct> Get()
        {
            return product.GetAllProductsInfo();
        }

        // GET api/<controller>/5
        /// <summary>
        /// Gets a single product
        /// </summary>
        /// <param name="id">The product's id</param>
        /// <returns></returns>
        public MyProduct Get(int id)
        {
            return product.GetProductInfo(id);
        }

        /// <summary>
        /// Orders a product 
        /// </summary>
        /// <param name="id">The product's id</param>
        /// <returns></returns>
        [HttpPatch]
        [Route("api/Products/{id}/order")]
        public HttpResponseMessage Order(int ? id)
        {
            if (!product.OrderProduct(id)) { 
                var message = string.Format("Product with id = {0} not found", id);
                HttpError err = new HttpError(message);
                return Request.CreateResponse(HttpStatusCode.NotFound,err);
            }
            else{
                return Request.CreateResponse(HttpStatusCode.OK, "Product ordered");
            }
            
        }

        /// <summary>
        /// Finds products
        /// </summary>
        /// <param name="query">The search query</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Products/find")]
        public IEnumerable<MyProduct> FindProducts([FromUri] string query)
        {
            return product.FindProducts(query);
        }

        /// <summary>
        /// Get the urls of the images of some product
        /// </summary>
        /// <param name="id">The id of the product</param>
        /// <returns>List of image urls</returns>
        [HttpGet]
        [Route("api/Products/{id}/images")]
        public IEnumerable<string> Images(int ? id)
        {
            return product.GetAllImagesForProductRemote(id);
        }

    }
}