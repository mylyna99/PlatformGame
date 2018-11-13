using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchLevels : MonoBehaviour {

	public string levelToLoad;
	
	void OpenLevel(Collider2D other)
	{
		Debug.Log("SHOULD LOAD LEVEL");
		if (other.CompareTag("Player"))
		{
			SceneManager.LoadScene(levelToLoad); //change level to our game
		}
	}
}
