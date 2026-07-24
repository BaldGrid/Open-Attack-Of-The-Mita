using UnityEngine;

public class PickupBob : MonoBehaviour
{
	private void Update()
	{
		base.transform.localPosition = new Vector3(0f, PickupBobValue.bobVal, 0f);
	}
}
