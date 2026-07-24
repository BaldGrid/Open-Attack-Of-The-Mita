using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameControllerScript : MonoBehaviour
{
	public CursorControllerScript cursorController;

	public PlayerScript player;

	public Transform playerTransform;

	public CharacterController playerCharacter;

	public Collider playerCollider;

	public Transform cameraTransform;

	public Camera camera;

	private int cullingMask;

	public EntranceScript entrance_0;

	public EntranceScript entrance_1;

	public EntranceScript entrance_2;

	public EntranceScript entrance_3;

	public GameObject baldiTutor;

	public BaldiScript baldiScrpt;

	public AudioClip aud_Prize;

	public AudioClip aud_tips;

	public AudioClip aud_PrizeMobile;

	public AudioClip aud_AllNotebooks;

	private bool flipped;

	public PlaytimeScript playtimeScript;

	public FirstPrizeScript firstPrizeScript;

	public AudioSource tutorBaldi;

	public Animator tutorAnimat;

	public RectTransform boots;

	public string mode;

	public float SceneNotebooks;

	public int notebooks;

	public GameObject[] notebookPickups;

	public int failedNotebooks;

	public bool spoopMode;

	public bool finaleMode;

	public bool debugMode;

	public bool mouseLocked;

	public int exitsReached;

	public int itemSelected;

	public GameObject GlitchVal;
	public GameObject GETOUT;

	public int[] item = new int[3];

	public RawImage[] itemSlot = new RawImage[3];

	private string[] itemNames = new string[13]
	{
		"Nothing", "Energy flavored Zesty Bar", "Yellow Door Lock", "Principal's Keys", "BSODA", "Quarter", "Baldi's Least Favorite Tape", "Alarm Clock", "WD-NoSquee (Door Type)", "Safety Scissors",
		"Big Ol' Boots", "Dangerous Teleporter", "Portal Poster"
	};

	public TMP_Text itemText;

	public Sprite[] itemSpr;

	public Object[] items = new Object[10];

	public Texture[] itemTextures = new Texture[10];

	public GameObject bsodaSpray;

	public GameObject alarmClock;

	public TMP_Text notebookCount;

	public GameObject pauseMenu;

	public GameObject highScoreText;

	public GameObject warning;

	public GameObject reticle;

	public RectTransform itemSelect;

	private int[] itemSelectOffset;

	public bool gamePaused;

	private bool learningActive;

	private float gameOverDelay;

	private AudioSource audioDevice;

	public AudioClip aud_Soda;

	public AudioClip aud_Spray;

	public AudioClip aud_buzz;

	public AudioClip aud_Hang;

	public AudioClip aud_MachineQuiet;

	public AudioClip aud_MachineStart;

	public AudioClip aud_MachineRev;

	public AudioClip aud_MachineLoop;

	public AudioClip aud_Switch;

	public AudioClip aud_Teleport;

	public AudioSource schoolMusic;

	public AudioSource LevelEnd;

	public AudioSource learnMusic;

	public Material OutOfBsoda;

	public Material OutOfZesty;

	public AudioClip Coin;

	public AILocationSelectorScript AILocationSelector;

	public AudioClip audDoorUnlock;

	public AudioClip audDoorLock;

	public AudioQueue pize;

	public Material FinalModeSky;

	public KidnapTimer kidnaptimer;

	public bool dialogue;

	public bool glitchlearn;

	public MathGameScript mgs;

	public GameObject hud;

	public GameObject quarter;

	public bool givingprize;

	public GameObject[] npcsToSpawn;

	public GameObject[] disablefinalthings;

	public GameObject finalcut;

	public SwingingDoorScript[] swingunlock;

	public GameObject[] bossthings;

	private bool finishSound;

	public AudioClip null_scare;

	public AudioReverbZone auds;

	public AudioSource PlayMusic;

	public Transform spoopstart;

	public Transform spoopstart2;

	public Transform spoopstart3;

	public Transform playstart;

	public PrincipalScript pris;

	public UnknownPri_NPC npcnull;

	public UnknownPT_NPC npcpt;

	public UnknownPrinc_NPC1 npcprin;

	public AudioClip[] aud_Glitch = new AudioClip[7];

	public List<Material> matsToModfiy = new List<Material>();

	public Color standardDarkColor = new Color(0.75f, 0.75f, 0.75f);

	public GameObject itesms;

	public Billboard bt;

	public DialogueManager dm;

	public bool bosson;

	public GameObject fadeing;

	public GameObject backsay;

	public AudioClip Baldloongetout;

	public Animator NotebooksAnim;
	
	public AudioClip NotebooksSound;

	public GameControllerScript()
	{
		itemSelectOffset = new int[3] { -84, -44, -5 };
	}

	private void Start()
	{
		cullingMask = camera.cullingMask;
		audioDevice = GetComponent<AudioSource>();
		mode = PlayerPrefs.GetString("CurrentMode");
		LockMouse();
		UpdateNotebookCount();
		itemSelected = 0;
		gameOverDelay = 0.8f;
	}
	[System.Obsolete]
	public void SetPulseSpeed(float speed)
	{
		
	}

	private void Update()
	{
		if (dialogue)
		{
			UnlockMouse();
		}
		if (!learningActive)
		{
			if (Input.GetButtonDown("Pause"))
			{
				if (!gamePaused)
				{
					PauseGame();
				}
				else
				{
					UnpauseGame();
				}
			}
			if (Input.GetKeyDown(KeyCode.Y) & gamePaused)
			{
				ExitGame();
			}
			else if (Input.GetKeyDown(KeyCode.N) & gamePaused)
			{
				UnpauseGame();
			}
			if (!gamePaused & (Time.timeScale != 1f))
			{
				Time.timeScale = 1f;
			}
			if (Input.GetMouseButtonDown(1) || (Input.GetKeyDown(KeyCode.Q) && Time.timeScale != 0f))
			{
				UseItem();
			}
			if (Input.GetAxis("Mouse ScrollWheel") > 0f && Time.timeScale != 0f)
			{
				DecreaseItemSelection();
			}
			else if (Input.GetAxis("Mouse ScrollWheel") < 0f && Time.timeScale != 0f)
			{
				IncreaseItemSelection();
			}
			if (Time.timeScale != 0f)
			{
				if (Input.GetKeyDown(KeyCode.Alpha1))
				{
					itemSelected = 0;
					UpdateItemSelection();
				}
				else if (Input.GetKeyDown(KeyCode.Alpha2))
				{
					itemSelected = 1;
					UpdateItemSelection();
				}
				else if (Input.GetKeyDown(KeyCode.Alpha3))
				{
					itemSelected = 2;
					UpdateItemSelection();
				}
			}
		}
		else if (Time.timeScale != 0f)
		{
			Time.timeScale = 0f;
		}
		if (player.gameOver || player.PTgameOver || player.BullygameOver || player.PRIgameOver)
		{
			if (!finishSound)
			{
				finishSound = true;
				audioDevice.PlayOneShot(null_scare);
			}
			if (player.PTgameOver)
			{
				bt.enabled = true;
			}
			Time.timeScale = 0f;
			auds.enabled = false;
			PlayMusic.enabled = false;
			dialogue = true;
			StartCoroutine(Flashlight());
			gameOverDelay -= Time.unscaledDeltaTime * 0.08f;
			camera.farClipPlane = gameOverDelay * 400f;
			AudioListener.pause = false;
			if (gameOverDelay <= 0f)
			{
				Time.timeScale = 1f;
				Application.Quit();
			}
		}
	}

	private void FixedUpdate()
	{
		if (finaleMode)
		{
			//RenderSettings.ambientLight = Color.Lerp(RenderSettings.ambientLight, standardDarkColor, 0.3f * Time.fixedDeltaTime);
			GETOUT.SetActive(value: true);
		}
		if (dialogue)
		{
			fadeing.SetActive(value: false);
		}
	}

	private void UpdateNotebookCount()
	{
		if (mode == "story" && !spoopMode)
		{
			notebookCount.text = notebooks + "/" + SceneNotebooks + " Notebooks";
		}
		else
		{
			notebookCount.text = notebooks + " Notebooks";
		}
		if (((float)notebooks == SceneNotebooks) & (mode == "story"))
		{
			SwingingDoorScript[] array = swingunlock;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].GetComponent<SwingingDoorScript>().UnlockDoor();
			}
		}
		if (spoopMode)
		{
			notebookCount.text = notebooks + "/" + SceneNotebooks + " N@T£B@@K$";
			notebookCount.text = notebooks + " N@T£B@@K$";
		}
	}

	public void CollectNotebook()
	{
		notebooks++;
		UpdateNotebookCount();
	}

	public void LockMouse()
	{
		if (!learningActive)
		{
			cursorController.LockCursor();
			mouseLocked = true;
			reticle.SetActive(value: true);
		}
	}

	public void UnlockMouse()
	{
		cursorController.UnlockCursor();
		mouseLocked = false;
		reticle.SetActive(value: false);
	}

	public void PauseGame()
	{
		if (!learningActive && !dialogue)
		{
			UnlockMouse();
			hud.SetActive(value: false);
			AudioListener.pause = true;
			kidnaptimer.countingdown = false;
			Time.timeScale = 0f;
			gamePaused = true;
			pauseMenu.SetActive(value: true);
			fadeing.SetActive(value: false);
		}
	}

	public void ExitGame()
	{
		gamePaused = false;
		AudioListener.pause = false;
		SceneManager.LoadScene("MainMenu");
	}

	public void UnpauseGame()
	{
		if (!bosson)
		{
			hud.SetActive(value: true);
		}
		fadeing.SetActive(value: true);
		kidnaptimer.countingdown = true;
		Time.timeScale = 1f;
		AudioListener.pause = false;
		gamePaused = false;
		pauseMenu.SetActive(value: false);
		LockMouse();
	}

	public void ActivateSpoopMode()
	{
		spoopMode = true;
		baldiTutor.SetActive(value: false);
		for (int i = 0; i < npcsToSpawn.Length; i++)
		{
			npcsToSpawn[i].gameObject.SetActive(value: true);
		}
		learnMusic.Stop();
		schoolMusic.Stop();
		SceneNotebooks = 7f;
		if (spoopMode)
		{
			notebookCount.text = notebooks + "/" + SceneNotebooks + " N@T£B@@K$";
			notebookCount.text = notebooks + " N@T£B@@K$";
		}
		itesms.SetActive(value: true);
		StartCoroutine(FadeOnFog());
	}

	public void activatecutscene()
	{
		for (int i = 0; i < npcsToSpawn.Length; i++)
		{
			npcsToSpawn[i].gameObject.SetActive(value: false);
		}
		for (int j = 0; j < disablefinalthings.Length; j++)
		{
			disablefinalthings[j].gameObject.SetActive(value: false);
		}
		finalcut.SetActive(value: true);
	}

	public void StartFirstBoss()
	{
		for (int i = 0; i < bossthings.Length; i++)
		{
			bossthings[i].gameObject.SetActive(value: true);
		}
        var walls = FindObjectsByType<WallShakingManager>(FindObjectsSortMode.None);
        //for (int i = 0; i < walls.Length; i++)
        //{
        //    var wall = walls[i];
        //    wall.StartShaking();
        //}
        hud.SetActive(value: false);
		fadeing.SetActive(value: true);
		finalcut.SetActive(value: false);
		baldiScrpt.agent.Warp(spoopstart.position);
		playtimeScript.agent.Warp(spoopstart2.position);
		pris.agent.Warp(spoopstart3.position);
		baldiScrpt.enabled = false;
		playtimeScript.enabled = false;
		pris.enabled = false;
		player.cc.enabled = false;
		playerTransform.position = playstart.position;
		player.cc.enabled = true;
		npcpt.enabled = true;
		npcprin.enabled = true;
		dialogue = false;
		itesms.SetActive(value: false);
		GlitchVal.SetActive(value: true);
		LoseItem(0);
		LoseItem(1);
		LoseItem(2);
		bosson = true;
	}

	public void ActivateFinaleMode()
	{
		audioDevice.PlayOneShot(Baldloongetout);
		finaleMode = true;
		entrance_0.Raise();
		entrance_1.Raise();
		entrance_2.Raise();
		entrance_3.Raise();
	}

	public void GetAngry(float value)
	{
		if (!spoopMode)
		{
			ActivateSpoopMode();
		}
		baldiScrpt.GetAngry(value);
	}

	public void ActivateLearningGame()
	{
		auds.enabled = false;
		fadeing.SetActive(value: false);
		learningActive = true;
		kidnaptimer.enablecutscene = false;
		UnlockMouse();
		tutorBaldi.Stop();
		kidnaptimer.mathon = true;
		if (!spoopMode)
		{
			schoolMusic.Stop();
			learnMusic.Play();
		}
	}

	public void ActivateLearningGame2()
	{
		fadeing.SetActive(value: false);
		auds.enabled = false;
		glitchlearn = true;
		learningActive = true;
		UnlockMouse();
		tutorBaldi.Stop();
		if (!spoopMode)
		{
			schoolMusic.Stop();
			learnMusic.Play();
			//NotebooksAnim.Play("NotebookImageAnimation", -1); 
			audioDevice.PlayOneShot(NotebooksSound);
		}
	}

	public void DeactivateLearningGame(GameObject subject)
	{
		NotebooksAnim.Play("NotebookImageAnimation", -1); 
		//audioDevice.PlayOneShot(NotebooksSound);
		fadeing.SetActive(value: true);
		auds.enabled = true;
		camera.cullingMask = cullingMask;
		learningActive = false;
		Object.Destroy(subject);
		kidnaptimer.mathon = false;
		LockMouse();
		if (player.stamina < 100f)
		{
			player.stamina = 100f;
		}
		if (!spoopMode)
		{
			schoolMusic.Play();
			learnMusic.Stop();
		}
		if ((notebooks >= 0) & givingprize)
		{
			quarter.SetActive(value: true);
			tutorAnimat.SetTrigger("Explain");
			tutorBaldi.PlayOneShot(aud_Prize);
		}
		if ((notebooks <= 1) & !givingprize)
		{
			tutorAnimat.SetTrigger("Explain");
			tutorBaldi.PlayOneShot(aud_tips);
		}
		if ((notebooks == 2) & (mode == "story"))
		{
			audioDevice.Stop();
			tutorBaldi.PlayOneShot(aud_AllNotebooks);
			audioDevice.Play();
		}
	}

	public void DeactivateLearningGame2(GameObject subject)
	{
		auds.enabled = true;
		fadeing.SetActive(value: true);
		camera.cullingMask = cullingMask;
		learningActive = false;
		Object.Destroy(subject);
		LockMouse();
		if (player.stamina < 100f)
		{
			player.stamina = 100f;
		}
		if (!spoopMode)
		{
			schoolMusic.Play();
			learnMusic.Stop();
		}
		else if (((float)notebooks == SceneNotebooks) & (mode == "story"))
		{
			activatecutscene();
		}
	}

	public IEnumerator Flashlight()
	{
		Random.Range(0.1f, 1f);
		RenderSettings.ambientLight = Color.red;
		yield return new WaitForSecondsRealtime(0.2f);
		RenderSettings.ambientLight = Color.magenta;
		yield return new WaitForSecondsRealtime(0.2f);
		RenderSettings.ambientLight = Color.yellow;
		yield return new WaitForSecondsRealtime(0.2f);
		RenderSettings.ambientLight = Color.green;
		yield return new WaitForSecondsRealtime(0.2f);
		RenderSettings.ambientLight = Color.blue;
		yield return new WaitForSecondsRealtime(0.2f);
		StartCoroutine(Flashlight());
	}

	private void IncreaseItemSelection()
	{
		itemSelected++;
		if (itemSelected > 2)
		{
			itemSelected = 0;
		}
		itemSelect.anchoredPosition = new Vector3(itemSelectOffset[itemSelected], -7f, 0f);
		UpdateItemName();
	}

	private void DecreaseItemSelection()
	{
		itemSelected--;
		if (itemSelected < 0)
		{
			itemSelected = 2;
		}
		itemSelect.anchoredPosition = new Vector3(itemSelectOffset[itemSelected], -7f, 0f);
		UpdateItemName();
	}

	private void UpdateItemSelection()
	{
		itemSelect.anchoredPosition = new Vector3(itemSelectOffset[itemSelected], -7f, 0f);
		UpdateItemName();
	}

	public void CollectItem(int item_ID)
	{
		if (item[itemSelected] == 0)
		{
			item[itemSelected] = item_ID;
			itemSlot[itemSelected].texture = itemTextures[item_ID];
		}
		else if (item[0] == 0)
		{
			item[0] = item_ID;
			itemSlot[0].texture = itemTextures[item_ID];
		}
		else if (item[1] == 0)
		{
			item[1] = item_ID;
			itemSlot[1].texture = itemTextures[item_ID];
		}
		else if (item[2] == 0)
		{
			item[2] = item_ID;
			itemSlot[2].texture = itemTextures[item_ID];
		}
		else
		{
			item[itemSelected] = item_ID;
			itemSlot[itemSelected].texture = itemTextures[item_ID];
		}
		UpdateItemName();
	}

	private void UseItem()
	{
		if (item[itemSelected] == 0)
		{
			return;
		}
		if (item[itemSelected] == 1)
		{
			player.stamina = player.maxStamina * 2f;
			ResetItem();
		}
		else if (item[itemSelected] == 2)
		{
			if (Physics.Raycast(Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0f)), out var hitInfo) && ((hitInfo.collider.tag == "SwingingDoor") & (Vector3.Distance(playerTransform.position, hitInfo.transform.position) <= 10f)))
			{
				hitInfo.collider.gameObject.GetComponent<SwingingDoorScript>().LockDoor(15f);
				ResetItem();
			}
		}
		else if (item[itemSelected] == 3)
		{
			if (Physics.Raycast(Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0f)), out var hitInfo2) && ((hitInfo2.collider.tag == "Door") & (Vector3.Distance(playerTransform.position, hitInfo2.transform.position) <= 10f)))
			{
				DoorScript component = hitInfo2.collider.gameObject.GetComponent<DoorScript>();
				if (component.DoorLocked)
				{
					component.UnlockDoor();
					component.OpenDoor();
					ResetItem();
				}
			}
		}
		else if (item[itemSelected] == 4)
		{
			Object.Instantiate(bsodaSpray, playerTransform.position, cameraTransform.rotation);
			ResetItem();
			player.ResetGuilt("drink", 1f);
			audioDevice.PlayOneShot(aud_Soda);
		}
		else if (item[itemSelected] == 5)
		{
			if (Physics.Raycast(Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0f)), out var hitInfo3))
			{
				if ((hitInfo3.collider.name == "BSODAMachine") & (Vector3.Distance(playerTransform.position, hitInfo3.transform.position) <= 10f))
				{
					ResetItem();
					CollectItem(4);
					hitInfo3.collider.gameObject.name = "EmptyMachine";
					hitInfo3.collider.gameObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material = OutOfBsoda;
					audioDevice.PlayOneShot(Coin);
				}
				else if ((hitInfo3.collider.name == "ZestyMachine") & (Vector3.Distance(playerTransform.position, hitInfo3.transform.position) <= 10f))
				{
					ResetItem();
					CollectItem(1);
					hitInfo3.collider.gameObject.name = "EmptyMachine";
					hitInfo3.collider.gameObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material = OutOfZesty;
					audioDevice.PlayOneShot(Coin);
				}
				else if ((hitInfo3.collider.name == "PayPhone") & (Vector3.Distance(playerTransform.position, hitInfo3.transform.position) <= 10f))
				{
					hitInfo3.collider.gameObject.GetComponent<TapePlayerScript>().Play();
					audioDevice.PlayOneShot(Coin);
					ResetItem();
				}
			}
		}
		else if (item[itemSelected] == 6)
		{
			if (Physics.Raycast(Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0f)), out var hitInfo4) && ((hitInfo4.collider.name == "TapePlayer") & (Vector3.Distance(playerTransform.position, hitInfo4.transform.position) <= 10f)))
			{
				hitInfo4.collider.gameObject.GetComponent<TapePlayerScript>().Play();
				ResetItem();
			}
		}
		else if (item[itemSelected] == 7)
		{
			Object.Instantiate(alarmClock, playerTransform.position, cameraTransform.rotation).GetComponent<AlarmClockScript>().baldi = baldiScrpt;
			ResetItem();
		}
		else if (item[itemSelected] == 8)
		{
			if (Physics.Raycast(Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0f)), out var hitInfo5) && ((hitInfo5.collider.tag == "Door") & (Vector3.Distance(playerTransform.position, hitInfo5.transform.position) <= 10f)))
			{
				hitInfo5.collider.gameObject.GetComponent<DoorScript>().SilenceDoor();
				ResetItem();
				audioDevice.PlayOneShot(aud_Spray);
			}
		}
		else if (item[itemSelected] == 9)
		{
			Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0f));
			RaycastHit hitInfo6;
			if (player.jumpRope)
			{
				player.DeactivateJumpRope();
				playtimeScript.Disappoint();
				ResetItem();
			}
			else if (Physics.Raycast(ray, out hitInfo6) && hitInfo6.collider.name == "1st Prize")
			{
				firstPrizeScript.GoCrazy();
				ResetItem();
			}
		}
		else if (item[itemSelected] == 10)
		{
			player.ActivateBoots();
			StartCoroutine(BootAnimation());
			ResetItem();
		}
		else if (item[itemSelected] == 11)
		{
			StartCoroutine(Teleporter());
			ResetItem();
		}
		else
		{
			if (item[itemSelected] != 12 || !Physics.Raycast(Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0f)), out var hitInfo7) || !hitInfo7.collider.name.Contains("Wall") || !(Vector3.Distance(playerTransform.position, hitInfo7.transform.position) <= 10f))
			{
				return;
			}
			PortalPosterWall component2 = hitInfo7.collider.gameObject.GetComponent<PortalPosterWall>();
			if (component2 != null)
			{
				component2.PlacePortal();
				if (component2.otherWall != null)
				{
					ResetItem();
				}
			}
		}
	}

	private IEnumerator FadeOnFog()
	{
		RenderSettings.fogDensity = 0f;
		RenderSettings.fog = true;
		while (RenderSettings.fogDensity < 0.02f)
		{
			RenderSettings.fogDensity += 0.025f * Time.deltaTime;
			yield return null;
		}
		RenderSettings.fogDensity = 0.02f;
	}

	private IEnumerator BootAnimation()
	{
		float time = 15f;
		float height = 244f;
		boots.gameObject.SetActive(value: true);
		Vector3 localPosition;
		while (height > -375f)
		{
			height -= 375f * Time.deltaTime;
			time -= Time.deltaTime;
			localPosition = boots.localPosition;
			localPosition.y = height;
			boots.localPosition = localPosition;
			yield return null;
		}
		localPosition = boots.localPosition;
		localPosition.y = -375f;
		boots.localPosition = localPosition;
		boots.gameObject.SetActive(value: false);
		while (time > 0f)
		{
			time -= Time.deltaTime;
			yield return null;
		}
		boots.gameObject.SetActive(value: true);
		while (height < 375f)
		{
			height += 244f * Time.deltaTime;
			localPosition = boots.localPosition;
			localPosition.y = height;
			boots.localPosition = localPosition;
			yield return null;
		}
		localPosition = boots.localPosition;
		localPosition.y = 244f;
		boots.localPosition = localPosition;
		boots.gameObject.SetActive(value: false);
	}

	private IEnumerator Teleporter()
	{
		playerCharacter.enabled = false;
		playerCollider.enabled = false;
		int teleports = Random.Range(12, 16);
		int teleportCount = 0;
		float baseTime = 0.2f;
		float currentTime = baseTime;
		float increaseFactor = 1.1f;
		while (teleportCount < teleports)
		{
			currentTime -= Time.deltaTime;
			if (currentTime < 0f)
			{
				Teleport();
				teleportCount++;
				baseTime *= increaseFactor;
				currentTime = baseTime;
			}
			if (flipped)
			{
				player.height = 6f;
			}
			else
			{
				player.height = 4f;
			}
			yield return null;
		}
		playerCharacter.enabled = true;
		playerCollider.enabled = true;
	}

	public void PlayRandomGlitchSound()
	{
		int num = Mathf.RoundToInt(Random.Range(0f, 6f));
		audioDevice.PlayOneShot(aud_Glitch[num]);
	}

	private void Teleport()
	{
		AILocationSelector.GetNewTarget();
		player.transform.position = AILocationSelector.transform.position + Vector3.up * player.height;
		audioDevice.PlayOneShot(aud_Teleport);
	}

	private void ResetItem()
	{
		item[itemSelected] = 0;
		itemSlot[itemSelected].texture = itemTextures[0];
		UpdateItemName();
	}

	public void LoseItem(int id)
	{
		item[id] = 0;
		itemSlot[id].texture = itemTextures[0];
		UpdateItemName();
	}

	private void UpdateItemName()
	{
		itemText.text = itemNames[item[itemSelected]];
	}

	public void MainExitReached()
	{
		exitsReached++;
		if (exitsReached == 1)
		{
			RenderSettings.skybox = FinalModeSky;
			standardDarkColor = new Color(0.75f, 0.25f, 0.25f);
			audioDevice.PlayOneShot(aud_Switch, 0.8f);
		}
		if (exitsReached == 2)
		{
			standardDarkColor = new Color(0.75f, 0.25f, 0.25f);
			audioDevice.PlayOneShot(aud_Switch, 0.8f);
		}
		if (exitsReached == 3)
		{
			standardDarkColor = new Color(0.75f, 0.25f, 0.25f);
			audioDevice.PlayOneShot(aud_Switch, 0.8f);
		}
	}

	public void DespawnCrafters()
	{
	}

	public void Fliparoo()
	{
		player.height = 6f;
		player.fliparoo = 180f;
		player.flipaturn = -1f;
		Camera.main.GetComponent<CameraScript>().offset = new Vector3(0f, -1f, 0f);
	}

	public void CraftersTeleport()
	{
		AILocationSelector.GetNewTarget();
		player.transform.position = AILocationSelector.transform.position + Vector3.up * player.height;
		AILocationSelector.GetNewTarget();
	}
}
