using Online_Shopping_Platform.Business.Operations.Product.Dtos;
using Online_Shopping_Platform.Business.Types;
using Online_Shopping_Platform.Data.Entities;
using Online_Shopping_Platform.Data.Repositories;
using Online_Shopping_Platform.Data.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Online_Shopping_Platform.Business.Operations.Product
{
    public class ProductManager : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;  // Unit of work for handling transactions
        private readonly IRepository<ProductEntity> _repository;  // Repository to interact with product entities in the database

        // Constructor to inject unit of work and repository dependencies
        public ProductManager(IUnitOfWork unitOfWork, IRepository<ProductEntity> repository)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
        }

        // Method to add a new product
        public async Task<ServiceMessage> AddProduct(AddProductDto product)
        {
            // Check if a product with the same name already exists in the database
            var hasProduct = _repository.GetAll(x => x.ProductName.ToLower() == product.ProductName.ToLower()).Any();

            if (hasProduct)
            {
                return new ServiceMessage
                {
                    IsSucceed = true,
                    Message = "Product already exists."  // Return message if product already exists
                };
            }

            // Create a new ProductEntity from the DTO
            var productEntity = new ProductEntity
            {
                ProductName = product.ProductName,
                Price = product.Price,
                StockQuantity = product.StockQuantity,  // Set price and stock quantity
            };

            _repository.Add(productEntity);  // Add the new product to the repository

            try
            {
                await _unitOfWork.SaveChangesAsync();  // Commit changes to the database
            }
            catch (Exception)
            {
                throw new Exception("An error occurred during the product registration.");  // Handle errors during save
            }

            return new ServiceMessage
            {
                IsSucceed = true  // Return success message after successful product addition
            };
        }
    }
}
