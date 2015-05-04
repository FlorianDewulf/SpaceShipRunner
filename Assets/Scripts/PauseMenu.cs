using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour {

	public GUISkin skin;
	bool pause = false;
	float saveTimeScale;


	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		joueur joueur = (joueur) GameObject.Find ("VaisseauRouge").GetComponent<joueur> ();
		if(Input.GetKeyUp(KeyCode.Escape) && joueur.estEnVie) {
			this.tooglePause();
		}
	}

	public bool estEnPause()
	{
		return pause;
	}

	void tooglePause() {
		if (this.pause == false) {
			this.pause = true;
			this.saveTimeScale = Time.timeScale;
			Time.timeScale = 0;
		} else {
			this.pause = false;
			Time.timeScale = this.saveTimeScale;
		}
	}

	void OnGUI()
	{
		GUI.skin = this.skin;

		if(pause)
		{
			GUI.Label(new Rect(Screen.width / 2 - 150, Screen.height / 2 - 250, 300, 100), "PAUSE");
			if(GUI.Button(new Rect(Screen.width / 2 - 250, Screen.height / 2 - 250 + 125, 500, 100), "REPRENDRE")) {
				this.tooglePause();
			} else if(GUI.Button(new Rect(Screen.width / 2 - 250, Screen.height / 2 - 250 + 125 + 125, 500, 100), "QUITTER")) {
				this.pause = false;
				Application.LoadLevel(0);
			}
		}
	}
}
