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
                new MenuItem2{ Page = new TabPage(),MenuTitle2="Editar Perfil",MenuDetail2="Edite propiedades"}

            };

           
            ListMenu.ItemTemplate = new DataTemplate(typeof(ImageCell));

            //asignar al template los valores mediante binding
            ListMenu.ItemTemplate.SetBinding(TextCell.TextProperty, "MenuTitle2");
            ListMenu.ItemTemplate.SetBinding(TextCell.DetailProperty, "MenuDetail2");
            ListMenu.ItemTemplate.SetBinding(ImageCell.ImageSourceProperty, "Imagen");
            //BindingContext = menu;

           
            ListMenu.Margin = new Thickness(0, 20, 0, 0);

        }
        void Handle_ItemSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
            var menu = e.SelectedItem as MenuItem2;
            if (menu != null)
            { Application.Current.MainPage = new MasterDetailPage { Master = new MenuMaster(), Detail = new NavigationPage(menu.Page) };
                // NavigationPage NP = ((NavigationPage)((MasterDetailPage)Application.Current.MainPage).Detail);
            //NP.PushAsync(menu.Page);
            }

          
           
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
    }

}
