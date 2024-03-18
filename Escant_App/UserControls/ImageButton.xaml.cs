using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Escant_App.UserControls
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ImageButton : ContentView
    {
        public event EventHandler Clicked;

        public static readonly BindableProperty CommandProperty =
            BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(Button), null);

        public ICommand Command
        {
            get
            {
                return (ICommand)GetValue(CommandProperty);
            }
            set
            {
                SetValue(CommandProperty, value);
            }
        }

        // Add CommandParameter Binding
        public static readonly BindableProperty CommandParameterProperty = 
            BindableProperty.Create(nameof(CommandParameter), typeof(string), typeof(ImageButton), null);
        public string CommandParameter
        {
            get { return (string)GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        public static readonly BindableProperty ButtonBackgroundColorProperty =
            BindableProperty.Create(nameof(ButtonBackgroundColor), typeof(Color), typeof(ImageButton), Color.Transparent);

        public Color ButtonBackgroundColor
        {
            get
            {
                return (Color)GetValue(ButtonBackgroundColorProperty);
            }
            set
            {
                SetValue(ButtonBackgroundColorProperty, value);
            }
        }

        public static readonly BindableProperty TextProperty =
            BindableProperty.Create(nameof(Text), typeof(string), typeof(ImageButton), null);

        public string Text
        {
            get
            {
                return (string)GetValue(TextProperty);
            }
            set
            {
                SetValue(TextProperty, value);
            }
        }

        public static readonly BindableProperty SourceProperty =
            BindableProperty.Create(nameof(Source), typeof(FileImageSource), typeof(ImageButton), null);

        public FileImageSource Source
        {
            get
            {
                return (FileImageSource)GetValue(SourceProperty);
            }
            set
            {
                SetValue(SourceProperty, value);
            }
        }

        public ImageButton()
        {
            InitializeComponent();
            root.BindingContext = this;
        }

        async void HandleClick(object sender, EventArgs e)
        {
            Clicked.Invoke(this, e);

            await root.ScaleTo(1.2, 100);
            await root.ScaleTo(1, 100);
        }
    }
}