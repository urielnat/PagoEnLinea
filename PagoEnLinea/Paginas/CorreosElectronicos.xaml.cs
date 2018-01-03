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
    public partial class CorreosElectronicos : ContentPage
    {
        public static List<Email> list;
        public static string resp;
        public static Email item;


        public CorreosElectronicos()
        {
            
            InitializeComponent();
            listView.ItemTapped += ListView_ItemTapped;
           
        }

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
                    await Navigation.PushAsync(new ModificarCorreo((e.Item as Email).id, 0) { BindingContext = (Email)e.Item });
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


                                var uri = new Uri(string.Format(Constantes.URL + "/email/eliminar/{0}", info.id));
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
        protected override void OnAppearing()
        {
            conectar();
            MessagingCenter.Subscribe<Modal>(this, "modificar", (Sender) =>
            {   MessagingCenter.Unsubscribe<Modal>(this, "modificar");
                
                Navigation.PushAsync(new ModificarCorreo(item.id, 0) { BindingContext = item });
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


                    var uri = new Uri(string.Format(Constantes.URL + "/email/eliminar/{0}", item.id));
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

        async void Handle_Clicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new ModificarCorreo(null,1) );
        }



    }
}
