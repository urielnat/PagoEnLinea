using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Xamarin.Forms;

namespace PagoEnLinea.PaginasPago
{
    public class DesglosePredios<T> : ContentPage
    {
        public static string DETALLES;
        public class WrappedSelection<T> : INotifyPropertyChanged
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
        public class WrappedItemSelectionTemplate : ViewCell
        {
            public WrappedItemSelectionTemplate() : base()
            {

                Label name = new Label();
                Label info = new Label();

                name.SetBinding(Label.TextProperty, new Binding("Item.Name"));
                info.SetBinding(Label.TextProperty, new Binding("Item.sinOrdenbimFin"));
                DETALLES = info.Text;
                Switch mainSwitch = new Switch();
                mainSwitch.SetBinding(Switch.IsToggledProperty, new Binding("IsSelected"));
                mainSwitch.SetBinding(Switch.IsEnabledProperty, new Binding("IsEnabled"));
                /**
                Button invisible = new Button();
                invisible.IsVisible = true;
                invisible.Text = "##########";
                var image = new Image();
                image.Source = "user.png";
                StackLayout stack = new StackLayout();
                Grid grid = new Grid();
                mainSwitch.BackgroundColor = Color.Green;
                mainSwitch.WidthRequest= 20;
                name.VerticalTextAlignment = TextAlignment.Center;
                name.HorizontalTextAlignment = TextAlignment.Start;
                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0, GridUnitType.Auto) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1,GridUnitType.Star) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(9, GridUnitType.Star) });


                grid.Children.Add(name,0,0,0,0);
                grid.Children.Add(mainSwitch, 0, 1,0,0);
                grid.Children.Add(image, 0, 1,0,0);

                 **/


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

            private void OnFocus(object sender, FocusEventArgs e)
            {
                (sender as Switch).IsToggled = ((sender as Switch).IsToggled) ? false : true;  
                System.Diagnostics.Debug.WriteLine("Focus");
            }
                
                
         


        }
        public static List<WrappedSelection<T>> WrappedItems = new List<WrappedSelection<T>>();
        public DesglosePredios(List<T> items)
        {
            WrappedItems = items.Select(item => new WrappedSelection<T>() { Item = item, IsSelected = true, IsEnabled = true }).ToList();

           


            ListView mainList = new ListView()
            {
                ItemsSource = WrappedItems,
                ItemTemplate = new DataTemplate(typeof(WrappedItemSelectionTemplate)),
            };

            foreach (var item in WrappedItems)
            {
                SelectAll();

            }

            var stack = new StackLayout();


            mainList.ItemSelected += async(sender, e) =>
            {
                

                if (e.SelectedItem == null) return;
                var resp = await  DisplayAlert("Seleccion","Seleccione una opción","Seleccionar","Ver detalles");
               
                if(resp){
                var o = (WrappedSelection<T>)e.SelectedItem;
                o.IsSelected = !o.IsSelected;
                if(o.IsSelected){
                    SelectAll();
                }else{
                    SelectNone();
                }
                }else{
                   
                    var seleccion =  mainList.SelectedItem as WrappedSelection<T>;
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
           
            button3.Clicked += (sender, e) => {
                Navigation.PopAsync();
            };
            
        }
        void SelectAll()
        {
            var x = 0;


            for (var i = 0; i < WrappedItems.Count(); i++)
            {
                if (WrappedItems[i].IsSelected)
                {
                    x = i;
                }
            }

            for (var i = 0; i < x; i++)
            {

                WrappedItems[i].IsSelected = true;

            }
            /**
              foreach (var wi in WrappedItems)
            {
                wi.IsSelected = true;
            }
             
              
            foreach (var w1 in WrappedItems){
                WrappedItems.Count();
            }**/
        }
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
        public List<T> GetSelection()
        {
            return WrappedItems.Where(item => item.IsSelected).Select(wrappedItem => wrappedItem.Item).ToList();

        }
        public List<T> GetSelectionh()
        {
            return WrappedItems.Where(item => item.IsSelected).Select(wrappedItem => wrappedItem.Item).ToList();

        }
       
    }
}
