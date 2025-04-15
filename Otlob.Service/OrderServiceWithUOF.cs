using Otlob.Core.Interfaces;
using Otlob.Core.Models;
using Otlob.Core.Order_Aggregate;
using Otlob.Core.Repositories;
using Otlob.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Address = Otlob.Core.Order_Aggregate.Address;

namespace Otlob.Service
{
    public class OrderServiceWithUOF : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBasketRepository _basketRepository;

        public OrderServiceWithUOF(IUnitOfWork unitOfWork, IBasketRepository basketRepository)
        {
            _unitOfWork = unitOfWork;
            _basketRepository = basketRepository;
        }

        public async Task<Order?> CreateOrderAsync(string buyerEmail, string basketId, int deliveryMethodId, Address shippingAddress)
        {
            #region 1.Get Basket From Basket Repository
            var basket = await _basketRepository.GetBasketAsync(basketId);
            #endregion

            #region 2.Get Selected Items at Basket From Product Repository
            var orderItems = new List<OrderItem>();

            if (basket?.Items.Count > 0)
            {
                foreach (var item in basket.Items)
                {
                    var product = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);

                    var productItemOrdered = new ProductItemOrdered(product.Id, product.Name, product.PictureUrl);

                    var orderItem = new OrderItem(productItemOrdered, item.Quantity, product.Price);

                    orderItems.Add(orderItem);
                }
            }
            #endregion

            #region 3.Calculate SubTotal
            var subTotal = orderItems.Sum(orderItem => orderItem.Price * orderItem.Quantity);
            #endregion

            #region 4.Get Delivery Method From DeliveryMethod Repository
            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(deliveryMethodId);
            #endregion

            #region 5.Create Order
            var order = new Order(buyerEmail, OrderStatus.Pending, shippingAddress, deliveryMethod, orderItems, subTotal);
            #endregion

            #region 6.Add Order Locally
            await _unitOfWork.Repository<Order>().AddAsync(order);
            #endregion

            #region 7.Save Order To Database
            var result = await _unitOfWork.CompleteAsync();

            return (result <= 0) ? null : order;
            #endregion
        }

        public Task<Order> GetOrderByIdAsync(string buyerEmail, int orderId)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
        {
            throw new NotImplementedException();
        }
    }
}
