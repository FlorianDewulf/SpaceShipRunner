using UnityEngine;
using System.Collections;

[System.Serializable]
public class Boundary
{
	public float xMin, xMax, zMin, zMax;
}


public class PlayerController : MonoBehaviour
{
	public float speed;
	public Boundary boundary;
	public joueur actif;
	
	
	void FixedUpdate ()
	{
			float moveHorizontal = Input.GetAxis ("Horizontal");
			
			Vector2 movement = new Vector2 (moveHorizontal, 0f);
			rigidbody2D.velocity = movement * speed;
			
			rigidbody2D.position = new Vector2 
				(
					Mathf.Clamp (rigidbody2D.position.x, boundary.xMin, boundary.xMax), 
					Mathf.Clamp (rigidbody2D.position.y, boundary.zMin, boundary.zMax)
					);


	}
}