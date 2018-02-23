using refactor_me.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace refactor_me.Repository
{
    public class ProductOptionRepository : IProductOptionRepository, IDisposable
    {
        private ProductContext db = new ProductContext();


        /// <summary>
        /// Adds a new product option to the specified product.
        /// </summary>
        /// <param name="productOption">productOption to be added to the specified product</param>
        /// <param name="productId">The productId of the product for which the productOption has to be added</param>
        public void AddProductOptionToProduct(ProductOption productOption, Guid productId)
        {
            db.ProductOptions.Add(productOption);
            // var product = db.Products.Where(p => p.Id == productId);
            // product..Add(productOption);
        }


        /// <summary>
        /// Gets all productoptions for a specified product
        /// </summary>
        /// <param name="productId">productId of the product</param>
        /// <returns>Returns all the productoptions for the specified product</returns>
        public IEnumerable<ProductOption> GetProductOptionsForProduct(Guid productId)
        {
            return db.ProductOptions.AsNoTracking()
                 .Where(p => p.ProductId == productId).ToList();

        }

        /// <summary>
        /// Gets the specified product option for the specified product.
        /// </summary>
        /// <param name="productId">The productId of the productoption to be retrieved from the database</param>
        /// <param name="productOptionId">The productOptionId of the prodctoption to be retrieved from the database</param>
        /// <returns>Returns the specified product option for the specified product</returns>
        public ProductOption GetProductOptionForProduct(Guid productId, Guid productOptionId)
        {
            return db.ProductOptions.AsNoTracking()
                .Where(p => p.ProductId == productId && p.Id == productOptionId).FirstOrDefault();
        }

        /// <summary>
        /// Updates the specified product option
        /// </summary>
        /// <param name="productOption">The product option to be updated</param>
        public void UpdateProductOption(ProductOption productOption)
        {
            db.Entry(productOption).State = System.Data.Entity.EntityState.Modified;
        }

        /// </summary>
        /// <param name="productOptionId">The productoptionId of the productoption</param>
        /// <returns>Returns true if the specified productOption exists otherwise returns false</returns>
        public bool ProductOptionExists(Guid productOptionId)
        {
            return db.ProductOptions.Any(p => p.Id == productOptionId);
        }

        /// <summary>
        /// Deletes the specified product option
        /// </summary>
        /// <param name="productOptionId">The productOptionId associated with the product option to be deleted</param>
        /// <param name="productId">The productId of the product option to be deleted</param>
        public void DeleteProductOption(Guid productOptionId, Guid productId)
        {
            var productOption = db.ProductOptions.Where(po => po.Id == productOptionId && po.ProductId == productId).SingleOrDefault();
            db.ProductOptions.Remove(productOption);
        }

        /// <summary>
        /// Save the changes to the database
        /// </summary>
        /// <returns>Returns true if the changes were updated successfully into the database</returns>
        public bool Save()
        {
            return (db.SaveChanges() >= 0);
        }

        /// <summary>
        /// Dispose the database context object
        /// </summary>
        /// <param name="disposing">Dispose the object if disposing parameter value is set to true</param>
        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (db != null)
                {
                    db.Dispose();
                    db = null;
                }
            }
        }

        /// <summary>
        /// Dispose the specified object and request common language runtime not to call the specified object
        /// </summary>
        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }
    }
}
