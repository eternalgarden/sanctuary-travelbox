/* 
      |\      _,,,---,,_
ZZZzz /,`.-'`'    -.  ;-;;,_
     |,4-  ) )-,_. ,\ (  `'-'
    '---''(_/--'  `-'\_)
*/

#if UNITY_EDITOR

using System;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace TravelBox.Common
{
    /*
    So it's a little baby script to make a Transform follow position and rotation of the scene camera.

    This is useful when trying out post processing and some other stuff that doesnt render in scene camera the
    way it will look in game mode.

    */
    [ExecuteInEditMode]
    public class MimicSceneCamera : MonoBehaviour
    {
        // -------------

        [SerializeField, ReadOnly] Vector3 _savedPosition;
        [SerializeField, ReadOnly] Quaternion _savedRotation;
        [SerializeField] Transform _mimePositionTransform;
        [SerializeField] Transform _mimerotationTransform;
        [SerializeField] bool _mimicSceneCamera;

        Camera _sceneViewCamera;

        void Awake()
        {
            // -------------
            
            if (Application.isPlaying)
            {
                ResetObjectPosition();
            }
            
            // -------------
        }

        void Update()
        {
            // -------------

            if (_mimicSceneCamera)
            {
                if (_sceneViewCamera != SceneView.lastActiveSceneView.camera)
                {
                    _sceneViewCamera = SceneView.lastActiveSceneView.camera;
                }

                _mimePositionTransform.position = _sceneViewCamera.transform.position;
                _mimerotationTransform.rotation = _sceneViewCamera.transform.rotation;
            }

            // -------------
        }

        [Button]
        void SaveThisObjectsLocation()
        {
            /* ⭐ ---- ---- */

            _mimicSceneCamera = false;

            _savedPosition = _sceneViewCamera.transform.position;
            _savedRotation = _sceneViewCamera.transform.rotation;

            /* ---- ---- 🌠 */
        }

        [Button]
        void ResetObjectPosition()
        {
            /* ⭐ ---- ---- */

            _mimicSceneCamera = false;

            _mimePositionTransform.position = _savedPosition;
            _mimerotationTransform.rotation = _savedRotation;

            /* ---- ---- 🌠 */
        }

        // -------------
    }
}

/* maria aurelia at 14 May 2022 🌊 */
/* dreamy guardian ASCII kitty by Felix Lee, found at asciiart.eu 🐱‍👤 */

#endif
