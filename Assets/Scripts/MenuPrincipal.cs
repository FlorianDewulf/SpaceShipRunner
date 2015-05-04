using UnityEngine;
using System.Collections;

public class MenuPrincipal : MonoBehaviour {

	public GUISkin skinPerso;
	private bool menuPrincipalActive = true;
	private bool menuOptionsActive = false;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI() {
		GUI.skin = this.skinPerso;

		if (this.menuPrincipalActive) {
			if (GUI.Button (new Rect (Screen.width / 2 - 150, Screen.height / 2 - 150 + 50, 350, 50), "JOUER")) {
				Application.LoadLevel (5);
			} else if (GUI.Button (new Rect (Screen.width / 2 - 150, Screen.height / 2 - 150 + 125, 350, 50), "BOUTIQUE")) {
				Application.LoadLevel (3);
			} else if (GUI.Button (new Rect (Screen.width / 2 - 150, Screen.height / 2 - 150 + 125 + 75, 350, 50), "SUCCÈS")) {
				Application.LoadLevel (4);
			} else if (GUI.Button (new Rect (Screen.width / 2 - 150, Screen.height / 2 - 150 + 125 + 75 + 75, 350, 50), "OPTIONS")) {
				this.menuPrincipalActive = false;
				this.menuOptionsActive = true;
			} else if (GUI.Button (new Rect (Screen.width / 2 - 150, Screen.height / 2 - 150 + 200 + 75 + 75, 350, 50), "QUITTER")) {
				Application.Quit ();
			}
		} else if (this.menuOptionsActive) {
			// Retreiving configuration
			GameObject Configuration = GameObject.Find("Configuration");
			Configuration ConfigurationScript = (Configuration) Configuration.GetComponent("Configuration");

			if (GUI.Button (new Rect (Screen.width / 2 - 150, Screen.height / 2 - 150 + 200 + 150, 300, 100), "RETOUR")) {
				ConfigurationScript.saveSon();
				this.menuPrincipalActive = true;
				this.menuOptionsActive = false;
			}

			ConfigurationScript.sonAmbiance = GUI.HorizontalSlider(new Rect (Screen.width / 2 - 450, Screen.height / 2 - 150 + 50, 300, 100), ConfigurationScript.sonAmbiance, 0.0F, 0.8F);
			GUI.Label(new Rect (Screen.width / 2 - 100, Screen.height / 2 - 175 + 50, 400, 100), "musique");

			ConfigurationScript.sonBruitage = GUI.HorizontalSlider(new Rect (Screen.width / 2 - 450, Screen.height / 2 - 150 + 200, 300, 100), ConfigurationScript.sonBruitage, 0.0F, 1.0F);
			GUI.Label(new Rect (Screen.width / 2 - 100, Screen.height / 2 - 175 + 200, 400, 100), "bruitages");
		}
	}
}

