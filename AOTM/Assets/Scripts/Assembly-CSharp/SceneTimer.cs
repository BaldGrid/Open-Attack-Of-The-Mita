using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTimer : MonoBehaviour
{
	public string scene;

	public float initialTime = 5f;

	private float time;

	private void Start()
	{
		time = initialTime;
	}

	private void Update()
	{
		time -= Time.unscaledDeltaTime;
		if (time <= 0f)
		{
			SceneManager.LoadScene(scene);
		}
	}
}
