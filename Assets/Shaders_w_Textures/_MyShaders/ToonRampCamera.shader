Shader "Custom/ToonRampCamera" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_Ramp ("Ramp (RGB)", 2D) = "white" {}
		_MainTex ("_MainTex", 2D) = "white" {}
		_ColorNoise ("ColorNoise", 2D) = "grey" {}
		_NoiseFilter ("NoiseGradient", 2D) = "grey" {}
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

	CGPROGRAM
    #pragma surface surf Ramp

    sampler2D _Ramp;
    sampler2D _ColorNoise;
    sampler2D _NoiseFilter;
    fixed4 _Color;

     half4 LightingRamp (SurfaceOutput s, half3 lightDir, half atten) {
//        half NdotL = dot (IN.viewDir;, lightDir);
//        half diff = (NdotL * .5 + .5);
//        half3 ramp = tex2D (_Ramp, float2(diff,diff)).rgb  * _Color;
        half4 c;
        c.rgb = _LightColor0.rgb  * atten;
        c.a = s.Alpha;
        return c;
    }

    struct Input {
        float2 uv_MainTex;
        float3 viewDir;
        float4 screenPos;
    };
    
    sampler2D _MainTex;
    
    void surf (Input IN, inout SurfaceOutput o) {
    	half NdotL = dot (normalize(IN.viewDir), o.Normal);
        half diff = (NdotL * .5 + .5);
        half3 ramp = tex2D (_Ramp, float2(diff,diff)).rgb  * _Color;
        half3 noise = (tex2D (_ColorNoise, IN.uv_MainTex).rgb * .5 - .5);
        half3 noiseFilter = (tex2D (_NoiseFilter, IN.uv_MainTex).a * .5 - .5);
        noise *= (noise - noiseFilter * .3);
        o.Albedo = tex2D (_MainTex, frac(IN.uv_MainTex)).rgb * ramp + noise;
    }
    ENDCG
	}
	FallBack "Diffuse"
}
