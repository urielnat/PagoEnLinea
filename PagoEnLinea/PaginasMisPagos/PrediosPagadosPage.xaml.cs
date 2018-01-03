using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using PagoEnLinea.Modelos;
using PagoEnLinea.PaginasPago;
using PagoEnLinea.servicios;
using Xamarin.Forms;

namespace PagoEnLinea.PaginasMisPagos
{
    public partial class PrediosPagadosPage : ContentPage
    {
        public static Pagos pag;
        public static List<Respuesta> list;
        public PrediosPagadosPage()
        {
            InitializeComponent();




        }

        async void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var info = (Respuesta)e.Item;






            HttpResponseMessage response;



            try
            {


                HttpClient cliente = new HttpClient();
                cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Application.Current.Properties["token"] as string);
                //response = await cliente.PostAsync("http://192.168.0.18:8080/management/audits/logout", new StringContent("", Encoding.UTF8, ContentType));
                // response = await cliente.PostAsync("http://192.168.0.18:8081/api/liquidacion-predials/adeudos", new StringContent(jsonstring, Encoding.UTF8, ContentType));
                response = await cliente.GetAsync("http://192.168.0.18:8081/api/historico-pagos-movil/detalles/" + info.idPago);
                var y = await response.Content.ReadAsStringAsync();



                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {

                    System.Diagnostics.Debug.WriteLine(y);
                    DetallesHIstorial detalles = new DetallesHIstorial();
                    detalles = JsonConvert.DeserializeObject<DetallesHIstorial>(y);

                    string desgloce="";
                    string tipo;

                    foreach(var det in detalles.respuesta){
                        tipo = (det.llave.Length >= 13) ? "Clave catastral: " : "No de liquidacion: ";
                        desgloce += "Concepto: " + det.concepto + "\n" +
                                                    tipo + det.llave + "\n" +
                                                      "Importe: $" + det.importe +"\n"+"\n" +
                                                      "\n";



                    }



                    await DisplayAlert("Detalles", desgloce, "OK");

                    //results.Text = "(none)";
                }




                else
                {
                    System.Diagnostics.Debug.WriteLine(y);



                    await DisplayAlert("Error", "No encontrado", "OK");


                }
            }
            catch (HttpRequestException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.InnerException.Message);

            }










            ((ListView)sender).SelectedItem = null;



        }


            async void conectar()
            {
                if (Application.Current.Properties.ContainsKey("token"))
                {


                    ClienteRest cliente = new ClienteRest();
                    var inf = await cliente.GET<Historial>("http://192.168.0.18:8081/api/historico-pagos-movil/");
                    list = new List<Respuesta>();
                    if (inf != null)
                    {
                        foreach (var dato in inf.respuesta)
                        {
                            //catdir.cp = dato.catalogoDir.cp;
                            list.Add(new Respuesta
                            {

                                idPago = dato.idPago,
                                fecha = dato.fecha,
                                importe = dato.importe,
                                estatus = dato.estatus




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
           // Navigation.PushAsync(new ModificarTelefono(null, null, 1));
        }
    }
}
