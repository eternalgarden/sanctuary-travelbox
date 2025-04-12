using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinemachinePlayerFollower : MonoBehaviour
{
    [Header("Camera rotation settings")]
    [SerializeField] float _horizontalSensitivity = 1f;
    [SerializeField] float _verticalSensitivity = 1f;

    Vector2 previousPlayerInput;
    Vector2 playerInputDelta;

    void Update()
    {
        Vector2 playerInput = new Vector2(
            Input.GetAxis("Mouse X"),
            Input.GetAxis("Mouse Y")
        );
        playerInput = Vector2.ClampMagnitude(playerInput, 1f);
        // playerInputDelta = previousPlayerInput - playerInput;
        // Debug.Log($"{playerInputDelta}");
        
        UpdateRotation(playerInput);
        previousPlayerInput = playerInput;
    }

    void UpdateRotation(Vector2 playerInput)
    {
        PlayerBasedRotation(playerInput.x);
        TiltRotation(playerInput.y);
    }

    void PlayerBasedRotation(float horizontalRotation)
    {
        transform.rotation *= Quaternion.AngleAxis(horizontalRotation * _horizontalSensitivity, Vector3.up);
    }

    void TiltRotation(float verticalRotation)
    {
        transform.rotation *= Quaternion.AngleAxis(verticalRotation * _verticalSensitivity * -1, Vector3.right);

        var angles = transform.localEulerAngles;
        angles.z = 0;

        var angle = angles.x;

        if (angle > 180 && angle < 340)
        {
            angles.x = 340;
        }
        else if (angle < 180 && angle > 40)
        {
            angles.x = 40;
        }

        transform.localEulerAngles = angles;
    }
}
