using UnityEngine.SceneManagement;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public string sceneName;

    public GameObject settingsPanel;

    public static MenuManager instance;

    void Start() 
    {

    }

    public void NewGame()
    {
        SceneManager.LoadScene(sceneName);
    }

    public void Quit() 
    {
        #if UNITY_EDITOR
            //Editor Unity
            UnityEditor.EditorApplication.isPlaying = false;
        #else    
            //Compiled Game
            Application.Quit();
        #endif
    }

    public void ShowSettings()
    {
        settingsPanel.SetActive(true);
    }

    public void Back()
    {
        settingsPanel.SetActive(false);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
