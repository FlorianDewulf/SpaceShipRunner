using UnityEngine;
using System.Collections;

public class joueur : MonoBehaviour 
{

	public Jukebox jukebox;

	bool estInvincible;
	bool estMagnetique;
	public bool estEnAcceleration;

	bool doitActiverBouclier;
	bool doitActiverDebut;
	bool doitActiverAimant;
	bool doitActiverVitesse;

	bool coroutineBouclier;
	bool coroutineAimant;
	bool coroutineVitesse;

	bool aUtiliseUneVie;
	public bool estEnVie;

	string typeMort;

	Configuration configuration;
	PauseMenu pauseMenu;

	public GameObject labelChoixJoueur;

	//public GameObject b
	public Animator animator;

	public float dureeBouclier;
	public float dureeAcceleration;
	public float dureeAimant;
	public float dureeDebut;

	public GUIText affichageScore;
	public GUIText affichageVie;
	public GUIText affichagePieces;
	
	public int scoreBonus;
	public int scorePieces;
	public int scoreDuree;

	public float rayonAimant;

	int vie;
	int score;
	int piece;

	public GameObject bouclier;
	public GameObject aimant;
	public GameObject accelerateur;

	void scoreSurLaDuree()
	{
		if (estEnVie && !pauseMenu.estEnPause())
			augmenterScore(scoreDuree);
	}

	void gererAimant(bool valeur)
	{
		Vector2 tmp = new Vector2 (transform.position.x, transform.position.y);
		Collider2D[] colliders;

		colliders = Physics2D.OverlapCircleAll (tmp, rayonAimant);
		foreach (Collider2D collider in colliders)
		{
			if (collider.gameObject.tag == "pieces")
			{
				collider.gameObject.GetComponent<Piece>().activerMouvementMagnetique(valeur);
			}

		}
	}
			
	void desactiveObjets()
	{
		GameObject[] ennemis, bonus, pieces;
		ennemis = GameObject.FindGameObjectsWithTag("ennemi");
		foreach (GameObject ennemi in ennemis) {
			ennemi.GetComponent<Obstacle>().Desactiver();
		}
		pieces = GameObject.FindGameObjectsWithTag("pieces");
		foreach (GameObject piece in pieces) {
			piece.GetComponent<Piece>().Desactiver();
		}
		bonus = GameObject.FindGameObjectsWithTag("bonus");
		foreach (GameObject bonu in bonus) {
			bonu.GetComponent<Bonus>().Desactiver();
		}
	}

	// Use this for initialization
	void Start () {

		Time.timeScale = 1.0f;

		jukebox = GameObject.Find ("Jukebox").GetComponent<Jukebox> ();
		pauseMenu = Camera.main.GetComponent<PauseMenu> ();
		InvokeRepeating("scoreSurLaDuree", 0, 0.1f); // calls Updatescore every second
		animator = GetComponent<Animator> ();
		estEnVie = true;
		configuration = GameObject.Find ("Configuration").GetComponent<Configuration> () ; //voir pour initialiser avec un fichier ou whatever

		animator.SetInteger ("Vaisseau", configuration.choixVaisseau);
		
		if (configuration.choixVaisseau == 2) {
			GameObject.Find("VaisseauRouge").GetComponent<BoxCollider2D>().size = new Vector2(1.05f, 1.05f);
		}
		else if (configuration.choixVaisseau == 1){
			GameObject.Find("VaisseauRouge").GetComponent<BoxCollider2D>().size = new Vector2(0.95f, 0.86f);
		}
		
		vie = 0;
		score = 0;
		piece = 0;

		augmenterVie (configuration.nombreDeVies);
		augmenterScore (0);
		augmenterPieces (0);
		//vie = 1;
		aUtiliseUneVie = false;

		estInvincible = false;
		estMagnetique = false;
		estEnAcceleration = false;
	
		coroutineAimant = false;
		coroutineBouclier = false;
		coroutineVitesse = false;

		doitActiverBouclier = false;
		doitActiverAimant = false;
		doitActiverVitesse = false;
		doitActiverDebut = true;

	}

	// Update is called once per frame
	void FixedUpdate () 
	{
		if (estMagnetique)
			gererAimant (false);
		if (estEnAcceleration)
			activeModeRapideObjets (true);
		gestionCoroutineBonus();
	}
	
	void augmenterScore(int valeur)
	{
		score += valeur;
		affichageScore.text = "Score : " + score.ToString ();
	}

	void augmenterVie(int valeur)
	{
		vie += valeur;
		affichageVie.text = "Vie : " + vie.ToString ();
	}

	void augmenterPieces(int valeur)
	{
		piece += valeur;
		affichagePieces.text = "Pieces : " + piece.ToString ();
	}

	void activeModeRapideObjets(bool valeur)
	{
		GameObject[] ennemis, bonus, pieces;
		ennemis = GameObject.FindGameObjectsWithTag("ennemi");
		foreach (GameObject ennemi in ennemis) {
			ennemi.GetComponent<Obstacle>().activerModeRapide(valeur);
		}
		pieces = GameObject.FindGameObjectsWithTag("pieces");
		foreach (GameObject piece in pieces) {
			piece.GetComponent<Piece>().activerModeRapide(valeur);
		}
		bonus = GameObject.FindGameObjectsWithTag("bonus");
		foreach (GameObject bonu in bonus) {
			bonu.GetComponent<Bonus>().activerModeRapide(valeur);
		}
	}

	void gestionCoroutineBonus()
	{
		if (doitActiverBouclier)
		{
			doitActiverBouclier = false;
			if (coroutineBouclier)
			{
				coroutineBouclier = false;
				bouclier.SetActive(false);
				estInvincible = false;
				StopCoroutine("gestionDebut");
				StopCoroutine("gestionInvincibilite");
			}
			if (coroutineBouclier == false)
			{
				StartCoroutine("gestionInvincibilite");
			}
		}
		if (doitActiverDebut)
		{
			doitActiverDebut = false;
			if (coroutineBouclier)
			{
				coroutineBouclier = false;
				bouclier.SetActive(false);
				estInvincible = false;
				StopCoroutine("gestionDebut");
			}
			if (coroutineBouclier == false)
			{
				StartCoroutine("gestionDebut");
			}
		}
		if (doitActiverAimant)
		{
			doitActiverAimant = false;
			if (coroutineAimant)
			{
				coroutineAimant = false;
				aimant.SetActive(false);
				StopCoroutine("gestionAimant");
			}
			if (coroutineAimant == false)
			{
				StartCoroutine("gestionAimant");
			}
		}
		if (doitActiverVitesse)
		{
			doitActiverVitesse = false;
			if (coroutineVitesse)
			{
				coroutineVitesse = false;
				accelerateur.SetActive(false);
				StopCoroutine("gestionSuperVitesse");
			}
			if (coroutineVitesse == false)
			{
				StartCoroutine("gestionSuperVitesse");
			}
		}
	}


	IEnumerator gestionDebut()
	{
		bouclier.SetActive (true);
		estInvincible = true;
		coroutineBouclier = true;
		yield return new WaitForSeconds(dureeDebut);
		coroutineBouclier = false;
		estInvincible = false;
		bouclier.SetActive(false);
	}

	IEnumerator gestionInvincibilite()
	{
		bouclier.SetActive (true);
		estInvincible = true;
		coroutineBouclier = true;
		yield return new WaitForSeconds(dureeBouclier);
		coroutineBouclier = false;
		estInvincible = false;
		bouclier.SetActive(false);
	}

	IEnumerator gestionAimant()
	{
		aimant.SetActive (true);
		gererAimant (false);
		estMagnetique = true;
		coroutineAimant = true;
		yield return new WaitForSeconds(dureeAimant);
		aimant.SetActive (false);
		coroutineAimant = false;
		estMagnetique = false;
	}

	IEnumerator gestionSuperVitesse()
	{
		accelerateur.SetActive(true);
		estEnAcceleration = true;
		coroutineVitesse = true;
		yield return new WaitForSeconds(dureeAcceleration);
		accelerateur.SetActive(false);
		activeModeRapideObjets (false);
		coroutineVitesse = false;
		estEnAcceleration = false;
		Time.timeScale = 1f;
	}

	IEnumerator gererMort()
	{
		if (!estInvincible && !estEnAcceleration && estEnVie)
		{
			jukebox.fxExplosion();
			aimant.SetActive(false);
			StopCoroutine("gestionAimant");
			estMagnetique = false;
			Camera.main.GetComponent<PiscineObjet>().activerGeneration(false);
			desactiveObjets ();
			activeModeRapideObjets (false);
			estEnVie = false;
			GameObject.Find("VaisseauRouge").GetComponent<PlayerController>().enabled = false;
			animator.SetTrigger("Mort");
		//	configuration.incre
			if (vie <= 0 || aUtiliseUneVie)
			{
				if(aUtiliseUneVie)
				{
					configuration.mort2 = typeMort;
					configuration.incNombreTotalMort(2);
				}
				else
				{
					configuration.incNombreTotalMort(1);
					configuration.mort1 = typeMort;
				}
				configuration.asgNombreDeVies(vie);
				configuration.score = score;
				configuration.incPieces(piece);
				animator.SetTrigger("ChoixFin");
				yield return new WaitForSeconds (1f);
				Application.LoadLevel(2);
				yield return null;
			}
			else
			{
				yield return new WaitForSeconds (0.5f);
				labelChoixJoueur.SetActive (true);
				yield return StartCoroutine(AttenteChoixJoueur());
			}
		}
	}
	
	IEnumerator AttenteChoixJoueur()
	{
		while (!Input.GetKeyDown(KeyCode.Y) && !Input.GetKeyDown(KeyCode.N))
			yield return null;
		
		string touchePresse = (string)Input.inputString;
		configuration.mort1 = typeMort;
		if (touchePresse == "y")
		{
			doitActiverDebut = true;
			GameObject.Find("VaisseauRouge").GetComponent<PlayerController>().enabled = true;
			Camera.main.GetComponent<PiscineObjet>().activerGeneration(true);
			animator.SetTrigger("Vivant");
			aUtiliseUneVie = true;
			augmenterVie(-1);
			Time.timeScale = 1f;
			estEnVie = true;
			this.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
			this.transform.localPosition = new Vector3(0, 0, 0);
			GameObject.Find("VaisseauRouge").GetComponent<BoxCollider2D>().size = new Vector2(0.4f, 0.28f);
			bouclier.transform.localPosition = new Vector3(0, 0, 0);
			bouclier.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
			accelerateur.transform.localPosition = new Vector3(0, 0.17f, 0);
			accelerateur.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
			aimant.transform.localPosition = new Vector3(0, 0.15f, 0);
			aimant.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
			
		}
		else
		{
			configuration.incNombreTotalMort(1);
			configuration.asgNombreDeVies(vie);
			configuration.score = score;
			configuration.incPieces(piece);
			animator.SetTrigger("ChoixFin");
			yield return new WaitForSeconds (1f);
			Application.LoadLevel(2);
		}
		Time.timeScale = 1f;
		labelChoixJoueur.SetActive (false);
	}

	void OnCollisionEnter2D(Collision2D collision) 
	{
		if (collision.gameObject.tag == "pieces")
		{
			collision.gameObject.GetComponent<Piece>().Desactiver();
			jukebox.fxPiece();
			augmenterScore(scorePieces);
			augmenterPieces(1);
		}
		else if (collision.gameObject.tag == "ennemi") 
		{
			collision.gameObject.GetComponent<Obstacle>().Desactiver();
			typeMort = collision.gameObject.name;
			StartCoroutine(gererMort());
		}
		else if (collision.gameObject.tag == "bordure")
		{
			typeMort = "laser";
			StartCoroutine(gererMort());
		}
		else if (collision.gameObject.tag == "bonus") 
		{
			if (collision.gameObject.name.Contains("aimant"))
			{
				jukebox.fxAimant();
				doitActiverAimant = true;
			}
			else if (collision.gameObject.name.Contains("bouclier"))
			{
				jukebox.fxBouclier ();
				doitActiverBouclier = true;
			}
			else if (collision.gameObject.name.Contains("acceleration"))
			{
				jukebox.fxVitesse();
				doitActiverVitesse = true;
			}
			else if (collision.gameObject.name.Contains("vie"))
			{
				jukebox.fxVie();
				augmenterVie(1);
			}
			collision.gameObject.GetComponent<Bonus>().Desactiver();
			augmenterScore(scoreBonus);
		}
	}
			
}
