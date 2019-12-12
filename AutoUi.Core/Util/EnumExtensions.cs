using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoUi.Core.Util
{
    public static class EnumUtils
    {
        public static List<T> GetListOfEnumValues<T>()
            where T:Enum
        {
            return new List<T>(typeof(T).GetEnumValues().Cast<T>());
        }
    }
}
