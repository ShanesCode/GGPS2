using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    static MusicController musicController;
    private void Awake()
    {
        if (musicController == null) {
            musicController = this; // In first scene, make us the singleton.
            DontDestroyOnLoad(gameObject);
        }
        else if(musicController != this) {
             Destroy(gameObject); // On reload, singleton already set, so destroy duplicate.
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
