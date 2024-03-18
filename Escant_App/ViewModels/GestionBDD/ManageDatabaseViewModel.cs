using DataModel;
using DataModel.DTO;
using DataModel.DTO.Administracion;
using DataModel.DTO.Clientes;
using DataModel.Infraestructura.Offline.DB;
using ManijodaServicios.Offline.Interfaz;
using Escant_App.Services;
using Escant_App.ViewModels.Base;
using Syncfusion.XlsIO;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using System.Linq.Expressions;
using Escant_App.Helpers;

using ManijodaServicios.Resources.Texts;
using DataModel.Enums;
using DataModel.Helpers;

using Xamarin.Essentials;
using Escant_App.Interfaces;

namespace Escant_App.ViewModels.GestionBDD
{
    public class ManageDatabaseViewModel : ViewModelBase
    {
        
        private readonly IServicioAdministracion_Empresa _servicioEmpresa;
        private readonly IServicioAdministracion_Catalogo _servicioCatalogo;                
        private readonly IServicioClientes_Cliente _servicioCliente;
        private ObservableCollection<Empresa> Empresas;
        private ObservableCollection<Catalogo> Catalogos;        
        private ObservableCollection<Cliente> Clientes;        

        public ManageDatabaseViewModel(IServicioAdministracion_Empresa servicioEmpresa,
             IServicioAdministracion_Catalogo servicioCatalogo,             
             IServicioClientes_Cliente servicioCliente)
        {
            
            _servicioEmpresa = servicioEmpresa;
            _servicioCatalogo = servicioCatalogo;                        
            _servicioCliente = servicioCliente;
        }

        public async Task LoadData()
        {
            IsBusy = true;            
            Empresas = await _servicioEmpresa.GetAllEmpresaAsync(x => x.Deleted == 0);
            Catalogos = await _servicioCatalogo.GetAllCatalogoAsync(x => x.Deleted == 0);            
            Clientes = await _servicioCliente.GetAllClienteAsync(x => x.Deleted == 0);            
            IsBusy = false;
        }

        public override async Task InitializeAsync(object navigationData)
        {

        }

        //public ICommand ImportDataFromExcelCommand => new Command(async () => await ImportDataFromExcelAsync());        
        public ICommand BackupDbCommand => new Command(async () => await BackupDbAsync());
        public ICommand RestoreDbCommand => new Command(async () => await RestoreDbAsync());        

        internal IStyle CellErrorStyle { get; set; }
        private IStyle GenerateCellErrorStyle(IWorkbook workbook)
        {
            if (workbook.Styles.Contains("CellErrorStyle"))
            {
                var styles = workbook.Styles.AsQueryable().GetEnumerator();
                while (styles.MoveNext())
                {
                    var item = (Syncfusion.XlsIO.Implementation.StyleImpl)styles.Current;
                    if (item.Name == "CellErrorStyle")
                        return item;
                }
            }
            IStyle cellErrorStyle = workbook.Styles.Add("CellErrorStyle");
            cellErrorStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
            cellErrorStyle.Font.Color = ExcelKnownColors.Red;
            cellErrorStyle.Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
            cellErrorStyle.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
            cellErrorStyle.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
            cellErrorStyle.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
            return cellErrorStyle;
        }

                

        public async Task BackupDbAsync()
        {
            IsBusy = true;
            var tick = $"{DateTime.Now.Ticks}";
            var last4Digit = tick.Substring(tick.Length - 4);
            bool backupResult = await Xamarin.Forms.DependencyService.Get<IBackupRestore>().BackupDb($"Escant_App_{DateTime.Now.ToString("yyyyMMdd")}{last4Digit}.db", DbContext.DB_NAME);
            IsBusy = false;
            if (backupResult)
                DialogService.ShowToast(TextsTranslateManager.Translate("BackupSuccess"));
            else
                await DialogService.ShowAlertAsync(TextsTranslateManager.Translate("BackupError"), TextsTranslateManager.Translate("Warning"), "OK");
        }

        public async Task RestoreDbAsync()
        {
            await NavigationService.NavigateToPopupAsync<RestoreDbPopupViewModel>();
        }

        private readonly ImageSource EmptyImageSource = ImageSource.FromStream(() =>
        {
            var stream = new MemoryStream();
            return stream;
        });

        
        private string GetEmpresaId(string identificacion)
        {
            return Empresas.FirstOrDefault(x => x.Identificacion.Equals(identificacion.Trim(), StringComparison.CurrentCultureIgnoreCase))?.Id;
        }

        private string GetClienteId(string identificacion)
        {
            return Clientes.FirstOrDefault(x => x.Identificacion.Equals(identificacion.Trim(), StringComparison.CurrentCultureIgnoreCase))?.Id;
        }        
    }
}
