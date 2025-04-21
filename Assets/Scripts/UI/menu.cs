using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class menu : MonoBehaviour
{
    [SerializeField] private string gameSceneName = "EditScene";
    
    public void StartGame()
    {
        // Load the first game scene
        SceneManager.LoadScene(gameSceneName);
    }
}
