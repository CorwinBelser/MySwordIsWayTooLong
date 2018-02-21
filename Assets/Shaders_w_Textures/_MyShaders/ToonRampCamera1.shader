Shader "Custom/ToonRampCamera1" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_Ramp ("Ramp (RGB)", 2D) = "white" {}
		_MainTex ("_MainTex", 2D) = "white" {}
		_ColorNoise ("ColorNoise", 2D) = "grey" {}
		_NoiseFilter ("NoiseGradient", 2D) = "grey" {}
	}

		SubShader
	{
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#pragma target 2.0
			
			#include "UnityCG.cginc"

		struct appdata_t {
        float4 vertex : POSITION;
        fixed4 color : COLOR;
 
        float2 uv : TEXCOORD0;
        float3 normal : NORMAL; // vertex normal
        UNITY_VERTEX_INPUT_INSTANCE_ID
    };
 
    struct v2f {
        float4 vertex : SV_POSITION;
        fixed4 color : COLOR;
        float2 uv : TEXCOORD0;
        float3 wpos : TEXCOORD1; // worldposition
        float3 normalDir : TEXCOORD2; // normal direction for rimlighting
 
    };

			sampler2D _MainTex;
			float4 _MainTex_ST;
			sampler2D _Ramp;
    		sampler2D _ColorNoise;
    		sampler2D _NoiseFilter;
    		fixed4 _Color;

			v2f vert (appdata_t v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;

				half3 noise = (tex2Dlod (_ColorNoise, float4(v.uv, 0 ,0)).rgb * .011);
				o.vertex.xyz += v.normal * noise;
				o.uv = TRANSFORM_TEX(v.uv,_MainTex);
        		// world position and normal direction
        		o.wpos = mul(unity_ObjectToWorld, v.vertex).xyz;
        		o.normalDir = normalize(mul(float4(v.normal, 0.0), unity_WorldToObject).xyz);
				return o;
			}

			fixed4 frag (v2f i) : SV_Target
			{
				float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.wpos.xyz);
				half diff = dot(viewDirection, i.normalDir) * .5 -.5;
        		half3 ramp = tex2D (_Ramp, float2(diff, diff)).rgb  * _Color;
        		half3 noise = (tex2D (_ColorNoise, i.wpos).rgb * .5 - .5);
        		half3 noiseFilter = (tex2D (_NoiseFilter, i.wpos).a * .5 - .5);
        		noise *= step(noiseFilter, abs(noise));
        		float4 col = tex2D(_MainTex, i.uv);
        		col.rgb = col.rgb * ramp + noise;
        		col.a = 1;
				return col;
			}
			ENDCG
		}
	}
}
