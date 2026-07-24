using UnityEngine;

public class GroundMove : MonoBehaviour
{
	public float speed;

	public float visualSpeedScalar;

	private Vector3 direction;

	private float currentScroll;

	private void Update()
	{
		currentScroll += Time.deltaTime * speed * visualSpeedScalar;
		GetComponent<Renderer>().material.mainTextureOffset = new Vector2(0f, currentScroll);
	}

	private void OnCollisionStay(Collision otherThing)
	{
		direction = base.transform.forward;
		direction *= speed;
		otherThing.rigidbody.AddForce(direction, ForceMode.Acceleration);
	}
}
