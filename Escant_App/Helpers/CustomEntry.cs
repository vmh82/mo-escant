using System;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
namespace Escant_App.Helpers
{
    [Preserve(AllMembers = true)]
    public class CustomEntry : Entry
    {
        public Syncfusion.SfPdfViewer.XForms.SfPdfViewer PdfViewer
        {
            get;
            set;
        }

        public bool IsPageNumberEntry
        {
            get; set;
        }

        public CustomEntry()
        {
        }
    }
}
