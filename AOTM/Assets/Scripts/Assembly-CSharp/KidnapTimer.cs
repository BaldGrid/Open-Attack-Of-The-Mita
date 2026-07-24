using UnityEngine;

public class KidnapTimer : MonoBehaviour
{
	public float Timer;

	public MathGameScript mgs;

	public GameControllerScript gc;

	public BaldiScript bsc;

	public Transform player;

	public GameObject balloonspawn;

	public bool mathon;

	public CutsceneManager csm;

	public bool countingdown;

	public bool enablecutscene;

	public bool cutsceneEnabled;

	private void Start()
	{
		countingdown = true;
		Timer = Random.Range(20f, 30f);
	}

	private void Update()
	{
		if (Timer > 0f && countingdown && !gc.gamePaused)
		{
			Timer -= 1f * Time.unscaledDeltaTime;
		}
		if (Timer < 0f && enablecutscene && !cutsceneEnabled)
		{
			mathon = true;
			gc.entrance_0.Lower();
			gc.entrance_1.Lower();
			gc.entrance_2.Lower();
			gc.entrance_3.Lower();
			balloonspawn.SetActive(value: true);
		}
	}
}
