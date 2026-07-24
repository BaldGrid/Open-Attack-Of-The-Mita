using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VertexGlitchManager : MonoBehaviour
{
	private float glitchVal;
	private float time = 0f;
	
	private bool isShakeGlitchUpdating = false;
	
	public bool isSliderEnabled = false;
	
	public Slider slider;
	
	public void Glitch()
	{
		if (glitchVal <= 0f)
		{
			StartCoroutine(UnGlitch());
		}
		glitchVal = 1f;
		Shader.SetGlobalFloat("_VertexGlitchSeed", Random.Range(0f, 1000f));
		Shader.SetGlobalFloat("_VertexGlitchIntensity", glitchVal * 3f);
		Shader.SetGlobalFloat("_TileVertexGlitchSeed", Random.Range(0f, 1000f));
		Shader.SetGlobalFloat("_TileVertexGlitchIntensity", glitchVal * 3f);
	}
	
	private IEnumerator UnGlitch()
	{
		yield return null;
		while (glitchVal > 0f)
		{
			glitchVal -= Time.deltaTime * 4f;
			Shader.SetGlobalFloat("_VertexGlitchIntensity", glitchVal * 3f);
			Shader.SetGlobalFloat("_TileVertexGlitchIntensity", glitchVal * 3f);
			yield return null;
		}
		glitchVal = 0f;
		Shader.SetGlobalFloat("_VertexGlitchIntensity", 0f);
		Shader.SetGlobalFloat("_TileVertexGlitchIntensity", 0f);
	}
	
	public void ShakeGlitch()
	{
		isShakeGlitchUpdating = true;
		time = 0f;
	}
	
	void Update()
	{
		if (isShakeGlitchUpdating)
		{
			time += Time.deltaTime;
			if (!isSliderEnabled)
			{
				if (time >= 10f)
				{
					isShakeGlitchUpdating = false;
					time = 0f;
				
				}
			}
			else
			{
				if (time >= slider.value)
				{
					isShakeGlitchUpdating = false;
					time = 0f;
				}
			}
			Shader.SetGlobalFloat("_VertexGlitchIntensity", time * 1f);
			Shader.SetGlobalFloat("_TileVertexGlitchIntensity", time * 1f);
			Shader.SetGlobalFloat("_VertexGlitchSeed", Random.Range(0f, 1000f));
			Shader.SetGlobalFloat("_TileVertexGlitchSeed", Random.Range(0f, 1000f));
		}
		else
		{
			time = 0f;
		}
	}
}
