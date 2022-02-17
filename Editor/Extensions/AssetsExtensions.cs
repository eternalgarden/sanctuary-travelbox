/* 
      |\      _,,,---,,_
ZZZzz /,`.-'`'    -.  ;-;;,_
     |,4-  ) )-,_. ,\ (  `'-'
    '---''(_/--'  `-'\_)
*/
using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TravelBox.Extensions
{
    public static class AssetsExtensions
    {
        // -------------
    
        public static string GetScriptFileDirectory(ScriptableObject so)
        {
            MonoScript ms = MonoScript.FromScriptableObject(so);

            string scriptFilePath = AssetDatabase.GetAssetPath(ms);
            string toolDirectory = Path.GetDirectoryName(scriptFilePath);

            toolDirectory = toolDirectory.Replace('\\', '/');

            return toolDirectory;
        }

        public static T LoadAssetAtPath<T>(string path) where T : class
        {
            Object o = AssetDatabase.LoadAssetAtPath(path, typeof(T));
            return o as T;
        }

        public static T LoadAssetOfType<T>(string assetName = null) where T : class
        {
            string nameFilter = assetName != null ? $"{assetName} " : "";
            string typeFilter = $"t:{typeof(T)}";
            string[] guids = AssetDatabase.FindAssets($"{nameFilter}{typeFilter}");

            if (guids.Length == 0)
            {
                Debug.LogError($"Didn't find an asset of type {typeof(T)}");
                return null;
            }
            else if (guids.Length > 1)
            {
                Debug.LogError($"Found multiple assets of type {typeof(T)}");
                return null;
            }
            else
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[0]);
                return LoadAssetAtPath<T>(path);
            }
        }
    
        // -------------
    }
}
/* maria aurelia at 10 February 2022 🌊 */
/* dreamy guardian ASCII kitty by Felix Lee, found at asciiart.eu 🐱‍👤 */