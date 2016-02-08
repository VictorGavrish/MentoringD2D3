namespace Task.DB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.Serialization;

    [DataContract(Name = "Order")]
    internal class OrderSurrogated
    {
        [DataMember]
        public int OrderID { get; set; }

        [StringLength(5)]
        [DataMember]
        public string CustomerID { get; set; }

        [DataMember]
        public int? EmployeeID { get; set; }

        [DataMember]
        public DateTime? OrderDate { get; set; }

        [DataMember]
        public DateTime? RequiredDate { get; set; }

        [DataMember]
        public DateTime? ShippedDate { get; set; }

        [DataMember]
        public int? ShipVia { get; set; }

        [DataMember]
        [Column(TypeName = "money")]
        public decimal? Freight { get; set; }

        [DataMember]
        [StringLength(40)]
        public string ShipName { get; set; }

        [DataMember]
        [StringLength(60)]
        public string ShipAddress { get; set; }

        [DataMember]
        [StringLength(15)]
        public string ShipCity { get; set; }

        [DataMember]
        [StringLength(15)]
        public string ShipRegion { get; set; }

        [DataMember]
        [StringLength(10)]
        public string ShipPostalCode { get; set; }

        [DataMember]
        [StringLength(15)]
        public string ShipCountry { get; set; }

        [IgnoreDataMember]
        public virtual Customer Customer { get; set; }

        [IgnoreDataMember]
        public virtual Employee Employee { get; set; }

        [IgnoreDataMember]
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }

        [IgnoreDataMember]
        public virtual Shipper Shipper { get; set; }
    }
}