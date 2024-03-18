using System;
using System.Collections.Generic;
using System.Text;

namespace DataModel.Base
{
    /// <summary>
    /// Clase Base de herencia para toda las entidades de negocio
    /// </summary>
    public class ClaseBase
    {
        /// <summary>
        /// Atributo para verificar si existe en base de datos
        /// </summary>
        protected bool _existeEnRepositorio;

        /// <summary>
        /// Indica si el registro existe en base de datos.
        /// </summary>
        public bool ExisteEnRepositorio
        {
            get { return _existeEnRepositorio; }
        }
    }
}
