using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleOrbitalCamera : MonoBehaviour
{
    public Transform target;

    public float offsetSpeed = 2f;
    public float distance = 10f;
    public float offsetY = 2;
    public float rotateSpeed = 20f;

    float xSpeed = 100;
    float ySpeed = 100;

    float x;
    float y;


    // float somefactor = 0.2f;

    void Start()
    {
        var angles = transform.eulerAngles;
        x = angles.x;
        y = angles.y;
    }

    void Update()
    {
        if (target)
        {
            // x += xSpeed * somefactor * Time.deltaTime;
            y += ySpeed * rotateSpeed / 100 * Time.deltaTime;

            Quaternion rotation = Quaternion.Euler(x,y,0);

            // Vector3 position = rotation * new Vector3(0,0,-distance) + target.position;

            transform.rotation = rotation;
            
            Vector3 position = -transform.forward * distance + target.position;

            position.y += offsetY;
            
            transform.position = position;
        }
        
        if(Input.GetKey(KeyCode.Q))
        {
            offsetY += offsetSpeed * Time.deltaTime;
        }
        if(Input.GetKey(KeyCode.E))
        {
            offsetY -= offsetSpeed * Time.deltaTime;
        }
        if(Input.GetKey(KeyCode.W))
        {
            distance -= offsetSpeed * Time.deltaTime;
        }
        if(Input.GetKey(KeyCode.S))
        {
            distance += offsetSpeed * Time.deltaTime;
        }
    }
}
