using System;
using System.Collections.Generic;
using PagoEnLinea.Paginas;
using Xamarin.Forms;

namespace PagoEnLinea
{
    public partial class Menu : MasterDetailPage
    {
        public Menu()
        {
            InitializeComponent();
            Init();
        }

         void Init()
        {
            List<MenuItem> menu = new List<MenuItem> {
                new MenuItem{ Page = new HomePage(),MenuTitle="Inicio",MenuDetail="Pagina de inicio"},
                new MenuItem{ Page = new TabPage(),MenuTitle="Editar Perfil",MenuDetail="Edite propiedades"}

            };

            ListMenu.ItemsSource = menu;
            ListMenu.Margin = new Thickness(0, 20, 0, 0);
            Detail = new HomePage();
        }


        void Handle_ItemSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
            var menu = e.SelectedItem as MenuItem;
            if(menu!=null){
                IsPresented = false;
                Detail = menu.Page;
            }
        }
    }

    public class MenuItem{
        public String MenuTitle
        {
            get;
            set;
        }

        public String MenuDetail
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
