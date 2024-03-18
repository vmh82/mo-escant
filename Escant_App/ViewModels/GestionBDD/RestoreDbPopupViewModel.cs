using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using DataModel.DTO;
using DataModel.Infraestructura.Offline.DB;

using ManijodaServicios.Offline.Interfaz;
using Escant_App.Services;
using Escant_App.ViewModels.Base;
using Syncfusion.ListView.XForms;
using Xamarin.Forms;
using ManijodaServicios.Resources.Texts;

namespace Escant_App.ViewModels.GestionBDD
{
    public class RestoreDbPopupViewModel: ViewModelBase
    {
        public RestoreDbPopupViewModel()
        {
        }

        internal SfListView ListView { get; set; }

        public async Task LoadData()
        {
            IsBusy = true;
            var allBackupFiles = await DependencyService.Get<IBackupRestore>().GetAllBackupFiles();
            if (allBackupFiles != null)
            {
                BackupFiles = allBackupFiles.OrderByDescending(x => x.LastModified).ToList();
                Device.BeginInvokeOnMainThread(() =>
                {
                    ListView.HeightRequest = BackupFiles.Count * 50;
                });
            }
            IsBusy = false;
        }

        private List<FileDto> _backupFiles = new List<FileDto>();
        public List<FileDto> BackupFiles
        {
            get => _backupFiles;
            set
            {
                _backupFiles = value;
                RaisePropertyChanged(() => BackupFiles);
            }
        }

        private FileDto _selectedRestoreFile;
        public FileDto SelectedRestoreFile
        {
            get => _selectedRestoreFile;
            set
            {
                _selectedRestoreFile = value;
                RaisePropertyChanged(() => SelectedRestoreFile);
            }
        }
       
        public ICommand OkCommand => new Command(async () => await RestoreDbAsync());
        public ICommand CancelCommand => new Command(async () => await CancelAsync());

        public async Task RestoreDbAsync()
        {
            IsBusy = true;
            if(SelectedRestoreFile != null && !string.IsNullOrEmpty( SelectedRestoreFile.FileName))
            {
                bool restoreResult = await DependencyService.Get<IBackupRestore>().RestoreDb(SelectedRestoreFile.FileName, DbContext.DB_NAME);
                if (restoreResult)
                    DialogService.ShowToast(TextsTranslateManager.Translate("RestoreSuccess"));
                else
                    await DialogService.ShowAlertAsync(TextsTranslateManager.Translate("RestoreError"), TextsTranslateManager.Translate("Warning"), "OK");
            }
            IsBusy = false;
            await NavigationService.RemovePopupAsync();
        }

        public async Task CancelAsync()
        {
            await NavigationService.RemovePopupAsync();
        }
    }
}
