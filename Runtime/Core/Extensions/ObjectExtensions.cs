/* 
      |\      _,,,---,,_
ZZZzz /,`.-'`'    -.  ;-;;,_
     |,4-  ) )-,_. ,\ (  `'-'
    '---''(_/--'  `-'\_)
*/
using System.Reflection;
using UnityEngine;

namespace TravelBox.Extensions
{
    public static class ObjectExtensions
    {
        // -------------

        public static string[] PrintableProperties(this object o)
        {
            string[] printableProperties;

            PropertyInfo[] pis = o.GetType().GetProperties();

            printableProperties = new string[pis.Length];

            for (int i = 0; i < pis.Length; i++)
            {
                
                var propertyInfo = pis[i];
                Debug.Log(propertyInfo.Name);

                string propertyTypeName = propertyInfo.PropertyType.Name;
                string propertyName = propertyInfo.Name;
                string propertyPrintableValue = propertyInfo.GetValue(o).ToString();
                string info = $"({propertyTypeName}) {propertyName}: {propertyPrintableValue}";
                printableProperties[i] = info;
            } 

            return printableProperties;
        }

        // -------------
    }
}
/* maria aurelia at 27 March 2022 🌊 */
/* dreamy guardian ASCII kitty by Felix Lee, found at asciiart.eu 🐱‍👤 */