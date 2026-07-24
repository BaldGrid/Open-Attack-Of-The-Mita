using UnityEngine;
using System.Collections.Generic;

[ExecuteInEditMode]
public class SmartTileConnector : MonoBehaviour
{
    [System.Serializable]
    public class TileRule
    {
        public string name;                     // 规则名称
        public int connectionMask;              // 改为int类型（原本是byte）
        public GameObject modelPrefab;          // 对应的模型预设
        public Quaternion rotation = Quaternion.identity; // 旋转
    }
    
    [Header("Tile规则")]
    public List<TileRule> tileRules = new List<TileRule>();
    
    [Header("默认模型")]
    public GameObject defaultModel;
    
    [Header("设置")]
    public float gridSize = 1f;                 // 网格大小
    public LayerMask tileLayerMask;             // Tile所在的层
    
    private GameObject currentModel;
    private Vector3 lastGridPosition;
    
    void Start()
    {
        UpdateTileModel();
    }
    
    void Update()
    {
        #if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            Vector3 currentGridPos = GetGridPosition(transform.position);
            if (currentGridPos != lastGridPosition)
            {
                lastGridPosition = currentGridPos;
                UpdateTileModel();
            }
        }
        #endif
    }
    
    void OnValidate()
    {
        if (!Application.isPlaying)
        {
            UpdateTileModel();
        }
    }
    
    void OnTransformParentChanged()
    {
        UpdateTileModel();
    }
    
    public void UpdateTileModel()
    {
        // 计算连接掩码
        int connectionMask = CalculateConnectionMask();
        
        // 查找匹配的规则
        TileRule matchedRule = FindMatchingRule(connectionMask);
        
        // 更新模型
        UpdateVisual(matchedRule);
        
        // 通知邻居更新
        UpdateNeighbors();
    }
    
    int CalculateConnectionMask()
    {
        int mask = 0;
        
        // 每个方向对应一个bit位
        // 位顺序：上(0)、下(1)、左(2)、右(3)、前(4)、后(5)
        
        // 检查6个主要方向
        if (HasNeighbor(Vector3.up))      mask |= 1 << 0;    // 00000001 (1)
        if (HasNeighbor(Vector3.down))    mask |= 1 << 1;    // 00000010 (2)
        if (HasNeighbor(Vector3.left))    mask |= 1 << 2;    // 00000100 (4)
        if (HasNeighbor(Vector3.right))   mask |= 1 << 3;    // 00001000 (8)
        if (HasNeighbor(Vector3.forward)) mask |= 1 << 4;    // 00010000 (16)
        if (HasNeighbor(Vector3.back))    mask |= 1 << 5;    // 00100000 (32)
        
        return mask;
    }
    
    bool HasNeighbor(Vector3 direction)
    {
        Vector3 checkPos = transform.position + direction * gridSize;
        
        // 使用网格对齐的位置进行检查
        checkPos = GetGridPosition(checkPos);
        
        Collider[] colliders = Physics.OverlapSphere(checkPos, gridSize * 0.1f, tileLayerMask);
        
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject != gameObject && 
                collider.GetComponent<SmartTileConnector>() != null)
            {
                return true;
            }
        }
        
        return false;
    }
    
    TileRule FindMatchingRule(int connectionMask)
    {
        foreach (TileRule rule in tileRules)
        {
            if (rule.connectionMask == connectionMask)
            {
                return rule;
            }
        }
        
        // 如果没有完全匹配的规则，寻找最接近的
        return FindClosestRule(connectionMask);
    }
    
    TileRule FindClosestRule(int connectionMask)
    {
        TileRule closestRule = null;
        int minDifference = int.MaxValue;
        
        foreach (TileRule rule in tileRules)
        {
            // 使用异或计算差异
            int difference = CountBits(connectionMask ^ rule.connectionMask);
            if (difference < minDifference)
            {
                minDifference = difference;
                closestRule = rule;
            }
        }
        
        // 如果没有找到规则，创建一个默认的
        if (closestRule == null)
        {
            closestRule = new TileRule() { 
                name = "Default",
                connectionMask = connectionMask,
                modelPrefab = defaultModel 
            };
        }
        
        return closestRule;
    }
    
    void UpdateVisual(TileRule rule)
    {
        // 销毁旧的模型
        if (currentModel != null)
        {
            #if UNITY_EDITOR
            if (!Application.isPlaying)
                DestroyImmediate(currentModel);
            else
                Destroy(currentModel);
            #endif
        }
        
        // 实例化新模型
        if (rule.modelPrefab != null)
        {
            currentModel = Instantiate(rule.modelPrefab, transform);
            currentModel.transform.localPosition = Vector3.zero;
            currentModel.transform.localRotation = rule.rotation;
            currentModel.transform.localScale = Vector3.one;
            
            // 确保新模型没有自己的碰撞体（避免冲突）
            Collider[] childColliders = currentModel.GetComponentsInChildren<Collider>();
            foreach (Collider collider in childColliders)
            {
                collider.enabled = false;
            }
        }
    }
    
    void UpdateNeighbors()
    {
        // 获取所有相邻的Tile
        Vector3[] directions = new Vector3[]
        {
            Vector3.up, Vector3.down, Vector3.left, 
            Vector3.right, Vector3.forward, Vector3.back
        };
        
        foreach (Vector3 direction in directions)
        {
            Vector3 neighborPos = transform.position + direction * gridSize;
            Collider[] colliders = Physics.OverlapSphere(neighborPos, gridSize * 0.1f, tileLayerMask);
            
            foreach (Collider collider in colliders)
            {
                SmartTileConnector neighborTile = collider.GetComponent<SmartTileConnector>();
                if (neighborTile != null && neighborTile != this)
                {
                    neighborTile.UpdateTileModel();
                }
            }
        }
    }
    
    Vector3 GetGridPosition(Vector3 position)
    {
        return new Vector3(
            Mathf.Round(position.x / gridSize) * gridSize,
            Mathf.Round(position.y / gridSize) * gridSize,
            Mathf.Round(position.z / gridSize) * gridSize
        );
    }
    
    int CountBits(int value)
    {
        int count = 0;
        while (value > 0)
        {
            count += value & 1;
            value >>= 1;
        }
        return count;
    }
    
    [ContextMenu("强制更新所有Tile")]
    void ForceUpdateAllTiles()
    {
        SmartTileConnector[] allTiles = FindObjectsOfType<SmartTileConnector>();
        foreach (SmartTileConnector tile in allTiles)
        {
            tile.UpdateTileModel();
        }
    }
    
    [ContextMenu("显示当前连接掩码")]
    void ShowCurrentMask()
    {
        int mask = CalculateConnectionMask();
        Debug.Log($"{gameObject.name}的连接掩码: {mask} (二进制: {System.Convert.ToString(mask, 2).PadLeft(6, '0')})");
    }
}