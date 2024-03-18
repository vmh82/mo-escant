﻿using DataModel.DTO.Iteraccion;
using Escant_App.ViewModels.Base;
using ManijodaServicios.AppSettings;
using ManijodaServicios.Resources.Texts;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Escant_App.ViewModels.Reportes
{
    public class ReportesViewModel : ViewModelBase
    {
        private Generales.Generales generales = new Generales.Generales();
        public ReportesViewModel()
        {
            armaMenu();
        }

        private ObservableCollection<MenuSistema> _menuSistema;
        public ObservableCollection<MenuSistema> MenuSistema
        {
            get => _menuSistema;
            set
            {
                _menuSistema = value;
                RaisePropertyChanged(() => MenuSistema);
            }
        }
        public void ShowMessage()
        {
            DialogService.ShowAlert(TextsTranslateManager.Translate("FeatureIsNotAvaible"), TextsTranslateManager.Translate("LegalInfo"), "OK");
        }
        async void armaMenu()
        {
            var menu = await generales.devuelveMenu(0, "Reportes");
            if (menu == null)
            {
                SettingsOnline.EsMenuVacio = true;
                //await Context.Logout();
            }
            else
            {
                SettingsOnline.EsMenuVacio = false;
                MenuSistema = new ObservableCollection<MenuSistema>(menu.OrderBy(z => z.Orden).Select(x => new MenuSistema()
                {
                    Title = TextsTranslateManager.Translate(x.LabelTitulo)
                      ,
                    Icon = ""
                      ,
                    Description = TextsTranslateManager.Translate(x.LabelDescripcion)
                    ,
                    IconoMenu = x.ImageIcon != null && x.ImageIcon.ToString() != "" ? JsonConvert.DeserializeObject<FuenteIconos>(x.ImageIcon.ToString()) : new FuenteIconos()
                    {
                        NombreIcono = "Configurar"
                                  ,
                        CodigoImagen = "&#xf0e8;"

                    }
                      ,
                    Type = Type.GetType(x.NombreFormulario)
                }).ToList());

            }
        }

    }

}
