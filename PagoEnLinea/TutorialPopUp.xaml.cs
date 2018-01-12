using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Rg.Plugins.Popup.Services;

namespace PagoEnLinea
{
    public partial class TutorialPopUp 
    {
        /// <summary>
        /// inicializa los componentes visuales de la pantalla, al ser de tipo popup no posee las mismas
        /// propiedades de una pagina común. El acomodo visual se encuentra en su archivo XAML
        /// 
        /// </summary>
        public TutorialPopUp()
        {
            InitializeComponent();
        }


        /// <summary>
        /// evento del boton "OK" que hace que esta subpantalla desapareza una vez que el usuario lo presiona
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        void Handle_Clicked(object sender, System.EventArgs e)
        {
            PopupNavigation.PopAsync();
        }
    }
}
