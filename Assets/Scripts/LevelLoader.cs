using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
	public void LevelOne()
	{
		SceneManager.LoadScene ("SceneOne");
	}
	public void LevelTwo()
	{
		SceneManager.LoadScene ("SceneTwo");
	}
}
