using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TravelBox.Extensions
{
    public static class RectTransformExtensions
    {
        // How come this isnt part of unity code
        // Thanks to Assembler-Maze at https://forum.unity.com/threads/setting-top-and-bottom-on-a-recttransform.265415/#post-3033860
        public static void SetRect(this RectTransform trs, float left, float top, float right, float bottom)
        {
            trs.offsetMin = new Vector2(left, bottom);
            trs.offsetMax = new Vector2(-right, -top);
        }
        
        /// <summary>
        /// Set top, left, right and bottom offsets for a RectTransform in fill mode.
        /// by Assembler-Maze at https://forum.unity.com/threads/setting-top-and-bottom-on-a-recttransform.265415/#post-3033860
        /// </summary>
        /// <param name="trs"></param>
        /// <param name="left"></param>
        /// <param name="top"></param>
        /// <param name="right"></param>
        /// <param name="bottom"></param>
        public static void SetFillRectTransformOffsets(this RectTransform trs, float left, float top, float right, float bottom)
        {
            trs.offsetMin = new Vector2(left, bottom);
            trs.offsetMax = new Vector2(-right, -top);
        }
        
        public static Canvas GetCanvas(this RectTransform rt)
        {
            return rt.gameObject.GetComponentInParent<Canvas>();
        }
        
        /// <summary>
        /// Getting the width in pixels of a given RectTransform
        /// by BjoernS at https://forum.unity.com/threads/solved-getting-position-and-size-of-recttransform-in-screen-coordinates.816888/
        /// </summary>
        /// <param name="rt"></param>
        /// <returns>Width of rt (in pixels)</returns>
        public static float GetPixelWidth(this RectTransform rt)
        {
            var w = (rt.anchorMax.x - rt.anchorMin.x) * Screen.width + rt.sizeDelta.x * rt.GetCanvas().scaleFactor;
            return w;
        }
        
        /// <summary>
        /// Getting the height in pixels of a given RectTransform
        /// by BjoernS at https://forum.unity.com/threads/solved-getting-position-and-size-of-recttransform-in-screen-coordinates.816888/
        /// </summary>
        /// <param name="rt"></param>
        /// <returns>Height of rt (in pixels)</returns>
        public static float GetPixelHeight(this RectTransform rt)
        {
            var h = (rt.anchorMax.y - rt.anchorMin.y) * Screen.height + rt.sizeDelta.y * rt.GetCanvas().scaleFactor;
            return h;
        }

        public static Vector2 GetPixelResolution(this RectTransform rt)
        {
            return new Vector2(GetPixelWidth(rt), GetPixelHeight(rt));
        }
    }
}