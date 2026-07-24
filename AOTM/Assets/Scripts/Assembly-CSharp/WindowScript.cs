using UnityEngine;
using UnityEngine.AI;

public class WindowScript : MonoBehaviour
{
	public bool enableOffMeshScript;

	public bool broken;

	public MeshRenderer window_In;

	public MeshRenderer window_Out;

	public MeshCollider meshCollider_In;

	public MeshCollider meshCollider_Out;

	public Material window_Broken;

	public void BreakWindow()
	{
		if (!broken)
		{
			base.gameObject.GetComponent<AudioSource>().Play();
			window_In.material = window_Broken;
			window_Out.material = window_Broken;
			meshCollider_In.enabled = false;
			meshCollider_Out.enabled = false;
			broken = true;
		}
	}

	public void OnTriggerEnter(Collider other)
	{
		if (((other.tag == "NPC") & (other.transform.name != "1st Prize")) && enableOffMeshScript)
		{
			BreakWindow();
		}
	}

	public void Update()
	{
		if (broken)
		{
			base.gameObject.GetComponent<NavMeshObstacle>().enabled = false;
		}
		else if (enableOffMeshScript)
		{
			base.gameObject.GetComponent<NavMeshObstacle>().enabled = false;
			base.gameObject.GetComponent<BoxCollider>().size = new Vector3(2f, 10f, 10f);
		}
		else
		{
			base.gameObject.GetComponent<NavMeshObstacle>().enabled = true;
			base.gameObject.GetComponent<BoxCollider>().size = new Vector3(6f, 10f, 10f);
		}
	}
}
