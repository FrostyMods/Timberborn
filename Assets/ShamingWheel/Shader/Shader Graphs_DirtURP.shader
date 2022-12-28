Shader "Shader Graphs/DirtURP" {
	Properties {
		_MainColor ("Main Color", Vector) = (0.7568628,0.6509804,0.4862745,1)
		[NoScaleOffset] _MainTex ("Albedo", 2D) = "white" {}
		_MainTexScale ("Albedo scale", Float) = 0
		_Metallic ("Metallic", Range(0, 1)) = 0
		_Glossiness ("Smoothness", Range(0, 1)) = 0
		[NoScaleOffset] _FadeTex ("Fade texture", 2D) = "white" {}
		[NoScaleOffset] _NoiseTex ("Noise texture", 2D) = "white" {}
		_NoiseTexScale ("Noise scale", Float) = 0
		_NoiseStrength ("Noise strength", Range(0, 1)) = 0
		[NoScaleOffset] _DetailMask ("Detail Mask", 2D) = "black" {}
		_DetailColor ("Detail Color", Vector) = (0.764151,0.764151,0.764151,0)
		_EmissionColor ("Emission Color", Vector) = (0,0,0,0)
		_Grayscale ("Grayscale", Range(0, 1)) = 0
		[HideInInspector] _QueueOffset ("_QueueOffset", Float) = 0
		[HideInInspector] _QueueControl ("_QueueControl", Float) = -1
		[HideInInspector] [NoScaleOffset] unity_Lightmaps ("unity_Lightmaps", 2DArray) = "" {}
		[HideInInspector] [NoScaleOffset] unity_LightmapsInd ("unity_LightmapsInd", 2DArray) = "" {}
		[HideInInspector] [NoScaleOffset] unity_ShadowMasks ("unity_ShadowMasks", 2DArray) = "" {}
	}
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType"="Opaque" }
		LOD 200
		CGPROGRAM
#pragma surface surf Standard
#pragma target 3.0

		sampler2D _MainTex;
		struct Input
		{
			float2 uv_MainTex;
		};

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}
	Fallback "Hidden/Shader Graph/FallbackError"
	//CustomEditor "UnityEditor.ShaderGraph.GenericShaderGraphMaterialGUI"
}