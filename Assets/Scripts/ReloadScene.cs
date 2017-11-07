using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadScene : MonoBehaviour {

	public void OnGUI()
	{
		if (GUI.Button(new Rect(76, 238, 52, 25), "Restart"))
	    UnityEngine.SceneManagement.SceneManager.LoadScene(0);
	}
}

