using ECommerce.Core.DataAccess.EntityFramework;
using ECommerce.DataAccess.Abstract;
using ECommerce.Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ECommerce.DataAccess.Concrete.EntityFramework
{
    public class EfProductDal : EfEntityRepositoryBase<Product, ECommerceContext>, IProductDal
    {
        public ICollection<Product> GetAll()
        {
            using (var context = new ECommerceContext())
            {
                var result = context.Products.Include(p => p.Brand).Include(p=>p.Category).ThenInclude(p=>p.Parent).ThenInclude(p=>p.Parent).Include(p=>p.ProductImages).ToList();
                return result;
            }
        }

        public Product Get(int ProdcutId)
        {
            using (var context = new ECommerceContext())
            {
                var result = context.Products.Include(p => p.Brand).Include(p => p.Category).ThenInclude(p => p.Parent).ThenInclude(p => p.Parent).Include(p => p.ProductImages).FirstOrDefault();
                return result;
            }
        }
    }
}
