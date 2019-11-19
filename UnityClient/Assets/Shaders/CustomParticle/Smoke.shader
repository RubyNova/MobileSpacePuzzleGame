// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "CustomParticles/Smoke"
{
    Properties
    {
        // Color property for material inspector, default to white
        _Color ("Smoke Color", Color) = (1,1,1,1)
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
            

            float4 vert (float4 vertex : POSITION) : SV_POSITION
            {
                return UnityObjectToClipPos(vertex);
            }
            

            fixed4 _Color;


            fixed4 frag () : SV_Target
            {
                return _Color;
            }
            ENDCG
        }
    }
}
