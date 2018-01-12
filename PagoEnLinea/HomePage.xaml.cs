using System;
using System.Collections.Generic;
using PagoEnLinea.Modelos;
using PagoEnLinea.Paginas;
using PagoEnLinea.PaginasMisPagos;
using PagoEnLinea.PaginasPago;
using PagoEnLinea.servicios;
using Xamarin.Forms;

namespace PagoEnLinea
{
    public partial class HomePage : ContentPage
    {
        /// <summary>
        /// constructor de la clase inicializa todos los componentes visuales y añade un evento 
        /// de tipo gesture el cual permite detactar cuando una imagen es presionada
        /// las imagenes corresponden a los módulos de la aplicación principales y al precionarlos
        /// cambian a la pantalla del modulo correspondiente
        /// </summary>
        public HomePage()
        {
         
            InitializeComponent();
            var tgr = new TapGestureRecognizer();
            var tgr2 = new TapGestureRecognizer();
            var tgr3 = new TapGestureRecognizer();
            var tgrs = new TapGestureRecognizer();
            var tgr5 = new TapGestureRecognizer();
            tgr.Tapped += (sender, e) => Navigation.PushAsync(new TabPage());
            tgr2.Tapped += (sender, e) => Navigation.PushAsync(new LiquidacionPage());
            tgr3.Tapped += (sender, e) => Navigation.PushAsync(new PredialPage());
            tgrs.Tapped += (sender, e) => Navigation.PushAsync(new CarritoPage());
            tgr5.Tapped += (sender, e) => Navigation.PushAsync(new PrediosPagadosPage());

            perfil.GestureRecognizers.Add(tgr);
            liquidacion.GestureRecognizers.Add(tgr2);
            predio.GestureRecognizers.Add(tgr3);
            carrit.GestureRecognizers.Add(tgrs);
            historil.GestureRecognizers.Add(tgr5);
            itemsCarrito.GestureRecognizers.Add(tgrs);

        }

     
        /// <summary>
        /// Método asincrono que consume al servicio InfoContribuyente para obtener el nombre del usuario logueado
        /// a su vez muestra en el carrito los items que han sido añadidos al carrito al obtener la cantidad de la
        /// base de datos interna
        /// </summary>
        async void conectar(){
            if (Application.Current.Properties.ContainsKey("token"))
            {

                ClienteRest cliente = new ClienteRest();
                var inf = await cliente.InfoUsuario<InfoUsuario>(Application.Current.Properties["token"] as string);
                if (inf != null)
                {
                    Nombre.Text = "Bienvenido(a): "+inf.persona.nombre;
                }

            }

            ((App)Application.Current).ResumeAtTodoId = -1;
           var list = await App.Database.GetItemsAsync();

            itemsCarrito.Text = list.Count.ToString();

            }



           
        
        /// <summary>
        /// al aparecer esta pantalla hace una llamada el metodo conectar(); para obtener el nombre de usuario
        /// </summary>

        protected override void OnAppearing()
        {
            conectar();
        }

    }
}
