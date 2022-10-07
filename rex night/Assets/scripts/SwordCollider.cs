using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collider : MonoBehaviour {

    [SerializeField]
    private string targetTag;

    void OnTriggerEnter2D(UnityEngine.Collider2D other)
    {
        if(other.tag==targetTag)
        {
            GetComponent<UnityEngine.Collider2D>().enabled = false;

        }
    }
}
