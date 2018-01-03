using System;
using System.Collections.Generic;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace PagoEnLinea.Modales
{
    public partial class Modal
    {
        public static string respuesta;
        public Modal()
        {
            InitializeComponent();
            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += (s, e) =>
            {
                if (s == modificar)
                {
                    MessagingCenter.Send(this, "modificar");
                    PopupNavigation.PopAsync();
                }
                else if (s == eliminar)
                {
                    MessagingCenter.Send(this, "eliminar");
                    PopupNavigation.PopAsync();
                }
                else
                {
                    PopupNavigation.PopAsync();
                }
            };
            modificar.GestureRecognizers.Add(tapGestureRecognizer);
            eliminar.GestureRecognizers.Add(tapGestureRecognizer);
            cancelar.GestureRecognizers.Add(tapGestureRecognizer);


        }


        public string Respuesta()
        {
            return respuesta;
        }
    }
}
