/* 
      |\      _,,,---,,_
ZZZzz /,`.-'`'    -.  ;-;;,_
     |,4-  ) )-,_. ,\ (  `'-'
    '---''(_/--'  `-'\_)
*/
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

namespace TravelBox.Common
{
    public static class EditorUtility
    {
        // -------------

        // Courtesy of glitchers at:
        // https://answers.unity.com/questions/486545/getting-all-assets-of-the-specified-type.html
        public static bool TryFindAssetsByType<T>(out List<T> assets) where T : UnityEngine.Object
        {
            assets = null;

            string[] guids = AssetDatabase.FindAssets(string.Format("t:{0}", typeof(T)));

            if (guids.Length > 0)
            {
                assets = new List<T>();

                for (int i = 0; i < guids.Length; i++)
                {
                    string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
                    T asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);
                    if (asset != null)
                    {
                        assets.Add(asset);
                    }
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        // -------------
    }
}
/* maria aurelia at 26 December 2021 🌊 */
/* dreamy guardian ASCII kitty by Felix Lee, found at asciiart.eu 🐱‍👤 */