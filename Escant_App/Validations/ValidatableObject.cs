using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Linq;
using Xamarin.Forms;

namespace Escant_App.Validations
{
    public class ValidatableObject<T> : BindableObject, IValidity
    {
        private readonly List<IValidationRule<T>> _validations;
        private readonly ObservableCollection<string> _errors;
        private T _value;
        private bool _isValid;
        private bool _hasError;
        private string _mensaje;

        public List<IValidationRule<T>> Validations => _validations;

        public ObservableCollection<string> Errors => _errors;

        public T Value
        {
            get
            {
                return _value;
            }

            set
            {
                _value = value;
                OnPropertyChanged();
            }
        }

        public bool IsValid
        {
            get
            {
                return _isValid;
            }

            set
            {
                _isValid = value;
                _errors.Clear();
                OnPropertyChanged();
            }
        }

        public bool HasError
        {
            get
            {
                return _hasError;
            }

            set
            {
                _hasError = value;                
                OnPropertyChanged();
            }
        }

        public string Mensaje
        {
            get
            {
                return _mensaje;
            }

            set
            {
                _mensaje= value;
                _errors.Clear();
                OnPropertyChanged();
            }
        }

        public ValidatableObject()
        {
            _isValid = true;
            _hasError = false;
            _errors = new ObservableCollection<string>();
            _validations = new List<IValidationRule<T>>();
        }

        public bool Validate()
        {
            Errors.Clear();

            IEnumerable<string> errors = _validations.Where(v => !v.Check(Value))
                                                     .Select(v => v.ValidationMessage);

            foreach (var error in errors)
            {
                Errors.Add(error);
            }

            IsValid = !Errors.Any();
            Mensaje = IsValid?"": _validations.First().ValidationMessage.ToString();
            HasError = !IsValid;

            return this.IsValid;
        }
    }
}
