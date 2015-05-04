using UnityEngine;
using System.Collections;

public class MenuChoixVaisseau : MonoBehaviour {

	public GUISkin skinPerso;
	Jukebox jukebox;
	Configuration configuration;
	
	public Texture vaisseauBleuActive;
	public Texture vaisseauVertActive;

	public Texture vaisseauBleuNonActive;
	public Texture vaisseauVertNonActive;

	public Texture vaisseauRouge;
	private Texture vaisseauBleu;
	private Texture vaisseauVert;

	public GUIText message;

	// Use this for initialization
	void Start () {
		this.configuration = GameObject.Find ("Configuration").GetComponent<Configuration> ();
		this.message.enabled = false;
		this.jukebox = GameObject.Find ("Jukebox").GetComponent<Jukebox> ();

		if (this.configuration.vaisseauBleu == 1) {
			this.vaisseauBleu = this.vaisseauBleuActive;
		} else {
			this.vaisseauBleu = this.vaisseauBleuNonActive;
		}

		if (this.configuration.vaisseauVert == 1) {
			this.vaisseauVert = this.vaisseauVertActive;
		} else {
			this.vaisseauVert = this.vaisseauVertNonActive;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI() {
		GUI.skin = this.skinPerso;

		if (GUI.Button (new Rect (75, Screen.height / 2 - 75, 200, 200), new GUIContent (this.vaisseauRouge))) {
			this.configuration.choixVaisseau = 0;
			jukebox.activerSonJeu();
			Application.LoadLevel(1);
		}

		if (GUI.Button (new Rect (325, Screen.height / 2 - 75, 200, 200), new GUIContent (this.vaisseauBleu))) {
			if (this.configuration.vaisseauBleu == 1) {
				this.configuration.choixVaisseau = 1;
				jukebox.activerSonJeu();
				Application.LoadLevel(1);
			} else {
				StartCoroutine(ShowMessage(3));
			}
		}

		if (GUI.Button (new Rect (575, Screen.height / 2 - 75, 200, 200), new GUIContent (this.vaisseauVert))) {
			if (this.configuration.vaisseauVert == 1) {
				this.configuration.choixVaisseau = 2;
				jukebox.activerSonJeu();
				Application.LoadLevel(1);
			} else {
				StartCoroutine(ShowMessage(3));
			}
		}

		this.skinPerso.button.fontSize = 20;
		if (GUI.Button (new Rect (Screen.width - 175, Screen.height - 75, 150, 50), "RETOUR")) {
			Application.LoadLevel (0);
		}
		this.skinPerso.button.fontSize = 45;;
	}

	IEnumerator ShowMessage (float delay) {
		//this.message.text = message;
		this.message.enabled = true;
		yield return new WaitForSeconds(delay);
		this.message.enabled = false;
	}
}

