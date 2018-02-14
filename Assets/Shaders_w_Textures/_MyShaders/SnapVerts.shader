// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/SnapVerts"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_PixelWidth("Pixel width", float) = 1.0
		_PixelHeight("Pixel height", float) = 1.0
		_ScreenWidth("Screen width", Range(0,5)) = 1.0
		_ScreenHeight("Screen height", float) = 1.0
	}
	SubShader
	{
		// No culling or depth
//		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };
 
            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };
 
            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _PixelWidth;
            float _PixelHeight;
            float _ScreenWidth;
            float _ScreenHeight;

            v2f vert (appdata v)
            {
                v2f o;
                float dx = _PixelWidth / _ScreenWidth;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.vertex = dx *  floor(o.vertex / dx);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
//                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = fixed4(i.uv.x, 0 , 0 ,1);
				return col;
			}
			ENDCG
		}
	}
}
