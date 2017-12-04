using System;
using SQLite;

namespace PagoEnLinea.Modelos
{
    public class Carrito
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public int price { get; set; }
        public string Name { get; set; }
        public string Notes { get; set; }
        public bool Done { get; set; }
    }
}
