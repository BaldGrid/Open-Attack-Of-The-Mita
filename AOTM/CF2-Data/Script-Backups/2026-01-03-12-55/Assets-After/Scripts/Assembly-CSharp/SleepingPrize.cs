using UnityEngine;

public class SleepingPrize : MonoBehaviour
{
	public Transform player;

	public float openingDistance;

	public GameObject sleep;

	public GameObject on;

	public AudioSource motor;

	public AudioClip prizeendvoice;

	public AudioClip ummmm;

	public AudioSource voice;

	public GameObject nulls;

	public bool played;

	public DoorScript door;

	private void Start()
	{
	}

	private void Update()
	{
		if (!voice.isPlaying & played)
		{
			played = false;
			voice.PlayOneShot(ummmm);
			nulls.SetActive(value: true);
		}
		if ((((ControlFreak2.CF2Input.GetMouseButtonDown(0) || ControlFreak2.CF2Input.GetKeyDown(KeyCode.E)) && Time.timeScale != 0f) & (base.name == "SleepingPrize")) && Physics.Raycast(Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0f)), out var _) && Vector3.Distance(player.position, base.transform.position) < openingDistance)
		{
			base.gameObject.name = "PrizeOn";
			sleep.SetActive(value: false);
			on.SetActive(value: true);
			motor.enabled = true;
			voice.PlayOneShot(prizeendvoice);
			played = true;
			door.LockDoor(999f);
		}
	}
}
