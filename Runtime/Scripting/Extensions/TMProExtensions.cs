/* 
      |\      _,,,---,,_
ZZZzz /,`.-'`'    -.  ;-;;,_
     |,4-  ) )-,_. ,\ (  `'-'
    '---''(_/--'  `-'\_)
*/
using System;
using TMPro;

namespace Sanctuary.Extensions
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
    
        // -------------
    }
}
/* maria aurelia at 15 December 2021 🌊 */
/* dreamy guardian ASCII kitty by Felix Lee, found at asciiart.eu 🐱‍👤 */