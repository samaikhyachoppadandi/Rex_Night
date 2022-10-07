
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{

    public void Quit()
    {
        Debug.Log("application quit");
        Application.Quit();
    }

}
