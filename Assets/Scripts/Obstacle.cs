using UnityEngine;
using System.Collections;

public class Obstacle : MonoBehaviour {
	public float vitesse = 0.6f;
	private float echelleX = 0.10F;
	private float echelleY = 0.15f;
	private string type = null;

	private int coordonneeYHauteur = 7;
	public bool mouvementRapide;

	// Update is called once per frame
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
		name = type;
		tag = "ennemi";
		vitesse = nouvelleVitesse;
	}
	
	public void Desactiver()
	{
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

	public void activerModeRapide(bool valeur)
	{
		mouvementRapide = valeur;
	}

	private void MouvementAutomatique()
	{
		Vector3 velocite = -vitesse * Time.deltaTime * transform.up;
		if (mouvementRapide)
			velocite *= 6;
		transform.Translate (velocite);
	}

}
