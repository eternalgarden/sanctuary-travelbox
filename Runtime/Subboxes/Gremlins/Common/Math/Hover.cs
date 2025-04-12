using UnityEngine;

public class Hover : MonoBehaviour
{
    [SerializeField, Range(-10, 10)] float Blop = 5f;
    [SerializeField] Vector3Scrollers RadiusPerAxis;
    [SerializeField] Vector3Scrollers SpeedPerAxis;
    [SerializeField] Vector3Scrollers OffsetPerAxis;

    [SerializeField] bool _randomizePhase;

    Quaternion startrot;
    Vector3 startpos;
    Vector3 phase;

    void Awake()
    {
        // -------------

        startpos = transform.localPosition;
        startrot = transform.localRotation;

        phase = Vector3.zero;

        if (_randomizePhase)
        {
            phase = new Vector3(
                Random.Range(-100f, 100f),
                Random.Range(-100f, 100f),
                Random.Range(-100f, 100f)
            );
        }

        // -------------
    }

    void Update()
    {
        // -------------

        HoverAnimation();

        // -------------
    }

    void HoverAnimation()
    {
        // -------------

        Vector3 hover = new Vector3
        (
            RadiusPerAxis.X * Mathf.Sin(Time.time * SpeedPerAxis.X / 10f + phase.x) + OffsetPerAxis.X,
            RadiusPerAxis.Y * Mathf.Sin(Time.time * SpeedPerAxis.Y / 10f + phase.y) + OffsetPerAxis.Y,
            RadiusPerAxis.Z * Mathf.Sin(Time.time * SpeedPerAxis.Z / 10f + phase.z) + OffsetPerAxis.Z
        );

        transform.localPosition = startpos + hover;
        transform.localRotation =
            startrot * Quaternion.FromToRotation(Vector3.up,
                -hover + Blop * Vector3.up); // radius is just used as strength here

        // -------------
    }

    [System.Serializable]
    public class Vector3Scrollers
    {
        // -------------

        [Range(-10, 10)] public float X = 1f;
        [Range(-10, 10)] public float Y = -1f;
        [Range(-10, 10)] public float Z = 1f;

        // -------------
    }
}