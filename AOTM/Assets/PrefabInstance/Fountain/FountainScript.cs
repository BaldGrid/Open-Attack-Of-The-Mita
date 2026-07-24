using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FountainScript : MonoBehaviour
{
	// Start is called before the first frame update
	public AudioClip slurp;

	AudioSource audioSource;

	// Start is called before the first frame update

	void Start()
	{
		audioSource = GetComponent<AudioSource>();
	}

	// Update is called once per frame
	void Update()
    {
    	if (Input.GetMouseButtonDown(0) && Time.timeScale != 0f)
		{
			Ray ray = Camera.main.ScreenPointToRay(new Vector3((float)(Screen.width / 2), (float)(Screen.height / 2), 0f));
			RaycastHit raycastHit;
			if (Physics.Raycast(ray, out raycastHit) && (raycastHit.collider == this.trigger & Vector3.Distance(this.player.position, base.transform.position) < this.openingDistance))
			{
			this.ps.stamina = this.ps.maxStamina;
			audioSource.PlayOneShot(slurp);
			}
		}	
    }
	
	public Transform player;
	
	public PlayerScript ps;
	
	public float openingDistance;
	
	public MeshCollider trigger;
	
	
}
