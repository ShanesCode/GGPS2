using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashCanvas : MonoBehaviour
{
    static SplashCanvas splashCanvas;

    private void Awake()
    {
        if (splashCanvas == null)
        {
            splashCanvas = this; // In first scene, make us the singleton.
            DontDestroyOnLoad(gameObject);
        }
        else if (splashCanvas != this)
        {
            Destroy(gameObject); // On reload, singleton already set, so destroy duplicate.
        }
    }
}
