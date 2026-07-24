using UnityEngine;

public class FireScript : MonoBehaviour
{
	public bool bigFire;

	public Animator sp;

	public float life = 15f;

	public AILocationSelectorScript AILocationSelectorScript;

	public GameObject fireParticle;

	public Transform wanderer;

	public Transform player;

	private void Update()
	{
		if (fireParticle.activeSelf)
		{
			life -= Time.deltaTime;
		}
		if (bigFire)
		{
			if (life <= 0f)
			{
				sp.Play("Fadebull 0");
				base.gameObject.GetComponent<BoxCollider>().enabled = false;
				fireParticle.GetComponent<AudioSource>().Stop();
			}
			if (life <= -4f)
			{
				Object.Destroy(base.gameObject, 0f);
			}
		}
		else if (life <= -4f)
		{
			Object.Destroy(base.gameObject, 0f);
		}
	}

	private void Start()
	{
		TeleportObject();
	}

	public void TeleportObject()
	{
		AILocationSelectorScript.GetNewTargetHallway();
		base.gameObject.transform.position = wanderer.position + new Vector3(0f, 5f, 0f);
		while ((base.transform.position - player.position).magnitude < 20f)
		{
			AILocationSelectorScript.GetNewTargetHallway();
			base.gameObject.transform.position = wanderer.position + new Vector3(0f, 5f, 0f);
		}
	}

	public void OnTriggerStay(Collider other)
	{
		if (bigFire)
		{
			if ((other.transform.name == "Fire(Passage Block-off)(Clone)") & !bigFire & (other.tag != "SwingingDoor") & (other.tag != "Door"))
			{
				TeleportObject();
			}
		}
		else if ((other.transform.name == "Fire(Small)(Clone)") & !bigFire & (other.tag != "SwingingDoor") & (other.tag != "Door"))
		{
			TeleportObject();
		}
	}
}
