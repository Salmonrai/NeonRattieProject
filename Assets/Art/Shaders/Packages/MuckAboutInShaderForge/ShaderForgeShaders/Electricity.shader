// Shader created with Shader Forge v1.38 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:Dissolve,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:0,dpts:2,wrdp:True,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:False,qofs:0,qpre:2,rntp:3,fgom:False,fgoc:True,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:True,fnfb:True,fsmp:False;n:type:ShaderForge.SFN_Final,id:4795,x:32724,y:32693,varname:node_4795,prsc:2|emission-2393-OUT,clip-481-OUT;n:type:ShaderForge.SFN_Tex2d,id:6074,x:32235,y:32601,ptovrint:False,ptlb:MainTex,ptin:_MainTex,varname:_MainTex,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:bad7f8db6252eb848a06bb0f66979751,ntxv:0,isnm:False|UVIN-1729-OUT;n:type:ShaderForge.SFN_Multiply,id:2393,x:32495,y:32793,varname:node_2393,prsc:2|A-6074-RGB,B-2053-RGB,C-797-RGB,D-4948-OUT;n:type:ShaderForge.SFN_VertexColor,id:2053,x:32235,y:32772,varname:node_2053,prsc:2;n:type:ShaderForge.SFN_Color,id:797,x:32235,y:32932,ptovrint:True,ptlb:Color,ptin:_TintColor,varname:_TintColor,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.3663495,c2:0.413271,c3:0.8897059,c4:1;n:type:ShaderForge.SFN_Append,id:5132,x:30338,y:32756,varname:node_5132,prsc:2|A-8928-OUT,B-932-OUT;n:type:ShaderForge.SFN_Time,id:8624,x:30338,y:32889,varname:node_8624,prsc:2;n:type:ShaderForge.SFN_ValueProperty,id:932,x:30152,y:32862,ptovrint:False,ptlb:Vspeed,ptin:_Vspeed,varname:node_932,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.2;n:type:ShaderForge.SFN_ValueProperty,id:8928,x:30152,y:32758,ptovrint:False,ptlb:Uspeed,ptin:_Uspeed,varname:node_8928,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.2;n:type:ShaderForge.SFN_Multiply,id:5492,x:30538,y:32756,varname:node_5492,prsc:2|A-5132-OUT,B-8624-T;n:type:ShaderForge.SFN_TexCoord,id:3850,x:30538,y:32902,varname:node_3850,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Add,id:5373,x:30717,y:32756,varname:node_5373,prsc:2|A-5492-OUT,B-3850-UVOUT;n:type:ShaderForge.SFN_Tex2d,id:8874,x:30891,y:32756,varname:node_8874,prsc:2,tex:1f799080de3b2ad47becdf3a7f20b447,ntxv:0,isnm:False|UVIN-5373-OUT,TEX-5802-TEX;n:type:ShaderForge.SFN_Tex2dAsset,id:5802,x:30717,y:32902,ptovrint:False,ptlb:Noise,ptin:_Noise,varname:node_5802,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:1f799080de3b2ad47becdf3a7f20b447,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Append,id:364,x:30338,y:33020,varname:node_364,prsc:2|A-3150-OUT,B-7376-OUT;n:type:ShaderForge.SFN_Multiply,id:4950,x:30538,y:33046,varname:node_4950,prsc:2|A-364-OUT,B-4910-T;n:type:ShaderForge.SFN_TexCoord,id:6307,x:30538,y:33176,varname:node_6307,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Time,id:4910,x:30338,y:33148,varname:node_4910,prsc:2;n:type:ShaderForge.SFN_ValueProperty,id:3150,x:30152,y:33020,ptovrint:False,ptlb:2Uspeed,ptin:_2Uspeed,varname:node_3150,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:-0.2;n:type:ShaderForge.SFN_ValueProperty,id:7376,x:30152,y:33116,ptovrint:False,ptlb:2Vspeed,ptin:_2Vspeed,varname:node_7376,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.05;n:type:ShaderForge.SFN_Add,id:3689,x:30717,y:33061,varname:node_3689,prsc:2|A-4950-OUT,B-6307-UVOUT;n:type:ShaderForge.SFN_Tex2d,id:1086,x:30891,y:32942,varname:node_1086,prsc:2,tex:1f799080de3b2ad47becdf3a7f20b447,ntxv:0,isnm:False|UVIN-3689-OUT,TEX-5802-TEX;n:type:ShaderForge.SFN_Slider,id:8554,x:30381,y:32602,ptovrint:False,ptlb:Disslove,ptin:_Disslove,varname:node_8554,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:1;n:type:ShaderForge.SFN_OneMinus,id:6108,x:30717,y:32602,varname:node_6108,prsc:2|IN-8554-OUT;n:type:ShaderForge.SFN_RemapRange,id:6405,x:30891,y:32602,varname:node_6405,prsc:2,frmn:0,frmx:1,tomn:-0.65,tomx:0.65|IN-6108-OUT;n:type:ShaderForge.SFN_Add,id:9989,x:31138,y:32602,varname:node_9989,prsc:2|A-6405-OUT,B-8874-R;n:type:ShaderForge.SFN_Add,id:3543,x:31138,y:32756,varname:node_3543,prsc:2|A-6405-OUT,B-1086-R;n:type:ShaderForge.SFN_Multiply,id:8198,x:31324,y:32602,varname:node_8198,prsc:2|A-9989-OUT,B-3543-OUT;n:type:ShaderForge.SFN_RemapRange,id:8672,x:31504,y:32602,varname:node_8672,prsc:2,frmn:0,frmx:1,tomn:-10,tomx:10|IN-8198-OUT;n:type:ShaderForge.SFN_Clamp01,id:3769,x:31698,y:32602,varname:node_3769,prsc:2|IN-8672-OUT;n:type:ShaderForge.SFN_OneMinus,id:6890,x:31880,y:32602,varname:node_6890,prsc:2|IN-3769-OUT;n:type:ShaderForge.SFN_Append,id:1729,x:32044,y:32565,varname:node_1729,prsc:2|A-6890-OUT,B-6497-OUT;n:type:ShaderForge.SFN_Vector1,id:6497,x:31880,y:32746,varname:node_6497,prsc:2,v1:0;n:type:ShaderForge.SFN_ValueProperty,id:4948,x:32235,y:33106,ptovrint:False,ptlb:Opacity,ptin:_Opacity,varname:node_4948,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:8;n:type:ShaderForge.SFN_Multiply,id:481,x:32495,y:32604,varname:node_481,prsc:2|A-8363-OUT,B-6074-R;n:type:ShaderForge.SFN_ValueProperty,id:8363,x:32235,y:32511,ptovrint:False,ptlb:Strength,ptin:_Strength,varname:node_8363,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;proporder:6074-797-932-8928-5802-8554-3150-7376-4948-8363;pass:END;sub:END;*/

Shader "Shader Forge/Electricity" {
    Properties {
        _MainTex ("MainTex", 2D) = "white" {}
        _TintColor ("Color", Color) = (0.3663495,0.413271,0.8897059,1)
        _Vspeed ("Vspeed", Float ) = 0.2
        _Uspeed ("Uspeed", Float ) = 0.2
        _Noise ("Noise", 2D) = "white" {}
        _Disslove ("Disslove", Range(0, 1)) = 0
        _2Uspeed ("2Uspeed", Float ) = -0.2
        _2Vspeed ("2Vspeed", Float ) = 0.05
        _Opacity ("Opacity", Float ) = 8
        _Strength ("Strength", Float ) = 1
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "Queue"="AlphaTest"
            "RenderType"="TransparentCutout"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend One One
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform float4 _TintColor;
            uniform float _Vspeed;
            uniform float _Uspeed;
            uniform sampler2D _Noise; uniform float4 _Noise_ST;
            uniform float _2Uspeed;
            uniform float _2Vspeed;
            uniform float _Disslove;
            uniform float _Opacity;
            uniform float _Strength;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 vertexColor : COLOR;
                UNITY_FOG_COORDS(1)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                float node_6405 = ((1.0 - _Disslove)*1.3+-0.65);
                float4 node_8624 = _Time;
                float2 node_5373 = ((float2(_Uspeed,_Vspeed)*node_8624.g)+i.uv0);
                float4 node_8874 = tex2D(_Noise,TRANSFORM_TEX(node_5373, _Noise));
                float4 node_4910 = _Time;
                float2 node_3689 = ((float2(_2Uspeed,_2Vspeed)*node_4910.g)+i.uv0);
                float4 node_1086 = tex2D(_Noise,TRANSFORM_TEX(node_3689, _Noise));
                float2 node_1729 = float2((1.0 - saturate((((node_6405+node_8874.r)*(node_6405+node_1086.r))*20.0+-10.0))),0.0);
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(node_1729, _MainTex));
                clip((_Strength*_MainTex_var.r) - 0.5);
////// Lighting:
////// Emissive:
                float3 emissive = (_MainTex_var.rgb*i.vertexColor.rgb*_TintColor.rgb*_Opacity);
                float3 finalColor = emissive;
                fixed4 finalRGBA = fixed4(finalColor,1);
                UNITY_APPLY_FOG_COLOR(i.fogCoord, finalRGBA, fixed4(0.5,0.5,0.5,1));
                return finalRGBA;
            }
            ENDCG
        }
    }
    CustomEditor "ShaderForgeMaterialInspector"
}
