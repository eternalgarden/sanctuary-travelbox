using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace TravelBox.Extensions
{
    public static class TypeExtensions
    {
        //
        // ─── FIELD TYPES ─────────────────────────────────────────────────
        //
            
        #region FIELD TYPES
        /*
        * Gets the type originally declaring a given field in case 
        * the passed type is just a child inheriting that field.
        * This thingy is necessary when some text fields were inherited
        */
        public static Type GetFieldDeclaringType(Type type, string field, BindingFlags bindingFlags)
        {
            // Debug.Log($"Searching {type} for {field} field");

            Type originalType = type;

            FieldInfo fieldInfo = type
                .GetFields(bindingFlags)
                .Where(f => f.DeclaringType == type)
                .FirstOrDefault(f => f.Name == field);

            if (fieldInfo == null)
            {
                // Debug.Log($"Didn't find, switching to base type of {type.BaseType}");

                type = type.BaseType;

                if (type == null)
                {
                    Debug.LogError("Failed to find enclosing type.");
                    return null;
                }

                type = GetFieldDeclaringType(type, field, bindingFlags);
            }

            return type;
        }

        #endregion FIELD TYPES

    }
}
