using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Drawing;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Escant_App.ViewModels.Base;
using Escant_App.Interfaces;

namespace Escant_App.Generales
{
    public class GeneraDocumentos:ViewModelBase
    {
        public GeneraDocumentos()
        {

        }
        public async Task<Stream> generaOrdenCompra(bool guardar)
        {
            Stream resultado;
            //Create a new PDF document
            PdfDocument document = new PdfDocument();
            //Add page to the PDF document
            PdfPage page = document.Pages.Add();
            //Create graphics instance
            PdfGraphics graphics = page.Graphics;
            Stream imageStream = null;

            //Captures the XAML page as image and returns the image in memory stream
            imageStream = new MemoryStream(DependencyService.Get<ISaveFile>().CaptureAsync().Result);

            //Load the image in PdfBitmap
            PdfBitmap image = new PdfBitmap(imageStream);

            //Draw the image to the page
            //graphics.DrawImage(image, 0, 0, page.GetClientSize().Width, page.GetClientSize().Height);

            //Set layout property to make the element break across the pages
            PdfLayoutFormat format = new PdfLayoutFormat();
            format.Break = PdfLayoutBreakType.FitPage;
            format.Layout = PdfLayoutType.Paginate;
            //Set paginate bounds.
            format.PaginateBounds = new RectangleF(0, 0, 500, 350);


            //Draw image
            //image.Draw(page, 0, 0, format);
            image.Draw(page, new RectangleF(0, 0, 500, 700), format);

            //Save the document into memory stream
            MemoryStream stream = new MemoryStream();
            document.Save(stream);

            stream.Position = 0;
            //Save the stream as a file in the device and invoke it for viewing
            if (guardar)
            {
                Xamarin.Forms.DependencyService.Get<ISaveFile>().SaveFile("test2.pdf", "application/pdf", stream);
            }
            resultado = stream;
            return resultado;
        }
    }
}
