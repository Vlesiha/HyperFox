using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class totalScore : MonoBehaviour
{
    [SerializeField] private Text scam;
    // Start is called before the first frame update
    void Start()
    {
        scam.text = PlayerPrefs.GetInt("TotalScore", 0).ToString();
    }

}
