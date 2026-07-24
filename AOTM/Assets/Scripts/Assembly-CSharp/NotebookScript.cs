using UnityEngine;

public class NotebookScript : MonoBehaviour
{
	public float openingDistance;

	public GameControllerScript gc;

	public BaldiScript bsc;

	public float respawnTime;

	public bool up;

	public Transform player;

	public GameObject learningGame;

	public GameObject learningGame2;

	public AudioSource audioDevice;

	public SpriteRenderer nbr;

	public Sprite regularnb;

	public Sprite glitchnb;

	public KidnapTimer timss;

	private void Start()
	{
		up = true;
	}

	private void Update()
	{
		if (gc.mode == "endless")
		{
			if (respawnTime > 0f)
			{
				if ((base.transform.position - player.position).magnitude > 60f)
				{
					respawnTime -= Time.deltaTime;
				}
			}
			else if (!up)
			{
				base.transform.position = new Vector3(base.transform.position.x, 4f, base.transform.position.z);
				up = true;
				audioDevice.Play();
			}
		}
		if ((ControlFreak2.CF2Input.GetMouseButtonDown(0) || (ControlFreak2.CF2Input.GetKeyDown(KeyCode.E) && Time.timeScale != 0f)) && Physics.Raycast(Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0f)), out var hitInfo) && ((hitInfo.transform.tag == "Notebook") & (Vector3.Distance(player.position, base.transform.position) < openingDistance)))
		{
			base.transform.position = new Vector3(base.transform.position.x, -20f, base.transform.position.z);
			up = false;
			respawnTime = 120f;
			gc.CollectNotebook();
			if (!gc.spoopMode)
			{
				GameObject obj = Object.Instantiate(learningGame);
				obj.GetComponent<MathGameScript>().gc = gc;
				obj.GetComponent<MathGameScript>().baldiScript = bsc;
				obj.GetComponent<MathGameScript>().playerPosition = player.position;
			}
			if (gc.spoopMode)
			{
				GameObject obj2 = Object.Instantiate(learningGame2);
				obj2.GetComponent<GlitchGame>().gc = gc;
				obj2.GetComponent<GlitchGame>().baldiScript = bsc;
				obj2.GetComponent<GlitchGame>().playerPosition = player.position;
			}
		}
		if (!gc.spoopMode)
		{
			nbr.sprite = regularnb;
		}
		else
		{
			nbr.sprite = glitchnb;
		}
	}
}
