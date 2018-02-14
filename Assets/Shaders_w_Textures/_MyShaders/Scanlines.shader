Shader "Custom/Scanlines"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_DistortTex ("DistortTexture", 2D) = "white" {}
		_Strength ("Strength", Range(0,1)) = -1
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

			float2 repeatY(in float2 _tile, in float repeat_times){
				float2 _output = _tile;
			    _output *= float2(1,repeat_times);
			    _output = frac(_output);
			    return _output;
			}

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;
			sampler2D _DistortTex;
			float _Strength;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 distortion = tex2D(_DistortTex, float2(frac((i.uv.y + -frac(_Time.x * 5)) * 5), i.uv.x));
				fixed xshift = distortion.r	 * _Strength;
//				fixed2 distorted = (float2(xshift, frac(i.uv.y * 5)));
				fixed4 col = tex2D(_MainTex, i.uv + float2(xshift, 0));
				// just invert the colors

				return col;
			}
			ENDCG
		}
	}
}
