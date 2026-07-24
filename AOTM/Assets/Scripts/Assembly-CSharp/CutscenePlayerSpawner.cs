using System.Collections;
using UnityEngine;

public class CutscenePlayerSpawner : MonoBehaviour
{
	public GameObject player;

	public GameObject pri;

	public GameObject cuts;

	public GameControllerScript gc;

	private void Start()
	{
		StartCoroutine(Cutscenestart());
	}

	public IEnumerator Cutscenestart()
	{
		yield return new WaitForSeconds(0.5f);
		player.SetActive(value: true);
		pri.SetActive(value: true);
		base.gameObject.SetActive(value: false);
		gc.fadeing.SetActive(value: true);
		cuts.SetActive(value: false);
		gc.dialogue = false;
	}
}
