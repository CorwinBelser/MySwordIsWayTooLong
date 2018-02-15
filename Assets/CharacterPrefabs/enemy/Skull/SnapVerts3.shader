// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Custom/SnapVerts3"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_PixelWidth("VertIntervalPixels", float) = 1.0
		_PixelHeight("PixelIntervalPixels", float) = 1.0
		_ScreenWidth("ScreenWidthPixels", Range(0,5)) = 1.0
		_ScreenHeight("ScreenHeightPixels", float) = 1.0
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 200
		Blend SrcAlpha OneMinusSrcAlpha
		ZWrite On

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog;
			#pragma target 3.0

			#include "UnityCG.cginc"

			struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };
 
            struct v2f
            {
                float4 vertex : SV_POSITION;
                UNITY_FOG_COORDS(1)
                float2 uv : TEXCOORD0;
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
                o.vertex = mul(unity_ObjectToWorld, v.vertex);
                o.vertex = dx *  floor(o.vertex / dx);
                o.vertex = UnityObjectToClipPos(mul(unity_WorldToObject, o.vertex));
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }
			fixed4 frag (v2f i) : SV_Target
			{
//   				float dx = _PixelHeight / _ScreenHeight;
//                i.uv = dx *  floor(i.uv / dx);
                fixed4 col = tex2D(_MainTex, i.uv);
				return col;
			}
			ENDCG
		}
	}
}
