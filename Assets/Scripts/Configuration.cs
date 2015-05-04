using UnityEngine;
using System.Collections;

public class Configuration : MonoBehaviour {

	public int nombreDeVies {
		get;
		private set;
	}

	public string mort1;
	public string mort2;
	public int nombreTotalMort;

	public int score;
	public int meilleurScore;
	public float sonAmbiance;
	public float sonBruitage;

	// Boutique
	public int vaisseauBleu;
	public int vaisseauVert;
	public int choixVaisseau;

	// Succes
	public int succesEntreeEnBourse;
	public int succesFinDeMoisDifficile;
	public int succesMidas;
	public int succesCresus;
	public int succesLoupDeWallStreet;

	public int pieces {
		get;
		private set;
	}

	public void incNombreTotalMort(int n) {
		this.nombreTotalMort += n;
		PlayerPrefs.SetInt ("nombreTotalMort", this.nombreTotalMort);
		PlayerPrefs.Save ();
	}

	public void asgNombreDeVies(int n) {
		this.nombreDeVies = n;
		PlayerPrefs.SetInt ("nombreDeVies", this.nombreDeVies);
		PlayerPrefs.Save ();
	}

	public void asgMeilleurScore(int n) {
		this.meilleurScore = n;
		PlayerPrefs.SetInt ("meilleurScore", this.meilleurScore);
		PlayerPrefs.Save ();
	}

	public void incPieces(int n) {
		this.pieces += n;
		PlayerPrefs.SetInt ("pieces", this.pieces);
		PlayerPrefs.Save ();
	}

	/// =============== VAISSEAUX

	public void debloquerVaisseauBleu() {
		this.vaisseauBleu = 1;
		PlayerPrefs.SetInt ("vaisseauBleu", this.vaisseauBleu);
		PlayerPrefs.Save ();
	}

	public void debloquerVaisseauVert() {
		this.vaisseauVert = 1;
		PlayerPrefs.SetInt ("vaisseauVert", this.vaisseauVert);
		PlayerPrefs.Save ();
	}

	/// =============== SUCCES
	public void debloquerEntreeEnBourse() {
		this.succesEntreeEnBourse = 1;
		PlayerPrefs.SetInt ("succesEntreeEnBourse", this.succesEntreeEnBourse);
		PlayerPrefs.Save ();
	}

	public void debloquerFinDeMoisDifficile() {
		this.succesFinDeMoisDifficile = 1;
		PlayerPrefs.SetInt ("succesFinDeMoisDifficile", this.succesFinDeMoisDifficile);
		PlayerPrefs.Save ();
	}

	public void debloquerMidas() {
		this.succesMidas = 1;
		PlayerPrefs.SetInt ("succesMidas", this.succesMidas);
		PlayerPrefs.Save ();
	}

	public void debloquerCresus() {
		this.succesCresus = 1;
		PlayerPrefs.SetInt ("succesCresus", this.succesCresus);
		PlayerPrefs.Save ();
	}

	public void debloquerLoupDeWallStreet() {
		this.succesLoupDeWallStreet = 1;
		PlayerPrefs.SetInt ("succesLoupDeWallStreet", this.succesLoupDeWallStreet);
		PlayerPrefs.Save ();
	}

	// Cette méthode permet de sauvegarder les variables de son uniquement
	// quand l'utilisateur à fini de déplacer la barre de volume
	// sinon, ces valeurs sont quasiment sauvegardées en boucle
	public void saveSon() {
		PlayerPrefs.SetFloat ("sonAmbiance", this.sonAmbiance);
		PlayerPrefs.SetFloat ("sonBruitage", this.sonBruitage);
	}


	// Use this for initialization
	void Start () {

		// RESET TOUT
//		PlayerPrefs.SetInt ("meilleurScore", 0);
//		PlayerPrefs.SetInt ("pieces", 0);
//		PlayerPrefs.SetInt ("nombreDeVies", 0);
//		PlayerPrefs.SetInt ("nombreTotalMort", 0);
//		PlayerPrefs.SetInt ("vaisseauBleu", 0);
//		PlayerPrefs.SetInt ("vaisseauVert", 0);
//		PlayerPrefs.SetInt ("succesEntreeEnBourse", 0);
//		PlayerPrefs.SetInt ("succesFinDeMoisDifficile", 0);
//		PlayerPrefs.SetInt ("succesMidas", 0);
//		PlayerPrefs.SetInt ("succesCresus", 0);
//		PlayerPrefs.SetInt ("succesLoupDeWallStreet", 0);
//		PlayerPrefs.Save ();
		// FIN RESET

		// On charge les paramètres de jeu
		this.sonBruitage = PlayerPrefs.GetFloat("sonBruitage", 1.0f);
		this.sonAmbiance = PlayerPrefs.GetFloat("sonAmbiance", 0.8f);
		this.meilleurScore = PlayerPrefs.GetInt ("meilleurScore", 0);
		this.pieces = PlayerPrefs.GetInt ("pieces", 0);
		this.nombreDeVies = PlayerPrefs.GetInt ("nombreDeVies", 0);
		this.nombreTotalMort = PlayerPrefs.GetInt ("nombreTotalMort", 0);

		// On charge les succès débloqués
		this.succesEntreeEnBourse = PlayerPrefs.GetInt ("succesEntreeEnBourse", 0);
		this.succesFinDeMoisDifficile = PlayerPrefs.GetInt ("succesFinDeMoisDifficile", 0);
		this.succesMidas = PlayerPrefs.GetInt ("succesMidas", 0);
		this.succesCresus = PlayerPrefs.GetInt ("succesCresus", 0);
		this.succesLoupDeWallStreet = PlayerPrefs.GetInt ("succesLoupDeWallStreet", 0);

		// On charge les vaisseaux achetés
		this.vaisseauBleu = PlayerPrefs.GetInt ("vaisseauBleu", 0);
		this.vaisseauVert = PlayerPrefs.GetInt ("vaisseauVert", 0);
	}
	
	void Awake() {
		// On fais en sorte que ce script soit passé entre les scènes mais qu'il ne soit pas dupliqué lors du retour au menu 
		DontDestroyOnLoad(this);
		if (FindObjectsOfType(GetType()).Length > 1) {
			Destroy(gameObject);
		}
	}
	
	// Update is called once per frame
	void Update () {
	}
}
