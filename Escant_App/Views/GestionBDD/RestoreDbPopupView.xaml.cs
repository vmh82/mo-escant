using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DataModel.DTO;
using Escant_App.ViewModels;
using Escant_App.ViewModels.GestionBDD;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;

namespace Escant_App.Views.GestionBDD
{
    public partial class RestoreDbPopupView : PopupPage
    {
        public RestoreDbPopupViewModel Context => (RestoreDbPopupViewModel)this.BindingContext;

        public RestoreDbPopupView()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Context.ListView = RestoreListView;
            Task.Run(async () =>
            {
                await Context.LoadData();
            });
        }

        void OnRestoreListView_SelectionChanged(object sender, Syncfusion.ListView.XForms.ItemSelectionChangedEventArgs e)
        {
            if (RestoreListView.SelectedItem != null)
                Context.SelectedRestoreFile = (FileDto)RestoreListView.SelectedItem;
        }
    }
}
