using System;

namespace Atea.Common.Extensions
{
    public static class EnumExtensions
    {
        public static TEnum Parse<TEnum>(this string value) where TEnum : struct
        {
            return Enum.TryParse(value, result: out TEnum result)
                ? result
                : throw new ArgumentException("Cannot parse \"" + value + "\" to Enum of type " + typeof(TEnum).FullName);
        }
    }
}