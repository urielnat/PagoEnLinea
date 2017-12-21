using System;
using SQLite;

namespace CarritoBD
{
    public class Carrito
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Done { get; set; }
        public double price { get; set; }
        public string owner { get; set; }
        public string ClaveCastrasl { set; get; } 
        public int NoLiquidacion { get; set; }

    }
}
