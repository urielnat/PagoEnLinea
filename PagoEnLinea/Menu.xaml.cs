using System;
using System.Collections.Generic;

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
                new MenuItem{ Page = new PerfilPage(),MenuTitle="Editar Perfil",MenuDetail="Edite propiedades"}

            };

            ListMenu.ItemsSource = menu;
            ListMenu.Margin = new Thickness(0, 20, 0, 0);
            Detail = new NavigationPage(new HomePage());
        }

        void Handle_ItemSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
            var menu = e.SelectedItem as MenuItem;
            if(menu!=null){
                IsPresented = false;
                Detail = new NavigationPage(menu.Page);
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
