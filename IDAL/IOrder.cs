using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delta.PECS.WebCSC.Model;

namespace Delta.PECS.WebCSC.IDAL
{
    /// <summary>
    /// Interface for order
    /// </summary>
    public interface IOrder
    {
        /// <summary>
        /// Method to add orders information
        /// </summary>
        /// <param name="orders">orders</param>
        int AddOrders(List<OrderInfo> orders);
    }
}