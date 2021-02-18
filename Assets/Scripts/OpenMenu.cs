using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpenMenu : MonoBehaviour
{
    public static MainMusic mainMusic;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void openmenu()
    {
        Destroy(GameObject.Find("MusicObj"));
        PlayerPrefs.DeleteKey("TotalScore");
        SceneManager.LoadScene("menu");
    }
}
