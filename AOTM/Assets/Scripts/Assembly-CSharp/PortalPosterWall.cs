using UnityEngine;

public class PortalPosterWall : MonoBehaviour
{
	public AudioSource gcAudio;

	public bool portalPlaced;

	public GameObject thisWall;

	public GameObject otherWall;

	public Material portalWallThis;

	public Material portalWallOther;

	public AudioClip audSuccess;

	public AudioClip audFailure;

	public void Start()
	{
		if (thisWall == null)
		{
			thisWall = base.gameObject;
		}
		if (gcAudio == null)
		{
			gcAudio = GameObject.FindWithTag("GameController").GetComponent<AudioSource>();
		}
	}

	public void PlacePortal()
	{
		if (!portalPlaced)
		{
			if (otherWall != null)
			{
				thisWall.GetComponent<MeshCollider>().enabled = false;
				otherWall.GetComponent<MeshCollider>().enabled = false;
				thisWall.GetComponent<MeshRenderer>().material = portalWallThis;
				otherWall.GetComponent<MeshRenderer>().material = portalWallOther;
				thisWall.GetComponent<PortalPosterWall>().portalPlaced = true;
				otherWall.GetComponent<PortalPosterWall>().portalPlaced = true;
				gcAudio.PlayOneShot(audSuccess);
			}
			else
			{
				gcAudio.PlayOneShot(audFailure);
			}
		}
	}
}
