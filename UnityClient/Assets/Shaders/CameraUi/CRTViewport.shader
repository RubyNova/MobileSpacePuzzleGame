Shader "ViewportEffects/CRTViewport"
{
    Properties
    {
        _MainTex ("Albedo Texture", 2D) = "white" {}
        _Intensity ("Intensity", float) = 0.0
        _WaveMultiplier ("Wave Multiplier", int) = 300
        _HighTintColour ("High tint colour", color) = (1,1,1,1)
        _LowTintColour ("Low tint colour", color) = (0,0,0,0)
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
            float _Intensity;
            int _WaveMultiplier;
            fixed4 _HighTintColour;
            fixed4 _LowTintColour;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                // just invert the colors
                float lerpWeight = clamp(sin(abs(i.uv.y) * _WaveMultiplier), 0.0, 1.0); //* _Transparency;
                col.rgb = col.rgb * lerp(_HighTintColour, _LowTintColour, lerpWeight);
                return col;
            }
            ENDCG
        }
    }
}
