using System;
using SQLite;

namespace CarritoBD
{
    /// <summary>
    /// Modelo de tipo carrito con las siguientes propiedades. Importar SQLite es posible añadir el atributo
    /// PrimaryKey con la propiedad de auto increment para poder guardar cada item en la base de datos interna del dispositivo móvil
    /// </summary>
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
        public int idLiqs { get; set; }

    }
}
