using refactor_me.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace refactor_me.Repository
{
    /// <summary>
    /// The productrepository
    /// </summary>
    public class ProductRepository : IProductRepository, IDisposable
    {
        /// <summary>
        /// The database context used to connect to the database and perform operations against database
        /// </summary>
        private ProductContext db = new ProductContext();

        /// <summary>
        /// Add the product
        /// </summary>
        /// <param name="product">The product to be added</param>
        public void Add(Product product)
        {
            db.Products.Add(product);
        }

        /// <summary>
        /// Update the product
        /// </summary>
        /// <param name="product">The product to be updated</param>
        public void Update(Product product)
        {
            db.Entry(product).State = System.Data.Entity.EntityState.Modified;

        }

        /// <summary>
        /// Delete the product along with the productoptions
        /// </summary>
        /// <param name="product">The product to be deleted along with its associated productoptions</param>
        public void DeleteProductWithOptions(Product product)
        {
            var productOptions = db.ProductOptions.Where(p => p.ProductId == product.Id);
            db.ProductOptions.RemoveRange(productOptions);
            db.Entry(product).State = System.Data.Entity.EntityState.Deleted;
        }

        /// <summary>
        /// Check if the product exists in the database
        /// </summary>
        /// <param name="productId"></param>
        /// <returns>Returns true if the product exists in the database otherwise returns false</returns>
        public bool ProductExists(Guid productId)
        {
            return db.Products.Any(p => p.Id == productId);
        }

        /// <summary>
        /// Gets all the products
        /// </summary>
        /// <returns>Returns all the products</returns>
        public IEnumerable<Product> GetAll()
        {
            return db.Products.AsNoTracking().ToList();
        }

        /// <summary>
        /// Get the product by productid
        /// </summary>
        /// <param name="guid">The productid</param>
        /// <returns>Returns the product with the specified productid</returns>
        public Product GetByID(Guid guid)
        {
            return db.Products.Find(guid);
        }

        /// <summary>
        /// Get the products with the specified name
        /// </summary>
        /// <param name="name">The name of the profuct</param>
        /// <returns>Returns the products matching the specified product name</returns>
        public IEnumerable<Product> GetProducts(string name)
        {
            return db.Products.AsNoTracking().Where(p => p.Name == name).ToList();//.Include("ProductOptions");
        }

        /// <summary>
        /// Update the specified product
        /// </summary>
        /// <param name="product">The product to be updated</param>
        public void UpdateProduct(Product product)
        {
            //return 0;
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
