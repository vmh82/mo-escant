using DataModel.DTO.Administracion;
using ManijodaServicios.Resources.Texts;
using Escant_App.ViewModels.Administracion;
using Escant_App.Views.Clientes;
using Escant_App.Views.Productos;
using Escant_App.Views.Seguridad;
using Syncfusion.XForms.PopupLayout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Escant_App.Views.Gestion;

namespace Escant_App.Views.Administracion
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CatalogoView : ContentPage
    {
        Catalogo item;
        Catalogo parentItem;
        SfPopupLayout popupLayout;
        public CatalogoViewModel Context => (CatalogoViewModel)this.BindingContext;
        public CatalogoView()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            Context.TreeView = treeView;
            await Context.InitializeAsync(null);
        }
        protected override void OnDisappearing()
        {
            RetornarRegistroSeleccionado();
            base.OnDisappearing();
        }

        private void catalogoCombobox_SelectionChanged(object sender, Syncfusion.XForms.ComboBox.SelectionChangedEventArgs e)
        {

        }

        private void treeView_ItemHolding(object sender, Syncfusion.XForms.TreeView.ItemHoldingEventArgs e)
        {
            Int32 top = 10;
            item = e.Node.Content as Catalogo;
            parentItem = e.Node.ParentNode != null ? e.Node.ParentNode.Content as Catalogo : null;
            popupLayout = new SfPopupLayout();
            if (item.EsCatalogo == 1)
            {
                popupLayout.PopupView.HeightRequest = 100;
            }
            else
            {
                popupLayout.PopupView.HeightRequest = 150;
            }
            popupLayout.PopupView.WidthRequest = 120;
            popupLayout.PopupView.ContentTemplate = new DataTemplate(() =>
            {

                var mainStack = new StackLayout();
                mainStack.BackgroundColor = Color.Teal;

                var editButton = new Button()
                {
                    Text = TextsTranslateManager.Translate("EditNodo"),
                    HeightRequest = 50,
                    BackgroundColor = Color.Teal,
                    TextColor = Color.White,
                    FontSize = 10
                };
                editButton.Clicked += EditButton_Clicked;
                mainStack.Children.Add(editButton);

                var addButton = new Button()
                {
                    Text = TextsTranslateManager.Translate("AddNodoHijo"),
                    HeightRequest = 50,
                    BackgroundColor = Color.Teal,
                    TextColor = Color.White,
                    FontSize=10
                };
                addButton.Clicked += AddButton_Clicked;
                mainStack.Children.Add(addButton);
                if (item.EsCatalogo == 0)
                {
                    var addParentButton = new Button()
                    {
                        Text = TextsTranslateManager.Translate("AddNodoPadre"),
                        HeightRequest = 50,
                        BackgroundColor = Color.Teal,
                        TextColor = Color.White,
                        FontSize = 10
                    };
                    addParentButton.Clicked += AddParentButton_Clicked;
                    mainStack.Children.Add(addParentButton);
                }                
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
                popupLayout.Show((double)e.Position.X, (double)(e.Position.Y)-60);
        }
        private void EditButton_Clicked(object sender, EventArgs e)
        {

            if (treeView == null)
                return;
            var editaRegistro = new Catalogo()
            {
                 Id=item.Id
                ,IdPadre = item.IdPadre
                ,CodigoCatalogo = item.CodigoCatalogo
                ,Empresa = item.Empresa
                ,Codigo=item.Codigo
                ,Nombre = item.Nombre
                ,EsMenu=item.EsMenu
                ,EsConstantes=item.EsConstantes
                ,EsCatalogo = item.EsCatalogo
                ,EsMedida = item.EsMedida
                ,EsCentral = item.EsCentral
                ,EsJerarquico = item.EsJerarquico
                ,EstaActivo = item.EstaActivo
                ,Nivel = item.Nivel
                ,Orden=item.Orden
                ,IdConversion = item.IdConversion
                ,ValorConversion = item.ValorConversion
                ,NombrePadre = parentItem != null ? parentItem.Nombre : ""
                ,EsEdicion=1
                ,ImageIcon=item.ImageIcon
                ,ValorConstanteNumerico = item.ValorConstanteNumerico
                ,ValorConstanteTexto = item.ValorConstanteTexto
                ,EsModulo = item.EsModulo
                ,PoseeFormulario=item.PoseeFormulario
                ,NombreFormulario = item.NombreFormulario
                ,LabelTitulo = item.LabelTitulo
                ,LabelDescripcion = item.LabelDescripcion
            };

            Context.EditCommandSync(editaRegistro);

            if (popupLayout != null)
            {
                popupLayout.Dismiss();
            }

        }
        private void AddButton_Clicked(object sender, EventArgs e)
        {

            if (treeView == null)
                return;
            var nuevoRegistro = new Catalogo()
            {
                IdPadre = item.Id
                ,
                CodigoCatalogo = item.CodigoCatalogo
                ,
                Empresa = item.Empresa
                ,
                EsCatalogo = 0
                ,
                EsMenu = item.EsMenu
                ,
                EsConstantes = item.EsConstantes
                ,
                EsMedida = item.EsMedida
                ,
                EsJerarquico = item.EsJerarquico                
                ,
                Nivel= parentItem!=null?parentItem.Nivel+1:0
                ,
                NombrePadre = item.Nombre
                ,
                EstaActivo = item.EstaActivo                
            };

            Context.AddCommandSync(nuevoRegistro);

            if (popupLayout != null)
            {
                popupLayout.Dismiss();
            }

        }
        private void AddParentButton_Clicked(object sender, EventArgs e)
        {

            if (treeView == null)
                return;
            var nuevoRegistro = new Catalogo()
            {
                    IdPadre = item.IdPadre
                ,   CodigoCatalogo = item.CodigoCatalogo
                ,   Empresa = item.Empresa
                ,   EsCatalogo = 0
                ,   EsMedida = item.EsMedida
                ,   EsCentral=0
                ,   EsJerarquico = item.EsJerarquico
                ,   EstaActivo = item.EstaActivo
                ,   Nivel=item.Nivel
                ,   Orden=item.Orden
                ,   IdConversion =item.IdConversion      
                ,   NombrePadre=parentItem!=null?parentItem.Nombre:""
            };
            var actualRegistro = new Catalogo()
            {                
                Id=item.Id
                ,Codigo=item.Codigo
                ,Nombre=item.Nombre
                ,CodigoCatalogo = item.CodigoCatalogo
                ,Empresa = item.Empresa
                ,EsCatalogo = 0
                ,EsMedida = item.EsMedida
                ,EsCentral=item.EsCentral
                ,EsJerarquico = item.EsJerarquico
                ,EstaActivo = item.EstaActivo
                ,Nivel=item.Nivel+1
                ,IdConversion = item.IdConversion
                ,ValorConversion = item.ValorConversion
                ,ValorConstanteNumerico = item.ValorConstanteNumerico
            };

            Context.AddParentCommandSync(nuevoRegistro,actualRegistro);

            if (popupLayout != null)
            {
                popupLayout.Dismiss();
            }

        }

        private void treeView_Loaded(object sender, Syncfusion.XForms.TreeView.TreeViewLoadedEventArgs e)
        {
                var count = Context.ImageNodeInfo!=null?Context.ImageNodeInfo.Count:0;
                var data = Context.ImageNodeInfo != null ? Context.ImageNodeInfo[count - 1]:null;
                treeView.BringIntoView(data);

        }

        private void treeView_ItemDoubleTapped(object sender, Syncfusion.XForms.TreeView.ItemDoubleTappedEventArgs e)
        {
            item = e.Node.Content as Catalogo;
            var pages = Navigation.NavigationStack.ToList();
            var previousPage = pages.ElementAt(pages.Count - 2);
            if (previousPage.ToString().Contains(nameof(AddEmpresaView))
             || previousPage.ToString().Contains(nameof(AddClienteView))
             || previousPage.ToString().Contains(nameof(AddUsuarioView))
             || previousPage.ToString().Contains(nameof(AddItemView))
             || previousPage.ToString().Contains(nameof(AddGestionView))
                )
            {
                Context.GoBack();
            }
            //RetornarRegistroSeleccionado();
        }


        

    private void RetornarRegistroSeleccionado()
        {
            if (item != null)
            {
                SendBackMessage(true);
            }
            else
            {
                SendBackMessage(false);
            }
        }

        private void SendBackMessage(bool hasData)
        {
            var pages = Navigation.NavigationStack.ToList();
            var previousPage = pages.ElementAt(pages.Count - 2);

            if (previousPage.ToString().Contains(nameof(AddEmpresaView)))
            {
                if (hasData)
                    MessagingCenter.Send<Catalogo>(item as Catalogo, Escant_App.AppSettings.Settings.CatalogoSeleccionadoAddEmpresaView);
                else
                    MessagingCenter.Send<Catalogo>(Escant_App.AppSettings.Settings.DefaultCatalogo, Escant_App.AppSettings.Settings.CatalogoSeleccionadoAddEmpresaView);
            }
            else if (previousPage.ToString().Contains(nameof(AddClienteView)))
            {
                if (hasData)
                    MessagingCenter.Send<Catalogo>(item as Catalogo, Escant_App.AppSettings.Settings.CatalogoSeleccionadoGeneroAddClienteView);
                else
                    MessagingCenter.Send<Catalogo>(Escant_App.AppSettings.Settings.DefaultCatalogo, Escant_App.AppSettings.Settings.CatalogoSeleccionadoGeneroAddClienteView);
            }
            else if (previousPage.ToString().Contains(nameof(AddUsuarioView)))
            {
                if (hasData)
                    MessagingCenter.Send<Catalogo>(item as Catalogo, Escant_App.AppSettings.Settings.CatalogoSeleccionadoPerfilAddUsuarioView);
                else
                    MessagingCenter.Send<Catalogo>(Escant_App.AppSettings.Settings.DefaultCatalogo, Escant_App.AppSettings.Settings.CatalogoSeleccionadoPerfilAddUsuarioView);
            }
            else if (previousPage.ToString().Contains(nameof(AddItemView)))
            {
                if (hasData)
                    MessagingCenter.Send<Catalogo>(item as Catalogo, Escant_App.AppSettings.Settings.CatalogoSeleccionadoAddItemView);
                else
                    MessagingCenter.Send<Catalogo>(Escant_App.AppSettings.Settings.DefaultCatalogo, Escant_App.AppSettings.Settings.CatalogoSeleccionadoAddItemView);
            }
            else if (previousPage.ToString().Contains(nameof(AddGestionView)))
            {
                if (hasData)
                    MessagingCenter.Send<Catalogo>(item as Catalogo, Escant_App.AppSettings.Settings.CatalogoSeleccionadoAddGestionView);
                else
                    MessagingCenter.Send<Catalogo>(Escant_App.AppSettings.Settings.DefaultCatalogo, Escant_App.AppSettings.Settings.CatalogoSeleccionadoAddGestionView);
            }
        }

    }
}