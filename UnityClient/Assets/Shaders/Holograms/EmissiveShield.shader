    Shader "Holograms/EmissiveShield"{
     
        Properties {
            _Colour ("Base Colour", Color)=(1,1,1,1)
            _particleColour ("ParticleColour", Color)=(1,1,1,1)
            _Glossiness ("Smoothness", Range(0,1))=0.5
            _Metallic ("Metallic", Range(0,1))=0.0
            _GIAlbedoColor ("Color Albedo (GI)", Color)=(1,1,1,1)
            _GIAlbedoColorMultiplier ("Intensity (GI)", Float)= 1.0
        }
        
        SubShader {
            Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha 
            Pass {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #include "UnityCG.cginc"
                
                struct v2f {
                    float4 vertex : SV_POSITION;
                };
                
                float rand(float3 myVector) {
                    return frac(sin(_Time[0] * dot(myVector ,float3(12.9898,78.233,45.5432))) * 43758.5453);
                }
            

                float4 vert (float4 vertex : POSITION) : SV_POSITION {
                    return UnityObjectToClipPos(vertex);
                }
            

                fixed4 _Colour;
                fixed4 _particleColour;

                fixed4 frag (v2f i) : SV_Target {
                    return _Colour + (_particleColour * rand(i.vertex.xyz));
                }
                ENDCG
        }
    }
        FallBack "Diffuse"
    }
