using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Events;

public class AppMinimizer : MonoBehaviour
{
    [DllImport("user32.dll")]
    private static extern bool ShowWindow(IntPtr hwnd, int nCmdShow);
    [DllImport("user32.dll")]
    private static extern IntPtr GetActiveWindow();

    [Header("Settings")]
    [SerializeField] KeyCode keyCode;

    [Space]
    [SerializeField] UnityEvent OnMaximized;

    void Update()
    {
        if (Input.GetKeyDown(keyCode))
        {
            MinimizeApp();
        }
    }

    void MinimizeApp()
    {
        ShowWindow(GetActiveWindow(), 2);
    }
}
