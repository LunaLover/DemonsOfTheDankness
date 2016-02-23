using UnityEngine;
using System.Collections;

public class Pause : MonoBehaviour {

	void FixedUpdate ()
    {
	    if (Input.GetKeyDown(KeyCode.Escape))
        {
            Screen.lockCursor = false;
            Time.timeScale = 0;
        }
	}
}
