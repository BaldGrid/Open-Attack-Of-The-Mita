using UnityEngine;

public class FloodTextureScript : MonoBehaviour
{
	public float FloodSpeed;

	public MeshRenderer FloodTexture;

	private void Start()
	{
		FloodSpeed = 0.25f;
	}

	private void Update()
	{
		float y = Time.time * FloodSpeed;
		FloodTexture.material.SetTextureOffset("_MainTex", new Vector2(0f, y));
	}
}
