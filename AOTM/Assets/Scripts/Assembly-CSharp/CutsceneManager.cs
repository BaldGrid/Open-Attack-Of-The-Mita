using System.Collections;
using UnityEngine;

public class CutsceneManager : MonoBehaviour
{
	public GameObject ogbaldi;

	public GameObject getrid;

	public Animator camersa;

	public GameObject player;

	public GameControllerScript gc;

	public AudioSource audios;

	public GameObject hud;

	public AudioClip ohonss;

	public AudioClip chokingloop;

	public AudioClip chokingend;

	public GameObject dialog;

	public GameObject timerss;

	private void Start()
	{
		gc.dialogue = true;
		StartCoroutine(Cutscenestart());
	}

	public IEnumerator Cutscenestart()
	{
		timerss.SetActive(value: false);
		gc.schoolMusic.Stop();
		player.SetActive(value: false);
		ogbaldi.SetActive(value: false);
		audios.Play();
		audios.PlayOneShot(ohonss);
		audios.clip = chokingloop;
		audios.loop = true;
		hud.SetActive(value: false);
		yield return new WaitForSeconds(5f);
		camersa.Play("BallonHerecutpart", -1, 0f);
		audios.PlayOneShot(chokingend);
		audios.loop = false;
		yield return new WaitForSeconds(2f);
		gc.dialogue = true;
		dialog.SetActive(value: true);
	}

	public void endcutscene()
	{
		new WaitForSeconds(2f);
		gc.glitchlearn = true;
		gc.dialogue = false;
		player.SetActive(value: true);
		getrid.SetActive(value: false);
		hud.SetActive(value: true);
		gc.ActivateSpoopMode();
	}
}
