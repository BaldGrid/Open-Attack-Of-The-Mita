using UnityEngine;

public class PickupBobValue : MonoBehaviour
{
	public static float bobVal;

	public float speed = 5f;

	private float val;

	private void Update()
	{
		val += Time.deltaTime;
		bobVal = Mathf.Sin(val * speed) / 2f;
	}
}
