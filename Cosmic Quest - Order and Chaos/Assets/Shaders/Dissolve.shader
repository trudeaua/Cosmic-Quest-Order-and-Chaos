/*
 * Adapted from tutorial at https://www.febucci.com/2018/09/dissolve-shader/
 */

Shader "Custom/Dissolve"
{
    Properties
    {
        _Color ("Color", Color) = (0.6,0.6,0.6,1)
        [NoScaleOffset] _MainTex ("Albedo", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        
        [NoScaleOffset] _BumpMap ("Normal Map", 2D) = "bump" {}
        
        [NoScaleOffset] _OcclusionMap ("Occlusion", 2D) = "black" {}
        _OcclusionStrength ("Amount", Range(0,1)) = 0.5
        
        [Toggle] _HasEmission ("Has Emission", Float) = 0
        [NoScaleOffset] _EmissionMap ("Emission Texture", 2D) = "white" {}
        [HDR] _EmissionColour ("Emission Colour", Color) = (1,1,1,1)
        
        // Dissolve properties
        [Header(Dissolve Properties)]
		[NoScaleOffset] _DissolveTexture("Dissolve Texture", 2D) = "white" {} 
		_DissolveAmount("Amount", Range(0,1)) = 0
		[HDR] _EdgeColor ("Edge Color", Color) = (1,1,1,1)

        [Header(Toon Properties)]
		[HDR]
		_AmbientColor("Ambient Color", Color) = (0.4,0.4,0.4,1)
		[HDR]
		_SpecularColor("Specular Color", Color) = (0.9,0.9,0.9,1)
		_Glossiness("Glossiness", Float) = 32
		[HDR]
		_RimColor("Rim Color", Color) = (1,1,1,1)
		_RimAmount("Rim Amount", Range(0, 1)) = 0.716
		_RimThreshold("Rim Threshold", Range(0, 1)) = 0.1
    }
    SubShader
	{
		Pass
		{
			Tags
			{
				"LightMode" = "ForwardBase"
				"PassFlags" = "OnlyDirectional"
			}
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fwdbase

			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "AutoLight.cginc"

			struct appdata
			{
				float4 vertex : POSITION;				
				float4 uv : TEXCOORD0;
				float3 normal : NORMAL;
			};

			struct v2f
			{
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
				float3 worldNormal : NORMAL;
				float3 viewDir : TEXCOORD1;
				SHADOW_COORDS(2)
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.worldNormal = UnityObjectToWorldNormal(v.normal);
				o.viewDir = WorldSpaceViewDir(v.vertex);
				TRANSFER_SHADOW(o)
				return o;
			}
			
			float4 _Color;
			float4 _AmbientColor;
			float _Glossiness;
			float4 _SpecularColor;
			float4 _RimColor;
			float _RimAmount;
			float _RimThreshold;
			float4 frag (v2f i) : SV_Target
			{
				float3 normal = normalize(i.worldNormal);
				float NdotL = dot(_WorldSpaceLightPos0, normal);
				float3 viewDir = normalize(i.viewDir);
				float3 halfVector = normalize(_WorldSpaceLightPos0 + viewDir);
				float NdotH = dot(normal, halfVector);
				float4 sample = tex2D(_MainTex, i.uv);
				float shadow = SHADOW_ATTENUATION(i);
				float lightIntensity = smoothstep(0, 0.01, NdotL * shadow);
				float specularIntensity = pow(NdotH * lightIntensity, _Glossiness * _Glossiness);
				float specularIntensitySmooth = smoothstep(0.005, 0.01, specularIntensity);
				float4 specular = specularIntensitySmooth * _SpecularColor;
				float4 rimDot = 1 - dot(viewDir, normal);
				float rimIntensity = rimDot * pow(NdotL, _RimThreshold);
				rimIntensity = smoothstep(_RimAmount - 0.01, _RimAmount + 0.01, rimIntensity);
				float4 rim = rimIntensity * _RimColor;
				float4 light = lightIntensity * _LightColor0;
				return _Color * sample * (_AmbientColor + light + specular + rim);
			}
			ENDCG
		}
		UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"
	}
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200
        Cull Off

        CGPROGRAM
        // Physically based Standard lighting model
        #pragma surface surf Standard addshadow

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_BumpMap;
        };

        fixed4 _Color;
        sampler2D _MainTex;
        sampler2D _BumpMap;
        
        half _Glossiness;
        half _Metallic;
        
        sampler2D _OcclusionMap;
        half _OcclusionStrength;
        
        half _HasEmission;
        sampler2D _EmissionMap;
        fixed4 _EmissionColour;
        
        // Dissolve properties
        sampler2D _DissolveTexture;
        half _DissolveAmount;
        fixed4 _EdgeColor;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Dissolve function
            half dissolve_value = tex2D(_DissolveTexture, IN.uv_MainTex).r;
            clip(dissolve_value - _DissolveAmount);
            
            // Emits edge color with 0.05 border size
            o.Emission = _EdgeColor * step(dissolve_value - _DissolveAmount, 0.05f);
            
            if (_HasEmission >= 1.0f)
            {
                o.Emission += tex2D(_EmissionMap, IN.uv_MainTex) * _EmissionColour;
            }
        
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            o.Normal = UnpackNormal (tex2D (_BumpMap, IN.uv_BumpMap));
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
            o.Occlusion = tex2D(_OcclusionMap, IN.uv_MainTex) * _OcclusionStrength;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
