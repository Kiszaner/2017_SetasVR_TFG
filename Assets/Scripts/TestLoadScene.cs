using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestLoadScene : MonoBehaviour {

	// Use this for initialization
	void Update () {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SceneManager.LoadScene("Main Menu");
        }
	}
}
