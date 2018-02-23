using System.Collections.Generic;

namespace refactor_me.Dto
{
    /// <summary>
    /// ProductOptionsDto class has a property which contains all the ProductOptions
    /// </summary>
    public class ProductOptionsDto
    {
        /// <summary>
        /// Items is a list of productoptions
        /// </summary>
        public IEnumerable<ProductOptionDto> Items { get; set; } = new List<ProductOptionDto>();
    }
}
