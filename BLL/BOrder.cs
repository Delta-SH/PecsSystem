using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delta.PECS.WebCSC.Model;
using Delta.PECS.WebCSC.IDAL;
using Delta.PECS.WebCSC.DALFactory;

namespace Delta.PECS.WebCSC.BLL
{
    /// <summary>
    /// A business componet to get order
    /// </summary>
    public class BOrder
    {
        // Get an instance of the Order using the DALFactory
        private static readonly IOrder orderDal = DataAccess.CreateOrder();

        /// <summary>
        /// Method to add order information
        /// </summary>
        /// <param name="order">order</param>
        public int AddOrder(OrderInfo order) {
            try {
                return orderDal.AddOrders(new List<OrderInfo>() { order });
            } catch {
                throw;
            }
        }

        /// <summary>
        /// Method to add orders information
        /// </summary>
        /// <param name="orders">orders</param>
        public int AddOrders(List<OrderInfo> orders) {
            try {
                return orderDal.AddOrders(orders);
            } catch {
                throw;
            }
        }
    }
}