// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/PixelShaderWithDepth" {
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_PixelWidth("Pixel width", float) = 1.0
		_PixelHeight("Pixel height", float) = 1.0
		_ScreenWidth("Screen width", float) = 1.0
		_ScreenHeight("Screen height", float) = 1.0
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
			half _PixelWidth;
			half _PixelHeight;
			half _ScreenWidth;
			half _ScreenHeight;

			//Fragment Shader
			half4 frag (v2f i) : COLOR{
			   float depthValue = 1 - Linear01Depth (tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.scrPos)).r);
			   half4 depth;
			
			   depth.r = depthValue;
			   depth.g = depthValue;
			   depth.b = depthValue;
			   depth.a = 1;

			   fixed4 col = tex2D(_MainTex, i.scrPos);
			   half dx = _PixelWidth * (1 / _ScreenWidth) * depthValue*2;
			   half dy = _PixelHeight * (1 / _ScreenHeight) * depthValue *2;
			   half2 coord = half2(dx*floor(i.scrPos.x / dx), dy * floor(i.scrPos.y / dy));
			   col = tex2D(_MainTex, coord);


			   return col;
			}
			ENDCG
		}
	}
	FallBack "Diffuse"
}