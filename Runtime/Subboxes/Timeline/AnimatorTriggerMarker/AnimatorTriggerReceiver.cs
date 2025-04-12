using UnityEngine;
using UnityEngine.Playables;

namespace TravelBox.Timeline
{
    [RequireComponent(typeof(Animator))]
    public class AnimatorTriggerReceiver : MonoBehaviour, INotificationReceiver
    {
        Animator animator;

        void Awake()
        {
            animator = GetComponent<Animator>();
        }

        public void OnNotify(Playable origin, INotification notification, object context)
        {
            if (notification is AnimatorTriggerMarker atm)
            {
                animator.SetTrigger(atm.animatorProperty);
            }
        }
    }

}