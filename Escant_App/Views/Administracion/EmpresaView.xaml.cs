﻿using DataModel.DTO.Administracion;
using Escant_App.ViewModels.Administracion;
using Escant_App.Views.Clientes;
using Escant_App.Views.Compras;
using Escant_App.Views.Productos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Escant_App.Views.Administracion
{
    /// <summary>
    /// Pantalla Maestra de las empresas y proveedores 
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EmpresaView : ContentPage
    {
        #region 0.InstanciaVinculaClase
        /// <summary>
        /// Instanciación de la clase con el FrontEnd
        /// </summary>
        public EmpresaViewModel Context => (EmpresaViewModel)this.BindingContext;
        #endregion 0.InstanciaVinculaClase

        #region 1.Constructor
        /// <summary>
        /// Constructor del código FrontEnd
        /// </summary>
        public EmpresaView()
        {
            InitializeComponent();            
        }
        #endregion 1.Constructor

        #region 2.MétodosFrontEnd
        /// <summary>
        /// Método Sobreescrito del evento aparecer de la Pantalla
        /// </summary>
        protected override void OnAppearing()
        {
            base.OnAppearing();
            Context.ListView = registroListView;
            registroListView?.ResetSwipe();
            Task.Run(async () =>
            {
                IsBusy = true;
                var pages = Navigation.NavigationStack.ToList();
                Context.PaginaPrevia = pages.ElementAt(pages.Count - 2);
                await Context.LoadData();
                IsBusy = false;
            });
        }
        /// <summary>
        /// Métodos Sobreescrito del evento desaparecer de la pantalla
        /// </summary>
        protected override void OnDisappearing()
        {
            RetornarRegistroSeleccionado();
            base.OnDisappearing();
        }
        /// <summary>
        /// Método para manejar el Retorno a una pantalla previa que la llamó
        /// </summary>
        private void RetornarRegistroSeleccionado()
        {
            if (registroListView.SelectedItem != null)
            {
                SendBackMessage(true);
            }
            else
            {
                SendBackMessage(false);
            }
        }
        /// <summary>
        /// Méotodo para Regresar los datos seleccionados a la pantalla de llamada
        /// </summary>
        /// <param name="hasData"></param>
        private void SendBackMessage(bool hasData)
        {
            var pages = Navigation.NavigationStack.ToList();
            var previousPage = pages.ElementAt(pages.Count - 2);

            if (previousPage.ToString().Contains(nameof(AddClienteView)))
            {
                if (hasData)
                    MessagingCenter.Send<Empresa>(registroListView.SelectedItem as Empresa, Escant_App.AppSettings.Settings.EmpresaSeleccionadaAddClienteView);
                else
                    MessagingCenter.Send<Empresa>(Escant_App.AppSettings.Settings.DefaultEmpresa, Escant_App.AppSettings.Settings.EmpresaSeleccionadaAddClienteView);
            }
            else if (previousPage.ToString().Contains(nameof(AddItemView)))
            {
                if (hasData)
                    MessagingCenter.Send<Empresa>(registroListView.SelectedItem as Empresa, Escant_App.AppSettings.Settings.EmpresaSeleccionadaAddItemView);
                else
                    MessagingCenter.Send<Empresa>(Escant_App.AppSettings.Settings.DefaultEmpresa, Escant_App.AppSettings.Settings.EmpresaSeleccionadaAddItemView);
            }
            else if (previousPage.ToString().Contains(nameof(IngresoComprasView)))
            {
                if (hasData)
                    MessagingCenter.Send<Empresa>(registroListView.SelectedItem as Empresa, Escant_App.AppSettings.Settings.EmpresaSeleccionadaIngresoComprasView);
                else
                    MessagingCenter.Send<Empresa>(Escant_App.AppSettings.Settings.DefaultEmpresa, Escant_App.AppSettings.Settings.EmpresaSeleccionadaIngresoComprasView);
            }
        }
        /// <summary>
        /// Método para buscar un valor en la pantalla
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void SearchBar_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            //await ((EmpresaViewModel)this.BindingContext).SearchRegistro(e.NewTextValue);
            if (registroListView.DataSource != null)
            {
                registroListView.DataSource.Filter = FilterContacts;
                registroListView.DataSource.RefreshFilter();
            }
            registroListView.RefreshView();
        }
        /// <summary>
        /// Método para disparar el inicio del botón deslizar para edición y/o mantenimiento
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Handle_SwipeStarted(object sender, Syncfusion.ListView.XForms.SwipeStartedEventArgs e)
        {
            Context.ItemIndex = -1;
        }
        /// <summary>
        /// Método para configurar el fin del botón deslizar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>        
        void Handle_SwipeEnded(object sender, Syncfusion.ListView.XForms.SwipeEndedEventArgs e)
        {
            Context.ItemIndex = e.ItemIndex;
        }
        /// <summary>
        /// Método para manejar el cambio de pantallas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        async void Handle_SelectionChanged(object sender, Syncfusion.ListView.XForms.ItemSelectionChangedEventArgs e)
        {
            var pages = Navigation.NavigationStack.ToList();
            var previousPage = pages.ElementAt(pages.Count - 2);
            if ( previousPage.ToString().Contains(nameof(AddClienteView))
                || previousPage.ToString().Contains(nameof(AddItemView))                
                )
            {
                await Context.GoBack();
            }
        }
        /// <summary>
        /// método para filtrar los datos a buscar en pantalla
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>

        private bool FilterContacts(object obj)
        {
            if (barraBusqueda == null || barraBusqueda.Text == null)
                return true;

            var taskInfo = obj as Empresa;
            return (taskInfo.Codigo.ToLower().Contains(barraBusqueda.Text.ToLower())
                || taskInfo.Descripcion.ToLower().Contains(barraBusqueda.Text.ToLower()));
        }

        #endregion 2.MétodosFrontEnd
    }
}