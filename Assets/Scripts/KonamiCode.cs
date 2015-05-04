using UnityEngine;

// Credits : http://blog.remibodin.fr/unity3d-konami-code/

public class KonamiCode : MonoBehaviour
{
	public float timeKey = 0f, timeCode = 0f;
	public string message;
	Jukebox jukebox;
	PiscineObjet piscine;
	
	KeyCode[] keycodes;
	int index = 0;
	float timeSinceStartCode = 0f, timeSinceLastKey = 0f;

	void Start() {
		this.jukebox = GameObject.Find ("Jukebox").GetComponent<Jukebox> ();
		this.piscine = GameObject.Find ("Main Camera").GetComponent<PiscineObjet> ();
	}

	void Awake()
	{
		this.keycodes = new KeyCode[]
		{
			KeyCode.UpArrow,
			KeyCode.UpArrow,
			KeyCode.DownArrow,
			KeyCode.DownArrow,
			KeyCode.LeftArrow,
			KeyCode.RightArrow,
			KeyCode.LeftArrow,
			KeyCode.RightArrow,
			KeyCode.B,
			KeyCode.A
		};
	}
	
	void OnEnable()
	{
		this.enabled = true;
	}

	void OnDestroy() {
		//this.piscine.konamiSwap (false);
	}
	
	void Update()
	{
		this.timeSinceLastKey += Time.deltaTime;
		this.timeSinceStartCode += Time.deltaTime;
		if (Input.anyKeyDown == false) return;
		if (Input.GetKeyDown(this.keycodes[index]) == false || this.timeSinceStartCode >= this.timeCode || this.timeSinceLastKey >= this.timeKey)
		{
			this.index = 0;
		}
		if (Input.GetKeyDown(this.keycodes[index]))
		{
			if (this.index == 0)
			{
				this.timeSinceStartCode = 0f;
			}
			this.timeSinceLastKey = 0f;
			this.index++;
			if (this.index >= this.keycodes.Length)
			{
				this.index = 0;
				this.jukebox.activerSonCastleVania();
				this.piscine.konamiSwap(true);
			}
		}
	}
}