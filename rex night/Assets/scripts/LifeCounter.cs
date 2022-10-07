
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class LifeCounter : MonoBehaviour {

    private Text livesText;

	void Awake () {
        livesText = GetComponent<Text>();	}
	
	// Update is called once per frame
	void Update ()
    {
        livesText.text = "LIVES: " + player.RemainingLives.ToString();
	}
}
