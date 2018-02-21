Shader "Custom/JamPixelShader"
{
	Properties
	{
		_MainTex ("MainTexture", 2D) = "white" {}
		_ColorTex ("ColorStepsTexture", 2D) = "white" {}
		_Strength ("Strength", Range(-1,1)) = -1
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

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
				float4 vertex : SV_POSITION;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;
			sampler2D _ColorTex;
			float _Strength;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				col.r = tex2D(_ColorTex, float2(col.r, .5)).r;
				col.a = 1;
				// just invert the colors
				return col;
			}
			ENDCG
		}
	}
}
