using System;
namespace PagoEnLinea.PaginasPago
{
    /// <summary>
    /// modelo de tipo CheckItem, contiene información acerca de una fecha especifica a pagar
    /// cuando se desgloza un predial
    /// </summary>
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
