using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitScreenAspect : MonoBehaviour
{
    [SerializeField] protected int _width = 1280, _height = 720;
    [SerializeField] protected FullScreenMode _screenMode = FullScreenMode.Windowed;

    protected virtual void Awake()
    {
        Screen.SetResolution(_width, _height, _screenMode);
    }
}
