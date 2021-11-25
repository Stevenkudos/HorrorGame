using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class UIFunctions : MonoBehaviour
{
    public GameObject player;
    public Canvas UI1;
    public Canvas UI2;

    public GameObject janitor;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void StartGame()
    {
        if (UI1 != null)
        {
            UI1.enabled = true;
            player.GetComponent<FirstPersonController>().enabled = true;
            UI2.enabled = false;
            StartCoroutine(JanitorAlive());
        }
    }

    IEnumerator JanitorAlive()
    {
        yield return new WaitForSeconds(10);
        janitor.SetActive(true);
    }
    public void LoadGame()
    {
        SceneManager.LoadScene("Level1");
    }
    
    public void LoadEnd()
    {
        Application.Quit();
    }
}
