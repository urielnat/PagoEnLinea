using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using PagoEnLinea.Modelos;
using PagoEnLinea.servicios;
using Xamarin.Forms;

namespace PagoEnLinea.Paginas
{
    public partial class Direccion : ContentPage
    {
        public static List<Modelos.infodir> list;
        public infodir infdir { set; get; }
        public CatalogoDir catdir { set; get; }
        public Direccion()
        {
          
            InitializeComponent();
            listView.ItemTapped+= ListView_ItemTapped;
        }

      async void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var info = (infodir)e.Item;

            var action = await DisplayActionSheet("¿Qué desea hacer?", "Cancelar", "Eliminar", "Modificar");
            if (action.Equals("Modificar"))
            {
                await Navigation.PushAsync(new ModificarDireccion(info.id, info.idCat, 0, info.estado, info.tipoasentamiento) { BindingContext = (infodir)e.Item });
            }
            if(action.Equals("Eliminar")){
                var respuesta = await DisplayAlert("Eliminar", "¿Esta seguro que desea eliminar esta dirección?", "Si", "Cancelar");{
                    if(respuesta){



                    HttpResponseMessage response;

                     
                        try
                        {
                            HttpClient cliente = new HttpClient();
                            //cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TOKEN);
                            if (Application.Current.Properties.ContainsKey("token"))
                            {

                                cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Application.Current.Properties["token"] as string);
                            }

                            var uri = new Uri(string.Format(Constantes.URL+"/direccion/eliminar/{0}", info.id));
                            response = await cliente.DeleteAsync(uri);
                            var y = await response.Content.ReadAsStringAsync();
                            System.Diagnostics.Debug.WriteLine(y);


                            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                            {

                                System.Diagnostics.Debug.WriteLine("Se eliminó con exito");
                                await  DisplayAlert("Eliminado","Dirección eliminada con exito","OK");
                                conectar();
                            }

                            else
                            {
                                System.Diagnostics.Debug.WriteLine(response);
                                var resp = JsonConvert.DeserializeObject<Respuesta>(y);

                                await DisplayAlert("Error", resp.respuesta, "OK");

                            }
                        }
                        catch (HttpRequestException ex)
                        {
                            System.Diagnostics.Debug.WriteLine(ex.InnerException.Message);
                            await DisplayAlert("Error", "No fué posible intente mas tarde", "OK");
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
                var cont = 1;

                ClienteRest cliente = new ClienteRest();
                var inf = await cliente.InfoUsuario<InfoUsuario>(Application.Current.Properties["token"] as string);
                list = new List<Modelos.infodir>();

                if (inf != null)
                {
                    foreach (var dato in inf.direcciones){
                       
                        list.Add(new Modelos.infodir
                        {
                            NumerodeDireccion = "Dirección " + cont+":",
                            id = dato.id,
                            calle = dato.calle,
                            numero = dato.numero,
                            numeroInterior = dato.numeroInterior,
                            tipo = dato.tipo,

                            cp = dato.catalogoDir.cp,
                            asentamiento = dato.catalogoDir.asentamiento,
                            municipio = dato.catalogoDir.municipio,
                            estado = dato.catalogoDir.estado,
                            pais = dato.catalogoDir.pais,
                            tipoasentamiento = dato.catalogoDir.tipoasentamiento,
                            ciudad = dato.catalogoDir.ciudad,
                            idCat = dato.catalogoDir.id
                               



                        });
                        cont++;
                        System.Diagnostics.Debug.WriteLine(dato.catalogoDir.municipio);
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
            Navigation.PushAsync(new ModificarDireccion(null, null, 1,null,null));
        }
    }
}
