using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class CraftersScript : MonoBehaviour
{
	public bool db;

	public bool angry;

	public float TheNotebooks;

	public bool gettingAngry;

	public float anger;

	private float forceShowTime;

	public Transform player;

	public CharacterController cc;

	public Transform playerCamera;

	public Transform baldi;

	public NavMeshAgent baldiAgent;

	public GameObject sprite;

	public GameControllerScript gc;

	[SerializeField]
	private NavMeshAgent agent;

	public Renderer craftersRenderer;

	public SpriteRenderer spriteImage;

	public Sprite angrySprite;

	private AudioSource audioDevice;

	public AudioClip aud_Intro;

	public AudioClip aud_Loop;

	public PlayerScript bs;

	public Transform SpriteTransform;

	private void Start()
	{
		agent = GetComponent<NavMeshAgent>();
		audioDevice = GetComponent<AudioSource>();
		sprite.SetActive(value: false);
	}

	private void Update()
	{
		if (forceShowTime > 0f)
		{
			forceShowTime -= Time.deltaTime;
		}
		if (gettingAngry)
		{
			anger += Time.deltaTime;
			if ((anger >= 1f) & !angry)
			{
				angry = true;
				audioDevice.PlayOneShot(aud_Intro);
				spriteImage.sprite = angrySprite;
			}
		}
		else if (anger > 0f)
		{
			anger -= Time.deltaTime;
		}
		if (!angry)
		{
			if ((((base.transform.position - agent.destination).magnitude <= 20f) & ((base.transform.position - player.position).magnitude >= 60f)) || forceShowTime > 0f)
			{
				sprite.SetActive(value: true);
			}
			return;
		}
		if (agent.speed < 45f)
		{
			agent.speed += 10f * Time.deltaTime;
		}
		TargetPlayer();
		if (!audioDevice.isPlaying)
		{
			audioDevice.PlayOneShot(aud_Loop);
		}
	}

	private void FixedUpdate()
	{
		if ((float)gc.notebooks >= TheNotebooks)
		{
			Vector3 direction = player.position - base.transform.position;
			if (Physics.Raycast(base.transform.position + Vector3.up * 2f, direction, out var hitInfo, float.PositiveInfinity, 769, QueryTriggerInteraction.Ignore) & (hitInfo.transform.tag == "Player") & craftersRenderer.isVisible & sprite.activeSelf)
			{
				gettingAngry = true;
			}
			else
			{
				gettingAngry = false;
			}
		}
	}

	public void GiveLocation(Vector3 location, bool flee)
	{
		if (!angry && agent.isActiveAndEnabled)
		{
			agent.SetDestination(location);
			if (flee)
			{
				forceShowTime = 3f;
			}
		}
	}

	private void TargetPlayer()
	{
		agent.SetDestination(player.position);
	}

	private void OnTriggerEnter(Collider other)
	{
		if ((other.tag == "Player") & angry)
		{
			StartCoroutine(Attack());
		}
	}

	public IEnumerator Attack()
	{
		base.gameObject.GetComponent<CapsuleCollider>().enabled = false;
		agent.speed = 0f;
		float speed = 400f;
		float acceleration = 80f;
		float spinDistance = 8f;
		spriteImage.sprite = angrySprite;
		Vector3 currentAngle = player.forward;
		float time = 0f;
		base.transform.position = new Vector3(player.position.x, base.transform.position.y, player.position.z) + player.forward * 8f;
		while (time < 15f)
		{
			currentAngle = Quaternion.AngleAxis(speed * Time.deltaTime, Vector3.up) * currentAngle;
			base.transform.position = new Vector3(player.position.x, base.transform.position.y, player.position.z) + currentAngle * spinDistance;
			speed += acceleration * Time.deltaTime;
			time += Time.deltaTime;
			yield return null;
		}
		Teleportpostition();
	}

	private void Teleportpostition()
	{
		cc.enabled = true;
		gc.CraftersTeleport();
		gc.DespawnCrafters();
	}
}
