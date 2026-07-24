using UnityEngine;

public class UnknownPriProjectile : MonoBehaviour
{
	public ProjectileSpawner projectileSpawner;

	public UnknownPri_NPC unknown;

	public GameObject visibleObject;

	public Transform targetTransform;

	public Vector3 origPosition;

	public bool thrown;

	public bool positionTaken;

	public bool held;

	public bool noCollider;

	public float life = 20f;

	public int currentLocationIndex;

	public string currentGameObjectName;

	public void Start()
	{
		currentGameObjectName = base.gameObject.name;
		Respawn();
	}

	public void TeleportProjectile()
	{
		int num = Random.Range(0, projectileSpawner.projectileSpawnPoints.Length - 1);
		base.transform.position = projectileSpawner.projectileSpawnPoints[num].transform.position + new Vector3(0f, 2f, 0f);
		origPosition = base.transform.position;
		currentLocationIndex = num;
	}

	private void Update()
	{
		if (unknown.gameOver && held)
		{
			SelfDestruct();
		}
		if (unknown.gameOver && thrown)
		{
			SelfDestruct();
		}
		if (held && !thrown)
		{
			base.transform.SetParent(Camera.main.transform);
			base.transform.position = new Vector3(targetTransform.position.x, 2f, targetTransform.position.z) + targetTransform.forward * 4f;
			base.transform.localRotation = Quaternion.Euler(0f, -90f, 0f);
			visibleObject.layer = 18;
		}
		if (thrown)
		{
			base.transform.position += base.transform.forward * 100f * Time.deltaTime;
			life -= Time.deltaTime;
			if (life <= 0f)
			{
				base.transform.position = origPosition;
				Respawn();
			}
			visibleObject.layer = 0;
		}
		if ((ControlFreak2.CF2Input.GetMouseButtonDown(1) | (ControlFreak2.CF2Input.GetKeyDown(KeyCode.Q) & !thrown)) && held)
		{
			ThrowProjectile();
		}
	}

	public void ThrowProjectile()
	{
		base.gameObject.name = "Thrown";
		base.gameObject.tag = "ActualProjectile";
		projectileSpawner.currentProjectile = null;
		thrown = true;
		held = false;
		noCollider = true;
		base.transform.SetParent(null);
		base.gameObject.GetComponent<SphereCollider>().enabled = true;
		base.transform.position = new Vector3(targetTransform.position.x, 2f, targetTransform.position.z);
		base.transform.rotation = targetTransform.rotation;
		life = 10f;
	}

	public void Respawn()
	{
		base.gameObject.tag = "Projectile";
		base.gameObject.name = currentGameObjectName;
		thrown = false;
		held = false;
		noCollider = false;
		base.gameObject.GetComponent<SphereCollider>().enabled = true;
		base.transform.rotation = Quaternion.identity;
		visibleObject.layer = 0;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (projectileSpawner.currentProjectile == null && ((other.tag == "Player") & !held) && !noCollider)
		{
			held = true;
			projectileSpawner.currentProjectile = this;
			base.gameObject.GetComponent<SphereCollider>().enabled = false;
		}
	}

	public void OnTriggerStay(Collider other)
	{
		if (!thrown && other.transform.tag == "Projectile")
		{
			TeleportProjectile();
		}
	}

	public void SelfDestruct()
	{
		projectileSpawner.projectileScripts.Remove(this);
		Object.Destroy(base.gameObject, 0f);
	}
}
