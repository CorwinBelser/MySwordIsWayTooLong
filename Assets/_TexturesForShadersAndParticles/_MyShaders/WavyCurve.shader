Shader "Custom/WavyCurve"
{
	Properties
	{
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Main Texture", 2D) = "white" {}
		_DistortTex ("Distort Texture", 2D) = "white" {}
		_AlphaRamp ("Alpha Ramp", 2D) = "white" {}
		_ExpandRate ("Rate", Range(0,.2)) = .05
	}
	SubShader
	{
		Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha

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
				float4 normal: NORMAL;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertexObjPos : TEXCOORD1;
				float4 vertex : SV_POSITION;
			};

			sampler2D _DistortTex;
			sampler2D _MainTex;
			sampler2D _AlphaRamp;
			float _ExpandRate;
			float4 _Color;

			v2f vert (appdata v)
			{
				v2f o;
				o.uv = v.uv;
				o.vertexObjPos = v.vertex;
				o.vertex = UnityObjectToClipPos(v.vertex);
				fixed4 dist = tex2Dlod(_DistortTex, float4((o.uv + float2(frac(_Time.x), frac(_Time.x))), 0, 0));

				o.vertex += (dist.r * 2 - 1) * v.vertex * .2;
				return o;
			}

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv) * _Color;
				float alpha = length(float2(i.vertexObjPos.x, i.vertexObjPos.y));
				fixed4 alphaRamp = tex2D(_AlphaRamp, float2(.5, alpha - _Time.w * _ExpandRate ));
				col.a *= alphaRamp;
				return col;
			}
			ENDCG
		}
	}
}
