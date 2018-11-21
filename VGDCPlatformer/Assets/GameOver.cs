using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour {

	public GameObject gameOverUI;
	public static bool isGameOver = false; 

	void Update()
	{
		gameOverUI.SetActive(isGameOver);
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			Debug.Log("GAME OVER MENU SHOULD SHOW");
			GameOverMenu();
		}
	}


	void GameOverMenu()
	{
		isGameOver = true;
		Time.timeScale = 0f;
	}


}
