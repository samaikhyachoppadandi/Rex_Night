using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collisionTrigger : MonoBehaviour {

    private BoxCollider2D playerCollider;

    [SerializeField]
    private BoxCollider2D platformCollider;

    [SerializeField]
    private BoxCollider2D platformTrigger; 

    // Use this for initialization
    void Start() {
        playerCollider = GameObject.Find("player").GetComponent<BoxCollider2D>();
        Physics2D.IgnoreCollision(platformCollider, platformTrigger, true);

    }
	
	void OnTriggerEnter2D(UnityEngine.Collider2D other)
    {
        if (other.gameObject.name == "player")
        {
            Physics2D.IgnoreCollision(platformCollider, playerCollider, true);
        }

    }

    void OnTriggerExit2D(UnityEngine.Collider2D other)
    {
        if(other.gameObject.name == "player")
        {
            Physics2D.IgnoreCollision(platformCollider, playerCollider, false);
        }

    }
}
