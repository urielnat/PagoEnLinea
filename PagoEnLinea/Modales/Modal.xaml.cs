using System;
using System.Collections.Generic;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace PagoEnLinea.Modales
{
    /// <summary>
    /// Esta subclase crea un cuadro de diálogo personalizado con las opciones de modifica, eliminar y cancelar
    /// este cuadro de dialogo es usado por la pantalla de correos elecrónicos en el módulo de perfil
    /// </summary>
    public partial class Modal
    {
      
        /// <summary>
        /// inicializa los componentes visuales correspondientes a su XAML
        /// añade un evento de tipo gesture a cada componente
        /// para detectar cuando el usuario presiona una opcián en el cuadro de diálogo
        /// </summary>
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


       
    }
}
