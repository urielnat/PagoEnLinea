using PagoEnLinea.Paginas;
using Xamarin.Forms;

namespace PagoEnLinea
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
           
            //MessagingCenter.Subscribe<PopupCarga>(this, "login", (Sender) => { MainPage = new NavigationPage(new Menu()); });
            if (Current.Properties.ContainsKey("token"))
            {
                MainPage = new MasterDetailPage { Master = new MenuMaster(), Detail = new NavigationPage(new HomePage()) };
                //var id = Current.Properties["user"] as string;
                // do something with id
            }else{
                Current.Properties.Clear();
                MainPage = new NavigationPage(new LoginPage());
            }

        }
    
   
        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
