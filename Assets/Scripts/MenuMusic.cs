using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuMusic : MonoBehaviour
{
    public GameObject music;
    public AudioSource audioSource;
    public Scene Scene;
    static bool playingMusic = false;
    // Start is called before the first frame update
    void Start()
    {

    }
    void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("menuMusic");
        if (objs.Length > 1)
        {
            Destroy(music);
        }
        
        DontDestroyOnLoad(music);
    }
    // Update is called once per frame
    void Update()
    {

    }
}
