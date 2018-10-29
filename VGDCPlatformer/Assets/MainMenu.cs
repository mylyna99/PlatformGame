using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	public void PlayGame()
	{
		SceneManager.LoadScene("Jump_Scene"); //change level to our game
	}

	public void QuitGame()
	{
		Debug.Log("QUIT");
		Application.Quit();
	}
}