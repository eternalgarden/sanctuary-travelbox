using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TravelBox.PostProcessing
{
    [ExecuteInEditMode]
    public class BlitMaterial : MonoBehaviour
    {
        [SerializeField] Material postProcessMaterial;

        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            Graphics.Blit(source, destination, postProcessMaterial);
        }
    }
}