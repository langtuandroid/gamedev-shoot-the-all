Shader "twoSide_PBR_SRP"
{
	Properties
	{
		_BaseColour("BaseColour", Color) = (1,1,1,0)
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		_MainTextureCutoutA("MainTexture-Cutout(A)", 2D) = "white" {}
		_SelfIllumination("SelfIllumination", Range( 0 , 2)) = 0.1
		_SpecColour("SpecColour", Color) = (1,1,1,0)
		_SpecularColTextureSmoothA("SpecularColTexture-Smooth(A)", 2D) = "white" {}
		_Smoothness("Smoothness", Range( 0 , 1)) = 0.5
		_NormalMap("NormalMap", 2D) = "bump" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Off
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf StandardSpecular keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
			half ASEVFace : VFACE;
		};

		uniform sampler2D _NormalMap;
		uniform float4 _NormalMap_ST;
		uniform float4 _BaseColour;
		uniform sampler2D _MainTextureCutoutA;
		uniform float4 _MainTextureCutoutA_ST;
		uniform float _SelfIllumination;
		uniform float4 _SpecColour;
		uniform sampler2D _SpecularColTextureSmoothA;
		uniform float4 _SpecularColTextureSmoothA_ST;
		uniform float _Smoothness;
		uniform float _Cutoff = 0.5;

		void surf( Input i , inout SurfaceOutputStandardSpecular o )
		{
			float2 uv_NormalMap = i.uv_texcoord * _NormalMap_ST.xy + _NormalMap_ST.zw;
			float3 tex2DNode20 = UnpackNormal( tex2D( _NormalMap, uv_NormalMap ) );
			float3 appendResult40 = (float3(tex2DNode20.r , tex2DNode20.g , ( tex2DNode20.b * -1.0 )));
			float3 switchResult39 = (((i.ASEVFace>0)?(tex2DNode20):(appendResult40)));
			o.Normal = switchResult39;
			float2 uv_MainTextureCutoutA = i.uv_texcoord * _MainTextureCutoutA_ST.xy + _MainTextureCutoutA_ST.zw;
			float4 tex2DNode10 = tex2D( _MainTextureCutoutA, uv_MainTextureCutoutA );
			float4 temp_output_11_0 = ( _BaseColour * tex2DNode10 );
			o.Albedo = temp_output_11_0.rgb;
			o.Emission = ( temp_output_11_0 * _SelfIllumination ).rgb;
			float2 uv_SpecularColTextureSmoothA = i.uv_texcoord * _SpecularColTextureSmoothA_ST.xy + _SpecularColTextureSmoothA_ST.zw;
			float4 tex2DNode19 = tex2D( _SpecularColTextureSmoothA, uv_SpecularColTextureSmoothA );
			o.Specular = ( _SpecColour * tex2DNode19 ).rgb;
			o.Smoothness = ( _Smoothness * tex2DNode19.b );
			o.Alpha = 1;
			clip( tex2DNode10.a - _Cutoff );
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}