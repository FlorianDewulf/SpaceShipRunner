using UnityEngine;
using System.Collections;

public class MenuScores : MonoBehaviour {

	public GUISkin skinPerso;
	Configuration configuration;
	bool recordBattu = false;
	Jukebox jukebox;
	

	// Use this for initialization
	void Start () {
		this.jukebox = GameObject.Find ("Jukebox").GetComponent<Jukebox> () ;
		this.configuration = GameObject.Find ("Configuration").GetComponent<Configuration> ();

		if (this.configuration.score > this.configuration.meilleurScore) {
			this.configuration.asgMeilleurScore(this.configuration.score);
			this.recordBattu = true;
		}

		if (this.configuration.pieces > 5 && this.configuration.succesEntreeEnBourse == 0) {
			this.configuration.debloquerEntreeEnBourse();
		}

		if (this.configuration.pieces > 300  && this.configuration.succesFinDeMoisDifficile == 0) {
			this.configuration.debloquerFinDeMoisDifficile();
		}

		if (this.configuration.pieces > 5000 && this.configuration.succesMidas == 0) {
			this.configuration.debloquerMidas();
		}

		if (this.configuration.pieces > 10000 && this.configuration.succesCresus == 0) {
			this.configuration.debloquerCresus();
		}

		if (this.configuration.pieces > 500000 && this.configuration.succesLoupDeWallStreet == 0) {
			this.configuration.debloquerLoupDeWallStreet();
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI() {
		GUI.skin = this.skinPerso;

		GUI.skin.label.fontSize = 40;

		if (GUI.Button (new Rect (Screen.width / 2 - 150, Screen.height / 2 - 150 + 350, 300, 100), "RETOUR")) {
			this.jukebox.activerSonMenu();
			Application.LoadLevel (0);
		}

		GUI.Label(new Rect (0, Screen.height / 2 - 150, Screen.width, 100), "Votre Score : " + this.configuration.score);
		GUI.Label(new Rect (0, Screen.height / 2 - 50, Screen.width, 100), "Meilleur Score : " + this.configuration.meilleurScore);

		GUI.skin.label.fontSize = 50;

		if (this.recordBattu == true) {
			GUI.skin.label.normal.textColor = new Color(0.8f, 0.2f, 0.2f);
			GUI.Label(new Rect (0, Screen.height / 2 + 50, Screen.width, 100), "Record Battu !");
			GUI.skin.label.normal.textColor = Color.white;
		}
	}
}

