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

        /// <summary>
        /// establece la pantalla de inicio de la aplicacion
        /// actualmente solo se encuentra habilitado para que la pagina de inicio sea el login ya que
        ///   Properties.Clear(); elimina toda la informacíon almacenada que en este caso es el token de login
        /// </summary>
        public App()
        {
            InitializeComponent();
            Properties.Clear();

            if (Current.Properties.ContainsKey("token"))
            {
                MainPage = new MasterDetailPage { Master = new MenuMaster(), Detail = new NavigationPage(new HomePage()) };
                //var id = Current.Properties["user"] as string;
                // do something with id
            }else{
                
                MainPage = new NavigationPage(new LoginPage());
            }
          


        }

        //instancia a la base de datos interna del teléfono la cual cuarda el carrito
        static TodoItemDatabase database;

   
        //si no se encuentra creada una base de datos interna primero se crea, en caso contrario solo se retorna
        //hacer esto desde esta clase permite que este disponible para todos los modulos
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
