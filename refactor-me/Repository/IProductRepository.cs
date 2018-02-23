using refactor_me.Models;
using System;
using System.Collections.Generic;

namespace refactor_me.Repository
{
    public interface IProductRepository
    {
        bool ProductExists(Guid productId);

        IEnumerable<Product> GetAll();
        Product GetByID(Guid guid);
        IEnumerable<Product> GetProducts(string name);
        void Add(Product product);
        void Update(Product product);
        void DeleteProductWithOptions(Product product);
        bool Save();

    }
}
