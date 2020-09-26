using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class sign_d : MonoBehaviour
{
    public GameObject signBox;
    public Text text;
    public string message;
    public bool playerInRange;
    void Start()
    {
        
    }
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = true;
            signBox.SetActive(true);
            text.text = message;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = false;
            signBox.SetActive(false);
        }
    }
}
