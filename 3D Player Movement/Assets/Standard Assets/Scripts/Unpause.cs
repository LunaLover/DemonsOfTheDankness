using UnityEngine;
using System.Collections;

public class Unpause : MonoBehaviour {

	public void unpause()
    {
        Screen.lockCursor = true;
        Time.timeScale = 1;
	}


}
