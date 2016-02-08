namespace Task.DB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Runtime.Serialization;

    [Serializable]
    public class Product : ISerializable
    {
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Product()
        {
            this.OrderDetails = new HashSet<OrderDetail>();
        }

        private Product(SerializationInfo info, StreamingContext context)
        {
            this.SetObjectData(info);
        }

        public int ProductID { get; set; }

        [Required]
        [StringLength(40)]
        public string ProductName { get; set; }

        public int? SupplierID { get; set; }

        public int? CategoryID { get; set; }

        [StringLength(20)]
        public string QuantityPerUnit { get; set; }

        [Column(TypeName = "money")]
        public decimal? UnitPrice { get; set; }

        public short? UnitsInStock { get; set; }

        public short? UnitsOnOrder { get; set; }

        public short? ReorderLevel { get; set; }

        public bool Discontinued { get; set; }

        [NonSerialized]
        private Category category;

        public virtual Category Category
        {
            get
            {
                return this.category;
            }

            set
            {
                this.category = value;
            }
        }

        [NonSerialized]
        private ICollection<OrderDetail> orderDetails;

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderDetail> OrderDetails
        {
            get
            {
                return this.orderDetails;
            }

            set
            {
                this.orderDetails = value;
            }
        }

        [NonSerialized]
        private Supplier supplier;

        public virtual Supplier Supplier
        {
            get
            {
                return this.supplier;
            }

            set
            {
                this.supplier = value;
            }
        }

        private void SetObjectData(SerializationInfo info)
        {
            this.ProductID = info.GetInt32(nameof(this.ProductID));
            this.ProductName = info.GetString(nameof(this.ProductName));
            this.SupplierID = (int?)info.GetValue(nameof(this.SupplierID), typeof(int?));
            this.CategoryID = (int?)info.GetValue(nameof(this.CategoryID), typeof(int?));
            this.QuantityPerUnit = info.GetString(nameof(this.QuantityPerUnit));
            this.UnitPrice = (decimal?)info.GetValue(nameof(this.UnitPrice), typeof(decimal?));
            this.UnitsInStock = (short?)info.GetValue(nameof(this.UnitsInStock), typeof(short?));
            this.ReorderLevel = (short?)info.GetValue(nameof(this.UnitsOnOrder), typeof(short?));
            this.Discontinued = info.GetBoolean(nameof(this.Discontinued));

            if (!info.GetBoolean(nameof(this.Category) + "IsNull"))
            {
                this.Category = new Category
                {
                    CategoryID = info.GetInt32(nameof(this.Category) + nameof(this.Category.CategoryID)),
                    CategoryName = info.GetString(nameof(this.Category) + nameof(this.Category.CategoryName)),
                    Description = info.GetString(nameof(this.Category) + nameof(this.Category.Description)),
                    Picture = (byte[])info.GetValue(nameof(this.Category) + nameof(this.Category.Picture), typeof(byte[]))
                };
            }

            var orderDetailsList = new List<OrderDetail>();

            for (var index = 0; index < info.GetInt32("orderDetailsCount"); index++)
            {
                var orderDetail = new OrderDetail
                {
                    OrderID = info.GetInt32($"{nameof(this.OrderDetails)}-{index}-{nameof(OrderDetail.OrderID)}"),
                    ProductID = info.GetInt32($"{nameof(this.OrderDetails)}-{index}-{nameof(OrderDetail.ProductID)}"),
                    UnitPrice = info.GetDecimal($"{nameof(this.OrderDetails)}-{index}-{nameof(OrderDetail.UnitPrice)}"),
                    Quantity = info.GetInt16($"{nameof(this.OrderDetails)}-{index}-{nameof(OrderDetail.Quantity)}"),
                    Discount = (float)info.GetValue($"{nameof(this.OrderDetails)}-{index}-{nameof(OrderDetail.Discount)}", typeof(float))
                };
                orderDetailsList.Add(orderDetail);
            }

            this.OrderDetails = orderDetailsList;

            if (!info.GetBoolean(nameof(this.Supplier) + "IsNull"))
            {
                this.Supplier = new Supplier
                {
                    SupplierID = info.GetInt32(nameof(this.Supplier) + nameof(this.Supplier.SupplierID)),
                    CompanyName = info.GetString(nameof(this.Supplier) + nameof(this.Supplier.CompanyName)),
                    ContactName = info.GetString(nameof(this.Supplier) + nameof(this.Supplier.ContactName)),
                    Address = info.GetString(nameof(this.Supplier) + nameof(this.Supplier.Address)),
                    City = info.GetString(nameof(this.Supplier) + nameof(this.Supplier.City)),
                    Region = info.GetString(nameof(this.Supplier) + nameof(this.Supplier.Region)),
                    PostalCode = info.GetString(nameof(this.Supplier) + nameof(this.Supplier.PostalCode)),
                    Country = info.GetString(nameof(this.Supplier) + nameof(this.Supplier.Country)),
                    Phone = info.GetString(nameof(this.Supplier) + nameof(this.Supplier.Phone)),
                    Fax = info.GetString(nameof(this.Supplier) + nameof(this.Supplier.Fax)),
                    HomePage = info.GetString(nameof(this.Supplier) + nameof(this.Supplier.HomePage))
                };
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(this.ProductID), this.ProductID);
            info.AddValue(nameof(this.ProductName), this.ProductName);
            info.AddValue(nameof(this.SupplierID), this.SupplierID);
            info.AddValue(nameof(this.CategoryID), this.CategoryID);
            info.AddValue(nameof(this.QuantityPerUnit), this.QuantityPerUnit);
            info.AddValue(nameof(this.UnitPrice), this.UnitPrice);
            info.AddValue(nameof(this.UnitsInStock), this.UnitsInStock);
            info.AddValue(nameof(this.UnitsOnOrder), this.UnitsOnOrder);
            info.AddValue(nameof(this.ReorderLevel), this.ReorderLevel);
            info.AddValue(nameof(this.Discontinued), this.Discontinued);

            if (this.Category == null)
            {
                info.AddValue(nameof(this.Category) + "IsNull", true);
            }
            else
            {
                info.AddValue(nameof(this.Category) + "IsNull", false);
                info.AddValue(nameof(this.Category) + nameof(this.Category.CategoryID), this.Category.CategoryID);
                info.AddValue(nameof(this.Category) + nameof(this.Category.CategoryName), this.Category.CategoryName);
                info.AddValue(nameof(this.Category) + nameof(this.Category.Description), this.Category.Description);
                info.AddValue(nameof(this.Category) + nameof(this.Category.Picture), this.Category.Picture);
            }

            var orderDetailsList = this.OrderDetails.ToList();
            info.AddValue("orderDetailsCount", orderDetailsList.Count);
            for (var index = 0; index < orderDetailsList.Count; index++)
            {
                var order = orderDetailsList[index];

                info.AddValue($"{nameof(this.OrderDetails)}-{index}-{nameof(order.OrderID)}", order.OrderID);
                info.AddValue($"{nameof(this.OrderDetails)}-{index}-{nameof(order.ProductID)}", order.ProductID);
                info.AddValue($"{nameof(this.OrderDetails)}-{index}-{nameof(order.UnitPrice)}", order.UnitPrice);
                info.AddValue($"{nameof(this.OrderDetails)}-{index}-{nameof(order.Quantity)}", order.Quantity);
                info.AddValue($"{nameof(this.OrderDetails)}-{index}-{nameof(order.Discount)}", order.Discount);
            }

            if (this.Supplier == null)
            {
                info.AddValue(nameof(this.Supplier) + "IsNull", true);
            }
            else
            {
                info.AddValue(nameof(this.Supplier) + "IsNull", false);
                info.AddValue(nameof(this.Supplier) + nameof(this.Supplier.SupplierID), this.Supplier.SupplierID);
                info.AddValue(nameof(this.Supplier) + nameof(this.Supplier.CompanyName), this.Supplier.CompanyName);
                info.AddValue(nameof(this.Supplier) + nameof(this.Supplier.ContactName), this.Supplier.ContactName);
                info.AddValue(nameof(this.Supplier) + nameof(this.Supplier.Address), this.Supplier.Address);
                info.AddValue(nameof(this.Supplier) + nameof(this.Supplier.City), this.Supplier.City);
                info.AddValue(nameof(this.Supplier) + nameof(this.Supplier.Region), this.Supplier.Region);
                info.AddValue(nameof(this.Supplier) + nameof(this.Supplier.PostalCode), this.Supplier.PostalCode);
                info.AddValue(nameof(this.Supplier) + nameof(this.Supplier.Country), this.Supplier.Country);
                info.AddValue(nameof(this.Supplier) + nameof(this.Supplier.Phone), this.Supplier.Phone);
                info.AddValue(nameof(this.Supplier) + nameof(this.Supplier.Fax), this.Supplier.Fax);
                info.AddValue(nameof(this.Supplier) + nameof(this.Supplier.HomePage), this.Supplier.HomePage);
            }
        }
    }
}