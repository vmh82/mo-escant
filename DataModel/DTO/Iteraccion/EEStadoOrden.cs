using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace DataModel.DTO.Iteraccion
{
    
    /// <summary>
    /// Shipment class
    /// </summary>
    public class EEstadoOrden : INotifyPropertyChanged
    {
        /// <summary>
        /// title field
        /// </summary>
        private string title;

        /// <summary>
        /// status title field
        /// </summary>
        private string titleStatus;

        /// <summary>
        /// date field
        /// </summary>
        private string date;

        /// <summary>
        /// Gets or sets the Title 
        /// </summary>
        public string Title
        {
            get
            {
                return title;
            }
            set
            {
                if (title != value)
                {
                    title = value;
                    RaisePropertyChange();
                }
            }
        }

        /// <summary>
        /// Gets or sets the TitleStatus
        /// </summary>
        public string TitleStatus
        {
            get
            {
                return titleStatus;
            }
            set
            {
                if (titleStatus != value)
                {
                    titleStatus = value;
                    RaisePropertyChange();
                }
            }
        }

        /// <summary>
        /// Gets or sets Date
        /// </summary>
        public string Date
        {
            get
            {
                return date;
            }
            set
            {
                if (date != value)
                {
                    date = value;
                    RaisePropertyChange();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChange([CallerMemberName] string propertyname = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
            }
        }
    }

    
}
