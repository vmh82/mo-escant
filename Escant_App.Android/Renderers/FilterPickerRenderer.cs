﻿using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Support.V4.Content;
using Escant_App.Droid;
using Escant_App.UserControls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.Content;
using Android.OS;

[assembly: ExportRenderer(typeof(FilterPicker), typeof(FilterPickerRenderer))]
namespace Escant_App.Droid
{
    public class FilterPickerRenderer : PickerRenderer
    {
        FilterPicker element;

        public FilterPickerRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement == null)
            {
                var customPicker = e.NewElement as FilterPicker;
                if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
                {
                    Control?.SetPadding(10, 0, 0, 10);
                    Control.TextSize *= (customPicker.FontSize * 0.01f);

                    var layoutParams = new MarginLayoutParams(Control.LayoutParameters);
                    layoutParams.SetMargins(0, 0, 0, 0);

                    LayoutParameters = layoutParams;
                    Control.LayoutParameters = layoutParams;
                    Control.SetPadding(0, 0, 0, 0);
                    Control.SetLineSpacing(0, 0.1f);
                    SetPadding(0, 0, 0, 0);
                }
                else
                {
                    //Control?.SetPadding(0, 0, 0, 10);
                    Control.TextSize *= (customPicker.FontSize * 0.012f);

                    var layoutParams = new MarginLayoutParams(Control.LayoutParameters);
                    layoutParams.SetMargins(0, 0, 0, 10);

                    LayoutParameters = layoutParams;
                    Control.LayoutParameters = layoutParams;
                    Control.SetPadding(0, 0, 0, 10);
                    //Control.SetLineSpacing(0, 0.1f);
                    //SetPadding(0, 0, 0, 0);
                }

            }

            element = (FilterPicker)this.Element;

            if (Control != null && this.Element != null && !string.IsNullOrEmpty(element.Image))
                Control.Background = AddPickerStyles(element.Image);

         
        }

        public LayerDrawable AddPickerStyles(string imagePath)
        {
            ShapeDrawable border = new ShapeDrawable();
            border.Paint.Color = Android.Graphics.Color.White;
            border.SetPadding(0, 0, 0, 0);
            border.Paint.SetStyle(Paint.Style.Stroke);

            Drawable[] layers = { border, GetDrawable(imagePath) };
            LayerDrawable layerDrawable = new LayerDrawable(layers);
            layerDrawable.SetLayerInset(0, 0, 0, 0, 0);

            return layerDrawable;
        }

        private BitmapDrawable GetDrawable(string imagePath)
        {
            int resID = Resources.GetIdentifier(imagePath, "drawable", this.Context.PackageName);
            var drawable = ContextCompat.GetDrawable(this.Context, resID);
            var bitmap = ((BitmapDrawable)drawable).Bitmap;

            var result = new BitmapDrawable(Resources, Bitmap.CreateScaledBitmap(bitmap, 35, 35, true));
            result.Gravity = Android.Views.GravityFlags.Right;

            return result;
        }

    }
}