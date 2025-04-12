using System.Collections.Generic;
using UnityEngine;

public static class GameObjectExtensions
{
    public static bool FindInterfaceOnAGameobject<T>(this GameObject gameObject, out T foundInterface) where T : class
    {
        foundInterface = null;
        MonoBehaviour[] components = gameObject.GetComponents<MonoBehaviour>();
        
        foreach (MonoBehaviour t in components)
        {
            if (t is not T yay) continue;
            foundInterface = yay;
            return true;
        }

        return false;
    }

    //
    // ─── COMPONENT SEARCHING ─────────────────────────────────────────
    //

    #region COMPONENT SEARCHING

    

    public static bool CheckForComponent<T>(this GameObject go, List<T> foundT) where T : Component
    {
        bool foundSometing = false;

        if (go.TryGetComponent<T>(out T component))
        {
            foundT.Add(component);
            foundSometing = true;
        }

        return foundSometing;
    }

    #endregion // COMPONENT SEARCHING
}
