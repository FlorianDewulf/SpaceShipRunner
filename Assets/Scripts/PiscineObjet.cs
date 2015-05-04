using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PiscineObjet : MonoBehaviour {

	private float incrementAcceleration = 0.01F;
	private float incrementAccelerationVaisseau = 0.001F;
	private float incrementAccelerationMoment = 0.01F;
	public float vitesse = 0.6f;
	private PauseMenu pauseMenu;
	private joueur joueur;
	private PlayerController vaisseau;

	private float coordonneeYHauteur = 1;
	private float echelleX = 0.20f;
	private float echelleY = 0.15f;

	private bool doitGenerer;
	private int nombreInvoke;
	//Piscine des objets

	Dictionary< string, Dictionary<string, GameObject[]> > piscine = new Dictionary<string, Dictionary<string, GameObject[]> >();

	//Dictionnaire des différents catégorie d'objet

	Dictionary<string, GameObject[]> piece = new Dictionary<string, GameObject[]>();
	Dictionary<string, GameObject[]> bonus = new Dictionary<string, GameObject[]>();
	Dictionary<string, GameObject[]> obstacle = new Dictionary<string, GameObject[]>();

	//Tableau des objets de type piece

	GameObject[] pieces = null;

	//Tableau des objets de type  bonus
	GameObject[] accelerations = null;
	GameObject[] aimants = null;
	GameObject[] boucliers = null;
	GameObject[] vies = null;

	//Tableau des objets de type  obstacle

	GameObject[] meteores = null;
	GameObject[] satelites = null;
	GameObject[] trounoirs = null;
	
	//Nombre maximum d'objet par type

	public int nombreDePieces = 50;
	public int nombreDeAccelerations = 5;
	public int nombreDeAimants = 5;
	public int nombreDeBoucliers = 5;
	public int nombreDeVies = 5;
	public int nombreDeMeteores = 30;
	public int nombreDeSatelites = 30;
	public int nombreDeTrounoirs = 30;


	//Objet restant

	public int PiecesRestante;
	public int AccelerationsRestante;
	public int AimantsRestante;
	public int BoucliersRestante;
	public int ViesRestante;
	public int MeteoresRestante;
	public int SatelitesRestante;
	public int TrounoirsRestante;


	private string[] bonusType = new string[] {"acceleration", "aimant", "bouclier", "vie"};
	private string[] obstacleType = new string[] {"meteore", "satelite", "trounoir"};

	public float momentSeconde = 0.6F;
	public int moment = 0;
	public float prochainMoment = 0.0F;
	public int vague = 0;
	public string obstacleEnCour;

	public bool konami_active;

	// Use this for initialization
	void Start () {
		pauseMenu = Camera.main.GetComponent<PauseMenu> ();
		joueur = GameObject.Find ("VaisseauRouge").GetComponent<joueur> ();
		vaisseau = GameObject.Find ("VaisseauRouge").GetComponent<PlayerController> ();
		doitGenerer = true;
		PiecesRestante = nombreDePieces;
		AccelerationsRestante = nombreDeAccelerations;
		AimantsRestante = nombreDeAimants;
		BoucliersRestante = nombreDeBoucliers;
		ViesRestante = nombreDeVies;
		MeteoresRestante = nombreDeMeteores;
		SatelitesRestante = nombreDeSatelites;
		TrounoirsRestante = nombreDeTrounoirs;

		piscine.Add ("bonus", bonus);
		piscine.Add ("obstacle", obstacle);
		piscine.Add ("piece", piece);

		accelerations = new GameObject[nombreDeAccelerations];
		piscine ["bonus"].Add ("accelerations", accelerations);
		InstanticiationAcceleration ();
		aimants = new GameObject[nombreDeAimants];
		piscine ["bonus"].Add ("aimants", aimants);
		InstanticiationAimant ();
		boucliers = new GameObject[nombreDeBoucliers];
		piscine ["bonus"].Add ("boucliers", boucliers);
		InstanticiationBouclier ();
		vies = new GameObject[nombreDeVies];
		piscine ["bonus"].Add ("vies", vies);
		InstanticiationVie ();
		meteores = new GameObject[nombreDeMeteores];
		piscine ["obstacle"].Add ("meteores", meteores);
		InstanticiationMeteore ();
		satelites = new GameObject[nombreDeSatelites];
		piscine ["obstacle"].Add ("satelites", satelites);
		InstanticiationSatelite ();
		trounoirs = new GameObject[nombreDeTrounoirs];
		piscine ["obstacle"].Add ("trounoirs", trounoirs);
		InstanticiationTrouNoir ();
		pieces = new GameObject[nombreDePieces];
		piscine ["piece"].Add ("pieces", pieces);
		InstanticiationPiece ();
		InvokeRepeating("accelerationSurLaDuree", 15, 2.0f); // calls Updatescore every second
		InvokeRepeating("accelerationSurLaDureeVaisseau", 15, 2.0f); // calls Updatescore every second
		InvokeRepeating("accelerationSurLaDureeMoment", 15, 2.0f); // calls Updatescore every second
		nombreInvoke = 0;
		konami_active = false;
	}

	public void konamiSwap(bool valeur)
	{
		konami_active = valeur;
		for (int i = 0; i < nombreDeTrounoirs; i++) {
			Animator tmp = piscine["obstacle"]["trounoirs"][i].GetComponent<Animator>();
			tmp.SetBool("Konami", valeur);
		}
	}

	public void activerGeneration(bool valeur)
	{
		doitGenerer = valeur;
		if (valeur == false)
		{
			vague = 0;
			moment = 0;
		}
	}

	public void accelerationSurLaDuree()
	{
		nombreInvoke++;
		if (joueur.estEnVie && !pauseMenu.estEnPause())
			IncrementerVitesse();
	}

	public void accelerationSurLaDureeVaisseau()
	{
		if (joueur.estEnVie && !pauseMenu.estEnPause())
			IncrementerVitesseVaisseau();
	}

	public void accelerationSurLaDureeMoment()
	{
		if (joueur.estEnVie && !pauseMenu.estEnPause())
			IncrementerVitesseMoment();
	}

	public void IncrementerVitesse()
	{
		vitesse += incrementAcceleration;
	}

	public void IncrementerVitesseVaisseau()
	{
		vaisseau.speed += incrementAccelerationVaisseau;
	}

	public void IncrementerVitesseMoment()
	{
		momentSeconde -= incrementAccelerationMoment;
	}

	// Update is called once per frame
	void Update () {
		if (vague == 0)
		{
			vague = Mathf.RoundToInt(Random.Range(1, 12));
			obstacleEnCour = obstacleType[Mathf.RoundToInt(Random.Range(0, obstacleType.Length))];
		}
		if (doitGenerer) {
			if (Time.time > prochainMoment) {
				prochainMoment = Time.time + momentSeconde;
				moment += 1;
				switch (vague) {
				case 1:
					VagueUne();
					break;
				case 2:
					VagueDeux();
					break;
				case 3:
					VagueTrois();
					break;
				case 4:
					VagueQuatre();
					break;
				case 5:
					VagueCinq();
					break;
				case 6:
					VagueSix();
					break;
				case 7:
					VagueSept();
					break;
				case 8:
					VagueHuit();
					break;
				case 9:
					VagueNeuf();
					break;
				case 10:
					VagueDix();
					break;
				case 11:
					VagueOnze();
					break;
				default:
					break;
				}
				if (moment == 6) {
					if (Mathf.RoundToInt(Time.time * 100.0F) % 5 == 0)
					{
						ActiverBonus(bonusType[Mathf.RoundToInt(Random.Range(0, bonusType.Length - 1))]);
					}
					else if (Mathf.RoundToInt(Time.time * 100.0F) % 13 == 0)
					{
						ActiverBonus("vie");
					}
					vague = 0;
					moment = 0;
				}
			}
		}
		if (nombreInvoke == 25)
			CancelInvoke();
	}


	//Instanciation Bonus

	private void InstanticiationAcceleration()
	{
		for (int i = 0; i < nombreDeAccelerations; i++) {
			piscine["bonus"]["accelerations"][i] = Instantiate(Resources.Load("Prefab_Boost")) as GameObject;
			piscine["bonus"]["accelerations"][i].SetActive(false);
		}
	}

	private void InstanticiationAimant()
	{
		for (int i = 0; i < nombreDeAimants; i++) {
			piscine["bonus"]["aimants"][i] = Instantiate(Resources.Load("Prefab_Aimant")) as GameObject;
			piscine["bonus"]["aimants"][i].SetActive(false);
		}
	}

	private void InstanticiationBouclier()
	{
		for (int i = 0; i < nombreDeBoucliers; i++) {
			piscine["bonus"]["boucliers"][i] = Instantiate(Resources.Load("Prefab_Bouclier")) as GameObject;
			piscine["bonus"]["boucliers"][i].SetActive(false);
		}
	}

	private void InstanticiationVie()
	{
		for (int i = 0; i < nombreDeVies; i++) {
			piscine["bonus"]["vies"][i] = Instantiate(Resources.Load("Prefab_Vie")) as GameObject;
			piscine["bonus"]["vies"][i].SetActive(false);
		}
	}

	//Instanciation Obstacle

	private void InstanticiationMeteore()
	{
		for (int i = 0; i < nombreDeMeteores; i++) {
			piscine["obstacle"]["meteores"][i] = Instantiate(Resources.Load("Prefab_Asteroide")) as GameObject;
			piscine["obstacle"]["meteores"][i].SetActive(false);
		}
	}

	private void InstanticiationSatelite()
	{
		for (int i = 0; i < nombreDeSatelites; i++) {
			piscine["obstacle"]["satelites"][i] = Instantiate(Resources.Load("Prefab_Satellite")) as GameObject;
			piscine["obstacle"]["satelites"][i].SetActive(false);
		}
	}

	private void InstanticiationTrouNoir()
	{
		for (int i = 0; i < nombreDeTrounoirs; i++) {
			piscine["obstacle"]["trounoirs"][i] = Instantiate(Resources.Load("Prefab_TrouNoir")) as GameObject;
			piscine["obstacle"]["trounoirs"][i].SetActive(false);
		}
	}

	//Instanciation Piece
	
	private void InstanticiationPiece()
	{
		for (int i = 0; i < nombreDePieces; i++) {
			piscine["piece"]["pieces"][i] = Instantiate(Resources.Load("Prefab_Piece")) as GameObject;
			piscine["piece"]["pieces"][i].SetActive(false);
		}
	}

	// Activation d'objet de categorie Bonus

	private GameObject ActiverAcceleration()
	{
		for (int i = 0; i < nombreDeAccelerations; i++) {
			if (piscine["bonus"]["accelerations"][i].active == false)
			{
				piscine["bonus"]["accelerations"][i].SetActive (true);
				piscine["bonus"]["accelerations"][i].GetComponent<Bonus>().Activer("acceleration", vitesse);
				return piscine["bonus"]["accelerations"][i];
			}
		}
		return null;
	}

	private GameObject ActiverAimant()
	{
		for (int i = 0; i < nombreDeAimants; i++) {
			if (piscine["bonus"]["aimants"][i].active == false)
			{
				piscine["bonus"]["aimants"][i].SetActive (true);
				piscine["bonus"]["aimants"][i].GetComponent<Bonus>().Activer("aimant", vitesse);
				AimantsRestante--;
				return piscine["bonus"]["aimants"][i];
			}
		}
		return null;
	}

	private GameObject ActiverBouclier()
	{
		for (int i = 0; i < nombreDeBoucliers; i++) {
			if (piscine["bonus"]["boucliers"][i].active == false)
			{
				piscine["bonus"]["boucliers"][i].SetActive (true);
				piscine["bonus"]["boucliers"][i].GetComponent<Bonus>().Activer("bouclier", vitesse);
				BoucliersRestante--;
				return piscine["bonus"]["boucliers"][i];
			}
		}
		return null;
	}

	private GameObject ActiverVie()
	{
		for (int i = 0; i < nombreDeVies; i++) {
			if (piscine["bonus"]["vies"][i].active == false)
			{
				piscine["bonus"]["vies"][i].SetActive (true);
				piscine["bonus"]["vies"][i].GetComponent<Bonus>().Activer("vie", vitesse);
				ViesRestante--;
				return piscine["bonus"]["vies"][i];
			}
		}
		return null;
	}

	// Activation d'objet de categorie Obstacle

	private GameObject ActiverMeteore()
	{
		for (int i = 0; i < nombreDeMeteores; i++) {
			if (piscine["obstacle"]["meteores"][i].active == false)
			{
				piscine["obstacle"]["meteores"][i].SetActive (true);
				piscine["obstacle"]["meteores"][i].GetComponent<Obstacle>().Activer("meteore", vitesse);
				MeteoresRestante--;
				return piscine["obstacle"]["meteores"][i];
			}
		}
		return null;
	}

	private GameObject ActiverSatelite()
	{
		for (int i = 0; i < nombreDeSatelites; i++) {
			if (piscine["obstacle"]["satelites"][i].active == false)
			{
				piscine["obstacle"]["satelites"][i].SetActive (true);
				piscine["obstacle"]["satelites"][i].GetComponent<Obstacle>().Activer("satelite", vitesse);
				SatelitesRestante--;
				return piscine["obstacle"]["satelites"][i];
			}
		}
		return null;
	}

	private GameObject ActiverTrouNoir()
	{
		for (int i = 0; i < nombreDeTrounoirs; i++) {
			if (piscine["obstacle"]["trounoirs"][i].active == false)
			{
				piscine["obstacle"]["trounoirs"][i].SetActive (true);
				piscine["obstacle"]["trounoirs"][i].GetComponent<Obstacle>().Activer("trounoir", vitesse);
				TrounoirsRestante--;
				Animator tmp = piscine["obstacle"]["trounoirs"][i].GetComponent<Animator>();
				tmp.SetBool("Konami", konami_active);
				return piscine["obstacle"]["trounoirs"][i];
			}
		}
		return null;
	}

	//Activation d'objet de categorie Piece

	private GameObject ActiverPiece()
	{
		for (int i = 0; i < nombreDePieces; i++) {
			if (piscine["piece"]["pieces"][i].active == false)
			{
				piscine["piece"]["pieces"][i].SetActive (true);
				piscine["piece"]["pieces"][i].GetComponent<Piece>().Activer("piece", vitesse);
				PiecesRestante--;
				return piscine["piece"]["pieces"][i];
			}
		}
		return null;
	}

	//Pattern

	private void GaucheCarrePiece()
	{
		GameObject haut = ActiverPiece ();
		if (haut != null && (PiecesRestante - 3) >= 0)
		{
			haut.transform.position += (coordonneeYHauteur + 1) * Vector3.up * echelleY - 3 * Vector3.right * echelleX;
			for (int i = 0; i < 3; i++)
			{
				if (i == 0)
					ActiverPiece ().transform.position += coordonneeYHauteur * Vector3.up * echelleY - 3 * Vector3.right * echelleX;
				else if (i == 1)
					ActiverPiece ().transform.position += (coordonneeYHauteur + 1) * Vector3.up * echelleY - 2 * Vector3.right * echelleX;
				else
					ActiverPiece ().transform.position += coordonneeYHauteur * Vector3.up * echelleY - 2 * Vector3.right * echelleX;
			}
		}
	}

	private void MilieuCarrePiece()
	{
		GameObject haut = ActiverPiece ();
		if (haut != null && (PiecesRestante - 3) >= 0)
		{
			haut.transform.position += (coordonneeYHauteur + 1) * Vector3.up * echelleY -  Vector3.right * echelleX;
			for (int i = 0; i < 3; i++)
			{
				if (i == 0)
					ActiverPiece ().transform.position += coordonneeYHauteur * Vector3.up * echelleY;
				else if (i == 1)
					ActiverPiece ().transform.position += (coordonneeYHauteur + 1) * Vector3.up * echelleY;
				else
					ActiverPiece ().transform.position += coordonneeYHauteur * Vector3.up * echelleY -  Vector3.right * echelleX;
			}
		}
	}

	private void DroiteCarrePiece()
	{
		GameObject haut = ActiverPiece ();
		if (haut != null  && (PiecesRestante - 3) >= 0)
		{
			haut.transform.position += (coordonneeYHauteur + 1) * Vector3.up * echelleY + 2 *  Vector3.right * echelleX;
			for (int i = 0; i < 3; i++)
			{
				if (i == 0)
					ActiverPiece ().transform.position += coordonneeYHauteur * Vector3.up * echelleY + 2 *  Vector3.right * echelleX;
				else if (i == 1)
					ActiverPiece ().transform.position += (coordonneeYHauteur + 1) * Vector3.up * echelleY +  Vector3.right * echelleX;
				else
					ActiverPiece ().transform.position += coordonneeYHauteur * Vector3.up * echelleY +  Vector3.right * echelleX;
			}
		}
	}

	private void LignePiece()
	{
		GameObject haut = ActiverPiece ();
		if (haut != null  && (PiecesRestante - 5) >= 0)
		{
			haut.transform.position += coordonneeYHauteur * Vector3.up * echelleY - 3 *  Vector3.right * echelleX;
			for (int i = 0; i < 5; i++)
			{
				if (i == 0)
					ActiverPiece ().transform.position += coordonneeYHauteur * Vector3.up * echelleY - 2 *  Vector3.right * echelleX;
				else if (i == 1)
					ActiverPiece ().transform.position += coordonneeYHauteur * Vector3.up * echelleY -  Vector3.right * echelleX;
				else if (i == 2)
					ActiverPiece ().transform.position += coordonneeYHauteur * Vector3.up * echelleY;
				else if (i == 3)
					ActiverPiece ().transform.position += coordonneeYHauteur * Vector3.up * echelleY +  Vector3.right * echelleX;
				else
					ActiverPiece ().transform.position += coordonneeYHauteur * Vector3.up * echelleY + 2 *  Vector3.right * echelleX;;
			}
		}
	}

	private void ColonnePiece(int x)
	{
		GameObject haut = ActiverPiece ();
		if (haut != null  && (PiecesRestante - 5) >= 0)
		{
			haut.transform.position += (coordonneeYHauteur + 5) * Vector3.up * echelleY + x *  Vector3.right * echelleX;
			for (int i = 0; i < 5; i++)
			{
				if (i == 0)
					ActiverPiece ().transform.position += (coordonneeYHauteur + 4) * Vector3.up * echelleY + x *  Vector3.right * echelleX;
				else if (i == 1)
					ActiverPiece ().transform.position += (coordonneeYHauteur + 3) * Vector3.up * echelleY + x *  Vector3.right * echelleX;
				else if (i == 2)
					ActiverPiece ().transform.position += (coordonneeYHauteur + 2) * Vector3.up * echelleY + x *  Vector3.right * echelleX;
				else if (i == 3)
					ActiverPiece ().transform.position += (coordonneeYHauteur + 1) * Vector3.up * echelleY + x *  Vector3.right * echelleX;
				else
					ActiverPiece ().transform.position += coordonneeYHauteur * Vector3.up * echelleY + x *  Vector3.right * echelleX;
			}
		}
	}

	private void DiagonalHautBasPiece ()
	{
		GameObject haut = ActiverPiece ();
		if (haut != null && (PiecesRestante - 5) >= 0)
		{
			haut.transform.position += (coordonneeYHauteur + 5) * Vector3.up * echelleY - 3 *  Vector3.right * echelleX;
			for (int i = 0; i < 5; i++)
			{
				if (i == 0)
					ActiverPiece ().transform.position += (coordonneeYHauteur + 4) * Vector3.up * echelleY - 2 *  Vector3.right * echelleX;
				else if (i == 1)
					ActiverPiece ().transform.position += (coordonneeYHauteur + 3) * Vector3.up * echelleY -  Vector3.right * echelleX;
				else if (i == 2)
					ActiverPiece ().transform.position += (coordonneeYHauteur + 2) * Vector3.up * echelleY;
				else if (i == 3)
					ActiverPiece ().transform.position += (coordonneeYHauteur + 1) * Vector3.up * echelleY + 1 *  Vector3.right * echelleX;
				else
					ActiverPiece ().transform.position += coordonneeYHauteur * Vector3.up * echelleY + 2 *  Vector3.right * echelleX;
			}
		}
	}

	private void DiagonalBasHautPiece ()
	{
		GameObject haut = ActiverPiece ();
		if (haut != null && (PiecesRestante - 5) >= 0)
		{
			haut.transform.position += (coordonneeYHauteur + 5) * Vector3.up * echelleY + 2 *  Vector3.right * echelleX;
			for (int i = 0; i < 5; i++)
			{
				if (i == 0)
					ActiverPiece ().transform.position += (coordonneeYHauteur + 4) * Vector3.up * echelleY +  Vector3.right * echelleX;
				else if (i == 1)
					ActiverPiece ().transform.position += (coordonneeYHauteur + 3) * Vector3.up * echelleY;
				else if (i == 2)
					ActiverPiece ().transform.position += (coordonneeYHauteur + 2) * Vector3.up * echelleY - 1 *  Vector3.right * echelleX;
				else if (i == 3)
					ActiverPiece ().transform.position += (coordonneeYHauteur + 1) * Vector3.up * echelleY - 2 *  Vector3.right * echelleX;
				else
					ActiverPiece ().transform.position += coordonneeYHauteur * Vector3.up * echelleY - 3 *  Vector3.right * echelleX;
			}
		}
	}

	private void GaucheRectanglePiece()
	{
		GaucheCarrePiece ();
		MilieuCarrePiece ();
	}

	private void DroiteRectanglePiece()
	{
		MilieuCarrePiece ();
		DroiteCarrePiece ();
	}

	private void GaucheLigneObstacle(string type)
	{
		//GameObject haut;
		switch (type) {
			case "meteore":
			GameObject haut = ActiverMeteore();
			if (haut != null  && (MeteoresRestante - 4) >= 0)
				{
					haut.transform.position += coordonneeYHauteur * Vector3.up * echelleY - 3 *  Vector3.right * echelleX;
					for (int i = 0; i < 4; i++)
					{
						if (i == 0)
							ActiverMeteore ().transform.position += coordonneeYHauteur * Vector3.up * echelleY - 2 *  Vector3.right * echelleX;
						else if (i == 1)
							ActiverMeteore ().transform.position += coordonneeYHauteur * Vector3.up * echelleY -  Vector3.right * echelleX;
						else if (i == 2)
							ActiverMeteore ().transform.position += coordonneeYHauteur * Vector3.up * echelleY;
						else
							ActiverMeteore ().transform.position += coordonneeYHauteur * Vector3.up * echelleY +  Vector3.right * echelleX;
					}
				}
				break;
			case "satelite":
				haut = ActiverSatelite();
			if (haut != null && (SatelitesRestante - 4) >= 0)
				{
					haut.transform.position += coordonneeYHauteur * Vector3.up * echelleY - 3 *  Vector3.right * echelleX;
					for (int i = 0; i < 4; i++)
					{
						if (i == 0)
							ActiverSatelite ().transform.position += coordonneeYHauteur * Vector3.up * echelleY - 2 *  Vector3.right * echelleX;
						else if (i == 1)
							ActiverSatelite ().transform.position += coordonneeYHauteur * Vector3.up * echelleY -  Vector3.right * echelleX;
						else if (i == 2)
							ActiverSatelite ().transform.position += coordonneeYHauteur * Vector3.up * echelleY;
						else
							ActiverSatelite ().transform.position += coordonneeYHauteur * Vector3.up * echelleY +  Vector3.right * echelleX;
					}
				}
				break;
			case "trounoir":
				haut = ActiverTrouNoir();
			if (haut != null && (TrounoirsRestante - 4) >= 0)
				{
					haut.transform.position += coordonneeYHauteur * Vector3.up * echelleY - 3 *  Vector3.right * echelleX;
					for (int i = 0; i < 4; i++)
					{
						if (i == 0)
							ActiverTrouNoir ().transform.position += coordonneeYHauteur * Vector3.up * echelleY - 2 *  Vector3.right * echelleX;
						else if (i == 1)
							ActiverTrouNoir ().transform.position += coordonneeYHauteur * Vector3.up * echelleY -  Vector3.right * echelleX;
						else if (i == 2)
							ActiverTrouNoir ().transform.position += coordonneeYHauteur * Vector3.up * echelleY;
						else
							ActiverTrouNoir ().transform.position += coordonneeYHauteur * Vector3.up * echelleY +  Vector3.right * echelleX;
					}
				}
				break;
			default:
				break;
		}
	}
	
	private void DroiteLigneObstacle(string type)
	{
		GameObject haut;
		switch (type) {
			case "meteore":
				haut = ActiverMeteore();
			if (haut != null && (MeteoresRestante - 4) >= 0)
				{
				haut.transform.position += coordonneeYHauteur * Vector3.up * echelleY - 2 *  Vector3.right * echelleX;
					for (int i = 0; i < 4; i++)
					{
						if (i == 0)
							ActiverMeteore ().transform.position += coordonneeYHauteur * Vector3.up * echelleY - 1 *  Vector3.right * echelleX;
						else if (i == 1)
							ActiverMeteore ().transform.position += coordonneeYHauteur * Vector3.up * echelleY;
						else if (i == 2)
							ActiverMeteore ().transform.position += coordonneeYHauteur * Vector3.up * echelleY +  Vector3.right * echelleX;
						else
							ActiverMeteore ().transform.position += coordonneeYHauteur * Vector3.up * echelleY + 2 *  Vector3.right * echelleX;
					}
				}
				break;
			case "satelite":
				haut = ActiverSatelite();
			if (haut != null && (SatelitesRestante - 4) >= 0)
				{
				haut.transform.position += coordonneeYHauteur * Vector3.up * echelleY - 2 *  Vector3.right * echelleX;
					for (int i = 0; i < 4; i++)
					{
						if (i == 0)
							ActiverSatelite ().transform.position += coordonneeYHauteur * Vector3.up * echelleY -  Vector3.right * echelleX;
						else if (i == 1)
							ActiverSatelite ().transform.position += coordonneeYHauteur * Vector3.up * echelleY;
						else if (i == 2)
							ActiverSatelite ().transform.position += coordonneeYHauteur * Vector3.up * echelleY +  Vector3.right * echelleX;
						else
							ActiverSatelite ().transform.position += coordonneeYHauteur * Vector3.up * echelleY + 2 *  Vector3.right * echelleX;
					}
				}
				break;
			case "trounoir":
				haut = ActiverTrouNoir();
			if (haut != null && (TrounoirsRestante - 4) >= 0)
				{
					haut.transform.position += coordonneeYHauteur * Vector3.up * echelleY - 2 *  Vector3.right * echelleX;
					for (int i = 0; i < 4; i++)
					{
						if (i == 0)
							ActiverTrouNoir ().transform.position += coordonneeYHauteur * Vector3.up * echelleY -  Vector3.right * echelleX;
						else if (i == 1)
							ActiverTrouNoir ().transform.position += coordonneeYHauteur * Vector3.up * echelleY;
						else if (i == 2)
							ActiverTrouNoir ().transform.position += coordonneeYHauteur * Vector3.up * echelleY +  Vector3.right * echelleX;
						else
							ActiverTrouNoir ().transform.position += coordonneeYHauteur * Vector3.up * echelleY + 2 *  Vector3.right * echelleX;
					}
				}
				break;
			default:
				break;
		}
	}

	private void DiagonalHautBasObstacle (string type)
	{
		GameObject haut;
		switch (type) {
		case "meteore":
			haut = ActiverMeteore();
			if (haut != null && (MeteoresRestante - 4) >= 0)
			{
				haut.transform.position += (coordonneeYHauteur + 4) * Vector3.up * echelleY - 2 *  Vector3.right * echelleX;
				for (int i = 0; i < 4; i++)
				{
					if (i == 0)
						ActiverMeteore ().transform.position += (coordonneeYHauteur + 3) * Vector3.up * echelleY - 1 *  Vector3.right * echelleX;
					else if (i == 1)
						ActiverMeteore ().transform.position += (coordonneeYHauteur + 2) * Vector3.up * echelleY;
					else if (i == 2)
						ActiverMeteore ().transform.position += (coordonneeYHauteur + 1) * Vector3.up * echelleY + 1 *  Vector3.right * echelleX;
					else
						ActiverMeteore ().transform.position += coordonneeYHauteur * Vector3.up * echelleY + 2 *  Vector3.right * echelleX;
				}
			}
			break;
		case "satelite":
			haut = ActiverSatelite();
			if (haut != null && (SatelitesRestante - 4) >= 0)
			{
				haut.transform.position += (coordonneeYHauteur + 4) * Vector3.up * echelleY - 2 *  Vector3.right * echelleX;
				for (int i = 0; i < 4; i++)
				{
					if (i == 0)
						ActiverSatelite ().transform.position += (coordonneeYHauteur + 3) * Vector3.up * echelleY - 1 *  Vector3.right * echelleX;
					else if (i == 1)
						ActiverSatelite ().transform.position += (coordonneeYHauteur + 2) * Vector3.up * echelleY;
					else if (i == 2)
						ActiverSatelite ().transform.position += (coordonneeYHauteur + 1) * Vector3.up * echelleY + 1 *  Vector3.right * echelleX;
					else
						ActiverSatelite ().transform.position += coordonneeYHauteur * Vector3.up * echelleY + 2 *  Vector3.right * echelleX;
				}
			}
			break;
		case "trounoir":
			haut = ActiverTrouNoir();
			if (haut != null && (TrounoirsRestante - 4) >= 0)
			{
				haut.transform.position += (coordonneeYHauteur + 4) * Vector3.up * echelleY - 2 *  Vector3.right * echelleX;
				for (int i = 0; i < 4; i++)
				{
					if (i == 0)
						ActiverTrouNoir ().transform.position += (coordonneeYHauteur + 3) * Vector3.up * echelleY - 1 *  Vector3.right * echelleX;
					else if (i == 1)
						ActiverTrouNoir ().transform.position += (coordonneeYHauteur + 2) * Vector3.up * echelleY;
					else if (i == 2)
						ActiverTrouNoir ().transform.position += (coordonneeYHauteur + 1) * Vector3.up * echelleY + 1 *  Vector3.right * echelleX;
					else
						ActiverTrouNoir ().transform.position += coordonneeYHauteur * Vector3.up * echelleY + 2 *  Vector3.right * echelleX;
				}
			}
			break;
		default:
			break;
		}
	}

	private void DiagonalBasHautObstacle (string type)
	{
		GameObject haut;
		switch (type) {
		case "meteore":
			haut = ActiverMeteore();
			if (haut != null && (MeteoresRestante - 4) >= 0)
			{
				haut.transform.position += (coordonneeYHauteur + 4) * Vector3.up * echelleY + 1 *  Vector3.right * echelleX;
				for (int i = 0; i < 4; i++)
				{
					if (i == 0)
						ActiverMeteore ().transform.position += (coordonneeYHauteur + 3) * Vector3.up * echelleY;
					else if (i == 1)
						ActiverMeteore ().transform.position += (coordonneeYHauteur + 2) * Vector3.up * echelleY - 1 *  Vector3.right * echelleX;
					else if (i == 2)
						ActiverMeteore ().transform.position += (coordonneeYHauteur + 1) * Vector3.up * echelleY - 2 *  Vector3.right * echelleX;
					else
						ActiverMeteore ().transform.position += coordonneeYHauteur * Vector3.up * echelleY - 3 *  Vector3.right * echelleX;
				}
			}
			break;
		case "satelite":
			haut = ActiverSatelite();
			if (haut != null && (SatelitesRestante - 4) >= 0)
			{
				haut.transform.position += (coordonneeYHauteur + 4) * Vector3.up * echelleY + 1 *  Vector3.right * echelleX;
				for (int i = 0; i < 4; i++)
				{
					if (i == 0)
						ActiverSatelite ().transform.position += (coordonneeYHauteur + 3) * Vector3.up * echelleY;
					else if (i == 1)
						ActiverSatelite ().transform.position += (coordonneeYHauteur + 2) * Vector3.up * echelleY - 1 *  Vector3.right * echelleX;
					else if (i == 2)
						ActiverSatelite ().transform.position += (coordonneeYHauteur + 1) * Vector3.up * echelleY - 2 *  Vector3.right * echelleX;
					else
						ActiverSatelite ().transform.position += coordonneeYHauteur * Vector3.up * echelleY - 3 *  Vector3.right * echelleX;
				}
			}
			break;
		case "trounoir":
			haut = ActiverTrouNoir();
			if (haut != null && (TrounoirsRestante - 4) >= 0)
			{
				haut.transform.position += (coordonneeYHauteur + 4) * Vector3.up * echelleY + 1 *  Vector3.right * echelleX;
				for (int i = 0; i < 4; i++)
				{
					if (i == 0)
						ActiverTrouNoir ().transform.position += (coordonneeYHauteur + 3) * Vector3.up * echelleY;
					else if (i == 1)
						ActiverTrouNoir ().transform.position += (coordonneeYHauteur + 2) * Vector3.up * echelleY - 1 *  Vector3.right * echelleX;
					else if (i == 2)
						ActiverTrouNoir ().transform.position += (coordonneeYHauteur + 1) * Vector3.up * echelleY - 2 *  Vector3.right * echelleX;
					else
						ActiverTrouNoir ().transform.position += coordonneeYHauteur * Vector3.up * echelleY - 3 *  Vector3.right * echelleX;
				}
			}
			break;
		default:
			break;
		}
	}

	private void ColonneObstacle(int x, string type)
	{
		GameObject haut;
		switch (type) {
		case "meteore":
			haut = ActiverMeteore();
			if (haut != null  && (MeteoresRestante - 4) >= 0)
			{
				haut.transform.position += (coordonneeYHauteur + 4) * Vector3.up * echelleY + x *  Vector3.right * echelleX;
				for (int i = 0; i < 4; i++)
				{
					if (i == 0)
						ActiverMeteore ().transform.position += (coordonneeYHauteur + 3) * Vector3.up * echelleY + x *  Vector3.right * echelleX;
					else if (i == 1)
						ActiverMeteore ().transform.position += (coordonneeYHauteur + 2) * Vector3.up * echelleY + x *  Vector3.right * echelleX;
					else if (i == 2)
						ActiverMeteore ().transform.position += (coordonneeYHauteur + 1) * Vector3.up * echelleY + x *  Vector3.right * echelleX;
					else
						ActiverMeteore ().transform.position += coordonneeYHauteur * Vector3.up * echelleY + x *  Vector3.right * echelleX;
				}
			}
			break;
		case "satelite":
			haut = ActiverSatelite();
			if (haut != null  && (SatelitesRestante - 4) >= 0)
			{
				haut.transform.position += (coordonneeYHauteur + 4) * Vector3.up * echelleY + x *  Vector3.right * echelleX;
				for (int i = 0; i < 4; i++)
				{
					if (i == 0)
						ActiverSatelite ().transform.position += (coordonneeYHauteur + 3) * Vector3.up * echelleY + x *  Vector3.right * echelleX;
					else if (i == 1)
						ActiverSatelite ().transform.position += (coordonneeYHauteur + 2) * Vector3.up * echelleY + x *  Vector3.right * echelleX;
					else if (i == 2)
						ActiverSatelite ().transform.position += (coordonneeYHauteur + 1) * Vector3.up * echelleY + x *  Vector3.right * echelleX;
					else
						ActiverSatelite ().transform.position += coordonneeYHauteur * Vector3.up * echelleY + x *  Vector3.right * echelleX;
				}
			}
			break;
		case "trounoir":
			haut = ActiverTrouNoir();
			if (haut != null  && (TrounoirsRestante - 4) >= 0)
			{
				haut.transform.position += (coordonneeYHauteur + 4) * Vector3.up * echelleY + x *  Vector3.right * echelleX;
				for (int i = 0; i < 4; i++)
				{
					if (i == 0)
						ActiverTrouNoir ().transform.position += (coordonneeYHauteur + 3) * Vector3.up * echelleY + x *  Vector3.right * echelleX;
					else if (i == 1)
						ActiverTrouNoir ().transform.position += (coordonneeYHauteur + 2) * Vector3.up * echelleY + x *  Vector3.right * echelleX;
					else if (i == 2)
						ActiverTrouNoir ().transform.position += (coordonneeYHauteur + 1) * Vector3.up * echelleY + x *  Vector3.right * echelleX;
					else
						ActiverTrouNoir ().transform.position += coordonneeYHauteur * Vector3.up * echelleY + x *  Vector3.right * echelleX;
				}
			}
			break;
		default:
			break;
		}

	}

	private void FormeXObstacle(string type)
	{
		GameObject haut;
		switch (type) {
		case "meteore":
			haut = ActiverMeteore();
			if (haut != null  && (MeteoresRestante - 9) >= 0)
			{
				haut.transform.position += (coordonneeYHauteur + 4) * Vector3.up * echelleY - 3 *  Vector3.right * echelleX;
				for (int i = 0; i < 9; i++)
				{
					if (i == 0)
						ActiverMeteore ().transform.position += (coordonneeYHauteur + 4) * Vector3.up * echelleY + 2 *  Vector3.right * echelleX;
					else if (i == 1)
						ActiverMeteore ().transform.position += (coordonneeYHauteur + 3) * Vector3.up * echelleY - 2 *  Vector3.right * echelleX;
					else if (i == 2)
						ActiverMeteore ().transform.position += (coordonneeYHauteur + 3) * Vector3.up * echelleY + 1 *  Vector3.right * echelleX;
					else if (i == 3)
						ActiverMeteore ().transform.position += (coordonneeYHauteur + 2) * Vector3.up * echelleY - 2 *  Vector3.right * echelleX;
					else if (i == 4)
						ActiverMeteore ().transform.position += (coordonneeYHauteur + 2) * Vector3.up * echelleY + 1 *  Vector3.right * echelleX;
					else if (i == 5)
						ActiverMeteore ().transform.position += (coordonneeYHauteur + 1) * Vector3.up * echelleY - 2 *  Vector3.right * echelleX;
					else if (i == 6)
						ActiverMeteore ().transform.position += (coordonneeYHauteur + 1) * Vector3.up * echelleY + 1 *  Vector3.right * echelleX;
					else if (i == 7)
						ActiverMeteore ().transform.position += coordonneeYHauteur * Vector3.up * echelleY - 3 *  Vector3.right * echelleX;
					else
						ActiverMeteore ().transform.position += coordonneeYHauteur * Vector3.up * echelleY + 2 *  Vector3.right * echelleX;
				}
			}
			break;
		case "satelite":
			haut = ActiverSatelite();
			if (haut != null  && (SatelitesRestante - 9) >= 0)
			{
				haut.transform.position += (coordonneeYHauteur + 4) * Vector3.up * echelleY - 3 *  Vector3.right * echelleX;
				for (int i = 0; i < 9; i++)
				{
					if (i == 0)
						ActiverSatelite ().transform.position += (coordonneeYHauteur + 4) * Vector3.up * echelleY + 2 *  Vector3.right * echelleX;
					else if (i == 1)
						ActiverSatelite ().transform.position += (coordonneeYHauteur + 3) * Vector3.up * echelleY - 2 *  Vector3.right * echelleX;
					else if (i == 2)
						ActiverSatelite ().transform.position += (coordonneeYHauteur + 3) * Vector3.up * echelleY + 1 *  Vector3.right * echelleX;
					else if (i == 3)
						ActiverSatelite ().transform.position += (coordonneeYHauteur + 2) * Vector3.up * echelleY - 2 *  Vector3.right * echelleX;
					else if (i == 4)
						ActiverSatelite ().transform.position += (coordonneeYHauteur + 2) * Vector3.up * echelleY + 1 *  Vector3.right * echelleX;
					else if (i == 5)
						ActiverSatelite ().transform.position += (coordonneeYHauteur + 1) * Vector3.up * echelleY - 2 *  Vector3.right * echelleX;
					else if (i == 6)
						ActiverSatelite ().transform.position += (coordonneeYHauteur + 1) * Vector3.up * echelleY + 1 *  Vector3.right * echelleX;
					else if (i == 7)
						ActiverSatelite ().transform.position += coordonneeYHauteur * Vector3.up * echelleY - 3 *  Vector3.right * echelleX;
					else
						ActiverSatelite ().transform.position += coordonneeYHauteur * Vector3.up * echelleY + 2 *  Vector3.right * echelleX;
				}
			}
			break;
		case "trounoir":
			haut = ActiverTrouNoir();
			if (haut != null  && (TrounoirsRestante - 9) >= 0)
			{
				haut.transform.position += (coordonneeYHauteur + 4) * Vector3.up * echelleY - 3 *  Vector3.right * echelleX;
				for (int i = 0; i < 9; i++)
				{
					if (i == 0)
						ActiverTrouNoir ().transform.position += (coordonneeYHauteur + 4) * Vector3.up * echelleY + 2 *  Vector3.right * echelleX;
					else if (i == 1)
						ActiverTrouNoir ().transform.position += (coordonneeYHauteur + 3) * Vector3.up * echelleY - 2 *  Vector3.right * echelleX;
					else if (i == 2)
						ActiverTrouNoir ().transform.position += (coordonneeYHauteur + 3) * Vector3.up * echelleY + 1 *  Vector3.right * echelleX;
					else if (i == 3)
						ActiverTrouNoir ().transform.position += (coordonneeYHauteur + 2) * Vector3.up * echelleY - 2 *  Vector3.right * echelleX;
					else if (i == 4)
						ActiverTrouNoir ().transform.position += (coordonneeYHauteur + 2) * Vector3.up * echelleY + 1 *  Vector3.right * echelleX;
					else if (i == 5)
						ActiverTrouNoir ().transform.position += (coordonneeYHauteur + 1) * Vector3.up * echelleY - 2 *  Vector3.right * echelleX;
					else if (i == 6)
						ActiverTrouNoir ().transform.position += (coordonneeYHauteur + 1) * Vector3.up * echelleY + 1 *  Vector3.right * echelleX;
					else if (i == 7)
						ActiverTrouNoir ().transform.position += coordonneeYHauteur * Vector3.up * echelleY - 3 *  Vector3.right * echelleX;
					else
						ActiverTrouNoir ().transform.position += coordonneeYHauteur * Vector3.up * echelleY + 2 *  Vector3.right * echelleX;
				}
			}
			break;
		default:
			break;
		}
	}

	private void ActiverBonus(string type)
	{
		int positionX = Mathf.RoundToInt(Random.Range(-3, 3));
		switch (type) {
		case "acceleration":
			if (AccelerationsRestante > 0)
				ActiverAcceleration().transform.position += positionX * Vector3.right * echelleX + coordonneeYHauteur * Vector3.up * echelleY;
			break;
		case "aimant":
			if (AimantsRestante > 0)
				ActiverAimant().transform.position += positionX * Vector3.right * echelleX + coordonneeYHauteur * Vector3.up * echelleY;
			break;
		case "bouclier":
			if (BoucliersRestante > 0)
				ActiverBouclier().transform.position += positionX * Vector3.right * echelleX + coordonneeYHauteur * Vector3.up * echelleY;
			break;
		case "vie":
			if (ViesRestante > 0)
				ActiverVie().transform.position += positionX * Vector3.right * echelleX + coordonneeYHauteur * Vector3.up * echelleY;
			break;
		default:
			break;
		}
	}

	public void ajoutObjet(string type)
	{
		switch (type) {
		case "aimant":
			AimantsRestante++ ;
			break;
		case "acceleration":
			AccelerationsRestante++;
			break;
		case "bouclier":
			BoucliersRestante++;
			break;
		case "vie":
			ViesRestante++;
			break;
		case "meteore":
			MeteoresRestante++;
			break;
		case "satelite":
			SatelitesRestante++;
			break;
		case "trounoir":
			TrounoirsRestante++;
			break;
		case "piece":
			PiecesRestante++;
			break;
		default:
			break;
		}
	}

	//Vague
	
	private void	VagueUne()
	{
		if (moment == 1 || moment == 5)
		{
			GaucheCarrePiece ();
			DroiteCarrePiece ();
		}
		else if (moment == 3) 
		{
			MilieuCarrePiece ();
		}
	}

	private void VagueDeux()
	{
		if (moment == 1)
		{
			ColonnePiece (-3);
		}
		else if (moment == 2)
		{
			DiagonalHautBasObstacle(obstacleEnCour);
		}
	}

	private void VagueTrois()
	{
		if (moment == 1)
		{
			ColonnePiece (2);
		}
		else if (moment == 2)
		{
			DiagonalBasHautObstacle(obstacleEnCour);
		}
	}

	private void VagueQuatre()
	{
		if (moment == 1)
		{
			DroiteLigneObstacle(obstacleEnCour);
		}
		else if (moment == 3)
		{
			LignePiece();
		}
		else if (moment == 5)
		{
			GaucheLigneObstacle(obstacleEnCour);
		}
	}

	private void VagueCinq()
	{
		if (moment == 1)
		{
			GaucheLigneObstacle(obstacleEnCour);
		}
		else if (moment == 3)
		{
			LignePiece();
		}
		else if (moment == 5)
		{
			DroiteLigneObstacle(obstacleEnCour);
		}
	}

	private void VagueSix()
	{
		if (moment == 1)
		{
			ColonnePiece(-1);
			ColonnePiece(0);
		}
		else if (moment == 2)
		{
			FormeXObstacle(obstacleEnCour);
		}
	}

	private void VagueSept()
	{
		if (moment == 1)
		{
			ColonnePiece(-3);
		}
		else if (moment == 2)
		{
			ColonneObstacle(-2,obstacleEnCour);
			ColonneObstacle(0, obstacleEnCour);
			ColonneObstacle(2, obstacleEnCour);
		}
	}

	private void VagueHuit()
	{
		if (moment == 1)
		{
			ColonnePiece(2);
		}
		else if (moment == 2)
		{
			ColonneObstacle(-3, obstacleEnCour);
			ColonneObstacle(-1, obstacleEnCour);
			ColonneObstacle(1, obstacleEnCour);
		}
	}

	private void VagueNeuf()
	{
		if (moment == 1)
		{
			ColonnePiece(-2);
			ColonnePiece(1);
		}
		else if (moment == 2)
		{
			ColonneObstacle(-1, obstacleEnCour);
			ColonneObstacle(0, obstacleEnCour);
		}
	}

	private void VagueDix()
	{
		if (moment == 1)
		{
			ColonnePiece(-1);
			ColonnePiece(0);
		}
		else if (moment == 2)
		{
			ColonneObstacle(-2, obstacleEnCour);
			ColonneObstacle(1, obstacleEnCour);
		}
	}

	private void VagueOnze()
	{
		if (moment == 1)
		{
			ColonnePiece(-3);
			ColonnePiece(-2);
			ColonnePiece(-1);
			ColonnePiece(0);
			ColonnePiece(1);
			ColonnePiece(2);
		}
	}
}



