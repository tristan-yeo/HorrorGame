using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class menu : MonoBehaviour
{
    [SerializeField] private string gameSceneName = "EditScene";
    [SerializeField] private string escapeSceneName = "Escaped";
    [SerializeField] private string menuSceneName = "Menu";
    //[SerializeField] private SceneController _sceneController;

    public void StartGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }
    public void Escaped()
    {
        SceneManager.LoadScene(escapeSceneName);
    }
    public void Restart()
    {
        Debug.Log("Restart pressed");
        SceneManager.LoadScene(gameSceneName);
    }
    public void Exit()
    {
        Debug.Log("Exit pressed");
        SceneManager.LoadScene(menuSceneName);
    }
    public void Quit()
    {
        Application.Quit();
    }
}
