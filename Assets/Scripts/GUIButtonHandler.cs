using UnityEngine;
using UnityEngine.SceneManagement;

public class GUIButtonHandler : MonoBehaviour
{
    public GameObject menu;
    private bool sceneLoaded = false;
    private bool gamePaused = false;

    private void Update()
    {
        showPauseMenu();
    }
    public void loadGame()
    {
        //load level one
        //when I load this level I want the canvas populate to the new level
        DontDestroyOnLoad(this.gameObject);
        menu.SetActive(false);
        sceneLoaded = true;
        SceneManager.LoadScene("SampleScene");
    }

    public void exitGame()
    {
        //this only works on a full build
        Application.Quit();
        Debug.Log("Exit Application..");
    }

    private void showPauseMenu()
    {
        if (Input.GetKeyDown(KeyCode.P) && sceneLoaded)
        {
            if(!gamePaused)
            {
                menu.SetActive(true);
                Time.timeScale = 0;
                gamePaused = true;
            }
            else
            {
                menu.SetActive(false);
                Time.timeScale = 1;
                gamePaused = false;
            }
            
        }
    }
}
