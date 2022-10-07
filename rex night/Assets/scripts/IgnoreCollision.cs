using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreCollision : MonoBehaviour {

    [SerializeField]
    private UnityEngine.Collider2D other;
	
    // Use this for initialization
	private void Awake ()
    {
        Physics2D.IgnoreCollision(GetComponent<UnityEngine.Collider2D>(), other, true);
		
	}
	
	
}
