    Shader "Custom/EmissiveShield"{
     
        Properties {
            _Color ("Color", Color)=(1,1,1,1)
            _Glossiness ("Smoothness", Range(0,1))=0.5
            _Metallic ("Metallic", Range(0,1))=0.0
            _GIAlbedoColor ("Color Albedo (GI)", Color)=(1,1,1,1)
            _GIAlbedoColorMultiplier ("Intensity (GI)", Float)= 1.0
        }
     
        SubShader {
        // ------------------------------------------------------------------
        // Extracts information for lightmapping, GI (emission, albedo, ...)
        // This pass it not used during regular rendering.
            Pass
            {
                Name "META"
                Tags {"LightMode"="Meta"}
                Cull Off
                CGPROGRAM
                #pragma vertex vert_meta
                #pragma fragment frag_meta2
                #pragma shader_feature _EMISSION
                #pragma shader_feature _METALLICGLOSSMAP
                #pragma shader_feature _DETAIL_MULX2
     
                #include"UnityStandardMeta.cginc"
                fixed4 _GIAlbedoColor;
                float _GIAlbedoColorMultiplier;
                float4 frag_meta2 (v2f_meta i): SV_Target
                {
                    // we're interested in diffuse & specular colors,
                    // and surface roughness to produce final albedo.
                   
                    FragmentCommonData data = UNITY_SETUP_BRDF_INPUT (i.uv);
                    UnityMetaInput o;
                    UNITY_INITIALIZE_OUTPUT(UnityMetaInput, o);
                    fixed4 c = _GIAlbedoColor;
                    o.Albedo = fixed3(c.rgb * _GIAlbedoColor.rgb);
                    o.Emission = _GIAlbedoColor * _GIAlbedoColorMultiplier;
                    return UnityMetaFragment(o);
                }
               

                ENDCG
            }
           
            Tags {"RenderType"="Opaque"}
            LOD 200
     
            CGPROGRAM
            // Physically based Standard lighting model, and enable shadows on all light types
            #pragma surface surf Standard fullforwardshadows nometa
            // Use shader model 3.0 target, to get nicer looking lighting
            #pragma target 3.0
     
            sampler2D _MainTex;
     
            struct Input {
                float3 worldPos;
            };
           
            half _Glossiness;
            half _Metallic;
            fixed4 _Color;
            
           
            void surf (Input IN, inout SurfaceOutputStandard o){
                float3 colour = _Color.rgb;
                o.Albedo = colour.rgb;
                // Metallic and smoothness come from slider variables
                o.Metallic = _Metallic;
                o.Smoothness = _Glossiness;
                o.Alpha = _Color.a * IN.worldPos.x;
                o.Emission = colour;
            }
            ENDCG
        }
     
        FallBack "Diffuse"
    }
