using System;
using System.Windows.Markup;

namespace SmartGen.Types
{
    public class EnumBindingSourceExtension : MarkupExtension
    {
        private Type _enumType;

        public Type EnumType
        {
            get => _enumType;
            set
            {
                if (value == _enumType) return;

                if (value != null)
                {
                    var enumType = Nullable.GetUnderlyingType(value) ?? value;

                    if (!enumType.IsEnum)
                        throw new ArgumentException("Type must be for an Enum.");
                }

                _enumType = value;
            }
        }

        public EnumBindingSourceExtension()
        {
        }

        public EnumBindingSourceExtension(Type enumType)
        {
            _enumType = enumType;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (_enumType == null)
                throw new InvalidOperationException("The EnumType must be specified");

            var actualEnumType = Nullable.GetUnderlyingType(_enumType) ?? _enumType;
            var enumValues = Enum.GetValues(actualEnumType);

            if (_enumType == actualEnumType) return enumValues;

            var tmp = Array.CreateInstance(actualEnumType, enumValues.Length + 1);
            enumValues.CopyTo(tmp, 1);

            return tmp;
        }
    }
}