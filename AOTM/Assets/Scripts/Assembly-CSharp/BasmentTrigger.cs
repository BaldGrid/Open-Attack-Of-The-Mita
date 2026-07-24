using UnityEngine;
using UnityEngine.SceneManagement;

public class BasmentTrigger : MonoBehaviour
{
	public GameControllerScript gc;

	private void OnTriggerEnter(Collider other)
	{
		if ((((float)gc.notebooks >= gc.SceneNotebooks) & (other.tag == "Player")) && gc.notebooks == 7)
		{
			SceneManager.LoadScene("Basement");
		}
	}
}
