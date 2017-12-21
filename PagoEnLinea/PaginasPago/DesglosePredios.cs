using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Xamarin.Forms;

namespace PagoEnLinea.PaginasPago
{
    public class DesglosePredios<T> : ContentPage
    {
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
        public List<WrappedSelection<T>> WrappedItems = new List<WrappedSelection<T>>();
        public DesglosePredios(List<T> items)
        {
            WrappedItems = items.Select(item => new WrappedSelection<T>() { Item = item, IsSelected = true, IsEnabled = false }).ToList();
            ListView mainList = new ListView()
            {
                ItemsSource = WrappedItems,
                ItemTemplate = new DataTemplate(typeof(WrappedItemSelectionTemplate)),
            };

            foreach (var item in WrappedItems)
            {
                SelectAll();
            }


            mainList.ItemSelected += (sender, e) =>
            {

                if (e.SelectedItem == null) return;
                var o = (WrappedSelection<T>)e.SelectedItem;
                o.IsSelected = !o.IsSelected;
                if(o.IsSelected){
                    SelectAll();
                }else{
                    SelectNone();
                }
              
                ((ListView)sender).SelectedItem = null; //de-select

            };

            mainList.SeparatorVisibility = SeparatorVisibility.None;
            Content = mainList;
           

            
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
    }
}
