using System.Collections.Generic;

namespace refactor_me.Dto
{
    /// <summary>
    /// ProductsDto class has a property which contains all the products
    /// </summary>
    public class ProductsDto
    {
        /// <summary>
        /// Items is a list of products
        /// </summary>
        public IEnumerable<ProductDto> Items { get; set; } = new List<ProductDto>();
    }
}
