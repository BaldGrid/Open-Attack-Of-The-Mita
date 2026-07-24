using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShakeEffect : MonoBehaviour
{
    [Header("物体筛选设置")]
    [SerializeField] private string[] nameFilters = new string[]
    {
        "Wall", "Floor", "Ceiling", "TileFloorEn", 
        "Carpet", "_Front", "_Side", "TileFloor"
    };
    
    [Header("抖动效果设置")]
    [SerializeField] private float shakeInterval = 1f;
    [SerializeField] private float shakeAngle = 8f;
    [SerializeField] private float shakeDuration = 0.2f;
    [SerializeField] private bool enableColorChange = true;
    [SerializeField] private float colorTransitionSpeed = 1f;
    
    [Header("消失效果设置")]
    [SerializeField] private Vector2 disappearDelayRange = new Vector2(2f, 5f);
    [SerializeField] private float disappearDuration = 1f;
    [SerializeField] private bool enableDisappearColor = true;
    
    [Header("调试设置")]
    [SerializeField] private bool enableShake = true;
    [SerializeField] private bool enableDisappear = true;
    [SerializeField] private int maxSimultaneousEffects = 10;

    private MeshRenderer[] sceneMeshRenderers;
    private Dictionary<Mesh, Vector3[]> originalVerticesCache = new Dictionary<Mesh, Vector3[]>();
    private Dictionary<Mesh, Coroutine> activeShakeCoroutines = new Dictionary<Mesh, Coroutine>();
    private Dictionary<MeshRenderer, Color> originalColorsCache = new Dictionary<MeshRenderer, Color>();
    private Dictionary<MeshRenderer, Coroutine> activeDisappearCoroutines = new Dictionary<MeshRenderer, Coroutine>();
    
    private int currentActiveEffects = 0;

    void Start()
    {
        InitializeMeshRenderers();
        
        if (enableShake)
        {
            StartCoroutine(NullBossShakeSchool());
        }
        
        if (enableDisappear)
        {
            StartCoroutine(RandomWallDisappear());
        }
    }

    void InitializeMeshRenderers()
    {
        MeshRenderer[] allRenderers = FindObjectsOfType<MeshRenderer>();
        List<MeshRenderer> validRenderers = new List<MeshRenderer>();

        foreach (MeshRenderer renderer in allRenderers)
        {
            bool isValid = false;
            string objName = renderer.gameObject.name;
            
            foreach (string filter in nameFilters)
            {
                if (objName.StartsWith(filter))
                {
                    isValid = true;
                    break;
                }
            }
            
            if (isValid)
            {
                validRenderers.Add(renderer);

                MeshFilter filterComponent = renderer.GetComponent<MeshFilter>();
                if (filterComponent != null && filterComponent.mesh != null)
                {
                    Mesh mesh = filterComponent.mesh;
                    if (!originalVerticesCache.ContainsKey(mesh))
                    {
                        originalVerticesCache[mesh] = mesh.vertices.Clone() as Vector3[];
                    }
                }

                if (!originalColorsCache.ContainsKey(renderer))
                {
                    originalColorsCache[renderer] = renderer.material.color;
                }
            }
        }

        sceneMeshRenderers = validRenderers.ToArray();
        Debug.Log($"初始化完成: 找到 {sceneMeshRenderers.Length} 个有效渲染器");
    }

    IEnumerator NullBossShakeSchool()
    {
        while (true)
        {
            yield return new WaitForSeconds(shakeInterval);

            if (currentActiveEffects >= maxSimultaneousEffects)
                continue;

            foreach (MeshRenderer meshRenderer in sceneMeshRenderers)
            {
                if (currentActiveEffects >= maxSimultaneousEffects)
                    break;
                    
                MeshFilter filter = meshRenderer.GetComponent<MeshFilter>();
                if (filter != null && filter.mesh != null)
                {
                    StartShakeEffect(filter.mesh, meshRenderer);
                }
            }
        }
    }

    IEnumerator RandomWallDisappear()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(disappearDelayRange.x, disappearDelayRange.y));

            if (sceneMeshRenderers.Length > 0 && 
                currentActiveEffects < maxSimultaneousEffects &&
                !activeDisappearCoroutines.ContainsValue(null))
            {
                int randomIndex = Random.Range(0, sceneMeshRenderers.Length);
                MeshRenderer randomRenderer = sceneMeshRenderers[randomIndex];
                
                if (randomRenderer != null && 
                    randomRenderer.enabled && 
                    !activeDisappearCoroutines.ContainsKey(randomRenderer))
                {
                    currentActiveEffects++;
                    Coroutine disappearCoroutine = StartCoroutine(DisappearAndAppear(randomRenderer));
                    activeDisappearCoroutines[randomRenderer] = disappearCoroutine;
                }
            }
        }
    }

    IEnumerator DisappearAndAppear(MeshRenderer meshRenderer)
    {
        Color originalColor = originalColorsCache.ContainsKey(meshRenderer) ? 
            originalColorsCache[meshRenderer] : meshRenderer.material.color;
        bool wasEnabled = meshRenderer.enabled;

        meshRenderer.enabled = false;

        yield return new WaitForSeconds(disappearDuration);

        meshRenderer.enabled = true;
        
        if (enableDisappearColor)
        {
            meshRenderer.material.color = originalColor;
        }

        if (activeDisappearCoroutines.ContainsKey(meshRenderer))
        {
            activeDisappearCoroutines.Remove(meshRenderer);
        }
        
        currentActiveEffects--;
    }

    void StartShakeEffect(Mesh mesh, MeshRenderer meshRenderer)
    {
        if (activeShakeCoroutines.ContainsKey(mesh))
        {
            StopCoroutine(activeShakeCoroutines[mesh]);
            activeShakeCoroutines.Remove(mesh);

            if (originalVerticesCache.ContainsKey(mesh))
            {
                mesh.vertices = originalVerticesCache[mesh];
                mesh.RecalculateNormals();
                mesh.RecalculateBounds();
            }

            if (enableColorChange && originalColorsCache.ContainsKey(meshRenderer))
            {
                meshRenderer.material.color = originalColorsCache[meshRenderer];
            }
            
            currentActiveEffects--;
        }

        currentActiveEffects++;
        Coroutine shakeCoroutine = StartCoroutine(ShakeWall(mesh, meshRenderer));
        activeShakeCoroutines[mesh] = shakeCoroutine;
    }

    IEnumerator ShakeWall(Mesh mesh, MeshRenderer meshRenderer)
    {
        if (!originalVerticesCache.ContainsKey(mesh))
        {
            currentActiveEffects--;
            yield break;
        }

        Vector3[] originalVertices = originalVerticesCache[mesh];
        Vector3[] distortedVertices = new Vector3[originalVertices.Length];

        Quaternion targetRotation = Quaternion.Euler(
            Random.Range(-shakeAngle, shakeAngle),
            Random.Range(-shakeAngle, shakeAngle),
            Random.Range(-shakeAngle, shakeAngle)
        );

        for (int i = 0; i < originalVertices.Length; i++)
        {
            distortedVertices[i] = targetRotation * originalVertices[i];
        }

        mesh.vertices = distortedVertices;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        Color originalColor = originalColorsCache.ContainsKey(meshRenderer) ? 
            originalColorsCache[meshRenderer] : meshRenderer.material.color;
        Color randomColor = new Color(Random.value, Random.value, Random.value);

        float elapsedTime = 0f;
        float colorPhase = 0f;

        while (elapsedTime <= shakeDuration)
        {
            float t = elapsedTime / shakeDuration;

            for (int i = 0; i < originalVertices.Length; i++)
            {
                distortedVertices[i] = Vector3.Lerp(distortedVertices[i], originalVertices[i], t);
            }

            mesh.vertices = distortedVertices;
            
            if (enableColorChange)
            {
                colorPhase += Time.deltaTime * colorTransitionSpeed;
                float colorT = Mathf.PingPong(colorPhase, 1f);
                meshRenderer.material.color = Color.Lerp(originalColor, randomColor, colorT);
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        mesh.vertices = originalVertices;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        
        if (enableColorChange)
        {
            meshRenderer.material.color = originalColor;
        }

        if (activeShakeCoroutines.ContainsKey(mesh))
        {
            activeShakeCoroutines.Remove(mesh);
        }
        
        currentActiveEffects--;
    }

    void OnDestroy()
    {
        foreach (var mesh in originalVerticesCache.Keys)
        {
            if (mesh != null)
            {
                mesh.vertices = originalVerticesCache[mesh];
                mesh.RecalculateNormals();
                mesh.RecalculateBounds();
            }
        }

        if (enableColorChange || enableDisappearColor)
        {
            foreach (var renderer in originalColorsCache.Keys)
            {
                if (renderer != null)
                {
                    renderer.material.color = originalColorsCache[renderer];
                    renderer.enabled = true;
                }
            }
        }

        foreach (var coroutine in activeDisappearCoroutines.Values)
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }
        }
        
        StopAllCoroutines();
    }

    public void StartShake()
    {
        StopAllEffects();
        enableShake = true;
        enableDisappear = true;
        
        if (enableShake)
        {
            StartCoroutine(NullBossShakeSchool());
        }
        
        if (enableDisappear)
        {
            StartCoroutine(RandomWallDisappear());
        }
    }

    public void StopShake()
    {
        StopAllEffects();
    }
    
    public void StopAllEffects()
    {
        StopAllCoroutines();
        enableShake = false;
        enableDisappear = false;

        foreach (var mesh in originalVerticesCache.Keys)
        {
            if (mesh != null)
            {
                mesh.vertices = originalVerticesCache[mesh];
                mesh.RecalculateNormals();
                mesh.RecalculateBounds();
            }
        }

        if (enableColorChange || enableDisappearColor)
        {
            foreach (var renderer in originalColorsCache.Keys)
            {
                if (renderer != null)
                {
                    renderer.material.color = originalColorsCache[renderer];
                    renderer.enabled = true;
                }
            }
        }

        activeDisappearCoroutines.Clear();
        activeShakeCoroutines.Clear();
        currentActiveEffects = 0;
    }
    
    public void SetShakeParameters(float interval, float angle, float duration, bool useColor, float colorSpeed)
    {
        shakeInterval = interval;
        shakeAngle = angle;
        shakeDuration = duration;
        enableColorChange = useColor;
        colorTransitionSpeed = colorSpeed;
    }
    
    public void SetDisappearParameters(Vector2 delayRange, float duration, bool useColor)
    {
        disappearDelayRange = delayRange;
        disappearDuration = duration;
        enableDisappearColor = useColor;
    }
    
    public void SetNameFilters(string[] filters)
    {
        nameFilters = filters;
        StopAllEffects();
        InitializeMeshRenderers();
    }
    
    public void ToggleColorChange(bool enable)
    {
        enableColorChange = enable;
        enableDisappearColor = enable;
    }
    
    public void EnableColorChange(bool enable)
    {
        enableColorChange = enable;
    }
    
    public void EnableDisappearColor(bool enable)
    {
        enableDisappearColor = enable;
    }
}