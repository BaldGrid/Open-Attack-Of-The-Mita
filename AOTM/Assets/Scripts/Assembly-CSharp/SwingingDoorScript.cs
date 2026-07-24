using UnityEngine;

public class SwingingDoorScript : MonoBehaviour
{
	public bool heardDoor;

	public GameControllerScript gc;

	public BaldiScript baldi;

	public MeshCollider barrier;

	public GameObject obstacle;

	public MeshCollider trigger;

	public MeshRenderer inside;

	public MeshRenderer outside;

	public Material closed;

	public Material open;

	public Material locked;

	public AudioClip doorOpen;

	public AudioClip baldiDoor;

	[SerializeField]
	private float openTime;

	private float lockTime;

	public bool bDoorOpen;

	public bool bDoorLocked;

	private bool requirementMet;

	private AudioSource myAudio;

	public KidnapTimer timers;
	private SubtitleManager sm;

	private void Start()
	{
		myAudio = GetComponent<AudioSource>();
		sm = FindObjectOfType<SubtitleManager>();
		LockDoor(999f);
	}

	private void Update()
	{
		if (openTime > 0f)
		{
			openTime -= 1f * Time.deltaTime;
		}
		if (lockTime > 0f)
		{
			lockTime -= Time.deltaTime;
		}
		else if (bDoorLocked)
		{
			UnlockDoor();
		}
		if ((openTime <= 0f) & bDoorOpen & !bDoorLocked)
		{
			heardDoor = false;
			bDoorOpen = false;
			inside.material = closed;
			outside.material = closed;
		}
		if (timers.Timer <= 0f && !bDoorOpen)
		{
			bDoorOpen = true;
			UnlockDoor();
		}
	}

	private void OnTriggerStay(Collider other)
	{
		if ((other.tag == "Player" || (other.tag == "NPC" && other.isTrigger)) && !bDoorLocked)
		{
			if (other.tag == "Player")
			{
				heardDoor = true;
				bDoorOpen = true;
				inside.material = open;
				outside.material = open;
				openTime = 2f;
				//sm.Add3DSubtitle("*Door Opens*", doorOpen.length, Color,red, base.transform);
			}
			else if (other.isTrigger)
			{
				heardDoor = true;
				bDoorOpen = true;
				inside.material = open;
				outside.material = open;
				openTime = 2f;
				//sm.Add3DSubtitle("*SLAM!*", doorOpen.length, Color.red, base.transform);
			}
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player" && !heardDoor && !bDoorLocked)
		{
			myAudio.PlayOneShot(doorOpen, 1f);
			if (other.tag == "Player" && baldi.isActiveAndEnabled)
			{
				baldi.Hear(base.transform.position, 1f);
			}
		}
	}

	public void LockDoor(float time)
	{
		barrier.enabled = true;
		obstacle.SetActive(value: true);
		bDoorLocked = true;
		lockTime = time;
		inside.material = locked;
		outside.material = locked;
	}

	public void UnlockDoor()
	{
		barrier.enabled = false;
		obstacle.SetActive(value: false);
		bDoorLocked = false;
		inside.material = closed;
		outside.material = closed;
	}
}
