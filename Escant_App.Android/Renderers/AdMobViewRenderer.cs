using Android.Content;
using Android.Gms.Ads;
using Escant_App.UserControls;
using Escant_App.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Escant_App.AppSettings;

[assembly: ExportRenderer(typeof(AdmobControl), typeof(AdMobRenderer))]
namespace Escant_App.Droid.Renderers
{
    public class AdMobRenderer : ViewRenderer<AdmobControl, AdView>
    {
        public AdMobRenderer(Context context) : base(context)
        {
        }

        private int GetSmartBannerDpHeight()
        {
            var dpHeight = Resources.DisplayMetrics.HeightPixels / Resources.DisplayMetrics.Density;

            if (dpHeight <= 400)
            {
                return 32;
            }
            if (dpHeight <= 720)
            {
                return 50;
            }
            return 90;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<AdmobControl> e)
        {
            base.OnElementChanged(e);
            
                if (Control == null)
                {
                    var adView = new AdView(Context)
                    {
                        //AdSize = AdSize.SmartBanner,
                        AdSize = AdSize.Banner,
                        AdUnitId = SystemConstant.AdIdAndroid_Banner
                    };

                    var requestbuilder = new AdRequest.Builder();

                    //adView.LoadAd(requestbuilder.Build());
                    /*
                    adView.LoadAd(new AdRequest.Builder().Build());
                    */
                    //e.NewElement.HeightRequest = GetSmartBannerDpHeight();
                    e.NewElement.HeightRequest = 50;

                    //SetNativeControl(adView);
                    /*
                    RequestConfiguration.Builder builder = new RequestConfiguration.Builder();
                    builder = builder.SetTestDeviceIds(new string[] { "ca-app-pub-3940256099942544~3347511713" });
                    MobileAds.RequestConfiguration = builder.Build();
                    */

                }            
            
        }
    }
}
