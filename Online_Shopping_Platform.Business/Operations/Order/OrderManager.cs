using Microsoft.EntityFrameworkCore;
using Online_Shopping_Platform.Business.Operations.Order.Dtos;
using Online_Shopping_Platform.Business.Types;
using Online_Shopping_Platform.Data.Entities;
using Online_Shopping_Platform.Data.Repositories;
using Online_Shopping_Platform.Data.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Online_Shopping_Platform.Business.Operations.Order
{
    public class OrderManager : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;  // Unit of work to handle transactions
        private readonly IRepository<OrderEntity> _orderRepository;  // Repository to interact with orders
        private readonly IRepository<OrderProductEntity> _orderProductRepository;  // Repository for order products

        // Constructor to inject unit of work and repositories
        public OrderManager(IUnitOfWork unitOfWork, IRepository<OrderEntity> orderRepository, IRepository<OrderProductEntity> orderProductRepository)
        {
            _unitOfWork = unitOfWork;
            _orderRepository = orderRepository;
            _orderProductRepository = orderProductRepository;
        }

        // Method to add a new order
        public async Task<ServiceMessage> AddOrder(AddOrderDto order)
        {
            // Check if the order already exists by ID
            var hasOrder = _orderRepository.GetAll(x => x.Id == order.Id).Any();
            if (hasOrder)
            {
                return new ServiceMessage
                {
                    IsSucceed = false,
                    Message = "Order already exists in the system."  // Return message if the order exists
                };
            }

            await _unitOfWork.BeginTransaction();  // Begin transaction for order creation

            var orderEntity = new OrderEntity
            {
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
            };

            _orderRepository.Add(orderEntity);  // Add the new order

            try
            {
                await _unitOfWork.SaveChangesAsync();  // Save order to database
            }
            catch (Exception)
            {
                throw new Exception("A problem was encountered during the order creation.");  // Handle error
            }

            // Add products to the order
            foreach (var productId in order.PorductIds)
            {
                var orderProduct = new OrderProductEntity
                {
                    OrderId = orderEntity.Id,
                    ProductId = productId,
                };

                _orderProductRepository.Add(orderProduct);  // Add order-product relation
            }

            try
            {
                await _unitOfWork.SaveChangesAsync();  // Commit changes to database
                await _unitOfWork.CommitTransaction();  // Commit transaction
            }
            catch (Exception)
            {
                await _unitOfWork.RollBackTransaction();  // Rollback transaction in case of error
                throw new Exception("An error was encountered while ordering products, and the process has been reset.");  // Handle error
            }

            return new ServiceMessage
            {
                IsSucceed = true  // Return success message
            };
        }

        // Method to adjust the total amount of an order
        public async Task<ServiceMessage> AdjustTotalAmount(int id, int changeTo)
        {
            var order = _orderRepository.GetById(id);  // Find the order by ID
            if (order is null)
            {
                return new ServiceMessage
                {
                    IsSucceed = false,
                    Message = "No order found matching this ID."  // Return error if order not found
                };
            }

            order.TotalAmount = changeTo;  // Update the total amount

            _orderRepository.Update(order);  // Update order in the repository

            try
            {
                await _unitOfWork.SaveChangesAsync();  // Save changes to database
            }
            catch (Exception)
            {
                throw new Exception("An error occurred while changing the total amount.");  // Handle error
            }

            return new ServiceMessage
            {
                IsSucceed = true  // Return success message
            };
        }

        // Method to delete an order
        public async Task<ServiceMessage> DeleteOrder(int id)
        {
            var order = _orderRepository.GetById(id);  // Find the order by ID
            if (order is null)
            {
                return new ServiceMessage
                {
                    IsSucceed = false,
                    Message = "The order to be deleted could not be found."  // Return error if order not found
                };
            }

            _orderRepository.Delete(id);  // Delete order from the repository

            try
            {
                await _unitOfWork.SaveChangesAsync();  // Save changes to database
            }
            catch (Exception)
            {
                throw new Exception("An error occurred during the deletion process.");  // Handle error
            }

            return new ServiceMessage
            {
                IsSucceed = true  // Return success message
            };
        }

        // Method to get details of a specific order
        public async Task<OrderDto> GetOrder(int id)
        {
            var order = await _orderRepository.GetAll(x => x.Id == id)  // Fetch order by ID
                .Select(x => new OrderDto
                {
                    TotalAmount = x.TotalAmount,
                    Id = x.Id,
                    Products = x.OrderProducts.Select(p => new OrderProductDto  // Select product details for the order
                    {
                        ProductId = p.ProductId,
                        ProductName = p.Product.ProductName,
                        Id = p.Id
                    }).ToList()
                }).FirstOrDefaultAsync();

            return order;  // Return order DTO
        }

        // Method to get all orders
        public async Task<List<OrderDto>> GetOrders()
        {
            var orders = await _orderRepository.GetAll()  // Fetch all orders
                .Select(x => new OrderDto
                {
                    TotalAmount = x.TotalAmount,
                    Id = x.Id,
                    Products = x.OrderProducts.Select(p => new OrderProductDto  // Select product details for each order
                    {
                        ProductId = p.ProductId,
                        ProductName = p.Product.ProductName,
                        Id = p.Id
                    }).ToList()
                }).ToListAsync();

            return orders;  // Return list of orders
        }

        // Method to update an existing order
        public async Task<ServiceMessage> UpdateOrder(UpdateOrderDto order)
        {
            var orderEntity = _orderRepository.GetById(order.Id);  // Find the order by ID
            if (orderEntity is null)
            {
                return new ServiceMessage
                {
                    IsSucceed = false,
                    Message = "Order not found."  // Return error if order not found
                };
            }

            await _unitOfWork.BeginTransaction();  // Begin transaction for order update

            orderEntity.OrderDate = order.OrderDate;  // Update order date
            orderEntity.TotalAmount = order.TotalAmount;  // Update total amount

            _orderRepository.Update(orderEntity);  // Update order entity in the repository

            try
            {
                await _unitOfWork.SaveChangesAsync();  // Save changes to database
            }
            catch (Exception)
            {
                await _unitOfWork.RollBackTransaction();  // Rollback transaction in case of error
                throw new Exception("An error was encountered while updating the order information.");  // Handle error
            }

            // Delete existing products associated with the order
            var orderProducts = _orderProductRepository.GetAll(x => x.OrderId == order.Id).ToList();
            foreach (var orderProduct in orderProducts)
            {
                _orderProductRepository.Delete(orderProduct, false);  // Hard delete existing order-product relations
            }

            // Add new products to the order
            foreach (var productId in order.PorductIds)
            {
                var orderProduct = new OrderProductEntity
                {
                    OrderId = orderEntity.Id,
                    ProductId = productId,
                };

                _orderProductRepository.Add(orderProduct);  // Add new order-product relation
            }

            try
            {
                await _unitOfWork.SaveChangesAsync();  // Save changes to database
                await _unitOfWork.CommitTransaction();  // Commit transaction
            }
            catch (Exception)
            {
                await _unitOfWork.RollBackTransaction();  // Rollback transaction in case of error
                throw new Exception("An error occurred while updating the order. The operations are being rolled back.");  // Handle error
            }

            return new ServiceMessage
            {
                IsSucceed = true,  // Return success message
            };
        }
    }

}
