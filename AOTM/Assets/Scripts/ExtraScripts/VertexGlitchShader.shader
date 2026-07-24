Shader "Custom/ETGsVertexGlitchShader"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _Color("Color", Color) = (1,1,1,1)
        _Brightness("Brightness", Range(0, 3)) = 1.3
        _VertexGlitchSeed("Vertex Glitch Seed", Float) = 1
        _VertexGlitchIntensity("Vertex Glitch Intensity", Range(0,2)) = 0.5
        _ScreenGlitchIntensity("Screen Glitch Intensity", Range(0,1)) = 0.25
        _Glossiness("Smoothness", Range(0,1)) = 0.3
        _Metallic("Metallic", Range(0,1)) = 0.0
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 300

        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Back

        CGPROGRAM
        // Surface Shader with lighting + vertex modification
        #pragma surface surf Standard fullforwardshadows vertex:vert alpha:fade
        #pragma target 3.0

        sampler2D _MainTex;
        fixed4 _Color;
        float _Brightness;
        float _VertexGlitchSeed;
        float _VertexGlitchIntensity;
        float _ScreenGlitchIntensity;
        half _Glossiness;
        half _Metallic;

        // ---- Noise helpers ----
        float hash(float n) { return frac(sin(n) * 43758.5453); }

        float noise3D(float3 x)
        {
            float3 p = floor(x);
            float3 f = frac(x);
            f = f * f * (3.0 - 2.0 * f);
            float n = p.x + p.y * 57.0 + 113.0 * p.z;
            return lerp(
                lerp(lerp(hash(n + 0.0), hash(n + 1.0), f.x),
                     lerp(hash(n + 57.0), hash(n + 58.0), f.x), f.y),
                lerp(lerp(hash(n + 113.0), hash(n + 114.0), f.x),
                     lerp(hash(n + 170.0), hash(n + 171.0), f.x), f.y),
                f.z);
        }

        struct Input
        {
            float2 uv_MainTex;
        };

        // ---- Static Vertex Glitch ----
        void vert(inout appdata_full v)
        {
            float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;

            // Static noise — not time-based
            float n = noise3D(worldPos * 2.0 + _VertexGlitchSeed * 10.0);
            n = round(n * 3.0) / 3.0;
            n = saturate((n - 0.5) * 2.0 + 0.5);

            v.vertex.xyz += v.normal * n * _VertexGlitchIntensity * 0.5;
        }

        // ---- Static Screen Glitch ----
        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            float2 uv = IN.uv_MainTex;

            // Static glitch pattern (no _Time)
            float glitchNoise = frac(sin(uv.y * 30.0 + _VertexGlitchSeed) * 43758.5453);
            float2 glitchOffset = float2(glitchNoise - 0.5, glitchNoise - 0.5) * _ScreenGlitchIntensity * 0.1;

            // Static RGB offsets (seed-based)
            float rShift = frac(sin(_VertexGlitchSeed * 2.7 + uv.x * 10.0) * 43758.5453) * _ScreenGlitchIntensity * 0.05;
            float gShift = frac(sin(_VertexGlitchSeed * 3.1 + uv.y * 12.0) * 43758.5453) * _ScreenGlitchIntensity * 0.05;
            float bShift = frac(sin(_VertexGlitchSeed * 1.9 + uv.x * 8.0) * 43758.5453) * _ScreenGlitchIntensity * 0.05;

            // Sample color channels with static offset
            fixed r = tex2D(_MainTex, uv + glitchOffset + float2(rShift, 0)).r;
            fixed g = tex2D(_MainTex, uv + glitchOffset + float2(0, gShift)).g;
            fixed b = tex2D(_MainTex, uv + glitchOffset + float2(-bShift, 0)).b;
            fixed a = tex2D(_MainTex, uv + glitchOffset).a;

            fixed4 c = fixed4(r, g, b, a) * _Color;

            // Apply brightness
            c.rgb *= _Brightness;

            o.Albedo = c.rgb;
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }
        ENDCG
    }

    FallBack "Transparent/Diffuse"
}
