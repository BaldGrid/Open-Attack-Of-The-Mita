using UnityEngine;

public class UnknownPriBlocker : MonoBehaviour
{
	public AILocationSelectorScript AILocationSelectorScript;

	public Transform wander;

	public Transform player;

	public FireScript fireSmall;

	public FireScript fireLarge;

	public float spawnCoolDown;

	public float minTime = 10f;

	public float maxTime = 15f;

	public void SpawnFire()
	{
		if (Random.Range(4, 12) < 7)
		{
			FireScript fireScript = Object.Instantiate(fireSmall);
			fireScript.AILocationSelectorScript = AILocationSelectorScript;
			fireScript.wanderer = wander;
			fireScript.player = player;
		}
		else
		{
			FireScript fireScript2 = Object.Instantiate(fireLarge);
			fireScript2.AILocationSelectorScript = AILocationSelectorScript;
			fireScript2.wanderer = wander;
			fireScript2.player = player;
		}
	}

	public void Update()
	{
		if (spawnCoolDown <= 0f)
		{
			spawnCoolDown = Random.Range(minTime, maxTime);
			SpawnFire();
		}
		spawnCoolDown -= 1.6f * Time.deltaTime;
	}

	public void Start()
	{
		spawnCoolDown = Random.Range(minTime, maxTime);
	}
}
