using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using PagoEnLinea.Modelos;
using PagoEnLinea.servicios;
using Xamarin.Forms;

namespace PagoEnLinea.Paginas
{
    public partial class Telefonos : ContentPage
    {
        public static List<Telefono> list;
        public Telefonos()
        {
         
            InitializeComponent();
            listView.ItemTapped+= ListView_ItemTapped;
          
        }

       async void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var info = (Telefono)e.Item;

            var action = await DisplayActionSheet("¿Qué desea hacer?", "Cancelar", "Eliminar", "Modificar");
            if (action.Equals("Modificar"))
            {
                await Navigation.PushAsync(new ModificarTelefono((e.Item as Telefono).id, (e.Item as Telefono).tipo, 0) { BindingContext = (Telefono)e.Item });
            }
            if (action.Equals("Eliminar"))
            {
                var respuesta = await DisplayAlert("Eliminar", "¿Esta seguro que desea eliminar este teléfono?", "Si", "Cancelar");
                {
                    if (respuesta)
                    {

                        HttpResponseMessage response;

                     
                        try
                        {
                            HttpClient cliente = new HttpClient();
                            //cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TOKEN);

                            if (Application.Current.Properties.ContainsKey("token"))
                            {

                                cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Application.Current.Properties["token"] as string);
                            }
                           
                            var uri = new Uri(string.Format(Constantes.URL + "/telefono/eliminar/{0}", info.id));
                            response = await cliente.DeleteAsync(uri);
                            var y = await response.Content.ReadAsStringAsync();
                            System.Diagnostics.Debug.WriteLine(y);


                            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                            {

                                System.Diagnostics.Debug.WriteLine("Se eliminó con exito");
                                await DisplayAlert("Eliminado", "Teléfono eliminado con exito", "OK");
                                conectar();

                            }

                            else
                            {
                                System.Diagnostics.Debug.WriteLine(response);
                                await DisplayAlert("Error", "No fué posible intente mas tarde", "OK");

                            }
                        }
                        catch (HttpRequestException ex)
                        {
                            System.Diagnostics.Debug.WriteLine(ex.InnerException.Message);

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
                list = new List<Telefono>();
                if (inf != null)
                {
                    foreach (var dato in inf.telefonos)
                    {
                        //catdir.cp = dato.catalogoDir.cp;
                        list.Add(new Telefono
                        {

                            id =dato.id,
                            telefono = dato.telefono,
                            lada = dato.lada,
                            tipo =dato.tipo



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
        }

        void Handle_Clicked(object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new ModificarTelefono(null,null,1));
        }
    }
}
