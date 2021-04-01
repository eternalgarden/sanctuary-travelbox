
/* 
Original idea and parts of code by 5argon / Sirawat Pitaksarit @ https://gametorrahod.com/uielements-custom-marker/ <3
Code is a bit messy in terms of CreateInspectorGUI method, oops
*/

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Timeline;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace PowderBox.Timeline
{
    [CustomEditor(typeof(AnimatorTriggerMarker))]
    [CanEditMultipleObjects]
    public class AnimatorTriggerMarkerDrawer : Editor
    {
        Animator _animator;

        VisualElement _mainDrawer;
        VisualElement _animatorPropertiesDrawer;
        VisualElement _triggerDrawer;
        VisualElement _boolDrawer;
        VisualElement _floatDrawer;
        VisualElement _intDrawer;

        SerializedProperty _markerTypeProperty;
        SerializedProperty _animatorPropertyName;
        SerializedProperty _boolValue;
        SerializedProperty _floatValue;
        SerializedProperty _intValue;

        public override VisualElement CreateInspectorGUI()
        {
            // -------------

            _mainDrawer = new VisualElement();

            _animator = GetBoundAnimator();

            GetSerializedProperties();

            AnimatorControllerParameterType parameterType = GetAnimatorParameterType(_markerTypeProperty.enumValueIndex);

            DrawHeader(parameterType);

            InitializeAnimatorPropertiesDrawer();

            RefreshDrawer(parameterType, false);

            return _mainDrawer;

            // -------------
        }

        private void GetSerializedProperties()
        {
            // -------------

            _markerTypeProperty = serializedObject.FindProperty(nameof(AnimatorTriggerMarker.animatorParameterType));
            _animatorPropertyName = serializedObject.FindProperty(nameof(AnimatorTriggerMarker.animatorProperty));
            _boolValue = serializedObject.FindProperty(nameof(AnimatorTriggerMarker.boolValue));
            _floatValue = serializedObject.FindProperty(nameof(AnimatorTriggerMarker.floatValue));
            _intValue = serializedObject.FindProperty(nameof(AnimatorTriggerMarker.intValue));

            // -------------
        }

        // This odity is required again because SerializedProperty of type enum requires an enum index instead of its value
        private AnimatorControllerParameterType GetAnimatorParameterType(int enumIndex)
        {
            // -------------

            var parameterEnumArray = Enum.GetValues(typeof(AnimatorControllerParameterType));
            return (AnimatorControllerParameterType)parameterEnumArray.GetValue(enumIndex);

            // -------------
        }

        private void InitializeAnimatorPropertiesDrawer()
        {
            // -------------

            _animatorPropertiesDrawer = new VisualElement();
            _mainDrawer.Add(_animatorPropertiesDrawer);

            _triggerDrawer = DrawAnimatorPropertyEditor("Available Triggers", AnimatorControllerParameterType.Trigger);
            _boolDrawer = DrawAnimatorPropertyEditor("Available Bools", AnimatorControllerParameterType.Bool);
            _floatDrawer = DrawAnimatorPropertyEditor("Available Floats", AnimatorControllerParameterType.Float);
            _intDrawer = DrawAnimatorPropertyEditor("Available Ints", AnimatorControllerParameterType.Int);

            // -------------
        }

        private void RefreshDrawer(AnimatorControllerParameterType newDrawerType, bool applyModifications)
        {
            // -------------

            _animatorPropertiesDrawer.Clear();

            switch (newDrawerType)
            {
                case AnimatorControllerParameterType.Trigger:
                    _animatorPropertiesDrawer.Add(_triggerDrawer);
                    break;
                case AnimatorControllerParameterType.Bool:
                    _animatorPropertiesDrawer.Add(_boolDrawer);
                    break;
                case AnimatorControllerParameterType.Float:
                    _animatorPropertiesDrawer.Add(_floatDrawer);
                    break;
                case AnimatorControllerParameterType.Int:
                    _animatorPropertiesDrawer.Add(_intDrawer);
                    break;
                default:
                    break;
            }

            if (applyModifications)
            {
                // Thats odd but we cannot use underlying enum value (since it has weird values in Unity) but an index instead.
                int index = Array.IndexOf(Enum.GetValues(newDrawerType.GetType()), newDrawerType);
                _markerTypeProperty.enumValueIndex = index;
                _animatorPropertyName.stringValue = null;
                serializedObject.ApplyModifiedProperties();
            }

            // ! This is is importaaaaant
            // Otherwise property fields won't be visible
            _mainDrawer.Bind(serializedObject);

            // -------------
        }

        private void DrawHeader(AnimatorControllerParameterType parameterType)
        {
            // -------------

            var header = new VisualElement();
            header.style.flexDirection = FlexDirection.RowReverse;

            var usagi = new Image();
            string path = "Assets/Shaders102/Shaders/MajorArcana/11-20/020_walkLesson/materials/JsF.gif";
            var obj = (Texture)AssetDatabase.LoadAssetAtPath(path, typeof(Texture));
            Texture tex = Resources.Load(path) as Texture;
            usagi.image = obj;
            usagi.style.height = 78.6f;
            usagi.style.width = 100;
            header.Add(usagi);

            var parameterSelection = new VisualElement();
            parameterSelection.style.flexBasis = StyleKeyword.Auto;
            parameterSelection.style.alignSelf = Align.FlexEnd;
            parameterSelection.style.flexGrow = 1.1f;

            var label = new Label("Select Animator Marker Parameter Type");
            label.style.unityFontStyleAndWeight = FontStyle.Bold;
            parameterSelection.Add(label);

            // Enum field triggering editor redraw
            EnumField enm = new EnumField(parameterType);
            enm.RegisterValueChangedCallback((x) => RefreshDrawer((AnimatorControllerParameterType)x.newValue, true));
            parameterSelection.Add(enm);

            header.Add(parameterSelection);
            _mainDrawer.Add(header);

            // -------------
        }

        private void DrawSeparatorLine(VisualElement vis)
        {
            // -------------

            var box = new Box();
            box.style.height = 1;
            box.style.marginTop = 5;
            box.style.marginBottom = 5;
            vis.Add(box);

            // -------------
        }

        private void DrawBase(VisualElement vis, string labelName)
        {
            // -------------

            DrawSeparatorLine(vis);

            var label = new Label(labelName);
            label.style.unityFontStyleAndWeight = FontStyle.Bold;
            vis.Add(label);

            var animatorPreview = new ObjectField { objectType = typeof(Animator) };
            var rac = new ObjectField { objectType = typeof(RuntimeAnimatorController) };
            animatorPreview.SetValueWithoutNotify(_animator);
            rac.SetValueWithoutNotify(_animator.runtimeAnimatorController);

            vis.Add(animatorPreview);
            vis.Add(rac);

            // -------------
        }

        private void DrawAnimatorPropertyField(VisualElement vis)
        {
            // -------------

            DrawSeparatorLine(vis);
            var label = new Label("Property Settings");
            label.style.unityFontStyleAndWeight = FontStyle.Bold;
            vis.Add(label);
            PropertyField pf = new PropertyField(_animatorPropertyName);
            pf.tooltip = "Animator property name that will be set when this marker triggers on a timeline.";
            vis.Add(pf);

            // -------------
        }

        private void DrawParameterSpecificField(VisualElement vis, AnimatorControllerParameterType animatorParameterType)
        {
            // -------------

            PropertyField propertyField;

            switch (animatorParameterType)
            {
                case AnimatorControllerParameterType.Bool:
                    propertyField = new PropertyField(_boolValue);
                    propertyField.tooltip = "...";
                    break;
                case AnimatorControllerParameterType.Float:
                    propertyField = new PropertyField(_floatValue);
                    propertyField.tooltip = "...";
                    break;
                case AnimatorControllerParameterType.Int:
                    propertyField = new PropertyField(_intValue);
                    propertyField.tooltip = "...";
                    break;
                default:
                    return;
            }

            vis.Add(propertyField);

            // -------------
        }

        private void DrawButtonList(VisualElement vis, AnimatorControllerParameterType parameterType)
        {
            // -------------

            if (_animator != null)
            {
                List<string> properties = _animator.parameters
                        .Where(x => x.type == parameterType)
                        .Select(x => x.name)
                        .ToList();

                foreach (string element in properties)
                {
                    Button btn = new Button(() =>
                    {
                        _animatorPropertyName.stringValue = element;
                        serializedObject.ApplyModifiedProperties();
                    })
                    { text = element };

                    btn.style.unityTextAlign = TextAnchor.MiddleLeft;
                    vis.Add(btn);
                }
            }

            // -------------
        }

        private VisualElement DrawAnimatorPropertyEditor(string label, AnimatorControllerParameterType animatorParameterType)
        {
            // -------------

            VisualElement vis = new VisualElement();
            DrawAnimatorPropertyField(vis);
            DrawParameterSpecificField(vis, animatorParameterType);
            DrawBase(vis, label);
            DrawButtonList(vis, animatorParameterType);
            return vis;

            // -------------
        }

        private Animator GetBoundAnimator()
        {
            // -------------

            if (target is AnimatorTriggerMarker atm && TimelineEditor.inspectedDirector != null)
            {
                if (TimelineEditor.inspectedDirector.GetGenericBinding(atm.parent) is Animator ani)
                {
                    ani.Rebind();
                    return ani;
                }
            }
            return null;

            // -------------
        }
    }
}