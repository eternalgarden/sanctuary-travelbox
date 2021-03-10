using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace PowderBox.Timeline
{
    [RequireComponent(typeof(PlayableDirector))]
    public class TimelinePauseReceiver : MonoBehaviour, INotificationReceiver
    {
        [Tooltip("Check if you want markers to be active only once")]
        [SerializeField] bool IgnorelistCalledMarkers = true;

        PlayableDirector playableDirector;
        List<string> IgnoredMarkerIDs;

        void Awake()
        {
            // -------------

            playableDirector = GetComponent<PlayableDirector>();
            if (IgnorelistCalledMarkers) ClearMarkerIgnorelist();

            // -------------
        }

        public void ClearMarkerIgnorelist()
        {
            // -------------

            IgnoredMarkerIDs = new List<string>();

            // -------------
        }

        public void OnNotify(Playable origin, INotification notification, object context)
        {
            // -------------

            if (notification is TimelinePauseMarker tpm)
            {
                if (tpm.IsActive)
                {
                    tpm.IsActive = false;
                    playableDirector.Pause();
                }

            }

            // -------------
        }
    }
}
