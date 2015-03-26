using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GuitarShopper_admin.Models
{
    public class OrderForTable
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public double Amount { get; set; }
        public string State { get; set; } //verwerking,geannuleerd,afgehandeld
        public string DateAdded { get; set; }
    }

    public partial class order
    {
        public static List<OrderForTable> GetAllOrders()
        {
            DataClassesDataContext dc = new DataClassesDataContext();

            List<OrderForTable> oft = new List<OrderForTable>();

            List<order> orders = dc.orders.ToList();

            foreach (order order in orders)
            {
                double orderTotal = 0;

                IQueryable<product> productorders = (from ord in dc.orders
                                               join ohp in dc.orders_has_products on ord.id equals ohp.orders_id
                                               join prod in dc.products on ohp.products_id equals prod.id
                                               select prod);

                foreach (product p in productorders)
                {
                    orderTotal += Convert.ToDouble(p.price);
                }

                oft.Add(new OrderForTable { OrderId = order.id, CustomerId = order.customer_id, Amount = orderTotal, DateAdded = order.date_added.ToString(),State = order.state });

            }

            return oft;
        }

        public static bool ChangeOrderState(int? orderId, string state)
        {
            if (orderId == null) return false;
            if (!(state == "geannuleerd" || state == "verwerking" || state == "afgehandeld")) //kan properder I know
            {
                return false;
            }

            DataClassesDataContext dc = new DataClassesDataContext();

            order o = dc.orders.FirstOrDefault(ord => ord.id == orderId.Value);
            if (o == null) return false;

            o.state = state;

            dc.SubmitChanges();
            return true;
        }

        public static bool DeleteFromId(int? id)
        {
            if (id == null) return false;
            DataClassesDataContext dc = new DataClassesDataContext();
            order o = dc.orders.FirstOrDefault(ord => ord.id == id.Value);
            if (o == null) return false;

            IEnumerable<orders_has_product> ohp = dc.orders_has_products.Where(ohpr => ohpr.orders_id == id.Value);
            try
            {
                dc.orders_has_products.DeleteAllOnSubmit(ohp);
                dc.orders.DeleteOnSubmit(o);
                dc.SubmitChanges();
            }
            catch (Exception ex)
            {

                return false;
            }
            
            return true;
        }

    }
}