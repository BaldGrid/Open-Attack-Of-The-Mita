using UnityEngine;

public class RotationSkybox : MonoBehaviour
{
	public float RotateSpeed = 1.2f;

	private void Update()
	{
		RenderSettings.skybox.SetFloat("_Rotation", Time.time * RotateSpeed);
	}
}
