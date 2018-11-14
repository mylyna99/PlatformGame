using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchLevels : MonoBehaviour {

	public string levelToLoad;
	
	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			Debug.Log("PLAYER TOUCHED: SHOULD LOAD LEVEL");
			SceneManager.LoadScene(levelToLoad); //change level to our game
		}
	}
}