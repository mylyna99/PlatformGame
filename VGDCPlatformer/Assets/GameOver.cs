using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour {

	public GameObject gameOverUI;
	public static bool isGameOver = false; 

	public AudioClip MusicClip;
	public AudioSource MusicSource;

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


	public void GameOverMenu()
	{
		isGameOver = true;
		Time.timeScale = 0f;
		MusicSource.clip = MusicClip;
		MusicSource.Stop();
	}


}
