using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PagoEnLinea.PaginasPago
{


    /// <summary>
    /// Esta clase muestra en pantalla el desgloce por fecha de un predio a pagar mediante una lista
    /// con multiples Switch que permiten seleccionar multiples fechas a la vez
    /// NO posee un XAML debido a que fué necesario realizar configuraciones especificas 
    /// en los componentes visuales  
    /// </summary>
    public class DesglosePredios<T> : ContentPage
    {


        public static string DETALLES;

        /// <summary>
        /// Subclase que establece propieades especificas para ser implementadas por los componentes visuales
        /// IsSelect es usada por los switch para detectar que se encuentran en modo toggled
        /// mientras que IsEnabled cuando los switch estan desabilitados (actualmente no esta implementada ya que
        /// no permitia cambiar el color del switch)
        /// 
        /// </summary>
        public class SeleccionPropiedades<T> : INotifyPropertyChanged
        {
            public T Item { get; set; }
            bool isSelected = false;

            public bool IsSelected
            {
                get
                {
                    return isSelected;
                }
                set
                {
                    if (isSelected != value)
                    {
                        isSelected = value;
                        PropertyChanged(this, new PropertyChangedEventArgs("IsSelected"));
                        //                      PropertyChanged (this, new PropertyChangedEventArgs (nameof (IsSelected))); // C# 6
                    }
                }
            }
            bool isEnabled = false;
            public bool IsEnabled
            {
                get
                {
                    return isEnabled;
                }
                set
                {
                    if (isEnabled != value)
                    {
                        isEnabled = value;
                        PropertyChanged(this, new PropertyChangedEventArgs("IsEnabled"));
                        //                      PropertyChanged (this, new PropertyChangedEventArgs (nameof (IsSelected))); // C# 6
                    }
                }
            }


            public event PropertyChangedEventHandler PropertyChanged = delegate { };
        }

        /// <summary>
        /// crea una celda personalizada para establecerla como modelo en la lista con dos views principales
        /// texto que contendra informacion (fechas y precio) y un switch
        /// </summary>
        public class CustomCell : ViewCell
        {
            public CustomCell() : base()
            {

                Label name = new Label();
              

                name.SetBinding(Label.TextProperty, new Binding("Item.Name"));
               
                Switch mainSwitch = new Switch();
                mainSwitch.SetBinding(Switch.IsToggledProperty, new Binding("IsSelected"));
                mainSwitch.SetBinding(Switch.IsEnabledProperty, new Binding("IsEnabled"));
            

                RelativeLayout layout = new RelativeLayout();


                layout.Children.Add(name,
                    Constraint.Constant(5),
                    Constraint.Constant(5),
                    Constraint.RelativeToParent(p => p.Width - 60),
                    Constraint.RelativeToParent(p => p.Height - 10)
                );
                
                layout.Children.Add(mainSwitch,
                    Constraint.RelativeToParent(p => p.Width - 55),
                    Constraint.Constant(5),
                    Constraint.Constant(50),
                    Constraint.RelativeToParent(p => p.Height - 10)
                );
               
                View = layout;
            }

          
                
                
         


        }

        public  List<SeleccionPropiedades<T>> WrappedItems = new List<SeleccionPropiedades<T>>();

        /// <summary>
        /// Constructor de la clase principal recibe los items (fechas y precios) a mostrar al usuario y establece
        /// propiedadades especificas antes de mostrarlas (implementado la subclase SeleccionPropiedades)
        /// asigna estos items a una lista con celdas personalizadas de tipo "CustomCell"
        /// Muestra un boton al cual le asigna un vento que llama al metodo SelectALll();
        /// añade un evento de tipo item tapped el cual muestra al usuario
        /// la opcioón de ver los detalles los detalles de una fecha en especifica de un predio a pagar
        /// o la opcion de selecciónar o deseleccionar esta fecha para pagarla
        /// 
        /// </summary>
        /// <param name="items">items que seran añadidos a la lista</param>
        public DesglosePredios(List<T> items)
        {
            WrappedItems = items.Select(item => new SeleccionPropiedades<T>() { Item = item, IsSelected = true, IsEnabled = true }).ToList();

           


            ListView mainList = new ListView()
            {
                ItemsSource = WrappedItems,
                ItemTemplate = new DataTemplate(typeof(CustomCell)),
            };


            var stack = new StackLayout();
           

            mainList.ItemSelected += async(sender, e) =>
            {

                if (e.SelectedItem == null) return;
                var item = (SeleccionPropiedades<T>)e.SelectedItem;
                var tipo = (item.IsSelected) ? "Deseleccionar" : "Seleccionar";
                var resp = await  DisplayAlert("Seleccion","Seleccione una opción",tipo,"Ver detalles");
               
                if(resp){
                var o = (SeleccionPropiedades<T>)e.SelectedItem;
                o.IsSelected = !o.IsSelected;
                if(o.IsSelected){
                        await SelectAll();
                }else{
                    SelectNone();
                }
                }else{
                  
                    var seleccion =  mainList.SelectedItem as SeleccionPropiedades<T>;
                    var prop = seleccion.Item as CheckItem;
                    
                    //var index = (mainList.ItemsSource as WrappedItems).IndexOf(e.SelectedItem as CheckItem);
                    await DisplayAlert("Detalles",prop.Details +"","OK");
                }
                ((ListView)sender).SelectedItem = null; //de-select

            };
            var button3 = new Button { Text = "agregar", VerticalOptions = LayoutOptions.End, HorizontalOptions = LayoutOptions.Center, TextColor= Color.White,BackgroundColor= Color.FromHex("#5CB85C") };
            mainList.SeparatorVisibility = SeparatorVisibility.None;
            stack.Children.Add(mainList);
            stack.Children.Add(button3);
           
            Content = stack;
           
            button3.Clicked += async(sender, e) => {
               
                 await SelectAll();
                await Navigation.PopModalAsync();
            };
            
        }

        /// <summary>
        /// Metodo que permite validar que no se puedan seleccionar fechas recientes sin seleccionar fechas anteriores
        /// para pagarlas
        /// </summary>
        /// <returns>The all.</returns>
        async Task SelectAll()
        {
            var x = 0;
            var match = false;

            for (var i = 0; i < WrappedItems.Count(); i++)
            {
                if (WrappedItems[i].IsSelected)
                {
                    x = i;
                }
            }

            for (var i = 0; i < x; i++)
            {
                if (!WrappedItems[i].IsSelected) { match = true; }
                WrappedItems[i].IsSelected = true;


            }

            if(match){
                await DisplayAlert("Información", "Algunos campos se seleccionaron automáticamente debido a que no es posible pagar una fecha con adeudos anteriores.", "OK");
  
            }

           
        }

        /// <summary>
        ///metodo que deselecciona todas automáticamente todas las fechas posteriores a una fecha fecha deseleccionada
        /// </summary>
        void SelectNone()
        {
            var y = 0;

            for (var i = 0; i < WrappedItems.Count(); i++)
            {
                if (!(WrappedItems[i].IsSelected))
                {
                    y = i;
                    break;
                }
            }


            for (var i = y; i < WrappedItems.Count(); i++)
            {

                WrappedItems[i].IsSelected = false;

            }
        }

       
        /// <summary>
        /// obtiene los items que tienen la propiedad IsSelected como true (cuando el switch de cada item esta habilitado)
        ///  y los retorna en forma de lista
        /// </summary>
        /// <returns>La lista de items</returns>
        public List<T> GetSelection()
        {
            return WrappedItems.Where(item => item.IsSelected).Select(wrappedItem => wrappedItem.Item).ToList();

        }
       
        protected override  bool OnBackButtonPressed()
        {
            // If you want to continue going back
            base.OnBackButtonPressed();
            Device.BeginInvokeOnMainThread(async () => {
                await  SelectAll();
                await  DisplayAlert("Información", "Algunos campos se seleccionaron automáticamente debido a que no es posible pagar una fecha con adeudos anteriores.", "OK");
            });
          
            return true;
        }
       
    }
}


