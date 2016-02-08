namespace Task.DB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Threading;
    using System.Xml.Serialization;

    [Serializable]
    public class Category
    {
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Category()
        {
            this.Products = new HashSet<Product>();
        }

        public int CategoryID { get; set; }

        [Required]
        [StringLength(15)]
        public string CategoryName { get; set; }

        [Column(TypeName = "ntext")]
        public string Description { get; set; }

        [Column(TypeName = "image")]
        public byte[] Picture { get; set; }

        [NonSerialized]
        private ICollection<Product> products;

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Product> Products
        {
            get
            {
                return this.products;
            }
            set
            {
                this.products = value;
            }
        }

        [OnSerializing]
        public void OnSerializing(StreamingContext context)
        {
            ThreadSetDataPair(nameof(this.CategoryID), this.CategoryID);
            ThreadSetDataPair(nameof(this.CategoryName), this.CategoryName);
            ThreadSetDataPair(nameof(this.Description), this.Description);
            ThreadSetDataPair(nameof(this.Picture), this.Picture);

            var productList = this.Products.ToList();
            ThreadSetDataPair("product-count", productList.Count);
            for (var index = 0; index < productList.Count; index++)
            {
                var product = productList[index];
                ThreadSetDataPair($"product{index}-{nameof(Product.ProductID)}", product.ProductID);
                ThreadSetDataPair($"product{index}-{nameof(Product.ProductName)}", product.ProductName);
                ThreadSetDataPair($"product{index}-{nameof(Product.SupplierID)}", product.SupplierID);
                ThreadSetDataPair($"product{index}-{nameof(Product.CategoryID)}", product.CategoryID);
                ThreadSetDataPair($"product{index}-{nameof(Product.QuantityPerUnit)}", product.QuantityPerUnit);
                ThreadSetDataPair($"product{index}-{nameof(Product.UnitPrice)}", product.UnitPrice);
                ThreadSetDataPair($"product{index}-{nameof(Product.UnitsInStock)}", product.UnitsInStock);
                ThreadSetDataPair($"product{index}-{nameof(Product.UnitsOnOrder)}", product.UnitsOnOrder);
                ThreadSetDataPair($"product{index}-{nameof(Product.Discontinued)}", product.Discontinued);
            }
        }

        private static void ThreadSetDataPair(string slot, object data)
        {
            var dataSlot = Thread.AllocateNamedDataSlot(slot);
            Thread.SetData(dataSlot, data);
        }

        [OnDeserializing]
        public void OnDeserializing(StreamingContext context)
        {
            this.CategoryID = ThreadGetData<int>(nameof(this.CategoryID));
            this.CategoryName = ThreadGetData<string>(nameof(this.CategoryName));
            this.Description = ThreadGetData<string>(nameof(this.Description));
            this.Picture = ThreadGetData<byte[]>(nameof(this.Picture));

            var productList = new List<Product>();
            var productCount = ThreadGetData<int>("product-count");
            for (var index = 0; index < productCount; index++)
            {
                var product = new Product
                {
                    ProductID = ThreadGetData<int>($"product{index}-{nameof(Product.ProductID)}"), 
                    ProductName = ThreadGetData<string>($"product{index}-{nameof(Product.ProductName)}"), 
                    SupplierID = ThreadGetData<int?>($"product{index}-{nameof(Product.SupplierID)}"), 
                    CategoryID = ThreadGetData<int?>($"product{index}-{nameof(Product.CategoryID)}"), 
                    QuantityPerUnit = ThreadGetData<string>($"product{index}-{nameof(Product.QuantityPerUnit)}"), 
                    UnitPrice = ThreadGetData<decimal?>($"product{index}-{nameof(Product.UnitPrice)}"), 
                    UnitsInStock = ThreadGetData<short?>($"product{index}-{nameof(Product.UnitsInStock)}"), 
                    UnitsOnOrder = ThreadGetData<short?>($"product{index}-{nameof(Product.UnitsOnOrder)}"), 
                    Discontinued = ThreadGetData<bool>($"product{index}-{nameof(Product.Discontinued)}")
                };
                productList.Add(product);
            }

            this.Products = productList;
        }

        private static T ThreadGetData<T>(string slot)
        {
            return (T)Thread.GetData(Thread.GetNamedDataSlot(slot));
        }
    }
}