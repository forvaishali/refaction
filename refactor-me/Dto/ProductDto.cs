using Newtonsoft.Json;
using refactor_me.Models;
using System.Collections.Generic;

namespace refactor_me.Dto
{
    /// <summary>
    /// Product DataTransferobject
    /// </summary>
    public class ProductDto
    {
        /// <summary>
        /// The productid
        /// </summary>
        public System.Guid Id { get; set; }

        /// <summary>
        /// The name of the product
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The description about the product
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The price of the product
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// The delivery price of the product
        /// </summary>
        public decimal DeliveryPrice { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonIgnore]
        public ICollection<ProductOption> ProductOptions { get; set; } = new List<ProductOption>();

    }

}
