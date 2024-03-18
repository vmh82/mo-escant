using DataModel.DTO.Iteraccion;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Text;

namespace DataModel.Base
{
    //TODO: will be deleted ?
    public class BaseModel: INotifyPropertyChanged
    {
        [PrimaryKey]
        public string Id { get; set; }
        public string CreatedBy { get; set; }
        public long? CreatedDate { get; set; }
        [Ignore]
        public DateTime CreatedDateValue => CreatedDate.HasValue ? new DateTime(1970, 1, 1).AddMilliseconds(CreatedDate.Value) : default(DateTime);
        [Ignore]
        public string CreatedDateFormato { get; set; } = String.Format("{0:G}", DateTime.Now);
        public string UpdatedBy { get; set; }
        public long? UpdatedDate { get; set; }
        [Ignore]
        public DateTime UpdatedDateValue => UpdatedDate.HasValue ? new DateTime(1970, 1, 1).AddMilliseconds(UpdatedDate.Value) : default(DateTime);
        [Ignore]
        public string UpdatedDateFormato { get; set; } = String.Format("{0:G}", DateTime.Now);
        public int Deleted { get; set; }
        public bool IsDeleted => Deleted == 1 ? true : false;

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void OnPropertyChanged<T>(Expression<Func<T>> selectorExpression)
        {
            if (selectorExpression == null)
                throw new ArgumentNullException("selectorExpression");
            var me = selectorExpression.Body as MemberExpression;

            // Nullable properties can be nested inside of a convert function
            if (me == null)
            {
                var ue = selectorExpression.Body as UnaryExpression;
                if (ue != null)
                    me = ue.Operand as MemberExpression;
            }

            if (me == null)
                throw new ArgumentException("The body must be a member expression");

            OnPropertyChanged(me.Member.Name);
        }

        protected void SetField<T>(ref T field, T value, Expression<Func<T>> selectorExpression, params Expression<Func<object>>[] additonal)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return;
            field = value;
            OnPropertyChanged(selectorExpression);
            foreach (var item in additonal)
                OnPropertyChanged(item);
        }


    }
    public class TrulyObservableCollection<T> : ObservableCollection<T>
      where T : INotifyPropertyChanged
    {        
        public TrulyObservableCollection()
        {
            CollectionChanged += FullObservableCollectionCollectionChanged;
        }

        public TrulyObservableCollection(IEnumerable<T> pItems) : this()
        {
            foreach (var item in pItems)
                Add(item);
        }

        private void FullObservableCollectionCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
                foreach (var item in e.NewItems)
                    ((INotifyPropertyChanged)item).PropertyChanged += ItemPropertyChanged;
            if (e.OldItems == null) return;
            {
                foreach (var item in e.OldItems)
                    ((INotifyPropertyChanged)item).PropertyChanged -= ItemPropertyChanged;
            }
        }

        private void ItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, sender, sender,
               IndexOf((T)sender));
            OnCollectionChanged(args);
        }
    }


}
