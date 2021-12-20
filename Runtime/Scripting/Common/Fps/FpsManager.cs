/* 
      |\      _,,,---,,_
ZZZzz /,`.-'`'    -.  ;-;;,_
     |,4-  ) )-,_. ,\ (  `'-'
    '---''(_/--'  `-'\_)
*/
using UnityEngine;

namespace Sanctuary.Core
{
    public class FpsManager : MonoBehaviour
    {
        // -------------

        [SerializeField] int fpsCap = 60;

        void Awake()
        {
            // -------------

            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = fpsCap;

            // -------------
        }

        // -------------
    }
}
/* maria aurelia at 09 November 2021 🌊 */
/* dreamy guardian ASCII kitty by Felix Lee, found at asciiart.eu 🐱‍👤 */