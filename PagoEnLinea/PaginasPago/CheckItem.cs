using System;
namespace PagoEnLinea.PaginasPago
{
    public class CheckItem
    {
        
        public string Name { get; set; }
        public double Pago { get; set; }
        public int binIn { get; set; }
        public int binfin { get; set; }
        public int sinOrdenbimFin { get; set; }
        public string Details { get; set; }
    }
}
