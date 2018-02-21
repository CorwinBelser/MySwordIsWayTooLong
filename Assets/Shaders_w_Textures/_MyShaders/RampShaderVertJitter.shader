Shader "Custom/RampShaderVertJitter" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_Ramp ("Ramp (RGB)", 2D) = "white" {}
		_MainTex ("_MainTex", 2D) = "white" {}
//		_ColorNoise ("ColorNoise", 2D) = "grey" {}
		_NoiseAmount ("NoiseAmount", Range(0,.2)) = 0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

	CGPROGRAM
    #pragma surface surf Ramp vertex:vert

    sampler2D _Ramp;
//    sampler2D _ColorNoise;
    float _NoiseAmount;
    fixed4 _Color;

    struct Input {
        float2 uv_MainTex;
    };

    half4 LightingRamp (SurfaceOutput s, half3 lightDir, half atten) {
        half NdotL = dot (s.Normal, lightDir);
        half diff = (NdotL * .5 + .5);
        half3 ramp = tex2D (_Ramp, float2(diff,diff)).rgb  * _Color;
        half4 c;
        c.rgb = _LightColor0.rgb * ramp * atten;
        c.a = s.Alpha;
        return c;
    }

    void vert (inout appdata_full v, out Input o) {
    	  UNITY_INITIALIZE_OUTPUT(Input,o);
    	  half4 noise = sin(_Time.w + v.vertex.y) * 2 - 1;
          v.vertex.xyz += v.normal * _NoiseAmount * noise.a;
    }
    
    sampler2D _MainTex;


    
    void surf (Input IN, inout SurfaceOutput o) {
        o.Albedo = tex2D (_MainTex, IN.uv_MainTex).rgb;
    }
    ENDCG
	}
	FallBack "Diffuse"
}
