using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace PagoEnLinea.Paginas
{
    /// <summary>
    /// esta pagina contiene pantallas, se pudera ver como un contenedor
    /// con pestañas que muestran contenido al presionarlas
    /// </summary>
    public partial class TabPage : TabbedPage
    {
        /// <summary>
        /// añade las pantallas a la TabbedPage
        /// </summary>
        public TabPage()
        {
            InitializeComponent();
            Children.Add(new DatosPersonales());
            Children.Add(new Facturacion());
            Children.Add(new Direccion());
            Children.Add(new CorreosElectronicos());
            Children.Add(new Telefonos());
           
        }
    }
}
