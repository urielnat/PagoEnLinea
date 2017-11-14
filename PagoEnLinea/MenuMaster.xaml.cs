using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using PagoEnLinea.Modelos;
using PagoEnLinea.Paginas;
using PagoEnLinea.servicios;
using Xamarin.Forms;

namespace PagoEnLinea
{
    public partial class MenuMaster : ContentPage
    {
        string texto;
        ClienteRest client = new ClienteRest();
        info inf = new info();
        public MenuMaster()
        {
            InitializeComponent();
            Init();


        }

   
       

        void Init()
        {
            List<string> list = new List<string>();
            ListMenu.ItemsSource = new MenuItem2[] {
                new MenuItem2{ Page = new HomePage(texto),MenuTitle2="Inicio",MenuDetail2="Pagina de inicio"},
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
        async void Handle_ItemSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
            var menu = e.SelectedItem as MenuItem2;
            if (menu != null&&menu.Tipo!=1)
            { Application.Current.MainPage = new MasterDetailPage { Master = new MenuMaster(), Detail = new NavigationPage(menu.Page) };
                // NavigationPage NP = ((NavigationPage)((MasterDetailPage)Application.Current.MainPage).Detail);
            //NP.PushAsync(menu.Page);
            }
            if(menu.Tipo==1){
                var respuesta = await DisplayAlert("Cerrar sesión", "¿Desea cerrar sesón?", "Si", "Cancelar");
                System.Diagnostics.Debug.WriteLine(respuesta);
                if(respuesta){
                    Application.Current.Properties.Clear();
                    Application.Current.MainPage = new LoginPage();
                }

            }
          
           
        }

        async void conectar()
        {
            if (Application.Current.Properties.ContainsKey("user"))
            {


                ClienteRest cliente = new ClienteRest();
                var inf = await cliente.InfoUsuario<InfoUsuario>(Application.Current.Properties["user"] as string);
                if (inf != null)
                {
                    lblNomre.Text = inf.persona.nombre;
                }

            }
        }
        protected override void OnAppearing()
        {
            conectar();
        }

    }


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
