using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

// BBCR Boss核心类
public class BBCRBoss : MonoBehaviour
{
    [Header("Boss基础属性")]
    public string bossName = "BBCR-001";
    public float baseSpeed = 3.5f;
    public float chaseSpeed = 5f;
    public float detectionRange = 15f;
    public float attackRange = 2f;
    public int maxHealth = 1000;
    [SerializeField] private int currentHealth;
    
    [Header("Baldi风格机制")]
    public float angerLevel = 0f;
    public float maxAnger = 100f;
    public float angerDecayRate = 0.5f;
    public float angerIncreasePerHit = 10f;
    
    [Header("数学题系统")]
    public List<MathProblem> mathProblems;
    public Transform questionUIAnchor;
    public GameObject questionUIPrefab;
    public float questionCooldown = 10f;
    private float questionTimer = 0f;
    
    [Header("状态系统")]
    public BossState currentState = BossState.Patrolling;
    public enum BossState
    {
        Patrolling,
        Chasing,
        Attacking,
        Stunned,
        SpecialAttack
    }
    
    [Header("音频系统")]
    public AudioClip chaseMusic;
    public AudioClip attackSound;
    public AudioClip questionSound;
    public AudioClip correctAnswerSound;
    public AudioClip wrongAnswerSound;
    
    // 引用
    private Transform player;
    private UnityEngine.AI.NavMeshAgent agent;
    private Animator animator;
    private AudioSource audioSource;
    private GameManager gameManager;
    
    // 当前问题
    private MathProblem currentProblem;
    private GameObject currentQuestionUI;
    
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        gameManager = FindObjectOfType<GameManager>();
        
        currentHealth = maxHealth;
        agent.speed = baseSpeed;
        
        InitializeMathProblems();
        
        // 开始巡逻
        StartCoroutine(PatrolRoutine());
    }
    
    void Update()
    {
        // 状态机更新
        switch (currentState)
        {
            case BossState.Patrolling:
                UpdatePatrolling();
                break;
            case BossState.Chasing:
                UpdateChasing();
                break;
            case BossState.Attacking:
                UpdateAttacking();
                break;
        }
        
        // 愤怒值衰减
        if (angerLevel > 0)
        {
            angerLevel = Mathf.Max(0, angerLevel - angerDecayRate * Time.deltaTime);
        }
        
        // 问题冷却
        if (questionTimer > 0)
        {
            questionTimer -= Time.deltaTime;
        }
    }
    
    #region 状态机方法
    
    void UpdatePatrolling()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        
        // 检测到玩家
        if (distanceToPlayer <= detectionRange)
        {
            currentState = BossState.Chasing;
            agent.speed = chaseSpeed;
            animator.SetBool("IsChasing", true);
            
            // 播放追逐音乐
            if (chaseMusic != null)
            {
                audioSource.clip = chaseMusic;
                audioSource.loop = true;
                audioSource.Play();
            }
            
            // 随机出题
            if (questionTimer <= 0 && Random.value > 0.7f)
            {
                AskRandomQuestion();
            }
        }
    }
    
    void UpdateChasing()
    {
        // 追逐玩家
        agent.SetDestination(player.position);
        
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        
        // 进入攻击范围
        if (distanceToPlayer <= attackRange)
        {
            currentState = BossState.Attacking;
            StartCoroutine(AttackRoutine());
        }
        
        // 超出检测范围
        if (distanceToPlayer > detectionRange * 1.5f)
        {
            currentState = BossState.Patrolling;
            agent.speed = baseSpeed;
            animator.SetBool("IsChasing", false);
            audioSource.Stop();
        }
        
        // 出题机会
        if (questionTimer <= 0 && Random.value > 0.8f)
        {
            AskRandomQuestion();
        }
    }
    
    void UpdateAttacking()
    {
        // 攻击状态由协程处理
    }
    
    IEnumerator PatrolRoutine()
    {
        while (currentState == BossState.Patrolling)
        {
            // 随机巡逻点
            Vector3 randomDirection = Random.insideUnitSphere * 20f;
            randomDirection += transform.position;
            
            UnityEngine.AI.NavMeshHit hit;
            if (UnityEngine.AI.NavMesh.SamplePosition(randomDirection, out hit, 20f, UnityEngine.AI.NavMesh.AllAreas))
            {
                agent.SetDestination(hit.position);
            }
            
            yield return new WaitForSeconds(Random.Range(5f, 10f));
        }
    }
    
    IEnumerator AttackRoutine()
    {
        animator.SetTrigger("Attack");
        
        // 播放攻击音效
        if (attackSound != null)
        {
            audioSource.PlayOneShot(attackSound);
        }
        
        // 检查是否击中玩家
        yield return new WaitForSeconds(0.3f);
        
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= attackRange * 1.2f)
        {
            // 玩家受到伤害
            PlayerController playerController = player.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.TakeDamage(20);
            }
            
            // 增加愤怒值
            angerLevel += angerIncreasePerHit;
        }
        
        yield return new WaitForSeconds(0.7f);
        
        // 返回追逐状态
        currentState = BossState.Chasing;
    }
    
    #endregion
    
    #region 数学题系统
    
    void InitializeMathProblems()
    {
        mathProblems = new List<MathProblem>();
        
        // 基础数学题
        mathProblems.Add(new MathProblem("5 + 3 = ?", "8", new string[] {"7", "9", "10"}, 1));
        mathProblems.Add(new MathProblem("12 - 4 = ?", "8", new string[] {"6", "7", "9"}, 2));
        mathProblems.Add(new MathProblem("6 × 2 = ?", "12", new string[] {"10", "11", "13"}, 3));
        mathProblems.Add(new MathProblem("15 ÷ 3 = ?", "5", new string[] {"4", "6", "7"}, 2));
        
        // 更难的题目
        mathProblems.Add(new MathProblem("√16 = ?", "4", new string[] {"2", "3", "5"}, 5));
        mathProblems.Add(new MathProblem("2² + 3² = ?", "13", new string[] {"10", "12", "14"}, 4));
    }
    
    void AskRandomQuestion()
    {
        if (mathProblems.Count == 0 || currentQuestionUI != null) return;
        
        questionTimer = questionCooldown;
        
        // 选择题目（根据愤怒值调整难度）
        int problemIndex;
        if (angerLevel > 70)
            problemIndex = Random.Range(mathProblems.Count - 2, mathProblems.Count);
        else if (angerLevel > 40)
            problemIndex = Random.Range(mathProblems.Count / 2, mathProblems.Count);
        else
            problemIndex = Random.Range(0, mathProblems.Count / 2);
        
        currentProblem = mathProblems[problemIndex];
        
        // 创建UI
        if (questionUIPrefab != null && questionUIAnchor != null)
        {
            currentQuestionUI = Instantiate(questionUIPrefab, questionUIAnchor.position, 
                                           Quaternion.identity, questionUIAnchor);
            
            QuestionUI uiScript = currentQuestionUI.GetComponent<QuestionUI>();
            if (uiScript != null)
            {
                uiScript.SetupQuestion(currentProblem, this);
            }
        }
        
        // 播放问题音效
        if (questionSound != null)
        {
            audioSource.PlayOneShot(questionSound);
        }
        
        // 强制玩家回答问题
        PlayerController playerController = player.GetComponent<PlayerController>();
        if (playerController != null)
        {
            playerController.SetAnswerMode(true, currentProblem.timeLimit);
        }
    }
    
    public void ReceiveAnswer(string answer)
    {
        if (currentProblem == null) return;
        
        bool isCorrect = (answer == currentProblem.correctAnswer);
        
        if (isCorrect)
        {
            // 回答正确：击晕Boss
            StartCoroutine(StunBoss(3f));
            angerLevel = Mathf.Max(0, angerLevel - 30);
            
            if (correctAnswerSound != null)
            {
                audioSource.PlayOneShot(correctAnswerSound);
            }
            
            // 给予玩家奖励
            PlayerController playerController = player.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.AddNotebooks(1);
            }
        }
        else
        {
            // 回答错误：Boss增强
            angerLevel += 20;
            agent.speed += 0.5f;
            chaseSpeed += 0.3f;
            
            if (wrongAnswerSound != null)
            {
                audioSource.PlayOneShot(wrongAnswerSound);
            }
        }
        
        // 清理问题UI
        if (currentQuestionUI != null)
        {
            Destroy(currentQuestionUI);
            currentQuestionUI = null;
        }
        
        currentProblem = null;
        
        // 更新玩家状态
        PlayerController pc = player.GetComponent<PlayerController>();
        if (pc != null)
        {
            pc.SetAnswerMode(false, 0);
        }
    }
    
    IEnumerator StunBoss(float duration)
    {
        BossState previousState = currentState;
        currentState = BossState.Stunned;
        
        agent.isStopped = true;
        animator.SetBool("IsStunned", true);
        
        yield return new WaitForSeconds(duration);
        
        agent.isStopped = false;
        animator.SetBool("IsStunned", false);
        
        currentState = previousState;
    }
    
    #endregion
    
    #region 战斗系统
    
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        angerLevel += angerIncreasePerHit;
        
        // 血量变化事件
        if (gameManager != null)
        {
            gameManager.UpdateBossHealth(currentHealth, maxHealth);
        }
        
        // 检查阶段转换
        CheckPhaseTransition();
        
        // 受伤动画
        animator.SetTrigger("TakeDamage");
        
        // 死亡检查
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    
    void CheckPhaseTransition()
    {
        float healthPercent = (float)currentHealth / maxHealth;
        
        if (healthPercent <= 0.3f && currentState != BossState.SpecialAttack)
        {
            // 进入狂暴阶段
            StartCoroutine(SpecialAttackPhase());
        }
        else if (healthPercent <= 0.6f && agent.speed < 7f)
        {
            // 进入第二阶段
            chaseSpeed = 6f;
            questionCooldown = 7f;
            
            // 添加更难的题目
            mathProblems.Add(new MathProblem("∫x² dx (从0到2)", "8/3", new string[] {"2", "3", "4"}, 8));
        }
    }
    
    IEnumerator SpecialAttackPhase()
    {
        currentState = BossState.SpecialAttack;
        
        // 停止移动
        agent.isStopped = true;
        
        // 播放特殊攻击动画
        animator.SetTrigger("SpecialAttack");
        
        // 蓄力效果
        yield return new WaitForSeconds(2f);
        
        // 释放全屏攻击
        GameObject[] attackZones = GameObject.FindGameObjectsWithTag("AttackZone");
        foreach (GameObject zone in attackZones)
        {
            zone.GetComponent<AttackZone>().Activate();
        }
        
        yield return new WaitForSeconds(1f);
        
        // 返回追逐状态
        currentState = BossState.Chasing;
        agent.isStopped = false;
        agent.speed = chaseSpeed * 1.5f; // 狂暴加速
    }
    
    void Die()
    {
        currentState = BossState.Stunned;
        agent.isStopped = true;
        
        // 死亡动画
        animator.SetTrigger("Die");
        
        // 停止所有声音
        audioSource.Stop();
        
        // 游戏胜利
        if (gameManager != null)
        {
            StartCoroutine(gameManager.BossDefeated());
        }
        
        // 销毁物体
        Destroy(gameObject, 5f);
    }
    
    #endregion
    
    #region 工具方法
    
    void OnDrawGizmosSelected()
    {
        // 绘制检测范围
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        
        // 绘制攻击范围
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
    
    #endregion
}

// 数学题数据结构
[System.Serializable]
public class MathProblem
{
    public string question;
    public string correctAnswer;
    public string[] wrongAnswers;
    public float timeLimit;
    
    public MathProblem(string q, string correct, string[] wrong, float time = 10f)
    {
        question = q;
        correctAnswer = correct;
        wrongAnswers = wrong;
        timeLimit = time;
    }
}

// 问题UI控制器
public class QuestionUI : MonoBehaviour
{
    public TextMeshProUGUI questionText;
    public TextMeshProUGUI[] answerButtons;
    
    private MathProblem currentProblem;
    private BBCRBoss boss;
    
    public void SetupQuestion(MathProblem problem, BBCRBoss callingBoss)
    {
        currentProblem = problem;
        boss = callingBoss;
        
        questionText.text = problem.question;
        
        // 混合正确答案和错误答案
        List<string> allAnswers = new List<string>();
        allAnswers.Add(problem.correctAnswer);
        allAnswers.AddRange(problem.wrongAnswers);
        
        // 随机排序
        for (int i = 0; i < allAnswers.Count; i++)
        {
            int randomIndex = Random.Range(i, allAnswers.Count);
            string temp = allAnswers[i];
            allAnswers[i] = allAnswers[randomIndex];
            allAnswers[randomIndex] = temp;
        }
        
        // 分配给按钮
        for (int i = 0; i < answerButtons.Length && i < allAnswers.Count; i++)
        {
            answerButtons[i].text = allAnswers[i];
        }
    }
    
    public void OnAnswerSelected(int buttonIndex)
    {
        if (boss != null && buttonIndex < answerButtons.Length)
        {
            string selectedAnswer = answerButtons[buttonIndex].text;
            boss.ReceiveAnswer(selectedAnswer);
        }
        
        Destroy(gameObject);
    }
}

// 玩家控制器（简版）
public class PlayerController : MonoBehaviour
{
    [Header("玩家属性")]
    public int maxHealth = 100;
    public int currentHealth;
    public int notebooks = 0;
    
    [Header("答题模式")]
    public bool inAnswerMode = false;
    public float answerTimeRemaining = 0f;
    
    private CharacterController controller;
    private GameManager gameManager;
    
    void Start()
    {
        controller = GetComponent<CharacterController>();
        gameManager = FindObjectOfType<GameManager>();
        currentHealth = maxHealth;
    }
    
    void Update()
    {
        if (!inAnswerMode)
        {
            // 正常移动逻辑
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            Vector3 move = new Vector3(horizontal, 0, vertical);
            controller.Move(move * 5f * Time.deltaTime);
        }
        else
        {
            // 答题模式倒计时
            answerTimeRemaining -= Time.deltaTime;
            if (answerTimeRemaining <= 0)
            {
                SetAnswerMode(false, 0);
                // 超时视为错误答案
                FindObjectOfType<BBCRBoss>().ReceiveAnswer("");
            }
        }
    }
    
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        
        if (gameManager != null)
        {
            gameManager.UpdatePlayerHealth(currentHealth, maxHealth);
        }
        
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    
    public void SetAnswerMode(bool active, float timeLimit)
    {
        inAnswerMode = active;
        answerTimeRemaining = timeLimit;
        
        // 可以在这里添加UI更新
    }
    
    public void AddNotebooks(int amount)
    {
        notebooks += amount;
        
        if (gameManager != null)
        {
            gameManager.UpdateNotebookCount(notebooks);
        }
    }
    
    void Die()
    {
        if (gameManager != null)
        {
            gameManager.GameOver();
        }
    }
}

// 游戏管理器
public class GameManager : MonoBehaviour
{
    public UnityEngine.UI.Slider bossHealthSlider;
    public UnityEngine.UI.Slider playerHealthSlider;
    public TextMeshProUGUI notebookText;
    public GameObject gameOverUI;
    public GameObject victoryUI;
    
    void Start()
    {
        if (gameOverUI != null) gameOverUI.SetActive(false);
        if (victoryUI != null) victoryUI.SetActive(false);
    }
    
    public void UpdateBossHealth(int current, int max)
    {
        if (bossHealthSlider != null)
        {
            bossHealthSlider.maxValue = max;
            bossHealthSlider.value = current;
        }
    }
    
    public void UpdatePlayerHealth(int current, int max)
    {
        if (playerHealthSlider != null)
        {
            playerHealthSlider.maxValue = max;
            playerHealthSlider.value = current;
        }
    }
    
    public void UpdateNotebookCount(int count)
    {
        if (notebookText != null)
        {
            notebookText.text = $"笔记本: {count}/7";
        }
    }
    
    public IEnumerator BossDefeated()
    {
        yield return new WaitForSeconds(2f);
        
        if (victoryUI != null)
        {
            victoryUI.SetActive(true);
        }
    }
    
    public void GameOver()
    {
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(true);
        }
    }
}

// 攻击区域（用于特殊攻击）
public class AttackZone : MonoBehaviour
{
    public ParticleSystem attackEffect;
    public float damage = 30f;
    
    public void Activate()
    {
        if (attackEffect != null)
        {
            attackEffect.Play();
        }
        
        // 检测玩家是否在区域内
        Collider[] hits = Physics.OverlapSphere(transform.position, 5f);
        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                PlayerController player = hit.GetComponent<PlayerController>();
                if (player != null)
                {
                    player.TakeDamage((int)damage);
                }
            }
        }
    }
}