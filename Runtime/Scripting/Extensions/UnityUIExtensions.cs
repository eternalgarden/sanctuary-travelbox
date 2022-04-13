using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TravelBox.Extensions
{
    public static class UnityUIExtensions
    {
        public static void SetUGUIText(this Button btn, string text)
        {
            /* ⭐ ---- ---- */

            TextMeshProUGUI[] textComponents = btn.GetComponentsInChildren<TextMeshProUGUI>(true);
            
            if (textComponents.Length == 0 ) throw new System.Exception("Used SetText unity button extension on a button that <<doesn't have a text component on it>>");
            if (textComponents.Length > 1) throw new System.Exception("Used SetText unity button extension on a button that <<has more than one text component>>");
            
            textComponents[0].text = text;
            
            /* ---- ---- 🌠 */
        }

        public static Color GetUGUIColor(this Button btn)
        {
            /* ⭐ ---- ---- */

            return btn.GetNestedUGUI().color;
            
            /* ---- ---- 🌠 */
        }

        public static void SetUGUIColor(this Button btn, Color color)
        {
            /* ⭐ ---- ---- */

            btn.GetNestedUGUI().color = color;
            
            /* ---- ---- 🌠 */
        }

        static TextMeshProUGUI GetNestedUGUI(this Button btn)
        {
            /* ⭐ ---- ---- */
            
            TextMeshProUGUI[] textComponents = btn.GetComponentsInChildren<TextMeshProUGUI>(true);
            
            if (textComponents.Length == 0 ) throw new System.Exception("Used SetText unity button extension on a button that <<doesn't have a text component on it>>");
            if (textComponents.Length > 1) throw new System.Exception("Used SetText unity button extension on a button that <<has more than one text component>>");

            return textComponents[0];
            
            /* ---- ---- 🌠 */
        }
    }
}
