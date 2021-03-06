﻿// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/PixelColorFalloffWithDepth" {
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_PixelWidth("Pixel width", float) = 1.0
		_PixelHeight("Pixel height", float) = 1.0
		_ScreenWidth("Screen width", float) = 1.0
		_ScreenHeight("Screen height", float) = 1.0
		_MeanShift("Mean Shift", Range(0,1)) = .5
		_FalloffGradient("FalloffGradient", 2D) = "white" {}
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		
		Pass{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			struct v2f {
			   float4 pos : SV_POSITION;
			   float4 scrPos:TEXCOORD1;
			};
			
			//Vertex Shader
			v2f vert (appdata_base v){
			   v2f o;
			   o.pos = UnityObjectToClipPos (v.vertex);
			   o.scrPos=ComputeScreenPos(o.pos);
			   //for some reason, the y position of the depth texture comes out inverted
			   o.scrPos.y = o.scrPos.y;
			   return o;
			}

			sampler2D _CameraDepthTexture;
			sampler2D _MainTex;
			sampler2D _FalloffGradient;
			half _PixelWidth;
			half _PixelHeight;
			half _ScreenWidth;
			half _ScreenHeight;
			half _MeanShift;

			//Fragment Shader
			half4 frag (v2f i) : COLOR{
			   float depthValue = Linear01Depth (tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.scrPos)).r);
			   half4 depth;

			   depthValue *= (_MeanShift * 2) - _MeanShift;

			   depth.r = depthValue;
			   depth.g = depthValue;
			   depth.b = depthValue;
			   depth.a = 1;

			   fixed4 col = tex2D(_MainTex, i.scrPos);
			   fixed4 falloffGrad = tex2D(_FalloffGradient, float2(depthValue * depthValue * depthValue, .5));
			   half dx = _PixelWidth * (1 / _ScreenWidth) * (1-depthValue)*2;
			   half dy = _PixelHeight * (1 / _ScreenHeight) * (1-depthValue) *2;
			   half2 coord = half2(dx*floor(i.scrPos.x / dx), dy * floor(i.scrPos.y / dy));
			   col = tex2D(_MainTex, coord) * falloffGrad.r;


			   return col;
			}
			ENDCG
		}
	}
	FallBack "Diffuse"
}