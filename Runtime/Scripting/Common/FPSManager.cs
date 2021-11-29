using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PowderBox.Common
{
public class FPSManager : MonoBehaviour
{
    void Awake()
    {
        Application.targetFrameRate = 60;
    }
}
}
