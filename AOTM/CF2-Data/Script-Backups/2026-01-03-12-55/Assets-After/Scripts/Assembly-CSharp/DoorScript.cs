using UnityEngine;

public class DoorScript : MonoBehaviour
{
	public float openingDistance;

	public Transform player;

	public BaldiScript baldi;

	public MeshCollider barrier;

	public MeshCollider trigger;

	public MeshCollider invisibleBarrier;

	public MeshRenderer inside;

	public MeshRenderer outside;

	public AudioClip doorOpen;

	public AudioClip doorClose;

	public AudioClip audDoorLockOpen;

	public AudioClip audDoorUnlock;

	public AudioClip audDoorLock;

	public Material closed;

	public Material open;

	public Material closedTwo;

	public Material openTwo;

	private bool bDoorOpen;

	private bool bDoorLocked;

	public int silentOpens;

	private float openTime;

	public float lockTime;

	private AudioSource myAudio;

	public bool DoorLocked => bDoorLocked;

	private void Start()
	{
		myAudio = GetComponent<AudioSource>();
	}

	private void Update()
	{
		if (lockTime > 0f)
		{
			lockTime -= 1f * Time.deltaTime;
		}
		else if (bDoorLocked)
		{
			UnlockDoor();
		}
		if (openTime > 0f)
		{
			openTime -= 1f * Time.deltaTime;
		}
		if ((openTime <= 0f) & bDoorOpen)
		{
			barrier.enabled = true;
			invisibleBarrier.enabled = true;
			bDoorOpen = false;
			inside.material = closed;
			outside.material = closedTwo;
			if (silentOpens <= 0)
			{
				myAudio.PlayOneShot(doorClose, 1f);
			}
		}
		if ((ControlFreak2.CF2Input.GetMouseButtonDown(0) || ControlFreak2.CF2Input.GetKeyDown(KeyCode.E)) && Time.timeScale != 0f && Physics.Raycast(Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0f)), out var hitInfo) && ((hitInfo.collider == trigger) & (Vector3.Distance(player.position, base.transform.position) < openingDistance) & !bDoorLocked))
		{
			if (!bDoorOpen && (baldi.isActiveAndEnabled & (silentOpens <= 0)))
			{
				baldi.Hear(base.transform.position, 1f);
			}
			OpenDoor();
			if (silentOpens > 0 && !bDoorOpen)
			{
				silentOpens--;
			}
		}
	}

	public void OpenDoor()
	{
		if (silentOpens <= 0 && !bDoorOpen)
		{
			myAudio.PlayOneShot(doorOpen, 1f);
		}
		barrier.enabled = false;
		invisibleBarrier.enabled = false;
		bDoorOpen = true;
		inside.material = open;
		outside.material = openTwo;
		openTime = 3f;
	}

	private void OnTriggerStay(Collider other)
	{
		if (!bDoorLocked & other.CompareTag("NPC"))
		{
			OpenDoor();
		}
	}

	public void LockDoor(float time)
	{
		bDoorLocked = true;
		myAudio.PlayOneShot(audDoorLock, 1f);
		lockTime = time;
	}

	public void UnlockDoor()
	{
		bDoorLocked = false;
		myAudio.PlayOneShot(audDoorUnlock, 1f);
	}

	public void OpenDoorLocked()
	{
		myAudio.PlayOneShot(audDoorLockOpen, 1f);
	}

	public void SilenceDoor()
	{
		silentOpens = 4;
	}
}
