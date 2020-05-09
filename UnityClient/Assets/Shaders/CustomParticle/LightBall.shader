Shader "CustomParticles/LightBall"
{
    Properties
    {
        // Color property for material inspector, default to white
        _Colour ("Light Colour", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags {"Queue"="Transparent" "IgnoreProjector"="True"}
        ZWrite Off
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

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 _Colour;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = _Colour;
                float distanceX = abs(abs(i.uv.x) - 0.5);
                float distanceY = abs(abs(i.uv.y) - 0.5);
                col.a = clamp(lerp(1, 0, 2 * (distanceX + distanceY)), 0, 1);
                return col;
            }
            ENDCG
        }
    }
}
