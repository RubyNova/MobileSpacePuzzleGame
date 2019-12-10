Shader "ViewportEffects/CRTViewport"
{
    Properties
    {
        _MainTex ("Albedo Texture", 2D) = "white" {}
        _WaveMultiplier ("Wave Multiplier", int) = 300
        _BorderLerpX ("Border Interpolation X", float) = 7
        _BorderLerpY ("Border Interpolation X", float) = 5
        _Offset ("Effect Offset", float) = 0
        _HighTintColour ("High tint colour", color) = (1,1,1,1)
        _LowTintColour ("Low tint colour", color) = (0,0,0,0)
        _EdgeTintColour ("Edge tint colour", color) = (0,0,0,0)
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

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

            sampler2D _MainTex;
            int _WaveMultiplier;
            float _BorderLerpX;
            float _BorderLerpY;
            float _Offset;
            fixed4 _HighTintColour;
            fixed4 _LowTintColour;
            fixed4 _EdgeTintColour;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                float lerpWeight = clamp(sin(abs(i.uv.y + _Offset) * _WaveMultiplier), 0.0, 1.0);
                float crtScreenEdgeMultiplierX = clamp(lerp(_BorderLerpX, 0, abs(i.uv.x - 0.5) * 2), 0.0, 1.0);
                float crtScreenEdgeMultiplierY = clamp(lerp(_BorderLerpY, 0, abs(i.uv.y - 0.5) * 2), 0.0, 1.0);
                fixed4 edgeTintColourAdjusted = _EdgeTintColour;
                edgeTintColourAdjusted = edgeTintColourAdjusted * crtScreenEdgeMultiplierX * crtScreenEdgeMultiplierY;
                col.rgb = (col.rgb * lerp(_HighTintColour, _LowTintColour, lerpWeight));
                col = col * edgeTintColourAdjusted;
                return col;
            }
            ENDCG
        }
    }
}
