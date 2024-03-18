using System;
using System.Collections.Generic;
using System.Text;

namespace Escant_App.Attributes
{
    public class StringValueAttribute : Attribute
    {
        public string StringValue { get; protected set; }
        public StringValueAttribute(string value)
        {
            this.StringValue = value;
        }
    }
}
