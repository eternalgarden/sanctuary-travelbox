using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PowderBox.Audio
{
    public class CameraFlagsSwitchEffect : BaseAudioEffect
    {
        [Space]
        [Header("Camera Flag Switch")]
        [SerializeField] Camera targetCamera;
        [SerializeField] CameraClearFlags activeFlags;
        [SerializeField] CameraClearFlags disabledFlags;

        protected override void OnEffectActivate()
        {
            spiritCamera.clearFlags = activeFlags;
        }

        protected override void OnEffectDeactivate()
        {
            spiritCamera.clearFlags = disabledFlags;
        }
    }
}