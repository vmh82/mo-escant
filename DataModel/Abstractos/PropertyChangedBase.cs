using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Text;

namespace DataModel.Abstractos
{
    public abstract class PropertyChangedBase : INotifyPropertyChanged
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
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
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
}
