Shader "Lines/LaserPointer"
{
    Properties
    {
        _MainTex ("Albedo Texture", 2D) = "white" {}
        _TintColour("Tint", Color) = (1,1,1,1)
        _Transparency("Transparency", Range(0.0, 0.5)) = 0.25
    }
    SubShader
    {
        Tags {"Queue"="Transparent" "RenderType"="Transparent" }
        LOD 100
        ZWrite off
        Blend SrcAlpha OneMinusSrcAlpha
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"
            
            float4 _TintColour;
            float _Transparency;
            
            float rand(float3 myVector) {
                    return frac(sin(_Time[0] * dot(myVector ,float3(12.9898,78.233,45.5432))) * 43758.5453);
            }

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o, o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                float transparency = clamp((col.r + col.g + col.b) / 3, 0.4, 1);
                col = clamp(col * _TintColour, _TintColour * 0.5, _TintColour);
                //col.a = transparency;
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
