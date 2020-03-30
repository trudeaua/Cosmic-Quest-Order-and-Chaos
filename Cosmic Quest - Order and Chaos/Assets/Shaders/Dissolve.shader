/*
 * Adapted from tutorial at https://www.febucci.com/2018/09/dissolve-shader/
 */

Shader "Custom/Dissolve"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
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
