using UnityEngine;
using System.Collections;

public class BackgroundScroller : MonoBehaviour
{
	public float scrollSpeed;
	public float tileSizeZ;
	public float incrementSpeed;
	private Vector3 startPosition;
	
	void Start ()
	{
		//InvokeRepeating("IncrementerVitesse", 0, 2.0f); // calls Updatescore every second
		startPosition = transform.position;
	}
	
	void Update ()
	{
		float newPosition = Mathf.Repeat(Time.time * scrollSpeed, tileSizeZ);
		transform.position = startPosition - Vector3.up * newPosition;
	}

	public void IncrementerVitesse()
	{
		scrollSpeed += incrementSpeed;
	}
}