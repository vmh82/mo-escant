using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Escant_App.ViewModels.Productos;
using DataModel;
using DataModel.DTO;
using DataModel.Helpers;
using Escant_App.ViewModels;
using Escant_App.AppSettings;
using DataModel.DTO.Productos;
using Syncfusion.XForms.PopupLayout;
using ManijodaServicios.Resources.Texts;

namespace Escant_App.Views.Productos
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PrecioView : ContentPage
    {
        Precio item;
        Precio parentItem;
        SfPopupLayout popupLayout;
        public PrecioViewModel Context => (PrecioViewModel)this.BindingContext;
        public PrecioView()
        {
            InitializeComponent();
            Context.MenuImagen = "\uf550";
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Context.TreeView = treeView;
        }

        private void treeView_ItemHolding(object sender, Syncfusion.XForms.TreeView.ItemHoldingEventArgs e)
        {
            Int32 top = 10;
            item = e.Node.Content as Precio;
            parentItem = e.Node.ParentNode != null ? e.Node.ParentNode.Content as Precio : null;
            popupLayout = new SfPopupLayout();
            popupLayout.PopupView.WidthRequest = 100;
            popupLayout.PopupView.HeightRequest = 60;
            popupLayout.PopupView.ContentTemplate = new DataTemplate(() =>
            {

                var mainStack = new StackLayout();
                mainStack.BackgroundColor = Color.Teal;

                var editButton = new Button()
                {
                    Text = item.EstaActivo == 1 ? TextsTranslateManager.Translate("DesactivarPrecio") : TextsTranslateManager.Translate("ActivarPrecio"),
                    HeightRequest = 50,
                    BackgroundColor = Color.Teal,
                    TextColor = Color.White,
                    FontSize = 10
                };
                editButton.Clicked += EditButton_Clicked;
                mainStack.Children.Add(editButton);
                return mainStack;
            });
            popupLayout.PopupView.ShowHeader = false;
            popupLayout.PopupView.ShowFooter = false;
            if (e.Position.Y + top <= treeView.Height && e.Position.X + top > treeView.Width)
                popupLayout.Show((double)(e.Position.X - top), (double)(e.Position.Y));
            else if (e.Position.Y + top > treeView.Height && e.Position.X + top < treeView.Width)
                popupLayout.Show((double)e.Position.X, (double)(e.Position.Y - top));
            else if (e.Position.Y + top > treeView.Height && e.Position.X + top > treeView.Width)
                popupLayout.Show((double)(e.Position.X - top), (double)(e.Position.Y - top));
            else
                popupLayout.Show((double)e.Position.X, (double)(e.Position.Y) - 60);
        }
        private void EditButton_Clicked(object sender, EventArgs e)
        {

            if (treeView == null)
                return;
            Context.EditCommandSync(item, parentItem);

            if (popupLayout != null)
            {
                popupLayout.Dismiss();
            }

        }


        private void treeView_Loaded(object sender, Syncfusion.XForms.TreeView.TreeViewLoadedEventArgs e)
        {
            /*var count = Context.ImageNodeInfo != null ? Context.ImageNodeInfo.Count : 0;
            var data = Context.ImageNodeInfo != null ? Context.ImageNodeInfo[count - 1] : null;
            treeView.BringIntoView(data);*/

        }

        async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            if (menu.Text == "\uf550")
            {
                Context.MenuImagen = "\uf057";
                Context.IsVisibleBotones = true;
            }
            else
            {
                Context.MenuImagen = "\uf550";
                Context.IsVisibleBotones = false;
            }            
        }
        async void AdicionarPerfilMenu_Tapped(object sender, EventArgs e)
        {
            await Context.CreaPrecio();

        }
        async void ActualizarPerfilMenu_Tapped(object sender, EventArgs e)
        {
            await Context.ActualizaPrecio();
        }
    }
}