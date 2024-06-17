using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public void RestartButton()
    {
        SceneManager.LoadScene("Map");
    }
    public void ExitButton() 
    {
        SceneManager.LoadScene("Begin Scene");
    }
}
