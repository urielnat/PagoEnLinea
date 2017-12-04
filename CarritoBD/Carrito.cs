using System;
using SQLite;

namespace CarritoBD
{
    public class Carrito
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Name { get; set; }
        public string Notes { get; set; }
        public bool Done { get; set; }
        public int price { get; set; }
    }
}
