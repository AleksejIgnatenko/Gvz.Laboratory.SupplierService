using Gvz.Laboratory.SupplierService.Abstractions;
using Gvz.Laboratory.SupplierService.Dto;
using Gvz.Laboratory.SupplierService.Entities;
using Gvz.Laboratory.SupplierService.Exceptions;
using Gvz.Laboratory.SupplierService.Models;
using Microsoft.EntityFrameworkCore;

namespace Gvz.Laboratory.SupplierService.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly GvzLaboratorySupplierServiceDbContext _context;
        private readonly ISupplierRepository _supplierRepository;

        public ProductRepository(GvzLaboratorySupplierServiceDbContext context, ISupplierRepository supplierRepository)
        {
            _context = context;
            _supplierRepository = supplierRepository;
        }

        public async Task<Guid> CreateProductAsync(ProductDto product)
        {
            var existingProduct = await _context.Products.FirstOrDefaultAsync(s => s.ProductName.Equals(product.ProductName));

            if (existingProduct == null)
            {
                var supplierEntity = await _context.Suppliers
                    .Where(p => product.SupplierIds.Contains(p.Id))
                    .ToListAsync();

                if (supplierEntity != null)
                {

                    var productEntity = new ProductEntity
                    {
                        Id = product.Id,
                        ProductName = product.ProductName,
                        Suppliers = supplierEntity,
                    };


                    await _context.Products.AddAsync(productEntity);
                    await _context.SaveChangesAsync();
                }
            }

            return product.Id;
        }

        public async Task<List<ProductModel>> GetSupplierProductsAsync(Guid supplierId)
        {
            var supplierEntity = await _context.Suppliers
                .Include(s => s.Products)
                .FirstOrDefaultAsync(s => s.Id == supplierId);

            if (supplierEntity == null)
            {
                throw new RepositoryException("Поставщик не найден");
            }

            var suppliers = supplierEntity.Products.Select(p => ProductModel.Create(p.Id, p.ProductName)).ToList();

            return suppliers;
        }

        public async Task<(List<ProductModel> products, int numberProducts)> GetSupplierProductsForPageAsync(Guid supplierId, int pageNumber)
        {
            var productEntities = await _context.Suppliers
                .Where(s => s.Id == supplierId)
                .SelectMany(p => p.Products)
                .Skip(pageNumber * 20)
                .Take(20)
                .ToListAsync();

            var numberProducts = await _context.Suppliers
                .Where(s => s.Id == supplierId)
                .SelectMany(p => p.Products)
                .CountAsync();

            var products = productEntities.Select(s => ProductModel.Create(s.Id, s.ProductName)).ToList();

            return (products, numberProducts);
        }

        public async Task<Guid> UpdateProductAsync(ProductDto product)
        {
            var supplierEntities = await _supplierRepository.GetSuppliersByIdsAsync(product.SupplierIds);

            var productEntity = await _context.Products
                .Include(p => p.Suppliers)
                .FirstOrDefaultAsync(p => p.Id == product.Id);

            if (productEntity != null)
            {
                productEntity.ProductName = product.ProductName;

                productEntity.Suppliers.Clear();
                productEntity.Suppliers.AddRange(supplierEntities);

                await _context.SaveChangesAsync();
            }

            return product.Id;
        }

        public async Task DeleteProductsAsync(List<Guid> ids)
        {
            var productEntities = await _context.Products
                .Include(p => p.Suppliers)
                .Where(s => ids.Contains(s.Id))
                .ToListAsync();

            foreach (var productEntity in productEntities)
            {
                productEntity.Suppliers.Clear();
            }

            await _context.SaveChangesAsync();

            await _context.Products
                .Where(s => ids.Contains(s.Id))
                .ExecuteDeleteAsync();
        }
    }
}
