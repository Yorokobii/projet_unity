using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {

	public void Start(){
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
	}

	public void Play(){
		SceneManager.LoadScene ("test_map1");
	}

	public void Tuto(){
		SceneManager.LoadScene ("DeplacerEtSauter");
	}

	public void Exit(){
		Application.Quit();
	}
}
