using UnityEngine;
using TMPro;
using System.Collections;
using System.Linq;

public class TextScrambler : MonoBehaviour
{
    [Header("Scramble Settings")]
    [Tooltip("Enable or disable the scrambling effect")]
    public bool startOnEnable = true;
    
    [Tooltip("Time interval between each scramble (in seconds)")]
    public float scrambleInterval = 0.5f;
    
    [Tooltip("Preserve original spacing and line breaks")]
    public bool preserveFormatting = true;
    
    [Header("Visual Options")]
    [Tooltip("Randomize scramble interval for more natural effect")]
    public bool randomizeInterval = false;
    
    [Tooltip("Minimum interval when randomizing")]
    public float minInterval = 0.3f;
    
    [Tooltip("Maximum interval when randomizing")]
    public float maxInterval = 0.8f;

    private TMP_Text textComponent;
    private string originalText;
    private Coroutine scrambleCoroutine;
    private bool isScrambling = false;
    private System.Random random;

    void Awake()
    {
        random = new System.Random();
    }

    void Start()
    {
        // Get the TextMeshPro component
        textComponent = GetComponent<TMP_Text>();
        
        if (textComponent == null)
        {
            Debug.LogError("TextScrambler requires a TextMeshPro component on the same GameObject!");
            enabled = false;
            return;
        }
        
        // Store the original text
        originalText = textComponent.text;
        
        if (startOnEnable)
        {
            StartInfiniteScrambling();
        }
    }

    /// <summary>
    /// Starts infinite text scrambling effect
    /// </summary>
    public void StartInfiniteScrambling()
    {
        if (isScrambling) return;
        
        StopInfiniteScrambling(); // Ensure no existing coroutine
        
        scrambleCoroutine = StartCoroutine(InfiniteScrambleRoutine());
        isScrambling = true;
    }

    /// <summary>
    /// Stops the infinite scrambling effect
    /// </summary>
    public void StopInfiniteScrambling()
    {
        if (scrambleCoroutine != null)
        {
            StopCoroutine(scrambleCoroutine);
            scrambleCoroutine = null;
        }
        
        isScrambling = false;
    }

    /// <summary>
    /// Coroutine that handles infinite scrambling animation
    /// </summary>
    private IEnumerator InfiniteScrambleRoutine()
    {
        isScrambling = true;
        
        while (true)
        {
            // Scramble the text
            string scrambledText = ScrambleText(originalText);
            textComponent.text = scrambledText;
            
            // Calculate wait time
            float waitTime = scrambleInterval;
            if (randomizeInterval)
            {
                waitTime = Random.Range(minInterval, maxInterval);
            }
            
            // Wait for the specified interval
            yield return new WaitForSeconds(waitTime);
        }
    }

    /// <summary>
    /// Randomly scrambles the characters in the input text
    /// </summary>
    private string ScrambleText(string input)
    {
        if (string.IsNullOrEmpty(input) || input.Length <= 1)
            return input;

        if (preserveFormatting)
        {
            // Split by lines if we want to preserve formatting
            string[] lines = input.Split('\n');
            string[] scrambledLines = new string[lines.Length];
            
            for (int i = 0; i < lines.Length; i++)
            {
                scrambledLines[i] = ScrambleSingleLine(lines[i]);
            }
            
            return string.Join("\n", scrambledLines);
        }
        else
        {
            return ScrambleSingleLine(input);
        }
    }

    /// <summary>
    /// Scrambles a single line of text using Fisher-Yates shuffle
    /// </summary>
    private string ScrambleSingleLine(string line)
    {
        if (string.IsNullOrEmpty(line) || line.Length <= 1)
            return line;

        // Convert to char array for manipulation
        char[] characters = line.ToCharArray();
        
        // Fisher-Yates shuffle algorithm
        int n = characters.Length;
        
        for (int i = n - 1; i > 0; i--)
        {
            int j = random.Next(i + 1);
            // Swap characters
            char temp = characters[i];
            characters[i] = characters[j];
            characters[j] = temp;
        }
        
        return new string(characters);
    }

    /// <summary>
    /// Scramble the text once immediately
    /// </summary>
    public void ScrambleOnce()
    {
        if (textComponent != null && !string.IsNullOrEmpty(originalText))
        {
            string scrambledText = ScrambleText(originalText);
            textComponent.text = scrambledText;
        }
    }

    /// <summary>
    /// Reset to original text
    /// </summary>
    public void ResetToOriginal()
    {
        if (textComponent != null && !string.IsNullOrEmpty(originalText))
        {
            textComponent.text = originalText;
        }
    }

    /// <summary>
    /// Change the text and update the original reference
    /// </summary>
    public void SetNewText(string newText)
    {
        originalText = newText;
        
        // If currently scrambling, update immediately
        if (isScrambling)
        {
            ScrambleOnce();
        }
        else
        {
            ResetToOriginal();
        }
    }

    /// <summary>
    /// Toggle scrambling on/off
    /// </summary>
    public void ToggleScrambling()
    {
        if (isScrambling)
        {
            StopInfiniteScrambling();
            ResetToOriginal();
        }
        else
        {
            StartInfiniteScrambling();
        }
    }

    /// <summary>
    /// Get current scrambling state
    /// </summary>
    public bool IsScrambling()
    {
        return isScrambling;
    }

    void OnEnable()
    {
        if (startOnEnable && !isScrambling)
        {
            StartInfiniteScrambling();
        }
    }

    void OnDisable()
    {
        StopInfiniteScrambling();
    }

    void OnDestroy()
    {
        StopInfiniteScrambling();
    }
}