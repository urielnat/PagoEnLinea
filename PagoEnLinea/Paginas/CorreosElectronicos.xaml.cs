using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using PagoEnLinea.Modelos;
using PagoEnLinea.servicios;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;
using PagoEnLinea.Paginas;
using PagoEnLinea.Modales;

namespace PagoEnLinea.Paginas
{
    /// <summary>
    /// esta clase muestra una pantalla todos los correos electrónicos que el usuario ha dado de alta
    /// permitiendole la posibilidad de modificarlos y eliminarlos
    /// </summary>
    public partial class CorreosElectronicos : ContentPage
    {
        public static List<Email> list;
        public static string resp;
        public static Email item;

        /// <summary>
        /// inicializa los componenetes visuales correspondientes a su XAML y añade un evento de tipo item tapped a la lista
        /// que contiene todos los corres electrónicos del usuario.
        /// </summary>
        public CorreosElectronicos()
        {
            
            InitializeComponent();
            listView.ItemTapped += ListView_ItemTapped;
           
        }



        /// <summary>
        /// evento que permite seleccionar un correo de la lista y elegir la opcion de modificarlo o eliminarlo
        /// unicamente en android se hará uso del cuadro de diálogo personalizado ya que en iOS
        /// sus configuraciones nativas no lo requieren. Así mismo consume los servicios correspondientes.
        /// </summary>
        /// <param name="sender">objeto que hace referencia al evento</param>         /// <param name="e">argumentos que son posibles de obtener apartir del objeto que hace llamada al evento</param>
        async void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            item = (Email)e.Item;
            var info = (Email)e.Item;
          
            //   System.Diagnostics.Debug.WriteLine(ModalPopUp.respuesta);


            if (Device.RuntimePlatform == Device.Android)
            {   await PopupNavigation.PushAsync(new Modal());
            }

              
            
            if(Device.RuntimePlatform == Device.iOS) { 
            var action = await DisplayActionSheet("¿Qué desea hacer?", "Cancelar", "Eliminar", "Modificar");
            if (!string.IsNullOrEmpty(action))
            {
                if (action.Equals("Modificar"))
                {
                        if(Application.Current.Properties.ContainsKey("user")){
                             if(item.correoe.ToUpper().Equals((Application.Current.Properties["user"] as string).ToUpper())){
                               await DisplayAlert("Advertencia", "No puedes modificar tu email de inicio de sesión", "OK");
                            }else{
                                await Navigation.PushAsync(new ModificarCorreo((e.Item as Email).id, 0) { BindingContext = (Email)e.Item });
                            }
                        }  
                }
                if (action.Equals("Eliminar"))
                {
                    var respuesta = await DisplayAlert("Eliminar", "¿Esta seguro que desea eliminar este correo?", "Si", "Cancelar");

                    {
                        if (respuesta)
                        {

                            HttpResponseMessage response;


                            try
                            {
                                HttpClient cliente = new HttpClient();
                                //cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TOKEN);


                                var uri = new Uri(string.Format(Constantes.URL_USUARIOS + "/email/eliminar/{0}", info.id));
                                if (Application.Current.Properties.ContainsKey("token"))
                                {

                                    cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Application.Current.Properties["token"] as string);
                                }
                                response = await cliente.DeleteAsync(uri);
                                var y = await response.Content.ReadAsStringAsync();
                                System.Diagnostics.Debug.WriteLine(y);


                                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                                {

                                    System.Diagnostics.Debug.WriteLine("Se eliminó con exito");
                                    await DisplayAlert("Eliminado", "Correo eliminado con exito", "OK");
                                    conectar();

                                }

                                else
                                {
                                    System.Diagnostics.Debug.WriteLine(response);
                                    var resp = JsonConvert.DeserializeObject<Respuesta>(y);

                                    await DisplayAlert("Error", resp.respuesta, "OK");
                                    //await DisplayAlert("Error", "No fué posible intente mas tarde", "OK");

                                }
                            }
                            catch (HttpRequestException ex)
                            {
                                System.Diagnostics.Debug.WriteLine(ex.InnerException.Message);

                            }



                        }
                    }
                }
            }
        }
            ((ListView)sender).SelectedItem = null;

        }

       /// <summary>
       /// llama al servicio para mostrar todos los corres electrónicos ingresados por el usuario y añadirlos a una lista.
       /// </summary>
        async void conectar()
        {
            if (Application.Current.Properties.ContainsKey("token"))
            {


                ClienteRest cliente = new ClienteRest();
                var inf = await cliente.InfoUsuario<InfoUsuario>(Application.Current.Properties["token"] as string);
                list = new List<Email>();
                if (inf != null)
                {
                    foreach (var dato in inf.email)
                    {
                        //catdir.cp = dato.catalogoDir.cp;
                        list.Add(new Email
                        {
                            id=dato.id,
                            correoe = dato.correoe,
                            tipo = dato.tipo,



                        });
                    }


                    BindingContext = list;
                    listView.ItemsSource = list;

                }

            }
        }



        /// <summary>
        /// llama el método conectar que permite mostrar los correos electrónicos al desplegar la pantalla correos electróncios
        /// del módulo perfil, a su vez esta conectado a la subpantalla "Modal" la cual muestra un cuadro de dialogo el cual
        /// a partir de el resultado obtenido permite modificar o eliminar un elemento de la lista de correos
        /// (unicamente en Android)
        /// </summary>
        protected override void OnAppearing()
        {
            conectar();
            MessagingCenter.Subscribe<Modal>(this, "modificar", (Sender) =>
            {   MessagingCenter.Unsubscribe<Modal>(this, "modificar");


                if(Application.Current.Properties.ContainsKey("user")){
                    if(item.correoe.ToUpper().Equals((Application.Current.Properties["user"] as string).ToUpper())){
                                DisplayAlert("Advertencia", "No puedes modificar tu email de inicio de sesión", "OK");
                            }else{
                                Navigation.PushAsync(new ModificarCorreo(item.id, 0) { BindingContext = item });
                            }
                        }  
               
                MessagingCenter.Unsubscribe<Modal>(this, "modificar");

            });
            MessagingCenter.Subscribe<Modal>(this, "eliminar", async (Sender) =>
            {
                MessagingCenter.Unsubscribe<Modal>(this, "eliminar");


                HttpResponseMessage response;


                try
                {
                    HttpClient cliente = new HttpClient();
                    //cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TOKEN);


                    var uri = new Uri(string.Format(Constantes.URL_USUARIOS + "/email/eliminar/{0}", item.id));
                    if (Application.Current.Properties.ContainsKey("token"))
                    {

                        cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Application.Current.Properties["token"] as string);
                    }
                    response = await cliente.DeleteAsync(uri);
                    var y = await response.Content.ReadAsStringAsync();
                    System.Diagnostics.Debug.WriteLine(y);


                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {

                        System.Diagnostics.Debug.WriteLine("Se eliminó con exito");
                        await DisplayAlert("Eliminado", "Correo eliminado con exito", "OK");
                        conectar();

                    }

                    else
                    {
                        System.Diagnostics.Debug.WriteLine(response);
                        var respu = JsonConvert.DeserializeObject<Respuesta>(y);

                        await DisplayAlert("Error", respu.respuesta, "OK");
                        //await DisplayAlert("Error", "No fué posible intente mas tarde", "OK");

                    }
                }
                catch (HttpRequestException ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.InnerException.Message);

                }



                MessagingCenter.Unsubscribe<Modal>(this, "eliminar");



            });

           
        }

        /// <summary>
        /// Evento click al presionar el boton flotante, muestra al usuario la pantalla modificar correo
        /// pero al no recibir parámetros se toma como una pantalla para añadir un nuevo correo electrónico
        /// </summary>
        /// <param name="sender">objeto que hace refencia al evento</param>
        /// <param name="e">propiedades o argumentos del objeto que son accesibles a travez de la llamada al evento</param>
        async void Handle_Clicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new ModificarCorreo(null,1) );
        }



    }
}
