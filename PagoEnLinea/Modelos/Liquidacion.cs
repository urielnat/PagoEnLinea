using System;
using System.Collections.Generic;

namespace PagoEnLinea.Modelos
{

    /// <summary>
    /// modelo de liquidaciones, empleado para manipular los JSON al consumir servicios
    /// </summary>

    public class Datos{
        public IList<Liquidacion> data { set; get; }

    }


    public class LiquidacionDesConcepto
    {
        public int id { get; set; }
        public int orden { get; set; }
        public double importe { get; set; }
        public int liquidacionId { get; set; }
        public string liquidacionNumeroLiquidacion { get; set; }
        public int conceptoId { get; set; }
        public string conceptoDescripcion { get; set; }
        public string claveConcepto { get; set; }
    }

    public class DesgloceConceptoDet
    {
        public int id { get; set; }
        public int orden { get; set; }
        public double importe { get; set; }
        public string descripcion { get; set; }
        public int bimInicial { get; set; }
        public int bimFinal { get; set; }
        public string tipo { get; set; }
        public int liquidacionId { get; set; }
        public string liquidacionNumeroLiquidacion { get; set; }
        public int conceptoId { get; set; }
        public object conceptoDescripcion { get; set; }
        public object claveConcepto { get; set; }
    }

    public class LiquidacionPredial
    {
        public int id { get; set; }
        public string clavecatastral { get; set; }
        public string clavecatastralant { get; set; }
        public double valorcatastral { get; set; }
        public int binInicial { get; set; }
        public int bimFinal { get; set; }
        public double supTerreno { get; set; }
        public double supConstruccion { get; set; }
        public string domicilio { get; set; }
        public string colonia { get; set; }
        public string tipoPredio { get; set; }
        public string concepto { get; set; }
        public string propietarios { get; set; }
        public string rfc { get; set; }
        public int liquidacionId { get; set; }
        public string liquidacionNumeroLiquidacion { get; set; }
    }


    public class Liquidacion
    {
        public int id { get; set; }
        public int numeroLiquidacion { get; set; }
        public string fechaCreacion { get; set; }
        public string vigencia { get; set; }
        public string referencia { get; set; }
        public string tipoLiquidacion { get; set; }
        public string concepto { get; set; }
        public double total { get; set; }
        public string totalLetra { get; set; }
        public double redondeo { get; set; }
        public string estatus { get; set; }
        public int catEmisorId { get; set; }
        public object catEmisorDescripcion { get; set; }
        public IList<LiquidacionDesConcepto> liquidacionDesConcepto { get; set; }
        public IList<DesgloceConceptoDet> desgloceConceptoDets { get; set; }
        public IList<object> desgloceDescripciones { get; set; }
        public LiquidacionPredial liquidacionPredial { get; set; }
        public int grupoId { get; set; }
        public object grupoDescripcion { get; set; }
    }

    public class GenerarLiquidacion
    {
        public int bimFin { get; set; }
        public int bimIni { get; set; }
        public string cveCatastral { get; set; }
        public bool esAgrupada = true;
        public bool esPagoEnLinea = true;
        public bool guardaLiquidacion = true;
    }

    public class Pago
    {
        public string auth { get; set; }
        public IList<int> idLiqs { get; set; }
    }


}
