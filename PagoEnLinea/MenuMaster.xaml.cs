﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using PagoEnLinea.Modelos;
using PagoEnLinea.Paginas;
using PagoEnLinea.PaginasMisPagos;
using PagoEnLinea.PaginasPago;
using PagoEnLinea.servicios;
using Xamarin.Forms;

namespace PagoEnLinea
{
    public partial class MenuMaster : ContentPage
    {

        public static string TOKEN="";

        ClienteRest client = new ClienteRest();
        info inf = new info();

        /// <summary>
        /// esta clase permite mostrar un menu lateral en la aplicación, mismo que da la opción al usuario
        /// de dirigirse a un modulo específico de la aplicacíon o cerrar su sesión
        /// en su constructor inicializa los componentes visuales ademas de que llama a los metodos init(); y conectar();
        /// añade un evento a la lista que contiene los elementos del menu laterl
        /// </summary>
        public MenuMaster()
        {
            InitializeComponent();
            Init();
            conectar();
            ListMenu.ItemTapped += OnitemTapped;
        }

   
       
        //inicializa la lista del menu lateral y añade propiedades visuales
        void Init()
        {
            List<string> list = new List<string>();
            ListMenu.ItemsSource = new MenuItem2[] {
                
                new MenuItem2{ Page = new HomePage(),MenuTitle2="Inicio",MenuDetail2="Pagina de inicio"},
                new MenuItem2{ Page = new PrediosPagadosPage(),MenuTitle2="Mis pagos",MenuDetail2="Vea sus pagos"},
                new MenuItem2{ Page = new PredialPage(),MenuTitle2="Predial en línea",MenuDetail2="Busqueda de predial"},
                new MenuItem2{ Page = new LiquidacionPage(),MenuTitle2="Liquidación en línea",MenuDetail2="Busqueda de liquidacíon"},
                new MenuItem2{ Page = new CarritoPage(),MenuTitle2="Carrito de compras",MenuDetail2="Elementos añadidos al carrito"},
                new MenuItem2{ Page = new TabPage(),MenuTitle2="Editar Perfil",MenuDetail2="Edite propiedades"},
                new MenuItem2{ MenuTitle2="Cerrar sesión",MenuDetail2="Cierre su sesión actual",Tipo=1}
            };

           
            ListMenu.ItemTemplate = new DataTemplate(typeof(ImageCell));

            //asignar al template los valores mediante binding
            ListMenu.ItemTemplate.SetBinding(TextCell.TextProperty, "MenuTitle2");
            ListMenu.ItemTemplate.SetBinding(TextCell.DetailProperty, "MenuDetail2");
            ListMenu.ItemTemplate.SetBinding(ImageCell.ImageSourceProperty, "imagen");
            //BindingContext = menu;

           
            ListMenu.Margin = new Thickness(0, 20, 0, 0);

        }

        /// <summary>
        /// evento al presionar un item de la lista muestra al usuario el módulo seleccionado
        /// pero sin quitar la parte visual del menu lateral
        /// existen dos tipos de opciones en la barra lateral la mayoria de items son de tipo "0"
        /// lo cual implica que redigen al usuario a otra pantalla al presionarse
        /// mientras que el tipo 1 es para cerrar sesión y consume al servicio de logout para notificar al servidors
        /// </summary>
        /// <param name="sender">objeto que hace referencia el eveto</param>
        /// <param name="e">propiedades del objeto en este caso es usado para obtener el item de la lista seleccionada y 
        /// acceder a su propiedad page la cual contiene la "pagina" o pantalla de la aplicación que se le mostrará al usuario
        /// </param>
        async void OnitemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null) return;

            var menu = e.Item as MenuItem2;
            if (menu != null && menu.Tipo != 1)
            {
                Application.Current.MainPage = new MasterDetailPage { Master = new MenuMaster(), Detail = new NavigationPage(menu.Page) };
               
            }
            if (menu.Tipo == 1)
            {
                var respuesta = await DisplayAlert("Cerrar sesión", "¿Desea cerrar sesión?", "Si", "Cancelar");
                System.Diagnostics.Debug.WriteLine(respuesta);
                if (respuesta)
                {
                    

                    HttpResponseMessage response;

                    string ContentType = "application/json"; // or application/xml

                    try
                    {
                        HttpClient cliente = new HttpClient();
                        cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TOKEN);

                        response = await cliente.PostAsync("http://192.168.0.18:8080/management/audits/logout", new StringContent("", Encoding.UTF8, ContentType));
                        var y = await response.Content.ReadAsStringAsync();
                        System.Diagnostics.Debug.WriteLine(y);

                       
                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {

                            System.Diagnostics.Debug.WriteLine("NOTIFICADO");

                          
                        }

                        else
                        {

                            var resp = JsonConvert.DeserializeObject<Respuesta>(y);

                            await DisplayAlert("Error", resp.respuesta, "OK");


                        }
                    }
                    catch (HttpRequestException ex)
                    {
                        System.Diagnostics.Debug.WriteLine(ex.InnerException.Message);

                    }

                    ((App)Application.Current).ResumeAtTodoId = -1;
                    var  list = await App.Database.GetItemsAsync();
                    foreach(var item in list){
                        await App.Database.DeleteItemAsync(item);
                    }

                    Application.Current.Properties.Clear();
                   
                    Application.Current.MainPage = new NavigationPage(new LoginPage());

                }

            }
           

           ((ListView)sender).SelectedItem = null; // de-select
        }


        /// <summary>
        /// Consume al servicio infoContribuyente para mostrar el nombre en la barra lateral
        /// </summary>
        async void conectar()
        {
            if (Application.Current.Properties.ContainsKey("token"))
            {

                TOKEN = Application.Current.Properties["token"] as string;

                ClienteRest cliente = new ClienteRest();
                var inf = await cliente.InfoUsuario<InfoUsuario>(Application.Current.Properties["token"] as string);
                if (inf != null)
                {
                    lblNomre.Text = inf.persona.nombre;
                }

            }
        }
        protected override void OnAppearing()
        {
            
        }

    }

    /// <summary>
    /// modelo del menu lateral
    /// </summary>
    public class MenuItem2
    {
        public String MenuTitle2
        {
            get;
            set;
        }

        public String MenuDetail2
        {
            get;
            set;
        }

        public Page Page
        {
            get;
            set;
        } 
        public int Tipo
        {
            get;
            set;
        }

        public ImageSource imagen{
            get;
            set;
        }
    }

}
