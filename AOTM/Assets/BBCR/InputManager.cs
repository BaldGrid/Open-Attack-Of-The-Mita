/*using UnityEngine;
using System.Collections;
using Script MPTKEvent;

// 如果缺少单例管理器，这里提供简化版本
public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }
    
    void MPTKEvent(MPTKEvent midiEvent)
    {
        if (Bm == null) return;

        if (Bm.BossActive && !Bm.holdBeat && midiEvent.Command == MPTKCommand.MetaEvent && midiEvent.Meta == MPTKMeta.TextEvent)
        {          
            if (glitchVal <= 0f) StartCoroutine(UnGlitch());
            glitchVal = 1f;
            Shader.SetGlobalFloat("_VertexGlitchSeed", Random.Range(0f, 1000f));
            Shader.SetGlobalFloat("_VertexGlitchIntensity", glitchVal * 3f);
            Shader.SetGlobalFloat("_TileVertexGlitchSeed", Random.Range(0f, 1000f));
            Shader.SetGlobalFloat("_TileVertexGlitchIntensity", glitchVal * 3f);
        }
    }


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void Rumble(float intensity, float duration)
    {
        // 手柄震动实现（如果有需要）
        // 简化版本：只打印日志
        Debug.Log($"Rumble - Intensity: {intensity}, Duration: {duration}");
    }
    
    public void SetColor(Color color)
    {
        // 设置颜色效果（可能是UI或屏幕颜色）
        Camera.main.backgroundColor = color;
    }
}

public class PlayerFileManager : MonoBehaviour
{
    public static PlayerFileManager Instance { get; private set; }
    public bool reduceFlashing = false; // 是否减少闪烁效果
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}

public class ModManager : MonoBehaviour
{
    public static bool GlitchStyle = false; // 是否启用特殊故障风格
}

public class GlitchEffectController : MonoBehaviour
{
    // 完整的AngerGlitch协程
    public static IEnumerator AngerGlitch(float wait)
    {
        float glitchRate = 0.5f;
        
        // 第一阶段：等待指定时间
        while (wait > 0f)
        {
            wait -= Time.deltaTime;
            yield return null;
        }
        wait = 0f;
        
        // 启用Shader故障效果
        Shader.SetGlobalInt("_ColorGlitching", 1);
        Shader.SetGlobalInt("_SpriteColorGlitching", 1);
        
        // 第二阶段：故障效果持续3秒
        while (wait < 3f)
        {
            wait += Time.deltaTime / (ModManager.GlitchStyle ? 2 : 1);
            
            // 设置顶点故障随机种子
            Shader.SetGlobalFloat("_VertexGlitchSeed", Random.Range(0f, 1000f));
            Shader.SetGlobalFloat("_TileVertexGlitchSeed", Random.Range(0f, 1000f));
            
            // 手柄震动
            if (InputManager.Instance != null)
                InputManager.Instance.Rumble(wait / 6f, 0.05f);
            
            // 根据是否减少闪烁选择不同的故障强度
            if (PlayerFileManager.Instance != null && !PlayerFileManager.Instance.reduceFlashing)
            {
                glitchRate -= Time.unscaledDeltaTime;
                Shader.SetGlobalFloat("_VertexGlitchIntensity", Mathf.Pow(wait, 2f));
                Shader.SetGlobalFloat("_TileVertexGlitchIntensity", Mathf.Pow(wait, 2f));
                Shader.SetGlobalFloat("_ColorGlitchPercent", wait * 0.05f);
                Shader.SetGlobalFloat("_SpriteColorGlitchPercent", wait * 0.05f);
                
                // 定期改变颜色
                if (glitchRate <= 0f)
                {
                    Shader.SetGlobalInt("_ColorGlitchVal", Random.Range(0, 4096));
                    Shader.SetGlobalInt("_SpriteColorGlitchVal", Random.Range(0, 4096));
                    
                    if (InputManager.Instance != null)
                        InputManager.Instance.SetColor(new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f)));
                    
                    glitchRate = 0.55f - wait * 0.1f;
                }
            }
            else
            {
                // 减少闪烁模式：使用更低的强度
                Shader.SetGlobalFloat("_ColorGlitchPercent", wait * 0.25f);
                Shader.SetGlobalFloat("_SpriteColorGlitchPercent", wait * 0.25f);
                Shader.SetGlobalFloat("_VertexGlitchIntensity", wait * 2f);
                Shader.SetGlobalFloat("_TileVertexGlitchIntensity", wait * 2f);
            }
            
            yield return null;
        }
        
        // 第三阶段：恢复正常的Shader设置
        Shader.SetGlobalFloat("_VertexGlitchIntensity", 0f);
        Shader.SetGlobalFloat("_TileVertexGlitchIntensity", 0f);
        Shader.SetGlobalInt("_ColorGlitching", 0);
        Shader.SetGlobalInt("_SpriteColorGlitching", 0);
    }
    
    // 使用方法示例
    public class ExampleUsage : MonoBehaviour
    {
        void Start()
        {
            // 示例1：延迟1秒后开始故障效果
            StartCoroutine(GlitchEffectController.AngerGlitch(1f));
            
            // 示例2：立即开始故障效果
            // StartCoroutine(GlitchEffectController.AngerGlitch(0f));
        }
        
        void Update()
        {
            // 按空格键触发故障效果
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartCoroutine(GlitchEffectController.AngerGlitch(0.5f));
            }
            
            // 按G键切换故障风格
            if (Input.GetKeyDown(KeyCode.G))
            {
                ModManager.GlitchStyle = !ModManager.GlitchStyle;
                Debug.Log($"GlitchStyle: {ModManager.GlitchStyle}");
            }
            
            // 按F键切换减少闪烁模式
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (PlayerFileManager.Instance != null)
                {
                    PlayerFileManager.Instance.reduceFlashing = !PlayerFileManager.Instance.reduceFlashing;
                    Debug.Log($"Reduce Flashing: {PlayerFileManager.Instance.reduceFlashing}");
                }
            }
        }
    }
}

// 如果使用Shader全局属性，需要确保它们在Shader中存在
// 可以在项目初始化时设置默认值
public class ShaderGlobalProperties : MonoBehaviour
{
    void Start()
    {
        // 初始化Shader全局属性
        Shader.SetGlobalInt("_ColorGlitching", 0);
        Shader.SetGlobalInt("_SpriteColorGlitching", 0);
        Shader.SetGlobalFloat("_VertexGlitchIntensity", 0f);
        Shader.SetGlobalFloat("_TileVertexGlitchIntensity", 0f);
        Shader.SetGlobalFloat("_ColorGlitchPercent", 0f);
        Shader.SetGlobalFloat("_SpriteColorGlitchPercent", 0f);
        Shader.SetGlobalInt("_ColorGlitchVal", 0);
        Shader.SetGlobalInt("_SpriteColorGlitchVal", 0);
        Shader.SetGlobalFloat("_VertexGlitchSeed", 0f);
        Shader.SetGlobalFloat("_TileVertexGlitchSeed", 0f);
    }
}
*/