using DataModel.DTO.Seguridad;
using ManijodaServicios.Resources.Texts;
using Escant_App.ViewModels.Seguridad;
using Syncfusion.XForms.PopupLayout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Escant_App.Views.Seguridad
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PerfilMenuView : ContentPage
    {
        PerfilMenu item;
        PerfilMenu parentItem;
        SfPopupLayout popupLayout;
        public PerfilMenuViewModel Context => (PerfilMenuViewModel)this.BindingContext;
        public PerfilMenuView()
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
            item = e.Node.Content as PerfilMenu;
            parentItem = e.Node.ParentNode != null ? e.Node.ParentNode.Content as PerfilMenu : null;
            popupLayout = new SfPopupLayout();            
            popupLayout.PopupView.WidthRequest = 100;
            popupLayout.PopupView.HeightRequest = 60;
            popupLayout.PopupView.ContentTemplate = new DataTemplate(() =>
            {

                var mainStack = new StackLayout();
                mainStack.BackgroundColor = Color.Teal;

                var editButton = new Button()
                {
                    Text = item.EstaActivo==1?TextsTranslateManager.Translate("DesactivarNodo"): TextsTranslateManager.Translate("ActivarNodo"),
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
            Context.EditCommandSync(item,parentItem);

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
           /* Image image = sender as Image;
            string filename = image.Source.ToString();
            if (filename == "File: cross_512.png")
            {
                contentPage.Opacity = 1;
                grid.BackgroundColor = Color.Transparent;
                contentPage.BackgroundColor = Color.White;
                menu.Source = "share.png";
                menu.WidthRequest = 50;
                menu.HeightRequest = 50;
                await Task.WhenAll(
                    fb.TranslateTo(0, 0),
                    twit.TranslateTo(0, 0)
                    );

                fb.IsVisible = false;
                twit.IsVisible = false;
                contentPage.InputTransparent = false;
            }
            else if (filename == "File: share.png")
            {
                fb.IsVisible = true;
                twit.IsVisible = true;
                //contentPage.InputTransparent = true;

                //contentPage.Opacity = 0.3;
                //grid.BackgroundColor = Color.LightBlue;
               // contentPage.BackgroundColor = Color.Transparent;
                menu.Source = "cross_512.png";
                menu.WidthRequest = 30;
                menu.HeightRequest = 30;

                await Task.WhenAll(
                    fb.TranslateTo(-80, 0),
                    twit.TranslateTo(-150, 0)
                    );


            }*/
        }
        async void AdicionarPerfilMenu_Tapped(object sender, EventArgs e)
        {
            await Context.CreaPerfilMenu();

        }        
        async void ActualizarPerfilMenu_Tapped(object sender, EventArgs e)
        {
            await Context.ActualizaPerfilMenu();
        }
    }
}