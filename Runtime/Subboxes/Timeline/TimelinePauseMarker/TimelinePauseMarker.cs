using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace TravelBox.Timeline
{
    [CustomStyle("TimeMarker")]
    public class TimelinePauseMarker : Marker, INotification
    {
        /*
        Note this likely has to be set to false when it caused a pause once, because on next 
        .Resume() call this would immediately get called again effectively making resume
        impossible. Found another workaround apart from that or even worse - slight time offsetting?
        */
        bool startsActive = true;
        bool isActive; // TODO Add a readonly inspector field

        public PropertyName id => $"PauseT{this.time}";

        public bool IsActive
        {
            get => isActive;
            set => isActive = value;
        }

        void OnEnable()
        {
            // -------------

            isActive = startsActive;

            // -------------
        }
    }
}
