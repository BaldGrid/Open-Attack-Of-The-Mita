using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitTriggerScript : MonoBehaviour
{
	public GameControllerScript gc;

	private void OnTriggerEnter(Collider other)
	{
		if (((float)gc.notebooks >= gc.SceneNotebooks) & (other.tag == "Player"))
		{
			if (gc.notebooks == 7)
			{
				SceneManager.LoadScene("EndCutScene");
			}
			else
			{
				SceneManager.LoadScene("Results");
			}
		}
	}
}
