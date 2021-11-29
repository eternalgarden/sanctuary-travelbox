using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// todo rename to MathPresenter
public class Circulate : MonoBehaviour
{
    public enum MathAnimation { Circulate, FlyAwayWithTan, SideRotate, Spiral, Hover }

    [SerializeField] float radius = 15f;
    [SerializeField] Vector3 radiusPerAxis;
    [SerializeField] Vector3 speedPerAxis;
    [SerializeField] Vector3 offsetPerAxis;
    [SerializeField] MathAnimation mathAnimation;

    float clock = 0f;
    Vector3 startpos;
    Quaternion startrot;
    Action onTick;

    void Start()
    {
        startpos = transform.localPosition;
        startrot = transform.localRotation;

        switch (mathAnimation)
        {
            case MathAnimation.Circulate:
                onTick += CirculateAnimation;
                break;
            case MathAnimation.FlyAwayWithTan:
                onTick += FlyAwayWithTan;
                break;
            case MathAnimation.SideRotate:
                onTick += SideRotate;
                break;
            case MathAnimation.Spiral:
                onTick += Spiral;
                break;
            case MathAnimation.Hover:
                onTick += Hover;
                break;
        }

    }

    // Update is called once per frame
    void Update()
    {
        clock += Time.deltaTime;

        // radius *=  Mathf.Sin(clock);
        onTick();

    }

    void CirculateAnimation()
    {
        Vector3 newPos = new Vector3
        (
            startpos.x + radius * Mathf.Cos(Time.time),
            startpos.y + radius * Mathf.Sin(Time.time),
            startpos.z
        );
        // todo oops i dont even know yet why the order of multiplication here matters. or even if it is a multiplication
        // transform.localRotation *= Quaternion.AngleAxis(Time.time, Vector3.up);
        newPos = transform.localRotation * newPos;
        transform.localPosition = newPos;
    }

    void Spiral()
    {
        Vector3 spiralPos = new Vector3
        (
            radius * Mathf.Cos(Time.time),
            radius * Mathf.Sin(Time.time),
            Mathf.PingPong(Time.time * speedPerAxis.z, 2)
        );

        spiralPos += startpos; //* I really need a quaternions tutorial

        // newPos = transform.localRotation * newPos;
        transform.localPosition = spiralPos;
    }

    void SideRotate()
    {
        // todo oki it does not work on parent because the children are not using their localrotation and position anymore
        float angle = Mathf.LerpUnclamped(0, 180, Mathf.Sin(Time.time));
        Debug.Log(angle);
        transform.localRotation = startrot * Quaternion.AngleAxis(angle, Vector3.up);//Quaternion.AngleAxis(Mathf.Sin(Time.time), Vector3.up);
        // transform.localRotation = startrot * Quaternion.(angle, Vector3.up);//Quaternion.AngleAxis(Mathf.Sin(Time.time), Vector3.up);

    }

    void Hover()
    {
        Vector3 hover = new Vector3
        (
            radiusPerAxis.x * Mathf.Sin(Time.time * speedPerAxis.x / 100f + offsetPerAxis.x),
            radiusPerAxis.y * Mathf.Sin(Time.time * speedPerAxis.y / 100f + offsetPerAxis.y),
            radiusPerAxis.z * Mathf.Sin(Time.time * speedPerAxis.z / 100f + offsetPerAxis.z)
        );

        transform.localPosition = startpos + hover;
        transform.localRotation = startrot * Quaternion.FromToRotation(Vector3.up, -hover + radius * Vector3.up); // radius is just used as strength here
    }

    private void FlyAwayWithTan()
    {
        Vector3 flyAwayPos = new Vector3
        (
            radius * Mathf.Cos(Time.time),
            radius * Mathf.Sin(Time.time),
            radius * Mathf.Tan(Time.time)
        );

        transform.localPosition = startpos + flyAwayPos;

    }
}

//     // Update is called once per frame
//     void FixedUpdate()
//     {
//         clock += Time.fixedTime;

//         transform.position = new Vector3
//         (
//             transform.position.x + Mathf.Cos(clock) / radius,
//             transform.position.y + Mathf.Sin(clock) / radius,
//             transform.position.z
//         );
//     }
// }
