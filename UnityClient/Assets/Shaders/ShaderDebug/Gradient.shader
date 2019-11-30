Shader "Debug/Gradient"
{
    Properties
    {
        _MainTex ("Albedo Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags {"RenderType"="Opaque" }
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
            fixed4 _ColourEnd;
            fixed4 _ColourBegin;
            float4 _MainTex_ST;
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
            _ColourBegin.rgba = 1,1,1,1;
            _ColourEnd.rgba = 0,0,0,1;
                // sample the texture
                fixed4 col = lerp(_ColourBegin, _ColourEnd, i.uv.y);
            col.a = 1;
    
                return col;
            }
            ENDCG
        }
    }
}
