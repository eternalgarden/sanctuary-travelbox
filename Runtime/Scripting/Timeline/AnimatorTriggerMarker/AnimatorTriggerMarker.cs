using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace PowderBox.Timeline
{
    [CustomStyle("Annotation")]
    public class AnimatorTriggerMarker : Marker, INotification
    {
        public AnimatorControllerParameterType animatorParameterType = AnimatorControllerParameterType.Trigger;

        [Tooltip("Notify this trigger string to INotificationReceiver (It should be the AnimatorTriggerReceiver) on the same GameObject as the bound Animator.")]
        public string animatorProperty;
        public bool boolValue;
        public float floatValue;
        public int intValue;
        public PropertyName id => animatorProperty.GetHashCode();
    }
}
