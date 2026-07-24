using UnityEngine;

public class ThrowableSpoopBalloon : MonoBehaviour
{
	public Material mat;

	private Rigidbody rb;

	public Transform player;

	public PlayerScript ps;

	public UnknownPri_NPC unknown;

	public GameControllerScript gameControllerScript;

	public bool thrown;

	public float speed = 50f;

	public void Start()
	{
		ps = Object.FindObjectOfType<PlayerScript>();
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
		if ((ControlFreak2.CF2Input.GetMouseButtonDown(0) || (ControlFreak2.CF2Input.GetKeyDown(KeyCode.E) && Time.timeScale != 0f)) && Physics.Raycast(Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0f)), out var hitInfo) && ((hitInfo.transform.tag == "SpoopBalloon") & (Vector3.Distance(player.position, base.transform.position) < 12f) & !thrown))
		{
			thrown = true;
			base.transform.rotation = player.transform.rotation;
			unknown.canGetHit = true;
			speed += 100f;
		}
		rb.velocity = base.transform.forward * speed;
	}

	public void SelfDestruct()
	{
		Object.Destroy(base.gameObject, 0f);
		unknown.canGetHit = false;
	}

	public void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.tag != "NPC" && collision.gameObject.tag == "Player")
		{
			Object.Destroy(base.gameObject, 0f);
			unknown.canGetHit = false;
			Debug.Log("Damage player.");
			unknown.glitchEffectsValue += 0.5f;
		}
		if (collision.gameObject.name == "Wall")
		{
			collision.collider.GetComponent<MeshRenderer>().material = mat;
		}
		ps.playerHealth--;
		Object.Destroy(base.gameObject, 0f);
		gameControllerScript.PlayRandomGlitchSound();
		unknown.canGetHit = false;
	}
}
