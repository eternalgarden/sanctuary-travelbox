using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TravelBox.PostProcessing;

namespace TravelBox.Audio
{
    public class SwitchMaterialColorEffect : BaseAudioEffect
    {
        [Space, Header("Enable Object Effect")]
        [SerializeField] Material material;
        [SerializeField] string colorPropertyName = "_Color";
        [ColorUsageAttribute(false, true)] public Color inactiveColor;
        [SerializeField] bool useDefaultColorOnInactive;
        [ColorUsageAttribute(false, true)] public Color activeColor;
        [SerializeField] BlitMaterial mainDepthPostProcess;

        Color defaultColor;

        void Awake()
        {
            // TODO
            // Add null checks

            defaultColor = material.GetColor(colorPropertyName);
        }

        void OnDisable()
        {
            material.SetColor(colorPropertyName, defaultColor);
        }

        protected override void OnEffectActivate()
        {
            material.SetColor(colorPropertyName, activeColor);
        }

        protected override void OnEffectDeactivate()
        {
            material.SetColor(colorPropertyName, useDefaultColorOnInactive ? defaultColor : inactiveColor);
        }
    }
}