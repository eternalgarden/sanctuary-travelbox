/* 
      |\      _,,,---,,_
ZZZzz /,`.-'`'    -.  ;-;;,_
     |,4-  ) )-,_. ,\ (  `'-'
    '---''(_/--'  `-'\_)
*/
using UnityEngine;

namespace Sanctuary.Core
{
    public class FocusManagement : MonoBehaviour
    {
        // -------------

        [SerializeField] int maxFocusedFramerate = 60;
        [SerializeField] int backgoundFramerate = 1;
        [SerializeField] bool alsoStopTime = true;

        void Awake()
        {
            // -------------

            /*
            https://docs.unity3d.com/ScriptReference/QualitySettings-vSyncCount.html

            If this setting is set to a value other than 'Don't Sync' (0), the value of Application.targetFrameRate will be ignored.
            */
            QualitySettings.vSyncCount = 0;

            // -------------
        }

        /*

        This would get called by unity engine as described here:
        https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnApplicationFocus.html

        */
        void OnApplicationFocus(bool hasFocus)
        {
            // -------------

            // TODO this requires rethinking

#if !UNITY_EDITOR
            if (hasFocus)
            {
                Application.targetFrameRate = maxFocusedFramerate;
            }
            else
            {
                Application.targetFrameRate = backgoundFramerate;
            }
#endif
#if UNITY_EDITOR
            if (hasFocus)
            {
                Application.targetFrameRate = maxFocusedFramerate;
            }
            else
            {
                Application.targetFrameRate = 23;
            }
#endif

            if (alsoStopTime)
            {
                Time.timeScale = hasFocus ? 1 : 0;
            }

            // -------------
        }

        // -------------
    }
}
/* maria aurelia at 03 December 2021 🌊 */
/* dreamy guardian ASCII kitty by Felix Lee, found at asciiart.eu 🐱‍👤 */