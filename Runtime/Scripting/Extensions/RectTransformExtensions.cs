using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PowderBox.Extensions
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
    }
}