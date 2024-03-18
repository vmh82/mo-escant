using System;
using System.Reflection;
using Android.Content.Res;
using Android.Content;
using Escant_App.Droid.Renderers;
using Escant_App.Views;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android.AppCompat;
using AndroidX.Fragment.App;


[assembly: ExportRenderer(typeof(NavigationPage), typeof(AnimationNavigationRenderer))]

namespace Escant_App.Droid.Renderers
{
    public class AnimationNavigationRenderer : NavigationPageRenderer, Android.Views.View.IOnClickListener
    {
        public AnimationNavigationRenderer(Context context) : base(context)
        {
        }
        protected override void SetupPageTransition(FragmentTransaction transaction, bool isPush)
        {
            if (isPush)
                transaction.SetCustomAnimations(Resource.Animation.enter_from_right, Resource.Animation.exit_to_left,
                    Resource.Animation.enter_from_left, Resource.Animation.exit_to_right);
            else
            {
                transaction.SetCustomAnimations(Resource.Animation.enter_from_left, Resource.Animation.exit_to_right,
                    Resource.Animation.enter_from_right, Resource.Animation.exit_to_left);

            }
        }

        /*
        #region nav toolbar

        private static readonly FieldInfo ToolbarFieldInfo;

        private bool _disposed;
        private AToolbar _toolbar;

        static AnimationNavigationRenderer()
        {
            // get _toolbar private field info
            ToolbarFieldInfo = typeof(NavigationPageRenderer).GetField("_toolbar",
                    BindingFlags.NonPublic | BindingFlags.Instance)
                ;
        }

        public void OnClick(Android.Views.View v)
        {
            // Call the NavigationPage which will trigger the default behavior
            // The default behavior is to navigate back if the Page derived classes return true from OnBackButtonPressed override            
            var curPage = Element.CurrentPage as MainView;
            if (curPage == null)
            {
                Element.PopAsync();
            }
            else
            {
                if (curPage.NeedOverrideSoftBackButton)
                    curPage.OnSoftBackButtonPressed();
                else Element.PopAsync();
            }
        }

        protected override void OnLayout(bool changed, int l, int t, int r, int b)
        {
            base.OnLayout(changed, l, t, r, b);

            UpdateToolbarInstance();
        }

        protected override void OnConfigurationChanged(Configuration newConfig)
        {
            base.OnConfigurationChanged(newConfig);

            UpdateToolbarInstance();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && !_disposed)
            {
                _disposed = true;

                RemoveToolbarInstance();
            }

            base.Dispose(disposing);
        }

        private void UpdateToolbarInstance()
        {
            RemoveToolbarInstance();
            GetToolbarInstance();
        }

        private void GetToolbarInstance()
        {
            try
            {
                //sai o cho nay nay
                //how to get toolbar navigation page
                _toolbar = (AToolbar)ToolbarFieldInfo.GetValue(this);
                //var mi = t.GetMethod(“BarOnNavigationClick”, BindingFlags.NonPublic | BindingFlags.Instance);
                _toolbar.SetNavigationOnClickListener(this);
            }
            catch (Exception exception)
            {
                System.Diagnostics.Debug.WriteLine($"Can't get toolbar with error: {exception.Message}");
            }
        }

        private void RemoveToolbarInstance()
        {
            if (_toolbar == null) return;
            _toolbar.SetNavigationOnClickListener(null);
            _toolbar = null;
        }

        #endregion
        */
    }
}