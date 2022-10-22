using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResetLevel : MonoBehaviour
{
	public Button[] ResetButtons;

	public void Reset()
	{
		PlayerPrefs.DeleteAll();
}
}
