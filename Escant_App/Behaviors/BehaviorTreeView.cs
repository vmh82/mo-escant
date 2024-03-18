using DataModel.DTO.Administracion;
using ManijodaServicios.AppSettings;
using ManijodaServicios.Offline.Interfaz;
using Escant_App;
using Escant_App.Services;
using Escant_App.ViewModels.Administracion;
using Escant_App.ViewModels.Base;
using Syncfusion.Core.XForms;
using Syncfusion.XForms.PopupLayout;
using Syncfusion.XForms.TreeView;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Escant_App.Behaviors
{
    public class BehaviorTreeView : Behavior<SfTreeView>
    {
        SfTreeView ListView;
        int sortorder = 0;
        Catalogo item;
        Catalogo parentItem;
        SfPopupLayout popupLayout;
        protected readonly IDialogService DialogService;
        protected readonly INavigationService NavigationService;
        public BehaviorTreeView()
        {
            DialogService = ViewModelLocator.Resolve<IDialogService>();
            NavigationService = ViewModelLocator.Resolve<INavigationService>();
        }
        protected override void OnAttachedTo(SfTreeView listView)
        {
            ListView = listView;            
            ListView.ItemHolding += ListView_ItemHolding;
            ListView.ItemDoubleTapped += ListView_ItemDoubleTapped;
            //ListView.ItemTapped += ListView_ItemTapped;
            base.OnAttachedTo(listView);
        }

        private void ListView_ItemTapped(object sender, Syncfusion.XForms.TreeView.ItemTappedEventArgs e)
        {
            if (popupLayout != null)
            {
                popupLayout.Dismiss();
            }
        }

        private void ListView_ItemDoubleTapped(object sender, ItemDoubleTappedEventArgs e)
        {
            if (popupLayout != null)
            {
                popupLayout.Dismiss();
            }
        }

              

        private void ListView_ItemHolding(object sender, ItemHoldingEventArgs e)
        {
            item = e.Node.Content as Catalogo;
            parentItem = e.Node.ParentNode!=null?e.Node.ParentNode.Content as Catalogo:null;
            popupLayout = new SfPopupLayout();
            popupLayout.PopupView.HeightRequest = 100;
            popupLayout.PopupView.WidthRequest = 100;
            popupLayout.PopupView.ContentTemplate = new DataTemplate(() =>
            {

                var mainStack = new StackLayout();
                mainStack.BackgroundColor = Color.Teal;

                var deletedButton = new Button()
                {
                    Text = "Add",
                    HeightRequest = 50,
                    BackgroundColor = Color.Teal,
                    TextColor = Color.White
                };
                /*deletedButton.Behaviors.Add(new EventToCommandBehavior {
                    EventName = "Clicked"
                    ,Command = new Command(async () => await AddButton_ClickedAsync())
                });*/
                deletedButton.Clicked += AddButton_Clicked;
                var sortButton = new Button()
                {
                    Text = "Sort",
                    HeightRequest = 50,
                    BackgroundColor = Color.Teal,
                    TextColor = Color.White
                };
                sortButton.Clicked += SortButton_Clicked;
                mainStack.Children.Add(deletedButton);
                mainStack.Children.Add(sortButton);
                return mainStack;

            });
            popupLayout.PopupView.ShowHeader = false;
            popupLayout.PopupView.ShowFooter = false;
            if (e.Position.Y + 100 <= ListView.Height && e.Position.X + 100 > ListView.Width)
                popupLayout.Show((double)(e.Position.X - 100), (double)(e.Position.Y));
            else if (e.Position.Y + 100 > ListView.Height && e.Position.X + 100 < ListView.Width)
                popupLayout.Show((double)e.Position.X, (double)(e.Position.Y - 100));
            else if (e.Position.Y + 100 > ListView.Height && e.Position.X + 100 > ListView.Width)
                popupLayout.Show((double)(e.Position.X - 100), (double)(e.Position.Y - 100));
            else
                popupLayout.Show((double)e.Position.X, (double)(e.Position.Y));

        }

        
        private void Dismiss()
        {
            popupLayout.IsVisible = false;
        }

        private async Task AddButton_ClickedAsync()
        {
            SettingsOnline.oCatalogo = new Catalogo()
            {
                EsCatalogo = 1
            };     
            await NavigationService.NavigateToPopupAsync<PopupAddCatalogoViewModel>(null); 
        }

        private void AddButton_Clicked(object sender, EventArgs e)
        {

            if (ListView == null)
                return;



            var source = ListView.ItemsSource as IList;
            //App.Current.MainPage.DisplayAlert("Alert", parentItem!=null?parentItem.Nombre:"" + " " + item.Nombre, "OK");
            var nuevoRegistro = new Catalogo()
            {
                 IdPadre=item.Id
                ,CodigoCatalogo=item.CodigoCatalogo
                ,Empresa=item.Empresa
                ,EsCatalogo=0                
                ,EsMedida=item.EsMedida
                ,EsJerarquico = item.EsJerarquico
                ,EstaActivo=item.EstaActivo
            };
            NavigationService.NavigateToPopupAsync<PopupAddCatalogoViewModel>(nuevoRegistro);

            if (popupLayout != null)
            {
                popupLayout.Dismiss();
            }

        }
        private void SortButton_Clicked(object sender, EventArgs e)
        {

            if (ListView == null)
                return;

            var source = ListView.ItemsSource as IList;
            /*
            if (source != null && source.Contains(item))
                source.Remove(item);
            else
                App.Current.MainPage.DisplayAlert("Alert", "Unable to delete the item", "OK");

            item = null;
            source = null;
            */
        }
    }

}
