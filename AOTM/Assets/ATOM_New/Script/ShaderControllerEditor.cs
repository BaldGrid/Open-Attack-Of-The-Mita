using UnityEngine;
using System.Collections.Generic;

public class GlobalShaderController : MonoBehaviour
{
    [Header("全局参数")]
    [Range(0, 50)] public float globalSpeed = 20f;
    [Range(0, 10)] public float globalAmplitude = 1f;
    [Range(0, 5)] public float globalHeight = 1f;
    
    [Header("控制选项")]
    public bool updateInRealtime = true;
    public bool affectAllSceneMaterials = true;
    public List<string> targetShaderNames = new List<string>() { "Vertex Modifier" };
    
    // 存储所有要修改的材质
    private List<Material> allMaterials = new List<Material>();
    private Dictionary<Material, Renderer> materialToRendererMap = new Dictionary<Material, Renderer>();
    
    void Start()
    {
        FindAndCollectMaterials();
    }
    
    void Update()
    {
        if (updateInRealtime)
        {
            UpdateAllMaterials();
        }
    }
    
    // 查找并收集所有使用目标着色器的材质
    public void FindAndCollectMaterials()
    {
        allMaterials.Clear();
        materialToRendererMap.Clear();
        
        // 查找场景中所有的渲染器
        Renderer[] allRenderers = FindObjectsOfType<Renderer>(true); // true表示包含隐藏对象
        
        foreach (Renderer renderer in allRenderers)
        {
            foreach (Material material in renderer.materials)
            {
                // 检查材质使用的着色器是否在目标列表中
                if (material.shader != null && 
                    targetShaderNames.Contains(material.shader.name))
                {
                    if (!allMaterials.Contains(material))
                    {
                        allMaterials.Add(material);
                        materialToRendererMap[material] = renderer;
                    }
                }
            }
        }
        
        Debug.Log($"找到 {allMaterials.Count} 个目标材质");
    }
    
    // 更新所有材质参数
    public void UpdateAllMaterials()
    {
        foreach (Material material in allMaterials)
        {
            if (material == null) continue;
            
            material.SetFloat("_Speed", globalSpeed);
            material.SetFloat("_Amnt", globalAmplitude);
            material.SetFloat("_Amount", globalHeight);
        }
    }
    
    // 批量设置参数
    public void SetAllParameters(float speed, float amplitude, float height)
    {
        globalSpeed = speed;
        globalAmplitude = amplitude;
        globalHeight = height;
        
        foreach (Material material in allMaterials)
        {
            if (material == null) continue;
            
            material.SetFloat("_Speed", speed);
            material.SetFloat("_Amnt", amplitude);
            material.SetFloat("_Amount", height);
        }
    }
    
    // 为特定物体设置不同参数
    public void SetParametersForGameObject(GameObject target, float speed, float amplitude, float height)
    {
        Renderer renderer = target.GetComponent<Renderer>();
        if (renderer != null)
        {
            foreach (Material material in renderer.materials)
            {
                material.SetFloat("_Speed", speed);
                material.SetFloat("_Amnt", amplitude);
                material.SetFloat("_Amount", height);
            }
        }
    }
    
    // 重置所有材质
    public void ResetAllMaterials()
    {
        globalSpeed = 20f;
        globalAmplitude = 1f;
        globalHeight = 1f;
        
        UpdateAllMaterials();
    }
    
    // 启用/禁用所有效果
    public void ToggleEffect(bool enabled)
    {
        float amplitude = enabled ? globalAmplitude : 0f;
        
        foreach (Material material in allMaterials)
        {
            if (material == null) continue;
            material.SetFloat("_Amnt", amplitude);
        }
    }
    
    // 在编辑器中刷新材质列表
    [ContextMenu("刷新材质列表")]
    void RefreshMaterials()
    {
        FindAndCollectMaterials();
    }
    
    void OnDestroy()
    {
        // 清理材质实例
        if (Application.isPlaying)
        {
            foreach (var kvp in materialToRendererMap)
            {
                if (kvp.Value != null)
                {
                    Destroy(kvp.Key);
                }
            }
        }
    }
}