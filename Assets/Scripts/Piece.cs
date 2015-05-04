using UnityEngine;
using System.Collections;

public class Piece : MonoBehaviour {
	public float vitesse = 0.6f;
	private float echelleX = 0.10f;
	private float echelleY = 0.15f;

	private string type = null;
	private int coordonneeYHauteur = 7;

	private GameObject joueur;
	private bool comportementNormal;
	public bool mouvementRapide;
	// Update is called once per frame
	void Start()
	{
		comportementNormal = true;
		joueur = GameObject.Find ("VaisseauRouge");
	}

	void Update ()
	{
		CompteARebours ();
		MouvementAutomatique ();
	}

	public void Activer(string typeObjet, float nouvelleVitesse)
	{
		mouvementRapide = false;
		transform.position = Vector3.zero;
		transform.position += coordonneeYHauteur * Vector3.up * echelleY + echelleX * Vector3.right;
		type = typeObjet;
		name = "pieces";
		tag = "pieces";
		vitesse = nouvelleVitesse;
	}

	public void Desactiver()
	{
		comportementNormal = true;
		this.gameObject.SetActive (false);
		Camera.main.GetComponent<PiscineObjet>().ajoutObjet(type);
	}

	private void CompteARebours()
	{
		Vector3 viewPos = Camera.main.WorldToViewportPoint (transform.position);
		if (viewPos.y <= 0.0f)
		{
			Desactiver ();
		}
	}

	public void activerMouvementMagnetique(bool valeur)
	{
		comportementNormal = valeur;
	}

	public void activerModeRapide(bool valeur)
	{
		mouvementRapide = valeur;
	}

	private void MouvementAutomatique()
	{
		if (comportementNormal) 
		{						
			Vector3 velocite = -vitesse * Time.deltaTime * transform.up;
			if (mouvementRapide)
				velocite *= 4;
			transform.Translate (velocite);
		} 
		else 
		{
			transform.position = Vector3.Lerp (transform.position, joueur.transform.position, 5 * Time.fixedDeltaTime);
		}
	}

}
