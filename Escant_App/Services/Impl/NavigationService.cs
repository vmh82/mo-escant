using ManijodaServicios.Offline.Interfaz;
using Escant_App.ViewModels;
using Escant_App.ViewModels.Base;
using Escant_App.ViewModels.Ventas;
using Escant_App.Views;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Escant_App.Services.Impl
{
    public class NavigationService : INavigationService
    {
        private readonly ISettingsService _settingsService;
        private readonly IAuthenticationService _authenticationService;
        public ViewModelBase PreviousPageViewModel
        {
            get
            {
                var mainPage = Application.Current.MainPage as NavigationView;
                var viewModel = mainPage.Navigation.NavigationStack[mainPage.Navigation.NavigationStack.Count - 1].BindingContext;
                return viewModel as ViewModelBase;
            }
        }
        public string PreviousPage
        {
            get
            {
                INavigation navigation = GetNavigation();
                var previousPage = navigation.NavigationStack[navigation.NavigationStack.Count - 2];
                return previousPage.ToString();
            }
        }

        public NavigationService(ISettingsService settingsService, IAuthenticationService authenticationService)
        {
            _settingsService = settingsService;
            _authenticationService = authenticationService;
        }

        public Task InitializeAsync()
        {
            //if (string.IsNullOrEmpty(_settingsService.AuthAccessToken))
            //    return NavigateToAsync<LoginViewModel>();
            //else
            //    return NavigateToAsync<MainViewModel>();
            //return NavigateToAsync<MainViewModel>();
            return NavigateToAsync<OrdenMaestroVentaViewModel>();
        }

        public Task NavigateToAsync<TViewModel>() where TViewModel : ViewModelBase
        {
            return InternalNavigateToAsync(typeof(TViewModel), null);
        }

        public Task NavigateToAsync<TViewModel>(object parameter) where TViewModel : ViewModelBase
        {
            return InternalNavigateToAsync(typeof(TViewModel), parameter);
        }
        public Task NavigateToAsync<TViewModel>(object parameter, object parameter1) where TViewModel : ViewModelBase
        {
            return InternalNavigateToAsync(typeof(TViewModel), parameter, parameter1);
        }
        public async Task NavigateToModalAsync<TViewModel>(object parameter) where TViewModel : ViewModelBase
        {
            await InternalNavigateToModalAsync(typeof(TViewModel), parameter);
        }
        public async Task<Page> NavigateToPopupAsync<TViewModel>() where TViewModel : ViewModelBase
        {
            return await InternalNavigateToPopupAsync(typeof(TViewModel), null);
        }

        public async Task<Page> NavigateToPopupAsync<TViewModel>(object parameter) where TViewModel : ViewModelBase
        {
            return await InternalNavigateToPopupAsync(typeof(TViewModel), parameter);
        }
        public async Task<Page> NavigateToPopupAsync<TViewModel>(object parameter,object parameter1) where TViewModel : ViewModelBase
        {
            return await InternalNavigateToPopupAsync(typeof(TViewModel), parameter, parameter1);
        }

        public Task NavigateToPageAsync(Page page, object parameter)
        {
            return InternalNavigateToAsync(page, parameter);
        }
        public async Task NavigateToRootPage()
        {
            INavigation navigation = GetNavigation();
            await navigation.PopAsync();
        }
        public async Task RemovePopupAsync()
        {
            INavigation navigation = GetNavigation();
            await navigation.PopPopupAsync();
        }
        public async Task RemoveModalAsync()
        {
            INavigation navigation = GetNavigation();
            await navigation.PopModalAsync();
        }
        public async Task RemoveLastFromBackStackAsync()
        {
            INavigation navigation = GetNavigation();
            await navigation.PopAsync();
        }

        public Task RemoveBackStackAsync()
        {
            INavigation navigation = GetNavigation();
           
            if (navigation != null)
            {
                for (int i = 0; i < navigation.NavigationStack.Count - 2; i++)
                {
                    var page = navigation.NavigationStack[i];
                    navigation.RemovePage(page);
                }
            }

            return Task.FromResult(true);
        }

        private async Task InternalNavigateToAsync(Type viewModelType, object parameter)
        {
            Page page = CreatePage(viewModelType, parameter);
            INavigation navigation = GetNavigation();
            
            await navigation.PushAsync(page);
            await (page.BindingContext as ViewModelBase).InitializeAsync(parameter);
        }

        private async Task InternalNavigateToAsync(Type viewModelType, object parameter,object parameter1)
        {
            Page page = CreatePage1(viewModelType, parameter,parameter1);
            INavigation navigation = GetNavigation();

            await navigation.PushAsync(page);
            await (page.BindingContext as ViewModelBase).InitializeAsync(parameter,parameter1);            

        }

        private async Task InternalNavigateToModalAsync(Type viewModelType, object parameter)
        {
            Page page = CreatePage(viewModelType, parameter);
            INavigation navigation = GetNavigation();
            await navigation.PushModalAsync(page);
            await (page.BindingContext as ViewModelBase).InitializeAsync(parameter);
        }
        private async Task<Page> InternalNavigateToPopupAsync(Type viewModelType, object parameter)
        {
            Page page = CreatePage(viewModelType, parameter);
            try
            {
                INavigation navigation = GetNavigation();

                await (page.BindingContext as ViewModelBase).InitializeAsync(parameter);
                await navigation.PushPopupAsync(page as PopupPage);
            }
            catch(Exception ex)
            {

            }
            return page;
        }
        private async Task<Page> InternalNavigateToPopupAsync(Type viewModelType, object parameter,object parameter1)
        {
            Page page = CreatePage1(viewModelType, parameter,parameter1);
            INavigation navigation = GetNavigation();

            await (page.BindingContext as ViewModelBase).InitializeAsync(parameter,parameter1);
            await navigation.PushPopupAsync(page as PopupPage);
            return page;
        }
        private async Task InternalNavigateToAsync(Page page, object parameter)
        {
            INavigation navigationPage = GetNavigation();
            await navigationPage.PushAsync(page);
            await (page.BindingContext as ViewModelBase).InitializeAsync(parameter);
        }
        private INavigation GetNavigation()
        {
            INavigation navigationPage;
            if (App.Current.MainPage is MasterDetailPage masterDetailPage)
            {
                if (masterDetailPage.Detail is NavigationPage navPage)
                {
                    navigationPage = navPage.Navigation;
                }
                else
                {
                    var detailNavigationPage = new NavigationPage(masterDetailPage);
                    navigationPage = detailNavigationPage.Navigation;
                }
            }
            else
            {
                navigationPage = App.Current.MainPage.Navigation;
            }
            return navigationPage;
        }
        private Type GetPageTypeForViewModel(Type viewModelType)
        {
            var viewName = viewModelType.FullName.Replace("Model", string.Empty);
            var viewModelAssemblyName = viewModelType.GetTypeInfo().Assembly.FullName;
            var viewAssemblyName = string.Format(CultureInfo.InvariantCulture, "{0}, {1}", viewName, viewModelAssemblyName);
            var viewType = Type.GetType(viewAssemblyName);
            return viewType;
        }

        private Page CreatePage(Type viewModelType, object parameter)
        {
            Type pageType = GetPageTypeForViewModel(viewModelType);
            if (pageType == null)
            {
                throw new Exception($"Cannot locate page type for {viewModelType}");
            }
            Page page = Activator.CreateInstance(pageType) as Page;
            return page;
        }
        private Page CreatePage1(Type viewModelType, object parameter,object parameter1)
        {
            Type pageType = GetPageTypeForViewModel(viewModelType);
            if (pageType == null)
            {
                throw new Exception($"Cannot locate page type for {viewModelType}");
            }
            Page page = Activator.CreateInstance(pageType) as Page;
            return page;
        }
    }
}
