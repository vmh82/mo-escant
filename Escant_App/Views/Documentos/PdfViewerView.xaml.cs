using Syncfusion.SfPdfViewer.XForms;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.Internals;
using Xamarin.Forms;
using System.IO;
using Xamarin.Forms.Xaml;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Drawing;
using System.Data;
using Syncfusion.Pdf.Grid;
using Escant_App.ViewModels.Documentos;
using Color = Xamarin.Forms.Color;
using Escant_App.Helpers;
using Escant_App.Interfaces;
using ManijodaServicios.AppSettings;
using DataModel.Helpers;
using ManijodaServicios.Resources.Texts;
using Escant_App.Views.Compras;
using DataModel.DTO.Compras;
using Escant_App.Views.Ventas;
using Xamarin.Essentials;

namespace Escant_App.Views.Documentos
{
    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PdfViewerView : ContentPage
    {
        bool m_isPageInitiated = false;
        bool isHighlightClicked = true;
        bool isUnderlineClicked = true;
        bool isStrikeThroughClicked = true;
        bool isAnnotationEditMode = false;
        TextMarkupAnnotation selectedAnnotation;
        Stream fileStream;
        #region 1. Vinculación View con ViewModel
        public PdfViewerViewModel Context => (PdfViewerViewModel)this.BindingContext;
        #endregion 1. Vinculación View con ViewModel

        public PdfViewerView()
        {
            InitializeComponent();
            try { 
            if (Application.Current.MainPage != null)
            {
                toolbar.WidthRequest = Application.Current.MainPage.Width;
                searchBar.WidthRequest = Application.Current.MainPage.Width;
            }
            textSearchEntry.TextChanged += TextSearchEntry_TextChanged;
            pdfViewerControl.UnhandledConditionOccurred += PdfViewerControl_UnhandledCondition;
            pdfViewerControl.TextMatchFound += PdfViewerControl_TextMatchFound;
            pdfViewerControl.SearchCompleted += PdfViewerControl_SearchCompleted;
            pdfViewerControl.CanUndoModified += PdfViewerControl_CanUndoModified;
            pdfViewerControl.CanRedoModified += PdfViewerControl_CanRedoModified;
            Context.HighlightColor = pdfViewerControl.AnnotationSettings.TextMarkup.Highlight.Color;
            Context.UnderlineColor = pdfViewerControl.AnnotationSettings.TextMarkup.Underline.Color;
            Context.StrikeThroughColor = pdfViewerControl.AnnotationSettings.TextMarkup.Strikethrough.Color;
            //cancelSearchButton.Clicked += CancelSearchButton_Clicked;
            m_isPageInitiated = true;
            pageNumberEntry.PdfViewer = pdfViewerControl;
            pageNumberEntry.IsPageNumberEntry = true;
            pageNumberEntry.Completed += PageNumberEntry_Completed;
            }
            catch(Exception ex)
            {

            }
        }
        protected override void OnDisappearing()
        {
            SendBackMessage(true);
            base.OnDisappearing();
        }

        private void PageNumberEntry_Completed(object sender, EventArgs e)
        {
            int pageNumber = 0;
            int.TryParse((sender as CustomEntry).Text, out pageNumber);

            if (pageNumber != 0)
                pdfViewerControl.GoToPage(pageNumber);
        }

        private void PdfViewerControl_CanRedoModified(object sender, CanRedoModifiedEventArgs args)
        {
            if (args.CanRedo)
                redoButton.TextColor = Color.FromHex("#0076FF");
            else
                redoButton.TextColor = Color.FromHex("#808080");
        }

        private void PdfViewerControl_CanUndoModified(object sender, CanUndoModifiedEventArgs args)
        {
            if (args.CanUndo)
                undoButton.TextColor = Color.FromHex("#0076FF");
            else
                undoButton.TextColor = Color.FromHex("#808080");
        }


        private void PdfViewerControl_TextMarkupSelected(object sender, TextMarkupSelectedEventArgs args)
        {
            selectedAnnotation = sender as TextMarkupAnnotation;
            isAnnotationEditMode = true;
            (BindingContext as PdfViewerViewModel).DeleteButtonColor = new Color(selectedAnnotation.Settings.Color.R, selectedAnnotation.Settings.Color.G, selectedAnnotation.Settings.Color.B);
        }

        private void PdfViewerControl_TextMarkupDeselected(object sender, TextMarkupDeselectedEventArgs args)
        {
            selectedAnnotation = null;
            isAnnotationEditMode = false;
        }

        private void DeleteButton_Clicked(object sender, EventArgs e)
        {
            if (selectedAnnotation != null)
            {
                pdfViewerControl.RemoveAnnotation(selectedAnnotation);
                selectedAnnotation = null;
                isAnnotationEditMode = false;
            }
        }

        private void CancelSearchButton_Clicked(object sender, EventArgs e)
        {
            textSearchEntry.Text = string.Empty;
        }

        private async void SaveButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                string filePath = DependencyService.Get<ISaveFile>().SaveMemory(pdfViewerControl.SaveDocument() as MemoryStream);
                /*Task.Run(async () =>
                {
                    await Context.GuardaArchivo(filePath);
                });*/
                await Context.GuardaArchivo(filePath);
                string message = "The PDF has been saved to " + filePath;
                DependencyService.Get<IAlertView>().Show(message);
            }
            catch(Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Importante", ex.Message.ToString(), "OK");
            }
        }

        void PdfViewerControl_TextMatchFound(object sender, TextMatchFoundEventArgs args)
        {
            searchNextButton.IsVisible = true;
            searchPreviousButton.IsVisible = true;
        }


        private void PdfViewerControl_SearchCompleted(object sender, TextSearchCompletedEventArgs args)
        {
            if (args.NoMatchFound)
            {
                DependencyService.Get<IAlertView>().Show("\"" + args.TargetText + "\"" + " not found.");
                searchNextButton.IsVisible = false;
                searchPreviousButton.IsVisible = false;
            }
            else if (args.NoMoreOccurrence)
                DependencyService.Get<IAlertView>().Show("No more occurrences.");
        }

        private void PdfViewerControl_UnhandledCondition(object sender, UnhandledConditionEventArgs args)
        {
            DependencyService.Get<IAlertView>().Show(args.Description);
        }

        private void TextSearchEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            if ((sender as Entry).Text == string.Empty)
            {
                pdfViewerControl.CancelSearch();
                searchNextButton.IsVisible = false;
                searchPreviousButton.IsVisible = false;
                //cancelSearchButton.IsVisible = false;
            }
            //else
              //  cancelSearchButton.IsVisible = true;
        }
        private void OnSearchClicked(object sender, EventArgs e)
        {
            textSearchEntry.Focus();
            pdfViewerControl.AnnotationMode = AnnotationMode.None;
        }

        private void OnShareButtonClicked(object sender, EventArgs e)
        {
            string filePath = DependencyService.Get<ISaveFile>().SaveMemory(pdfViewerControl.SaveDocument() as MemoryStream);
            //string filePath = DependencyService.Get<ISaveFile>().SavePdf("documentoPdf.pdf", "application/pdf", pdfViewerControl.SaveDocument() as MemoryStream);
            //DependencyService.Get<IShareFile>().Show("Share", "Sharing PDF", filePath);
            ShareAsync(filePath);
        }

        private async Task ShareAsync(string filePath)
        {
            var streamFile =File.Open(filePath, FileMode.Open);
            var cacheFile = Path.Combine(FileSystem.CacheDirectory, "documentoPdf.pdf");
            using (var file = new FileStream(cacheFile, FileMode.Create, FileAccess.Write))
            {
                streamFile.CopyTo(file);
            }
            var request = new ShareFileRequest();

            request.Title = "Share document";
            request.File = new ShareFile(cacheFile);
            await Share.RequestAsync(request);
        }

        private void OnCancelClicked(object sender, EventArgs e)
        {
            textSearchEntry.Text = string.Empty;
            textSearchEntry.Focus();
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            if (Application.Current.MainPage != null && Application.Current.MainPage.Width > 0 && m_isPageInitiated)
            {
                toolbar.WidthRequest = Application.Current.MainPage.Width;
                searchBar.WidthRequest = Application.Current.MainPage.Width;
            }
        }

        private void ColorButton_Clicked(object sender, EventArgs e)
        {
            switch ((sender as Button).CommandParameter.ToString())
            {
                case "Cyan":
                    {
                        if (!isAnnotationEditMode)
                        {
                            if (pdfViewerControl.AnnotationMode == AnnotationMode.Highlight)
                                pdfViewerControl.AnnotationSettings.TextMarkup.Highlight.Color = Color.FromHex("#00FFFF");
                            if (pdfViewerControl.AnnotationMode == AnnotationMode.Underline)
                                pdfViewerControl.AnnotationSettings.TextMarkup.Underline.Color = Color.FromHex("#00FFFF");
                            if (pdfViewerControl.AnnotationMode == AnnotationMode.Strikethrough)
                                pdfViewerControl.AnnotationSettings.TextMarkup.Strikethrough.Color = Color.FromHex("#00FFFF");
                        }
                        else
                        {
                            if (selectedAnnotation != null)
                                selectedAnnotation.Settings.Color = Color.FromHex("#00FFFF");
                        }
                    }
                    break;

                case "Green":
                    {
                        if (!isAnnotationEditMode)
                        {
                            if (pdfViewerControl.AnnotationMode == AnnotationMode.Highlight)
                                pdfViewerControl.AnnotationSettings.TextMarkup.Highlight.Color = Color.Green;
                            if (pdfViewerControl.AnnotationMode == AnnotationMode.Underline)
                                pdfViewerControl.AnnotationSettings.TextMarkup.Underline.Color = Color.Green;
                            if (pdfViewerControl.AnnotationMode == AnnotationMode.Strikethrough)
                                pdfViewerControl.AnnotationSettings.TextMarkup.Strikethrough.Color = Color.Green;
                        }
                        else
                        {
                            if (selectedAnnotation != null)
                                selectedAnnotation.Settings.Color = Color.Green;
                        }
                    }
                    break;

                case "Yellow":
                    {
                        if (!isAnnotationEditMode)
                        {
                            if (pdfViewerControl.AnnotationMode == AnnotationMode.Highlight)
                                pdfViewerControl.AnnotationSettings.TextMarkup.Highlight.Color = Color.Yellow;
                            if (pdfViewerControl.AnnotationMode == AnnotationMode.Underline)
                                pdfViewerControl.AnnotationSettings.TextMarkup.Underline.Color = Color.Yellow;
                            if (pdfViewerControl.AnnotationMode == AnnotationMode.Strikethrough)
                                pdfViewerControl.AnnotationSettings.TextMarkup.Strikethrough.Color = Color.Yellow;
                        }
                        else
                        {
                            if (selectedAnnotation != null)
                                selectedAnnotation.Settings.Color = Color.Yellow;
                        }
                    }
                    break;

                case "Magenta":
                    {
                        if (!isAnnotationEditMode)
                        {
                            if (pdfViewerControl.AnnotationMode == AnnotationMode.Highlight)
                                pdfViewerControl.AnnotationSettings.TextMarkup.Highlight.Color = Color.FromHex("#FF00FF");
                            if (pdfViewerControl.AnnotationMode == AnnotationMode.Underline)
                                pdfViewerControl.AnnotationSettings.TextMarkup.Underline.Color = Color.FromHex("#FF00FF");
                            if (pdfViewerControl.AnnotationMode == AnnotationMode.Strikethrough)
                                pdfViewerControl.AnnotationSettings.TextMarkup.Strikethrough.Color = Color.FromHex("#FF00FF");
                        }
                        else
                        {
                            if (selectedAnnotation != null)
                                selectedAnnotation.Settings.Color = Color.FromHex("#FF00FF");
                        }
                    }
                    break;

                case "Black":
                    {
                        if (!isAnnotationEditMode)
                        {
                            if (pdfViewerControl.AnnotationMode == AnnotationMode.Highlight)
                                pdfViewerControl.AnnotationSettings.TextMarkup.Highlight.Color = Color.Black;
                            if (pdfViewerControl.AnnotationMode == AnnotationMode.Underline)
                                pdfViewerControl.AnnotationSettings.TextMarkup.Underline.Color = Color.Black;
                            if (pdfViewerControl.AnnotationMode == AnnotationMode.Strikethrough)
                                pdfViewerControl.AnnotationSettings.TextMarkup.Strikethrough.Color = Color.Black;
                        }
                        else
                        {
                            if (selectedAnnotation != null)
                                selectedAnnotation.Settings.Color = Color.Black;
                        }
                    }
                    break;

                case "White":
                    {
                        if (!isAnnotationEditMode)
                        {
                            if (pdfViewerControl.AnnotationMode == AnnotationMode.Highlight)
                                pdfViewerControl.AnnotationSettings.TextMarkup.Highlight.Color = Color.White;
                            if (pdfViewerControl.AnnotationMode == AnnotationMode.Underline)
                                pdfViewerControl.AnnotationSettings.TextMarkup.Underline.Color = Color.White;
                            if (pdfViewerControl.AnnotationMode == AnnotationMode.Strikethrough)
                                pdfViewerControl.AnnotationSettings.TextMarkup.Strikethrough.Color = Color.White;
                        }
                        else
                        {
                            if (selectedAnnotation != null)
                                selectedAnnotation.Settings.Color = Color.White;
                        }
                    }
                    break;
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Context.pdfViewer = pdfViewerControl;
            //fileStream = typeof(App).GetTypeInfo().Assembly.GetManifestResourceStream("GettingStarted.Assets.GIS Succinctly.pdf");            


        }        
        private Stream CreateNewPDF()
        {
            MemoryStream memoryStream = new MemoryStream();

            //Create a new PDF document
            PdfDocument document = new PdfDocument();

            // Set the page size
            document.PageSettings.Size = PdfPageSize.A4;

            //Add a page to the document
            PdfPage page = document.Pages.Add();

            //Create PDF graphics for the page
            PdfGraphics graphics = page.Graphics;

            //Set the font
            PdfFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 20);

            //Draw the text
            graphics.DrawString("Hello World!!!", font, PdfBrushes.Black, new PointF(0, 0));

            //Save the document
            document.Save(memoryStream);

            //Close the document
            document.Close(true);

            if (memoryStream.CanSeek)
                memoryStream.Position = 0;

            return memoryStream;
        }

        private Stream CreateTablePdf()
        {
            //Create a new PDF document.
            PdfDocument doc = new PdfDocument();
            //Add a page.
            PdfPage page = doc.Pages.Add();
            //Create a PdfGrid.
            PdfGrid pdfGrid = new PdfGrid();
            //Add values to list
            List<object> data = new List<object>();
            Object row1 = new { ID = "E01", Name = "Clay" };
            Object row2 = new { ID = "E02", Name = "Thomas" };
            Object row3 = new { ID = "E03", Name = "Andrew" };
            Object row4 = new { ID = "E04", Name = "Paul" };
            Object row5 = new { ID = "E05", Name = "Gray" };
            data.Add(row1);
            data.Add(row2);
            data.Add(row3);
            data.Add(row4);
            data.Add(row5);
            //Add list to IEnumerable
            IEnumerable<object> dataTable = data;
            //Assign data source.
            pdfGrid.DataSource = dataTable;
            //Draw grid to the page of PDF document.
            pdfGrid.Draw(page, new PointF(10, 10));
            //Save the PDF document to stream.
            MemoryStream memoryStream = new MemoryStream();
            doc.Save(memoryStream);
            //Close the document.
            doc.Close(true);
            if (memoryStream.CanSeek)
                memoryStream.Position = 0;

            return memoryStream;
        }
        

        // Create Orders table
        private DataTable CreateOrdersTable()
        {
            // Create a DataTable
            DataTable ordersTable = new DataTable("Orders");
            DataColumn dtColumn;
            DataRow dtRow;

            // Create OrderId column
            dtColumn = new DataColumn();
            dtColumn.DataType = Type.GetType("System.String");
            dtColumn.ColumnName = "ProductId";
            dtColumn.AutoIncrement = true;
            dtColumn.Caption = "Product ID";
            dtColumn.ReadOnly = true;
            dtColumn.Unique = true;
            ordersTable.Columns.Add(dtColumn);

            // Create Name column.
            dtColumn = new DataColumn();
            dtColumn.DataType = Type.GetType("System.String");
            dtColumn.ColumnName = "ProductName";
            dtColumn.Caption = "Item Name";
            ordersTable.Columns.Add(dtColumn);

            // Create CustId column which Reprence Cust Id from
            // The cust Table
            dtColumn = new DataColumn();
            dtColumn.DataType = Type.GetType("System.Decimal");
            dtColumn.ColumnName = "Cantidad";
            dtColumn.Caption = "Cantidad";
            ordersTable.Columns.Add(dtColumn);

            // Create Description column.
            dtColumn = new DataColumn();
            dtColumn.DataType = Type.GetType("System.Decimal");
            dtColumn.ColumnName = "ValorUnitario";
            dtColumn.Caption = "Valor Unitario";
            ordersTable.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = Type.GetType("System.Decimal");
            dtColumn.ColumnName = "ValorDescuento";
            dtColumn.Caption = "Valor Descuento";
            ordersTable.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = Type.GetType("System.Decimal");
            dtColumn.ColumnName = "Precio";
            dtColumn.Caption = "Precio";
            ordersTable.Columns.Add(dtColumn);

            // ADD two rows to the customer Id 1001
            dtRow = ordersTable.NewRow();
            dtRow["ProductId"] = 0;
            dtRow["ProductName"] = "ASP Book";
            dtRow["Cantidad"] = 4;
            dtRow["ValorUnitario"] = "100";
            dtRow["ValorDescuento"] = "0";
            dtRow["Precio"] = "100";
            ordersTable.Rows.Add(dtRow);
            dtRow = ordersTable.NewRow();
            dtRow["ProductId"] = 1;
            dtRow["ProductName"] = "C# Book";
            dtRow["Cantidad"] = 2;
            dtRow["ValorUnitario"] = "30";
            dtRow["ValorDescuento"] = "0";
            dtRow["Precio"] = "30";
            ordersTable.Rows.Add(dtRow);

            return ordersTable;
        }

        private void SendBackMessage(bool hasData)
        {
            var pages = Navigation.NavigationStack.ToList();
            var previousPage = pages.ElementAt(pages.Count - 2);

            if (previousPage.ToString().Contains(nameof(IngresoComprasView)))
            {
                if (hasData)
                    MessagingCenter.Send<Orden>(Context.OrdenSeleccionado, Escant_App.AppSettings.Settings.PdfViewerIngresoComprasView);
                else
                    MessagingCenter.Send<Orden>(Context.OrdenSeleccionado, Escant_App.AppSettings.Settings.PdfViewerIngresoComprasView);
            }
            else if (previousPage.ToString().Contains(nameof(IngresoVentasView)))
            {
                if (hasData)
                    MessagingCenter.Send<Orden>(Context.OrdenSeleccionado, Escant_App.AppSettings.Settings.PdfViewerIngresaVentasView);
                else
                    MessagingCenter.Send<Orden>(Context.OrdenSeleccionado, Escant_App.AppSettings.Settings.PdfViewerIngresaVentasView);
            }            
        }

    }
}