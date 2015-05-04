using UnityEngine;
using System.Collections;

public class Jukebox : MonoBehaviour {

	// Les différentes audio sources
	Configuration configuration;
	AudioSource[] audioSources;
	AudioSource audioAmbiance;
	AudioSource audioBruitage;

	// Les musiques
	AudioClip musiqueMenu;
	AudioClip musiqueJeu;
	AudioClip musiqueCastleVania;

	// Les bruitages
	AudioClip bruitagePiece;
	AudioClip bruitageExplosion;
	AudioClip bruitageAimant;
	AudioClip bruitageVie;
	AudioClip bruitageBouclier;
	AudioClip bruitageVitesse;


	// Use this for initialization
	void Start () {
		// On récupère la configuration et on crée les sources audio
		this.configuration = GameObject.Find ("Configuration").GetComponent<Configuration> ();
		this.audioSources = GameObject.Find ("Jukebox").GetComponents<AudioSource>();

		this.audioAmbiance = this.audioSources[0];
		this.audioBruitage = this.audioSources[1];


		// On initialise le volume en fonction de la configuration
		this.audioAmbiance.volume = configuration.sonAmbiance;
		this.audioBruitage.volume = configuration.sonBruitage;

		this.chargerMusique ();

		this.audioAmbiance.clip = this.musiqueMenu;
		this.audioAmbiance.Play ();
	}

	void chargerMusique() {
		// Les musiques d'abord
		this.musiqueMenu = (AudioClip) Resources.Load("Music/musique_menu", typeof(AudioClip));
		this.musiqueJeu = (AudioClip) Resources.Load("Music/musique_jeu2", typeof(AudioClip));
		this.musiqueCastleVania = (AudioClip) Resources.Load("Music/castlevania", typeof(AudioClip));


		// Puis les bruitages
		this.bruitagePiece = (AudioClip) Resources.Load("Music/piece", typeof(AudioClip));
		this.bruitageExplosion = (AudioClip) Resources.Load("Music/explosion", typeof(AudioClip));
		this.bruitageAimant = (AudioClip) Resources.Load("Music/aimant", typeof(AudioClip));
		this.bruitageVie = (AudioClip) Resources.Load("Music/vie", typeof(AudioClip));
		this.bruitageBouclier = (AudioClip) Resources.Load("Music/bouclier", typeof(AudioClip));
		this.bruitageVitesse = (AudioClip) Resources.Load("Music/vitesse", typeof(AudioClip));
	}

	public void fxPiece() {
		this.audioBruitage.PlayOneShot (this.bruitagePiece);
	}

	public void fxExplosion() {
		this.audioBruitage.PlayOneShot (this.bruitageExplosion);
	}

	public void fxAimant() {
		this.audioBruitage.PlayOneShot (this.bruitageAimant);
	}

	public void fxVie() {
		this.audioBruitage.PlayOneShot (this.bruitageVie);
	}

	public void fxBouclier() {
		this.audioBruitage.PlayOneShot (this.bruitageBouclier);
	}

	public void fxVitesse() {
		this.audioBruitage.PlayOneShot (this.bruitageVitesse);
	}

	public void activerSonMenu() {
		if (this.audioAmbiance != null) {
			this.audioAmbiance.Stop ();
			this.audioAmbiance.clip = this.musiqueMenu;
			this.audioAmbiance.Play ();
		}
	}

	public void activerSonCastleVania() {
		if (this.audioAmbiance != null) {
			this.audioAmbiance.Stop ();
			this.audioAmbiance.clip = this.musiqueCastleVania;
			this.audioAmbiance.Play ();
		}
	}

	public void activerSonJeu() {
		if (this.audioAmbiance != null) {
			this.audioAmbiance.Stop ();
			this.audioAmbiance.clip = this.musiqueJeu;
			this.audioAmbiance.Play ();
		}
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
		// Le volume est mis à jour directement ici si la configuration change
		this.audioAmbiance.volume = configuration.sonAmbiance;
		this.audioBruitage.volume = configuration.sonBruitage;
	}
}
