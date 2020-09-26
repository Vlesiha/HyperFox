using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class button : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    
    }
    public void lvl1()
    {
        PlayerPrefs.DeleteKey("TotalScore");
        SceneManager.LoadScene("SampleScene");

    }
    public void lvl2()
    {
        PlayerPrefs.DeleteKey("TotalScore");
        SceneManager.LoadScene("secscene");

    }
    public void lvl3()
    {
        PlayerPrefs.DeleteKey("TotalScore");
        SceneManager.LoadScene("thlvl");

    }
    public void lvl4()
    {
        PlayerPrefs.DeleteKey("TotalScore");
        SceneManager.LoadScene("4lvl");

    }
    public void lvl5()
    {
        PlayerPrefs.DeleteKey("TotalScore");
        SceneManager.LoadScene("fin");

    }
    public void openmenu()
    {
        PlayerPrefs.DeleteKey("TotalScore");
        SceneManager.LoadScene("menu");

    }
    public void howtoplay()
    {
        SceneManager.LoadScene("rules");

    }

    public void exitGame()
    {
        Application.Quit();
    }
}
