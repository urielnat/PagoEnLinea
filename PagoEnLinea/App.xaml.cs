using CarritoBD.Data;
using CarritoBD.Interface;

using PagoEnLinea.Paginas;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace PagoEnLinea
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            Properties.Clear();
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
          // MainPage = new RegistroPage2(null, null, null);


        }
        static TodoItemDatabase database;

   

        public static TodoItemDatabase Database
        {
            get
            {
                if (database == null)
                {
                    database = new TodoItemDatabase(DependencyService.Get<IFileHelper>().GetLocalFilePath("TodoSQLite.db3"));
                }
                return database;
            }
        }
    
        public int ResumeAtTodoId { get; set; }
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
