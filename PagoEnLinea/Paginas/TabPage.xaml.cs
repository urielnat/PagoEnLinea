using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace PagoEnLinea.Paginas
{
    public partial class TabPage : TabbedPage
    {
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
