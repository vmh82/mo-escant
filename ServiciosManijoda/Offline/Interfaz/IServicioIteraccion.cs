using DataModel.DTO.Iteraccion;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ManijodaServicios.Offline.Interfaz
{
    
        #region Galeria
        /// <summary>
        /// Interfaz método Servicio de Administración de Catálogos
        /// </summary>
        public interface IServicioIteraccion_Galeria
        {
            Task<ObservableCollection<Galeria>> GetAllGaleriaAsync(Expression<Func<Galeria, bool>> expression);
            Task<string> InsertGaleriaAsync(Galeria objeto);
            Task<int> ImportGaleriaAsync(IEnumerable<Galeria> objetos);
            Task UpdateGaleriaAsync(Galeria objeto);
            Task DeleteGaleriaAsync(Galeria objeto);
        }
    #endregion Galeria

    #region TablasBdd
    /// <summary>
    /// Interfaz método Servicio de Administración de Catálogos
    /// </summary>
    public interface IServicioIteraccion_TablasBdd
    {
        Task<ObservableCollection<TablasBdd>> GetAllTablasBddAsync(Expression<Func<TablasBdd, bool>> expression);
        Task<ObservableCollection<TablasBdd>> GetAllTablasBddFullAsync(TablasBdd objeto);
        Task<string> InsertTablasBddAsync(TablasBdd objeto);
        Task<int> ImportTablasBddAsync(IEnumerable<TablasBdd> objetos);
        Task UpdateTablasBddAsync(TablasBdd objeto);
        Task DeleteTablasBddAsync(TablasBdd objeto);
        Task DeleteTablasBddAllDataAsync(string nombreTabla);
    }
    #endregion TablasBdd
}
