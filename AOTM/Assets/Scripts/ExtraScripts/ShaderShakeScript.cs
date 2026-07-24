using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderShakeScript : MonoBehaviour
{
    [Header("Settings")]
    public float Speed = 2f;
    public float Intensity = 0.2f;
    public float WaitTime = 0.5f;
    public bool Shaking;
    public bool ScreenGlitch;
    public bool NULLVertexGameOver;
    public float Brightness = 1;

    private float glitchIntensity, ScreenGlitchIntensity, Intensity2;
    private float GlitchGameoverIntensity = 0;

    private readonly List<Material> targetMaterials = new List<Material>();

    void Awake()
    {
        CacheMaterialsByShaderProperties();
    }

    void Start()
    {
        Intensity2 = Random.Range(-Intensity, Intensity);
        StartCoroutine(ShakeTime());
    }

    void CacheMaterialsByShaderProperties()
    {
        targetMaterials.Clear();
        Material[] allMaterials = Resources.FindObjectsOfTypeAll<Material>();

        foreach (Material mat in allMaterials)
        {
            // Skip internal garbage
            if (mat == null) continue;
            if (mat.hideFlags != HideFlags.None) continue;
            if (mat.name.Contains("(Instance)")) continue;
            if (mat.shader == null) continue;

            bool hasVertex = mat.HasProperty("_VertexGlitchIntensity");
            bool hasScreen = mat.HasProperty("_ScreenGlitchIntensity");

            if (hasVertex || hasScreen)
            {
                if (!targetMaterials.Contains(mat))
                    targetMaterials.Add(mat);
            }
        }
    }

    void Update()
    {
        foreach (Material mat in targetMaterials)
        {
            if (!NULLVertexGameOver)
            {
                GlitchGameoverIntensity = 0;

                if (Shaking)
                {
                    float current = mat.GetFloat("_VertexGlitchIntensity");

                    if (Mathf.Abs(current) > 0.0001f)
                    {
                        glitchIntensity = Mathf.Lerp(glitchIntensity, 0f, Time.deltaTime * Speed);
                        ScreenGlitchIntensity = Mathf.Lerp(ScreenGlitchIntensity, 0f, Time.deltaTime * Speed);

                        if (mat.HasProperty("_VertexGlitchIntensity"))
                            mat.SetFloat("_VertexGlitchIntensity", glitchIntensity);

                        if (mat.HasProperty("_ScreenGlitchIntensity"))
                            mat.SetFloat("_ScreenGlitchIntensity",
                                ScreenGlitch ? ScreenGlitchIntensity : 0f);
                    }
                }
                else
                {
                    if (mat.HasProperty("_VertexGlitchIntensity"))
                        mat.SetFloat("_VertexGlitchIntensity", 0f);

                    if (mat.HasProperty("_ScreenGlitchIntensity"))
                        mat.SetFloat("_ScreenGlitchIntensity", 0f);
                }
            }
            else
            {
                // Game Over glitch mode
                if (mat.HasProperty("_VertexGlitchSeed"))
                    mat.SetFloat("_VertexGlitchSeed", Random.Range(-1000, 1000));

                if (mat.HasProperty("_VertexGlitchIntensity"))
                    mat.SetFloat("_VertexGlitchIntensity", GlitchGameoverIntensity);

                if (ScreenGlitch && mat.HasProperty("_ScreenGlitchIntensity"))
                    mat.SetFloat("_ScreenGlitchIntensity", GlitchGameoverIntensity);

                GlitchGameoverIntensity += 0.0005f;
            }

            if (mat.HasProperty("_Brightness"))
                mat.SetFloat("_Brightness", Brightness);
        }
    }

    IEnumerator ShakeTime()
    {
        foreach (Material mat in targetMaterials)
        {
            if (Shaking)
            {
                StartCoroutine(RandomRange());

                if (mat.HasProperty("_VertexGlitchSeed"))
                    mat.SetFloat("_VertexGlitchSeed", Random.Range(-1000, 1000));

                if (mat.HasProperty("_VertexGlitchIntensity"))
                    mat.SetFloat("_VertexGlitchIntensity", Intensity2);

                if (mat.HasProperty("_ScreenGlitchIntensity"))
                {
                    if (ScreenGlitch)
                    {
                        mat.SetFloat("_ScreenGlitchIntensity", Intensity2);
                        ScreenGlitchIntensity = Intensity2;
                    }
                    else
                    {
                        mat.SetFloat("_ScreenGlitchIntensity", 0);
                    }
                }

                glitchIntensity = Intensity2;
            }
        }

        yield return new WaitForSeconds(WaitTime);
        StartCoroutine(ShakeTime());
    }

    IEnumerator RandomRange()
    {
        Intensity2 = (Random.Range(0, 2) == 0) ? -Intensity : Intensity;
        yield return null;
    }
}
