using System.Collections;
using Kino;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using MidiPlayerTK;  // 确保有这个命名空间引用

public class UnknownPT_NPC : MonoBehaviour
{
    public GameObject o;
    public ProjectileSpawner projectileSpawner;
    public Transform player;
    public PlayerScript ps;
    public Transform look;
    private AudioSource audioDevice;
    public GameControllerScript gc;
    public GameObject jumpscareHud;
    private NavMeshAgent agent;
    public float minTime = 5f;
    public float maxTime = 10f;
    public float redBallonChance = 16f;
    public float spoopBalloonCoolDown;
    public float chaseSpeed = 13f;
    public float stunCooldown;
    public float glitchEffectsValue;
    public int Health = 5;
    public bool chasingDuringBossfight;
    public bool goUp;
    public bool playerSighted;
    public bool talking;
    public bool pushingBack;
    public bool interactable;
    public bool gameOver;
    public bool canGetHit;
    public bool killable;
    public bool idle;
    public bool sequenceFinished;
    public bool angry;
    public bool stunned;
    public bool bossfightBegan;
    public AudioClip aud_Speech;
    public AudioClip aud_Hit;
    public AudioClip aud_Stunned;
    public AudioClip aud_Regret;
    public AudioClip aud_Ahhhhh;
    public Animator sprite;
    public GameObject bossloopmus;
    public AudioSource bossloops;
    public AudioSource endaudio;
    public AudioSource ptmus;
    public GameObject cuts;
    public GameObject playerob;
    public CapsuleCollider cp;
    public MidiFilePlayer midiPlayer;
    
    private void Start()
    {
        cp.enabled = false;
        audioDevice = GetComponent<AudioSource>();
        agent = GetComponent<NavMeshAgent>();
        angry = true;
        agent.speed = 0f;
        interactable = false;
        base.gameObject.name = "BossTime";
        ptmus.enabled = false;
        o.SetActive(value: true);
        audioDevice.spatialBlend = 0f;        
        StartCoroutine(BeginBossFight());
    }

    private void OnEnable()
    {
        if (Camera.main.GetComponent<AnalogGlitch>() != null)
        {
            Camera.main.GetComponent<AnalogGlitch>().enabled = true;
            Camera.main.GetComponent<AnalogGlitch>().colorDrift = 0f;
            Camera.main.GetComponent<AnalogGlitch>().verticalJump = 0f;
            Camera.main.GetComponent<AnalogGlitch>().scanLineJitter = 0f;
        }
        SetAllWindowsToWalkable();
    }

    public void SetAllWindowsToWalkable()
    {
        WindowScript[] array = Object.FindObjectsOfType<WindowScript>();
        for (int i = 0; i < array.Length; i++)
        {
            array[i].enableOffMeshScript = true;
        }
    }

    public void setmusicpitch(float val)
    {
        bossloops.pitch += val;
        endaudio.pitch += val;
    }

    public void LoadNextScene(string name)
    {
        Object.Destroy(Camera.main);
        SceneManager.LoadScene(name);
    }

    public void Hit()
	{
    	if (Health >= 3)
    	{
        	projectileSpawner.SpawnMultipleProjectiles(2);
    	}
    	else if (Health >= 1)
    	{
        	projectileSpawner.SpawnMultipleProjectiles(1);
    	}
    	Health--;
    	minTime -= 0.23181817f;
    	maxTime -= 16f / 45f;
    	redBallonChance *= 1.25f;
    	canGetHit = false;
    	pushingBack = true;
    	stunned = true;
    	chaseSpeed += 1.2f;
    	player.GetComponent<PlayerScript>().walkSpeed += 1f;
    	player.GetComponent<PlayerScript>().runSpeed = player.GetComponent<PlayerScript>().walkSpeed;
    	audioDevice.PlayOneShot(aud_Stunned);
    	if (midiPlayer != null)
    		{
        	// 增加0.15倍速
        		float currentSpeed = midiPlayer.MPTK_Speed;
        		float newSpeed = currentSpeed + 0.05f;
	
        		// 可选：限制最大速度（比如不超过3.0倍速）
        		if (newSpeed > 114514.0f)
        		{
        	    	newSpeed = 114514.0f;
        		}
        
        		midiPlayer.MPTK_Speed = newSpeed;
			}
    }



    public IEnumerator BeginBossFight()
    {
        // 可选：在Boss战开始时重置MIDI速度
        //if (midiPlayer != null)
        //{
        //    midiPlayer.MPTK_Speed = 1.0f; // 重置为正常速度
        //}
        
        bossloopmus.SetActive(value: true);
        canGetHit = false;
        killable = false;
        audioDevice.PlayOneShot(aud_Hit);
        player.GetComponent<PlayerScript>().walkSpeed += 4f;
        player.GetComponent<PlayerScript>().runSpeed = player.GetComponent<PlayerScript>().walkSpeed;
        idle = false;
        pushingBack = true;
        yield return new WaitForSeconds(0.2f);
        pushingBack = false;
        yield return new WaitForSeconds(aud_Hit.length - 0.2f);
        audioDevice.PlayOneShot(aud_Regret);
        yield return new WaitForSeconds(aud_Regret.length);
        chasingDuringBossfight = true;
        killable = true;
        bossfightBegan = true;
        goUp = true;
        audioDevice.PlayOneShot(aud_Ahhhhh);
        yield return new WaitForSeconds(aud_Ahhhhh.length);
        projectileSpawner.SpawnPreProjectiles();
        projectileSpawner.GetComponent<UnknownPriBlocker>().enabled = true;
        if (!gameOver)
        {
            glitchEffectsValue = 0f;
            goUp = false;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "ActualProjectile" && other.GetComponent<UnknownPriProjectile>().thrown)
        {
            Hit();
            stunCooldown = 2f;
            other.GetComponent<UnknownPriProjectile>().SelfDestruct();
        }
    }

    private void FixedUpdate()
    {
        if (glitchEffectsValue > 1.5f)
        {
            glitchEffectsValue = 1.5f;
        }
        if (gameOver)
        {
            if (goUp)
            {
                glitchEffectsValue += 0.1f * Time.deltaTime;
                chaseSpeed = 0f;
                Camera.main.GetComponent<AnalogGlitch>().horizontalShake += 0.07f * Time.unscaledDeltaTime;
            }
        }
        else if (goUp)
        {
            glitchEffectsValue += 0.2f * Time.deltaTime;
        }
        if (pushingBack)
        {
            if (bossfightBegan)
            {
                agent.velocity = base.transform.forward * -20f;
            }
            else
            {
                agent.velocity = base.transform.forward * -75f;
            }
        }
        if (glitchEffectsValue >= 0f && !gameOver)
        {
            glitchEffectsValue -= 0.075f * Time.deltaTime;
        }
        if (playerSighted)
        {
            _ = player;
        }
        look.LookAt(new Vector3(base.transform.position.x, base.transform.position.y, base.transform.position.z));
        Camera.main.GetComponent<AnalogGlitch>().colorDrift = glitchEffectsValue;
        Camera.main.GetComponent<AnalogGlitch>().scanLineJitter = glitchEffectsValue;
        Camera.main.GetComponent<AnalogGlitch>().verticalJump = glitchEffectsValue / 2f;
    }

    private void Update()
    {
        Vector3 direction = player.position - base.transform.position;
        if (Physics.Raycast(base.transform.position, direction, out var hitInfo, float.PositiveInfinity, 769, QueryTriggerInteraction.Ignore) & (hitInfo.transform.tag == "Player"))
        {
            playerSighted = true;
        }
        else
        {
            playerSighted = false;
        }
        if (Time.timeScale != 0f && !audioDevice.isPlaying && (talking & !sequenceFinished))
        {
            spoopBalloonCoolDown = Random.Range(minTime, maxTime);
            killable = true;
            interactable = true;
            agent.speed = 0f;
            talking = false;
            idle = false;
            sequenceFinished = true;
            GetComponent<CapsuleCollider>().radius = 1f;
        }
        if (bossfightBegan)
        {
            if (stunned)
            {
                sprite.SetBool("disappointed", value: true);
                stunCooldown -= Time.deltaTime;
            }
            else
            {
                sprite.SetBool("disappointed", value: false);
            }
            if (chasingDuringBossfight)
            {
                agent.speed = chaseSpeed;
                agent.SetDestination(player.position);
                if (playerSighted && !stunned && !gameOver)
                {
                    spoopBalloonCoolDown -= Time.deltaTime;
                }
            }
        }
        if (sequenceFinished && !angry)
        {
            if (!gameOver)
            {
                agent.speed = 15f;
            }
            agent.SetDestination(player.position);
        }
        if (Health == 0 && angry)
        {
            cuts.SetActive(value: true);
            playerob.SetActive(value: false);
            base.gameObject.SetActive(value: false);
        }
        if ((stunCooldown <= 0f) & stunned)
        {
            stunned = false;
            pushingBack = false;
        }
    }
}