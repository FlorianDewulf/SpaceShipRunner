using UnityEngine;
using System.Collections;

public class MenuBoutique : MonoBehaviour {

	public GUISkin skinPerso;
	Configuration configuration;

	public GUIText message;

	public Texture vaisseauBleuActive;
	public Texture vaisseauVertActive;
	
	public Texture vaisseauBleuNonActive;
	public Texture vaisseauVertNonActive;
	
	public Texture vaisseauRouge;
	private Texture vaisseauBleu;
	private Texture vaisseauVert;

	// Use this for initialization
	void Start () {
		this.configuration = GameObject.Find ("Configuration").GetComponent<Configuration> ();

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
	
		GUI.skin.label.fontSize = 25;
		GUI.Label(new Rect (5, Screen.height / 2 - 200, 250, 100), "Pièces : " + this.configuration.pieces);

		GUI.Button (new Rect (100, Screen.height / 2 - 25, 150, 150), new GUIContent (this.vaisseauRouge));
		GUI.Button (new Rect (350, Screen.height / 2 - 25, 150, 150), new GUIContent (this.vaisseauBleu));
		GUI.Button (new Rect (600, Screen.height / 2 - 25, 150, 150), new GUIContent (this.vaisseauVert));

		GUI.Label(new Rect (100, Screen.height / 2 - 95, 150, 50), "Gratuit");
		GUI.Label(new Rect (350, Screen.height / 2 - 95, 150, 50), "500 pièces");
		GUI.Label(new Rect (600, Screen.height / 2 - 95, 150, 50), "1000 pièces");

		if (this.configuration.vaisseauBleu == 0) {
			if (GUI.Button (new Rect (350, Screen.height / 2 + 150, 150, 50), "Acheter")) {
				if (this.configuration.pieces >= 500) {
					this.configuration.incPieces(-500);
					this.configuration.debloquerVaisseauBleu();
				} else {
					StartCoroutine(ShowMessage("Vous n'avez pas assez de pièces", 3));
				}
			}
		}

		if (this.configuration.vaisseauVert == 0) {
			if (GUI.Button (new Rect (600, Screen.height / 2 + 150, 150, 50), "Acheter")) {
				if (this.configuration.pieces >= 1000) {
					this.configuration.incPieces(-1000);
					this.configuration.debloquerVaisseauVert();
				} else {
					StartCoroutine(ShowMessage("Vous n'avez pas assez de pièces", 3));
				}
			}
		}

		GUI.skin.label.fontSize = 50;
		GUI.skin.button.fontSize = 45;
	}

	IEnumerator ShowMessage (string text, float delay) {
		this.message.text = text;
		this.message.enabled = true;
		yield return new WaitForSeconds(delay);
		this.message.enabled = false;
	}
}

