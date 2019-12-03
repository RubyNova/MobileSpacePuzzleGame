    Shader "Holograms/EmissiveShield"{
     
        Properties {
            _Colour ("Base Colour", Color)=(1,1,1,1)
            _particleColour ("ParticleColour", Color)=(1,1,1,1)
            _Glossiness ("Smoothness", Range(0,1))=0.5
            _Metallic ("Metallic", Range(0,1))=0.0
            _GIAlbedoColor ("Color Albedo (GI)", Color)=(1,1,1,1)
            _GIAlbedoColorMultiplier ("Intensity (GI)", Float)= 1.0
            _EmissionTex ("Emission Texture (GI)", 2D) = "white"{}
        }
        
        SubShader {
                // ------------------------------------------------------------------
        // Extracts information for lightmapping, GI (emission, albedo, ...)
        // This pass it not used during regular rendering.
/*        Pass
        {
            Name "META"
            Tags {"LightMode"="Meta"}
            Cull Off
            CGPROGRAM
 
            #include"UnityStandardMeta.cginc"
 
            sampler2D _EmissionTex;
            float _GIAlbedoColorMultiplier;
            fixed4 _GIAlbedoColor;
            float4 frag_meta2 (v2f_meta i): SV_Target
            {
                // We're interested in diffuse & specular colors
                // and surface roughness to produce final albedo.
                
               
                FragmentCommonData data = UNITY_SETUP_BRDF_INPUT (i.uv);
                UnityMetaInput o;
                UNITY_INITIALIZE_OUTPUT(UnityMetaInput, o);
                fixed4 c = tex2D (_EmissionTex, i.uv);
                o.Albedo = _GIAlbedoColor.rgb * _GIAlbedoColorMultiplier;
                o.Emission = Emission(i.uv.xy);
                return UnityMetaFragment(o);
            }
           
            #pragma vertex vert_meta
            #pragma fragment frag_meta2
            #pragma shader_feature _EMISSION
            #pragma shader_feature ___ _DETAIL_MULX2
            ENDCG
        }*/
        
        

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
                    fixed4 returnValue = lerp(_Colour, _particleColour, round(rand(i.vertex.xyz)));
                    returnValue.a = _Colour.a;
                    return returnValue;
                }
                ENDCG
        }
    }
        FallBack "Diffuse"
    }
