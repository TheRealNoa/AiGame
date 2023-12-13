using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenue : MonoBehaviour
{
    public GameObject pauseMenu;
    public bool ispaused;
    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (ispaused)
            {
                resume();

            }
            else
            {
                OnApplicationPause();
                
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            ExitGameFunction();
        }
    }
    void ExitGameFunction()
    {
    Application.Quit();
    }

    public void OnApplicationPause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        ispaused = true;
    }
    public void resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        ispaused = false;
    }
}
