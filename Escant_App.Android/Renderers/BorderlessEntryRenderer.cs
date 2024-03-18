using Android.Content;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Escant_App.Droid.Renderers;
using Escant_App.UserControls;
using Android.Widget;
using System.ComponentModel;
using Android.Text.Method;

[assembly: ExportRenderer(typeof(BorderlessEntry), typeof(BorderlessEntryRenderer))]
namespace Escant_App.Droid.Renderers
{
    public class BorderlessEntryRenderer : EntryRenderer
    {
        private BorderlessEntry element;
        private EditText native;
        public BorderlessEntryRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            element = (BorderlessEntry)Element ?? null;
            native = Control as EditText;

            UpdateKeyboard();
            if (e.OldElement == null)
            {
                Control.Background = null;
                var nativeEditText = (EditText)Control;
                nativeEditText.SetSelectAllOnFocus(true);
            }
        }
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (Control == null) return;

            else if (e.PropertyName == InputView.KeyboardProperty.PropertyName)
                UpdateKeyboard();
        }

        private void UpdateKeyboard()
        {
            //Implementation of the numeric keyboard (we simply add the NumberFlagSigned)
            native = Control as EditText;

            var defaultNumericKeyboard = Android.Text.InputTypes.ClassNumber | Android.Text.InputTypes.NumberFlagDecimal;
            var correnctNumericKeyboard = Android.Text.InputTypes.ClassNumber | Android.Text.InputTypes.NumberFlagSigned | Android.Text.InputTypes.NumberFlagDecimal;

            if (native.InputType == defaultNumericKeyboard)
            {
                native.InputType =
                Android.Text.InputTypes.ClassNumber |
                Android.Text.InputTypes.NumberFlagSigned |
                Android.Text.InputTypes.NumberFlagDecimal;
                native.KeyListener = DigitsKeyListener.GetInstance(string.Format("7890.-"));
            }
            else if (native.InputType == correnctNumericKeyboard)
            {
                // Even though in the next line the InputType is set to the same it is already set, this seems to 
                // fix the problem with the decimal separator: Namely, a local other than english is set, the point 
                // does not have any effect in the numeric keyboard. Re-setting the InputType seems to fix this.
                native.InputType =
                Android.Text.InputTypes.ClassNumber |
                Android.Text.InputTypes.NumberFlagSigned |
                Android.Text.InputTypes.NumberFlagDecimal;
            }
        }
    }
}