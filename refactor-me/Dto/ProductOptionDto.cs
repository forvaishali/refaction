using System;

namespace refactor_me.Dto
{
    /// <summary>
    /// The productoption DataTransferobject
    /// </summary>
    public class ProductOptionDto
    {
        /// <summary>
        /// The productoptionid of the productoption
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The productid of the product associated with the product
        /// </summary>
        public Guid ProductId { get; set; }

        /// <summary>
        /// The name of the product
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The description about the product
        /// </summary>
        public string Description { get; set; }
    }
}