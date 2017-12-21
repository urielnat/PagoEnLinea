using System;
using System.Collections.Generic;

namespace PagoEnLinea.Modelos
{
    public class Detalle
    {
        public object id { get; set; }
        public int orden { get; set; }
        public double importe { get; set; }
        public string descripcion { get; set; }
        public int bimInicial { get; set; }
        public int bimFinal { get; set; }
        public string tipo { get; set; }
        public object liquidacionId { get; set; }
        public object liquidacionNumeroLiquidacion { get; set; }
        public int conceptoId { get; set; }
        public string conceptoDescripcion { get; set; }
        public string claveConcepto { get; set; }
    }

    public class Adeudo
    {
        public int bimInicial { get; set; }
        public int bimFinal { get; set; }
        public double impuesto { get; set; }
        public int fccznt { get; set; }
        public int feas { get; set; }
        public int ftzt { get; set; }
        public int recargo { get; set; }
        public int gastoEjecucion { get; set; }
        public int multa { get; set; }
        public int dap { get; set; }
        public string tipoAdeudo { get; set; }
        public int valorCatastral { get; set; }
        public double total { get; set; }
        public IList<Detalle> detalle { get; set; }
    }

    public class AdeudosDet
    {
        public object id { get; set; }
        public int orden { get; set; }
        public double importe { get; set; }
        public string descripcion { get; set; }
        public int bimInicial { get; set; }
        public int bimFinal { get; set; }
        public string tipo { get; set; }
        public object liquidacionId { get; set; }
        public object liquidacionNumeroLiquidacion { get; set; }
        public int conceptoId { get; set; }
        public string conceptoDescripcion { get; set; }
        public string claveConcepto { get; set; }
    }

    public class Predio
    {
        public string cveCatastral { get; set; }
        public string cveCatastralCorta { get; set; }
        public string cveCatastralAnt { get; set; }
        public string calle { get; set; }
        public object numeroInt { get; set; }
        public string numeroExt { get; set; }
        public string colonia { get; set; }
        public string tipoPredio { get; set; }
        public string zona { get; set; }
        public object claveZona { get; set; }
        public object usoPredio { get; set; }
        public object claveusoPredio { get; set; }
        public object factorIndiviso { get; set; }
        public object supTerrenoComun { get; set; }
        public double supTerrenoPriv { get; set; }
        public object supTerrenoEscrituras { get; set; }
        public double supTerrenoTotal { get; set; }
        public double valorTerreno { get; set; }
        public object supConstruccionComun { get; set; }
        public double supConstruccionPriv { get; set; }
        public double supConstruccionTotal { get; set; }
        public double valorConstruccion { get; set; }
        public double valorCatastral { get; set; }
        public object porcentajeDedicadoUsoHabitacion { get; set; }
        public string propietario { get; set; }
        public IList<object> coopropietarios { get; set; }
        public IList<Adeudo> adeudos { get; set; }
        public IList<AdeudosDet> adeudosDet { get; set; }
        public object causaDap { get; set; }
        public object fechaRegularizacionTenenciaTierra { get; set; }
        public object estadoPredio { get; set; }
    }

    public class Clave
    {
        public string bimFin { get; set; }
        public string bimIni { get; set; }
        public string cveCatastral { get; set; }
    }

    public class DesglosePredio{
        public int bimFin { get; set; }
        public int bimIni { get; set; }
        public double pago { get; set; }
        public int SinOrdenBiFin { get; set; }

    }

    public class HistorialPagos{
        public DateTime fechaPago { get; set; }
        public String clave { get; set; }
        public String bimestreInicial { get; set; }
        public String bimestreFinal { get; set; }
        public String caja { get; set; }
        public String folio { get; set; }
        public double total { get; set; }
        public String liquidacion { get; set; }
        public String liquidacionDesc { get; set; }
        public String recibo { get; set; }
        public String estatusRecibo { get; set; }


    }

    public class Pagos{

        public IList<HistorialPagos> pagos { get; set; }
    }


    public class MostrarHistorialPagos
    {
        public String fechaPago { get; set; }
        public String clave { get; set; }
        public String bimestreInicial { get; set; }
        public String bimestreFinal { get; set; }
        public String total { get; set; }
        public String liquidacion { get; set; }
        public String liquidacionDesc { get; set; }
        public String recibo { get; set; }
        public String estatusRecibo { get; set; }


    }

    public class Msg
    {
        public bool ok { get; set; }
        public string mensaje { get; set; }
        public object objeto { get; set; }
    }
}
