using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PowderBox
{
    public static class CollectionExtensions
    {
        public static T RandomElement<T>(this List<T> list, T excludedElement = null) where T : class
        {
            int randomIndex = Random.Range(0, list.Count);
            T item = list[randomIndex];
            if (excludedElement != null && item == excludedElement)
            {
                return list.RandomElement(excludedElement);
            }
            else
            {
                return list[randomIndex];
            }
        }

        public static T RandomElement<T>(this T[] array, T excludedElement = null) where T : class
        {
            int randomIndex = Random.Range(0, array.Length);
            T item = array[randomIndex];
            if (excludedElement != null && item == excludedElement)
            {
                return array.RandomElement(excludedElement);
            }
            else
            {
                return array[randomIndex];
            }
        }
    }
}
