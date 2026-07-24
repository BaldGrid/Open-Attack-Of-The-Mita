Shader "Vertex Modifier" {
     Properties {
       _MainTex ("Texture", 2D) = "white" {}
       _Amount ("Height Adjustment", Float) = 1.0
       _Amnt ("Amplitude Adjustment", Float) = 1.0
       _Speed ("Pulse Speed", Float) = 1.0
       _DisplacementTexture("Displacement Texture", 2D) = "white"{}
     }
     SubShader {
       Tags { "RenderType" = "Opaque" }
       CGPROGRAM
       #pragma surface surf Lambert vertex:vert
       struct Input {
           float2 uv_MainTex;
       };
 
       // Access the shaderlab properties
       float _Amount;
       float _Amnt;
       float _Speed;
       sampler2D _MainTex;
       sampler2D _DisplacementTexture;
 
       // Vertex modifier function
       void vert (inout appdata_full v) {
          _Amount = sin((_Time * 50 * _Speed) * 2);
          float value = tex2Dlod(_DisplacementTexture, v.texcoord*20).x * _Amount * _Amnt;
                v.vertex.xyz += v.normal.xyz * value * .3 * sin((_Time * 0.1));
       }
 
       // Surface shader function
       void surf (Input IN, inout SurfaceOutput o) {
          o.Albedo = tex2D (_MainTex, IN.uv_MainTex).rgba;
       }
       ENDCG
     }
   }