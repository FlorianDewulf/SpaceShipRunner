using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MenuSucces : MonoBehaviour {

	// La configuration de la scene de base
	public GUISkin skinPerso;
	Configuration configuration;
	public GUIText message;

	// Les succes
	public Texture entreeEnBourseActive;
	public Texture entreeEnBourseInactive;
	public string entreeEnBourse;

	public Texture finDeMoisDifficileActive;
	public Texture finDeMoisDifficileInactive;
	public string finDeMoisDifficile;

	public Texture midasActive;
	public Texture midasInactive;
	public string midas;

	public Texture cresusActive;
	public Texture cresusInactive;
	public string cresus;
	
	public Texture loupWallStreetActive;
	public Texture loupWallStreetInactive;
	public string loupWallStreet;

	bool brule = false; // Laser
	bool philae = false; // Asteroide
	bool galileo = false; // Satellite

	bool stopCar = false; // 10 obstacles


	// Use this for initialization
	void Start () {
		this.configuration = GameObject.Find ("Configuration").GetComponent<Configuration> ();

		// On cache le message timé de base
		this.message.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI() {
		GUI.skin = this.skinPerso;

		GUI.skin.button.fontSize = 20;
		if (GUI.Button (new Rect (Screen.width - 175, Screen.height - 75, 150, 50), "RETOUR")) {
			Application.LoadLevel (0);
		}

		if (GUI.Button (new Rect (100, Screen.height / 2 - 50, 75, 75), new GUIContent ((this.configuration.succesEntreeEnBourse == 1) ? this.entreeEnBourseActive : this.entreeEnBourseInactive))) {
			StartCoroutine(ShowMessage(this.entreeEnBourse, 5));
		}

		if (GUI.Button (new Rect (100 + 150, Screen.height / 2 - 50, 75, 75), new GUIContent ((this.configuration.succesFinDeMoisDifficile == 1) ? this.finDeMoisDifficileActive : this.finDeMoisDifficileInactive))) {
			StartCoroutine(ShowMessage(this.finDeMoisDifficile, 5));
		}

		if (GUI.Button (new Rect (100 + 150 + 150, Screen.height / 2 - 50, 75, 75), new GUIContent ((this.configuration.succesMidas == 1) ? this.midasActive : this.midasInactive))) {
			StartCoroutine(ShowMessage(this.midas, 5));
		}

		if (GUI.Button (new Rect (100 + 150 + 150 + 150, Screen.height / 2 - 50, 75, 75), new GUIContent ((this.configuration.succesCresus == 1) ? this.cresusActive : this.cresusInactive))) {
			StartCoroutine(ShowMessage(this.cresus, 5));
		}

		if (GUI.Button (new Rect (100 + 150 + 150 + 150 + 150, Screen.height / 2 - 50, 75, 75), new GUIContent ((this.configuration.succesLoupDeWallStreet == 1) ? this.loupWallStreetActive : this.loupWallStreetInactive))) {
			StartCoroutine(ShowMessage(this.loupWallStreet, 5));
		}



		GUI.skin.label.fontSize = 50;
		GUI.skin.button.fontSize = 45;
	}

	IEnumerator ShowMessage (string text, float delay) {
		if (this.message.enabled == false) {
			this.message.text = text;
			this.message.enabled = true;
			yield return new WaitForSeconds (delay);
			this.message.enabled = false;
		}
	}
}

