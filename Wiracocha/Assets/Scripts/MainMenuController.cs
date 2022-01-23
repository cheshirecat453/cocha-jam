using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    
    public void PlayGame()
    {
        SceneManager.LoadScene("World1_1");

    }

    public void Quit()
    {
        Debug.Log("Game Over");
        Application.Quit();
    }

}
