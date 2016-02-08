namespace Task.DB
{
    using System.Runtime.Serialization;

    public class OrderDetailsSerializationSurrogate : ISerializationSurrogate
    {
        public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
        {
            var orderDetail = (OrderDetail)obj;
            info.AddValue("OrderID", orderDetail.OrderID);
            info.AddValue("ProductID", orderDetail.ProductID);
            info.AddValue("UnitPrice", orderDetail.UnitPrice);
            info.AddValue("Quantity", orderDetail.Quantity);
            info.AddValue("Discount", orderDetail.Discount);
            info.AddValue("Order", orderDetail.Order);
            orderDetail.Product.OrderDetails = null;
            info.AddValue("Product", orderDetail.Product);
        }

        public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
        {
            var orderDetail = (OrderDetail)obj;
            orderDetail.OrderID = info.GetInt32("OrderID");
            orderDetail.ProductID = info.GetInt32("ProductID");
            orderDetail.UnitPrice = info.GetDecimal("UnitPrice");
            orderDetail.Quantity = info.GetInt16("Quantity");
            orderDetail.Discount = info.GetSingle("Discount");
            orderDetail.Order = (Order)info.GetValue("Order", typeof(Order));
            orderDetail.Product = (Product)info.GetValue("Product", typeof(Product));
            return orderDetail;
        }
    }
}