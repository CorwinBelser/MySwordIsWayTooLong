Shader "Custom/RampShaderWTexture2" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_Ramp ("Ramp (RGB)", 2D) = "white" {}
		_MainTex ("_MainTex", 2D) = "white" {}
		_ColorNoise ("ColorNoise", 2D) = "grey" {}
		_NoiseHardness ("NoiseHardness", Range(0,1)) = 0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

	CGPROGRAM
    #pragma surface surf Ramp

    sampler2D _Ramp;
    sampler2D _ColorNoise;
    fixed4 _Color;
    float _NoiseHardness;

     half4 LightingRamp (SurfaceOutput s, half3 lightDir, half atten) {
        half NdotL = dot (s.Normal, lightDir);
        half diff = (NdotL * .5 + .5);
        half3 ramp = tex2D (_Ramp, float2(diff,diff)).rgb  * _Color;
        half4 c;
        c.rgb = _LightColor0.rgb * ramp * atten;
        c.a = s.Alpha;
        return c;
    }

    struct Input {
        float2 uv_MainTex;
    };
    
    sampler2D _MainTex;
    
    void surf (Input IN, inout SurfaceOutput o) {
        half3 noise = (tex2D (_ColorNoise, IN.uv_MainTex).rgb * 2 - 1);
        noise *= step(_NoiseHardness, abs(noise));
        o.Albedo = tex2D (_MainTex, IN.uv_MainTex).rgb + noise;
    }
    ENDCG
	}
	FallBack "Diffuse"
}
