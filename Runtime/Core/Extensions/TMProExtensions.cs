/* 
      |\      _,,,---,,_
ZZZzz /,`.-'`'    -.  ;-;;,_
     |,4-  ) )-,_. ,\ (  `'-'
    '---''(_/--'  `-'\_)
*/
using System;
using TMPro;
using UnityEngine;

namespace TravelBox.Extensions
{
    public static class TMProExtensions
    {
        // -------------
        
        public static int GetIntValue(this TMP_InputField inputField)
        {
            /* ⭐ ---- ---- */
            
            if(int.TryParse(inputField.text, out int value))
            {
                return value;
            }
            else
            {
                throw new Exception("Input field isn't of int type for gameobject: " + inputField.gameObject.name);
            }
            
            /* ---- ---- 🌠 */
        }

        // * Rich text information here
        // * http://digitalnativestudios.com/textmeshpro/docs/rich-text/

        public static string ColourTMProString(this string text, Color color)
        {
            /* ⭐ ---- ---- */
            
            string hex = ColorUtility.ToHtmlStringRGB(color);
            return $"<color=#{hex}>{text}</color>";
            
            /* ---- ---- 🌠 */
        }
    
        // -------------
    }
}
/* maria aurelia at 15 December 2021 🌊 */
/* dreamy guardian ASCII kitty by Felix Lee, found at asciiart.eu 🐱‍👤 */