using System;
using System.Diagnostics;
using System.Linq;

namespace AutoUi.Core.Services
{
    public class PoorMansObjectCloner
    {
        public static T Clone<T>(T sourceObject)
            where T: new()
        {
            return Clone<T, T>(sourceObject);
        }

        public static T2 Clone<T1, T2>(T1 sourceObject)
            where T2: new()
        {
            var targetObject = new T2();

            CopyProperties(sourceObject, targetObject);

            return targetObject;
        }

        public static void CopyProperties<T1, T2>(T1 sourceObject, T2 targetObject)
            where T2 : new()
        {
            var propertiesOfT1 = typeof(T1).GetProperties();
            var propertiesOfT2 = typeof(T2).GetProperties();

            foreach (var sourceProperty in propertiesOfT1)
            {
                var targetProperty = propertiesOfT2.FirstOrDefault(x => x.Name == sourceProperty.Name);
                if (targetProperty == null)
                    continue;

                try
                {
                    var v = sourceProperty.GetValue(sourceObject);
                    targetProperty.SetValue(targetObject, v);
                }
                catch (Exception)
                {
                    Debug.WriteLine($"Kopieren der Property {sourceProperty.Name} vom Typ {typeof(T1).Name} nach {typeof(T2).Name} ist fehlgeschlagen");
                }
            }
        }
    }
}
