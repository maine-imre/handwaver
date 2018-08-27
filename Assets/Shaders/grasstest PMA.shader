// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Shader created with Shader Forge v1.36 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.36;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:1,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:4013,x:33496,y:32738,varname:node_4013,prsc:2|diff-7379-OUT,normal-1577-OUT;n:type:ShaderForge.SFN_Lerp,id:7379,x:33290,y:32788,varname:node_7379,prsc:2|A-9423-RGB,B-7082-RGB,T-4508-OUT;n:type:ShaderForge.SFN_Lerp,id:1577,x:33304,y:33110,varname:node_1577,prsc:2|A-6475-RGB,B-750-RGB,T-4508-OUT;n:type:ShaderForge.SFN_Tex2d,id:9423,x:33083,y:32657,varname:node_9423,prsc:2,tex:732c3e90ef442d6418df24c490553483,ntxv:3,isnm:False|TEX-7772-TEX;n:type:ShaderForge.SFN_Tex2dAsset,id:7772,x:32767,y:32681,ptovrint:False,ptlb:Grass,ptin:_Grass,varname:node_7772,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:732c3e90ef442d6418df24c490553483,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:7082,x:33083,y:32852,varname:node_7082,prsc:2,tex:732c3e90ef442d6418df24c490553483,ntxv:0,isnm:False|UVIN-3751-OUT,TEX-7772-TEX;n:type:ShaderForge.SFN_Tex2d,id:6475,x:33084,y:33069,varname:node_6475,prsc:2,tex:563f14cd42be4ce469ad528d579d2e04,ntxv:3,isnm:True|TEX-6272-TEX;n:type:ShaderForge.SFN_Tex2d,id:750,x:33093,y:33257,varname:node_750,prsc:2,tex:563f14cd42be4ce469ad528d579d2e04,ntxv:3,isnm:True|UVIN-3751-OUT,TEX-6272-TEX;n:type:ShaderForge.SFN_Multiply,id:3751,x:32827,y:32988,varname:node_3751,prsc:2|A-2858-UVOUT,B-2100-OUT;n:type:ShaderForge.SFN_TexCoord,id:2858,x:32587,y:32968,varname:node_2858,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Vector1,id:2100,x:32587,y:33153,varname:node_2100,prsc:2,v1:0.5;n:type:ShaderForge.SFN_Clamp01,id:4508,x:32757,y:33234,varname:node_4508,prsc:2|IN-8035-OUT;n:type:ShaderForge.SFN_Tex2dAsset,id:6272,x:32807,y:33446,ptovrint:False,ptlb:GrassNormal,ptin:_GrassNormal,varname:node_6272,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:563f14cd42be4ce469ad528d579d2e04,ntxv:3,isnm:False;n:type:ShaderForge.SFN_Power,id:8035,x:32581,y:33235,varname:node_8035,prsc:2|VAL-3784-OUT,EXP-8151-OUT;n:type:ShaderForge.SFN_Divide,id:3784,x:32395,y:33235,varname:node_3784,prsc:2|A-7899-OUT,B-9893-OUT;n:type:ShaderForge.SFN_Vector1,id:8151,x:32531,y:33429,varname:node_8151,prsc:2,v1:1;n:type:ShaderForge.SFN_Vector1,id:9893,x:32282,y:33411,varname:node_9893,prsc:2,v1:87;n:type:ShaderForge.SFN_Distance,id:7899,x:32185,y:33235,varname:node_7899,prsc:2|A-2050-XYZ,B-2472-XYZ;n:type:ShaderForge.SFN_FragmentPosition,id:2050,x:31966,y:33235,varname:node_2050,prsc:2;n:type:ShaderForge.SFN_ViewPosition,id:2472,x:31978,y:33436,varname:node_2472,prsc:2;proporder:7772-6272;pass:END;sub:END;*/

Shader "Shader Forge/grasstest PMA" {
    Properties {
        _Grass ("Grass", 2D) = "white" {}
        _GrassNormal ("GrassNormal", 2D) = "bump" {}
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform float4 _LightColor0;
            uniform sampler2D _Grass; uniform float4 _Grass_ST;
            uniform sampler2D _GrassNormal; uniform float4 _GrassNormal_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float3 tangentDir : TEXCOORD3;
                float3 bitangentDir : TEXCOORD4;
                LIGHTING_COORDS(5,6)
                UNITY_FOG_COORDS(7)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos(v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float4 node_6475 = tex2D(_GrassNormal,TRANSFORM_TEX(i.uv0, _GrassNormal));
                float2 node_3751 = (i.uv0*0.5);
                float4 node_750 = tex2D(_GrassNormal,TRANSFORM_TEX(node_3751, _GrassNormal));
                float node_4508 = saturate(pow((distance(i.posWorld.rgb,_WorldSpaceCameraPos)/87.0),1.0));
                float3 normalLocal = lerp(node_6475.rgb,node_750.rgb,node_4508);
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
/////// Diffuse:
                float NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 directDiffuse = max( 0.0, NdotL) * attenColor;
                float3 indirectDiffuse = float3(0,0,0);
                indirectDiffuse += UNITY_LIGHTMODEL_AMBIENT.rgb; // Ambient Light
                float4 node_9423 = tex2D(_Grass,TRANSFORM_TEX(i.uv0, _Grass));
                float4 node_7082 = tex2D(_Grass,TRANSFORM_TEX(node_3751, _Grass));
                float3 diffuseColor = lerp(node_9423.rgb,node_7082.rgb,node_4508);
                float3 diffuse = (directDiffuse + indirectDiffuse) * diffuseColor;
/// Final Color:
                float3 finalColor = diffuse;
                fixed4 finalRGBA = fixed4(finalColor,1);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                float4 finalFinalColor = finalRGBA; 
                finalFinalColor.rgb *= finalFinalColor.a; 
                return finalFinalColor;
            }
            ENDCG
        }
        Pass {
            Name "FORWARD_DELTA"
            Tags {
                "LightMode"="ForwardAdd"
            }
            Blend One One, Zero One
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDADD
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #pragma multi_compile_fwdadd_fullshadows
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform float4 _LightColor0;
            uniform sampler2D _Grass; uniform float4 _Grass_ST;
            uniform sampler2D _GrassNormal; uniform float4 _GrassNormal_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float3 tangentDir : TEXCOORD3;
                float3 bitangentDir : TEXCOORD4;
                LIGHTING_COORDS(5,6)
                UNITY_FOG_COORDS(7)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos(v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                float4 finalFinalColor = o; 
                finalFinalColor.rgb *= finalFinalColor.a; 
                return finalFinalColor;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float4 node_6475 = tex2D(_GrassNormal,TRANSFORM_TEX(i.uv0, _GrassNormal));
                float2 node_3751 = (i.uv0*0.5);
                float4 node_750 = tex2D(_GrassNormal,TRANSFORM_TEX(node_3751, _GrassNormal));
                float node_4508 = saturate(pow((distance(i.posWorld.rgb,_WorldSpaceCameraPos)/87.0),1.0));
                float3 normalLocal = lerp(node_6475.rgb,node_750.rgb,node_4508);
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
/////// Diffuse:
                float NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 directDiffuse = max( 0.0, NdotL) * attenColor;
                float4 node_9423 = tex2D(_Grass,TRANSFORM_TEX(i.uv0, _Grass));
                float4 node_7082 = tex2D(_Grass,TRANSFORM_TEX(node_3751, _Grass));
                float3 diffuseColor = lerp(node_9423.rgb,node_7082.rgb,node_4508);
                float3 diffuse = directDiffuse * diffuseColor;
/// Final Color:
                float3 finalColor = diffuse;
                fixed4 finalRGBA = fixed4(finalColor * 1,0);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                float4 finalFinalColor = finalRGBA; 
                finalFinalColor.rgb *= finalFinalColor.a; 
                return finalFinalColor;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
