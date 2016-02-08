namespace Task.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Order Details")]
    [Serializable]
    public class OrderDetail
    {

        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int OrderID { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ProductID { get; set; }

        [Column(TypeName = "money")]
        public decimal UnitPrice { get; set; }

        public short Quantity { get; set; }

        public float Discount { get; set; }

        [NonSerialized]
        private Order order;


        public virtual Order Order
        {
            get
            {
                return this.order;
            }

            set
            {
                this.order = value;
            }
        }

        [NonSerialized]
        private Product product;

        public virtual Product Product
        {
            get
            {
                return this.product;
            }

            set
            {
                this.product = value;
            }
        }
    }
}