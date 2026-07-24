using UnityEngine;

public class TreeMove : MonoBehaviour
{
	private float scrollSpeed = 1000f;

	private Renderer rend;

	private void Start()
	{
		rend = GetComponent<Renderer>();
	}

	private void Update()
	{
		float x = Time.time * scrollSpeed;
		rend.material.mainTextureOffset = new Vector2(x, 0f);
	}
}
