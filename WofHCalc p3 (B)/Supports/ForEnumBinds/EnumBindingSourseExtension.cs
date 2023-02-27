using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace WofHCalc.Supports.ForEnumBinds
{
    internal class EnumBindingSourse : MarkupExtension
    {
        public Type EnumType { get; private set; }
        public EnumBindingSourse(Type enumtype)
        {
            if (enumtype is null || !enumtype.IsEnum)
                throw new Exception("EnumType must not be a null and type Enum");
            EnumType = enumtype;
        }
        public override object ProvideValue(IServiceProvider serviceProvider) => Enum.GetValues(EnumType).Cast<Enum>().Select(e => new KeyValuePair<object,string>(e, e.Description()));
    }

    public static class EnumHelper
    {
        public static string Description(this Enum value)
        {
            var attributes = value.GetType().GetField(value.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (attributes.Any())
                return (attributes.First() as DescriptionAttribute).Description;
            else return "";
        }
    }
}
