using UnityEngine;

public class PRIUnknownPriBalloon : MonoBehaviour
{
	public Material mat;

	private Rigidbody rb;

	private SpriteRenderer spriteRenderer;

	public Sprite[] colorSprite;

	public UnknownPrinc_NPC1 unknown;

	public GameControllerScript gameControllerScript;

	public bool thrown;

	public float speed = 50f;

	public SpriteRenderer sprite;

	public PlayerScript ps;

	public void Start()
	{
		ps = Object.FindObjectOfType<PlayerScript>();
		RandomizeSprite();
		spriteRenderer = GetComponentInChildren<SpriteRenderer>();
		rb = GetComponent<Rigidbody>();
		if (unknown.Health == 9)
		{
			speed = 100f;
		}
		else if (unknown.Health == 8)
		{
			speed = 105f;
		}
		if (unknown.Health == 7)
		{
			speed = 110f;
		}
		else if (unknown.Health == 6)
		{
			speed = 115f;
		}
		if (unknown.Health == 5)
		{
			speed = 120f;
		}
		else if (unknown.Health == 4)
		{
			speed = 125f;
		}
		if (unknown.Health == 3)
		{
			speed = 130f;
		}
		else if (unknown.Health == 2)
		{
			speed = 140f;
		}
		if (unknown.Health == 1)
		{
			speed = 150f;
		}
	}

	private void Update()
	{
		rb.velocity = base.transform.forward * speed;
	}

	public void RandomizeSprite()
	{
		sprite.sprite = colorSprite[Random.Range(0, 4)];
	}

	public void SelfDestruct()
	{
		Object.Destroy(base.gameObject, 0f);
	}

	public void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.tag != "NPC" && collision.gameObject.tag == "Player")
		{
			Object.Destroy(base.gameObject, 0f);
			unknown.canGetHit = false;
			unknown.glitchEffectsValue += 0.35f;
			gameControllerScript.PlayRandomGlitchSound();
			Debug.Log("Damage player.");
		}
		if (collision.gameObject.name == "Wall")
		{
			collision.collider.GetComponent<MeshRenderer>().material = mat;
		}
		ps.playerHealth--;
		gameControllerScript.PlayRandomGlitchSound();
		Object.Destroy(base.gameObject, 0f);
	}
}
