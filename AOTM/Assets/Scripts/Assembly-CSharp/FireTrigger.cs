using UnityEngine;

public class FireTrigger : MonoBehaviour
{
	public void OnTriggerEnter(Collider other)
	{
		if ((other.tag == "Player") & (other.tag != "SwingingDoor") & (other.tag != "Door"))
		{
			GetComponent<SphereCollider>().enabled = false;
			GetComponent<FireScript>().fireParticle.SetActive(value: true);
		}
	}
}
