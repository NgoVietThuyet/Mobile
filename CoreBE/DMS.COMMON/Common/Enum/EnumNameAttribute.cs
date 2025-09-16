using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.COMMON.Common.Enum
{
    public class EnumNameAttribute : Attribute
    {
        public string Name { get; set; }
        public EnumNameAttribute(string name)
        {
            Name = name;
        }
    }
    public class EnumValueAttribute : Attribute
    {
        public string Value { get; set; }
        public EnumValueAttribute(string value)
        {
            Value = value;
        }
    }
}
