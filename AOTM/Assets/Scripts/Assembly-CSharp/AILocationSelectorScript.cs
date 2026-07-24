using UnityEngine;

public class AILocationSelectorScript : MonoBehaviour
{
	public Transform[] newLocation = new Transform[521];

	public AmbienceScript ambience;

	private int id;

	public void GetNewTarget()
	{
		id = Mathf.RoundToInt(Random.Range(0f, 521f));
		base.transform.position = newLocation[id].position;
		ambience.PlayAudio();
	}

	public void GetNewTargetHallway()
	{
		id = Mathf.RoundToInt(Random.Range(0f, 177f));
		base.transform.position = newLocation[id].position;
		ambience.PlayAudio();
	}

	public void QuarterExclusive()
	{
		id = Mathf.RoundToInt(Random.Range(1f, 177f));
		base.transform.position = newLocation[id].position;
	}
}
