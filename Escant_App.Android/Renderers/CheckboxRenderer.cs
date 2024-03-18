using Xamarin.Forms;
using System.ComponentModel;
using Xamarin.Forms.Platform.Android;
using Android.Content;
using Android.Content.Res;
using Android.Widget;
using AndroidX.AppCompat.Widget;
using Escant_App.Droid.Renderers;
using Escant_App.UserControls;


[assembly: ExportRenderer(typeof(Checkbox), typeof(CheckboxRenderer))]
namespace Escant_App.Droid.Renderers
{

    public class CheckboxRenderer : ViewRenderer<Checkbox, AppCompatCheckBox>, CompoundButton.IOnCheckedChangeListener
    {
        private const int DEFAULT_SIZE = 28;

        public CheckboxRenderer(Context context) : base(context)
        {
        }

        /// <summary>
        /// Used for registration with dependency service to ensure it isn't linked out
        /// </summary>
        public static void Init()
        {
            // intentionally empty
        }

        /// <summary>
        /// Update element bindable property from event
        /// </summary>
        /// <param name="buttonView">Button view.</param>
        /// <param name="isChecked">If set to <c>true</c> is checked.</param>
        public void OnCheckedChanged(CompoundButton buttonView, bool isChecked)
        {
            ((IViewController)Element).SetValueFromRenderer(Checkbox.IsCheckedProperty, isChecked);
            Element.CheckedCommand?.Execute(Element.CheckedCommandParameter);
        }

        public override SizeRequest GetDesiredSize(int widthConstraint, int heightConstraint)
        {
            var sizeConstraint = base.GetDesiredSize(widthConstraint, heightConstraint);

            if (sizeConstraint.Request.Width == 0)
            {
                var width = widthConstraint;
                if (widthConstraint <= 0)
                {
                    System.Diagnostics.Debug.WriteLine("Default values");
                    width = DEFAULT_SIZE;
                }
                else if (widthConstraint <= 0)
                {
                    width = DEFAULT_SIZE;
                }

                sizeConstraint = new SizeRequest(new Size(width, sizeConstraint.Request.Height),
                    new Size(width, sizeConstraint.Minimum.Height));
            }

            return sizeConstraint;
        }


        /// <summary>
        /// Called when the control is created or changed
        /// </summary>
        /// <param name="e">E.</param>
        protected override void OnElementChanged(ElementChangedEventArgs<Checkbox> e)
        {
            base.OnElementChanged(e);
            if (e.NewElement != null)
            {
                if (Control == null)
                {
                    var checkBox = new AppCompatCheckBox(Context);

                    if (Element.OutlineColor != default(Color))
                    {
                        var backgroundColor = GetBackgroundColorStateList(Element.OutlineColor);
                        checkBox.SupportButtonTintList = backgroundColor;
                        checkBox.BackgroundTintList = GetBackgroundColorStateList(Element.InnerColor);
                        checkBox.ForegroundTintList = GetBackgroundColorStateList(Element.OutlineColor);

                    }
                    checkBox.SetOnCheckedChangeListener(this);
                    SetNativeControl(checkBox);
                }

                Control.Checked = e.NewElement.IsChecked;
            }
        }


        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName == nameof(Element.IsChecked))
            {
                Control.Checked = Element.IsChecked;
            }
            else
            {
                var backgroundColor = GetBackgroundColorStateList(Element.CheckColor);
                Control.SupportButtonTintList = backgroundColor;
                Control.BackgroundTintList = GetBackgroundColorStateList(Element.InnerColor);
                Control.ForegroundTintList = GetBackgroundColorStateList(Element.OutlineColor);
            }
        }

        /// <summary>
        /// Sync from native control
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        private void CheckBoxCheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            Element.IsChecked = e.IsChecked;
        }


        private ColorStateList GetBackgroundColorStateList(Color color)
        {
            return new ColorStateList(
                new[]
                {
                new[] {-global::Android.Resource.Attribute.StateEnabled}, // checked
                new[] {-global::Android.Resource.Attribute.StateChecked}, // unchecked
                new[] {global::Android.Resource.Attribute.StateChecked} // checked
                },
                new int[]
                {
                color.WithSaturation(0.1).ToAndroid(),
                color.ToAndroid(),
                color.ToAndroid()
                });
        }

    }

    /*
    public class CheckboxCustomRenderer : ViewRenderer<Checkbox, CheckBox>
    {
        private CheckBox checkBox;

        public CheckboxCustomRenderer(Context context) : base(context)
        {
        }
        protected override void OnElementChanged(ElementChangedEventArgs<Checkbox> e)
        {
            base.OnElementChanged(e);
            var model = e.NewElement;
            checkBox = new CheckBox(Context);
            checkBox.Tag = this;
            CheckboxPropertyChanged(model, null);
            checkBox.SetOnClickListener(new ClickListener(model));
            SetNativeControl(checkBox);
        }
        private void CheckboxPropertyChanged(Checkbox model, string propertyName)
        {
            if(propertyName == null || Checkbox.IsCheckedProperty.PropertyName == propertyName)
            {
                checkBox.Checked = model.IsChecked;
            }
            if(propertyName == null || Checkbox.ColorProperty.PropertyName == propertyName)
            {
                int[][] states =
                {
                    new int[] {Android.Resource.Attribute.StateEnabled}, //enabled
                    new int[] { Android.Resource.Attribute.StateEnabled }, //disabled
                    new int[] { Android.Resource.Attribute.StateChecked }, //unchecked
                    new int[] { Android.Resource.Attribute.StatePressed }  //pressed
                };
                var checkBoxColor = (int)model.Color.ToAndroid();
                int[] colors = { checkBoxColor, checkBoxColor, checkBoxColor, checkBoxColor };
                var myList = new Android.Content.Res.ColorStateList(states, colors);
                checkBox.ButtonTintList = myList;
            }
        }
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (checkBox != null)
            {
                base.OnElementPropertyChanged(sender, e);
                CheckboxPropertyChanged((Checkbox)sender, e.PropertyName);
            }
        }
        public class ClickListener : Java.Lang.Object, IOnClickListener
        {
            private Checkbox _myCheckbox;
            public ClickListener(Checkbox myCheckbox)
            {
                this._myCheckbox = myCheckbox;
            }
            public void OnClick(global::Android.Views.View v)
            {
                _myCheckbox.IsChecked = !_myCheckbox.IsChecked;
            }
        }
    }
    */
}
