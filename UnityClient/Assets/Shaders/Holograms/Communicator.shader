Shader "Unlit/Communicator"
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
            
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _TintColour;
            float _Transparency;
            bool _ShouldInvert = false;
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv) + _TintColour;
                col.a = clamp(sin(abs(i.uv.y) * 100), 0.0, 0.5) * _Transparency + (clamp(sin(abs(i.uv.x) * 100), 0.0, 0.5) * _Transparency); //fmod(2, i.uv.y); //lerp(0, _Transparency, i.uv.y);

    
                return col;
            }
            ENDCG
        }
    }
}