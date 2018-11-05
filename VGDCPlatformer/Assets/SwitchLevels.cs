using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchLevels : MonoBehaviour {

	void OpenLevel(Collider other)
	{
		SceneManager.LoadScene("FallenLevelScene"); //change level to our game
	}
}
