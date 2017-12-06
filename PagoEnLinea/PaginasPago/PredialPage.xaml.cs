using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using CarritoBD;
using Newtonsoft.Json;
using PagoEnLinea.Modelos;
using Xamarin.Forms;
using System.Linq;

namespace PagoEnLinea.PaginasPago
{
    public partial class PredialPage : ContentPage
    {
        public static List<Carrito> list;
        public static List<DesglosePredio> despre;
        public static Predio pred;
        public PredialPage()
        {
            list = new List<Carrito>();
            InitializeComponent();

       
            listView.ItemTapped += OnitemTapped;
        }
        DesglosePredios<CheckItem> multiPage;
        async void OnitemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null) return;
            // do something with e.SelectedItem
            var resp = await DisplayAlert("Predio", "¿Qué desea hacer?", "Agregar al carrito", "ver detalles");
            if (!resp)
            {

                var items = new List<CheckItem>();

                foreach(var item in despre){
                    items.Add(new CheckItem
                    {
                        Name = item.bimIni + "-" + item.bimFin + "  $" + item.pago,
                        Pago = item.pago
                    });
                }

               

                if (multiPage == null)
                    multiPage = new DesglosePredios<CheckItem>(items) { Title = "Seleccione el pago" };

                await Navigation.PushAsync(multiPage);
             
            }
            else
            {
                var todoItem = (Carrito)e.Item;
                await App.Database.SaveItemAsync(todoItem);
                await DisplayAlert("Añadido", "Se añadio al carrito", "OK");
                listView.ItemsSource = null;

            }

     ((ListView)sender).SelectedItem = null; // de-select
        }


        void conectar()
        {






            if (multiPage != null)
            {
                double valores = 0;
                var answers = multiPage.GetSelection();
                foreach (var a in answers)
                {
                    valores += a.Pago;
                }
                if (answers.Count > 0)
                {
                    list = new List<Carrito>{new Carrito
                        {
                                    Name = "Predio:" +pred.cveCatastral, Description = "ejemplo", price = valores

                        }};
                    BindingContext = list;
                    listView.ItemsSource = list;

                }
                else
                {
                    
                }

            }
            else
            {
            }










        }
        protected override void OnAppearing()
        {
            conectar();

        }

        async void Handle_SearchButtonPressed(object sender, System.EventArgs e)
        {
            //throw new NotImplementedException();


            HttpResponseMessage response;

            string ContentType = "application/json"; // or application/xml

            Clave clv = new Clave();

            clv.cveCatastral = buscar.Text;


            var jsonstring = JsonConvert.SerializeObject(clv);

            try
            {


                HttpClient cliente = new HttpClient();
                cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "eyJhbGciOiJIUzUxMiJ9.eyJzdWIiOiJhZG1pbiIsImF1dGgiOiJDQUpBX0NBSkVSTyxST0xFX0FETUlOLFJPTEVfVVNFUiIsInBzdG8iOiJbXSIsIm5tIjoiSVNBQUMiLCJhcDEiOiJCQVVUSVNUQSIsImFwMiI6IkNBTUlOTyIsImV4cCI6MTUxMjU3Njg3NX0.44Hcocf_Bawf3ducLYWItimyAXrEk2FgIB0ifpAY6LYvnZt8kwI9j9KEue5x3F2V4XBc_emliLOxFYFze-yZHA");
                //response = await cliente.PostAsync("http://192.168.0.18:8080/management/audits/logout", new StringContent("", Encoding.UTF8, ContentType));
                response = await cliente.PostAsync("http://192.168.0.18:8081/api/liquidacion-predials/adeudos", new StringContent(jsonstring, Encoding.UTF8, ContentType));
                var y = await response.Content.ReadAsStringAsync();



                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {

                    System.Diagnostics.Debug.WriteLine(y);
                    pred = new Predio();
                    pred = JsonConvert.DeserializeObject<Predio>(y);

                   despre = new List<DesglosePredio>();
                    double TOTAL = 0;
                    foreach(var total in pred.adeudos){
                        TOTAL += total.total;
                        despre.Add( new DesglosePredio{
                            bimIni = total.bimInicial,
                            bimFin = total.bimFinal,
                            pago = total.total
                        });
                    }
                    despre=  Ordenar(despre);

                    if (multiPage != null)
                    {
                        double valores = 0;
                        var answers = multiPage.GetSelection();
                        foreach (var a in answers)
                        {
                            valores += a.Pago;
                        }
                        if (answers.Count > 0)
                        {
                            list = new List<Carrito>{new Carrito
                        {
                                    Name = "Predio:" +pred.cveCatastral, Description = "ejemplo", price = valores

                        }};
                            BindingContext = list;
                            listView.ItemsSource = list;

                        }
                        else
                        {
                            list = new List<Carrito> { new Carrito{
                                    Name = "Predio: "+pred.cveCatastral,
                            Description = "ejemplo",
                                    price = TOTAL
                        }
                    };
                            BindingContext = list;
                            listView.ItemsSource = list;
                            //results.Text = "(none)";
                        }

                    }
                    else
                    {
                        list = new List<Carrito> { new Carrito{
                                Name = "Predio: "+pred.cveCatastral,
                                Description = pred.colonia+" "+pred.calle + " "+ pred.numeroExt,
                                price = TOTAL
                        }
                    };
                        BindingContext = list;
                        listView.ItemsSource = list;
                        //results.Text = "(none)";
                    }


                }

                else
                {
                    System.Diagnostics.Debug.WriteLine(y);

                    var resp = JsonConvert.DeserializeObject<Respuesta>(y);

                    await DisplayAlert("Error", resp.respuesta, "OK");


                }
            }
            catch (HttpRequestException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.InnerException.Message);

            }




        }

        void Handle_ItemSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
            //throw new NotImplementedException();
        }


        List<DesglosePredio> Ordenar(List<DesglosePredio> t){
            var newList = t.OrderBy(x => x.bimFin).ToList();
            return newList;
        }
    }
}
