Shader "Custom/ETGsVertexGlitchShader_99%_NOBUG"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _Color("Color", Color) = (1,1,1,1)
        _Brightness("Brightness", Range(0,3)) = 1.3
        _VertexGlitchSeed("Vertex Glitch Seed", Float) = 1
        _VertexGlitchIntensity("Vertex Glitch Intensity", Range(-2,2)) = 0.5
        _ScreenGlitchIntensity("Screen Glitch Intensity", Range(0,1)) = 0.25
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 100

        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Back

        CGPROGRAM
        #pragma surface surf Lambert vertex:vert alpha:fade
        #pragma target 3.0

        sampler2D _MainTex;
        fixed4 _Color;
        half _Brightness;
        half _VertexGlitchSeed;
        half _VertexGlitchIntensity;
        half _ScreenGlitchIntensity;

        struct Input
        {
            half2 uv_MainTex;
        };

        // ⚠ REQUIRED surface-shader vertex format
        struct appdata
        {
            float4 vertex : POSITION;
            half3 normal  : NORMAL;
            half2 texcoord : TEXCOORD0;
            UNITY_VERTEX_INPUT_INSTANCE_ID
        };

        // 🔥 Ultra-fast hash (no trig)
        inline half hashWS(half3 p)
        {
            p = frac(p * 0.1031h);
            p += dot(p, p.yzx + 33.33h);
            return frac((p.x + p.y) * p.z);
        }

        void vert(inout appdata v)
        {
            UNITY_SETUP_INSTANCE_ID(v);

            half3 wp = mul(unity_ObjectToWorld, v.vertex).xyz * 2.0h;
            half n = hashWS(floor(wp + _VertexGlitchSeed));
            n = step(0.5h, n);

            v.vertex.xyz += v.normal * n * _VertexGlitchIntensity * 0.5h;
        }

        void surf(Input IN, inout SurfaceOutput o)
        {
            half2 uv = IN.uv_MainTex;

            half gn = frac(dot(uv, half2(12.9898h,78.233h)) + _VertexGlitchSeed);
            uv += (gn - 0.5h) * _ScreenGlitchIntensity * 0.1h;

            fixed4 col = tex2D(_MainTex, uv) * _Color;
            col.rgb *= _Brightness;

            o.Albedo = col.rgb;
            o.Alpha  = col.a;
        }
        ENDCG
    }

    FallBack "Transparent/Diffuse"
}
