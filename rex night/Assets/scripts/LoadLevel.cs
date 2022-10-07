using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevel : MonoBehaviour {
    [SerializeField]
    private string loadLevel;

    void OnTriggerEnter2D(UnityEngine.Collider2D other)
    {
        if(other.tag=="Player")
        {
            SceneManager.LoadScene(loadLevel);
        }
    }

}
