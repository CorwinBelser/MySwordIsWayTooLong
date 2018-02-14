﻿Shader "Custom/redmoodyshader" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_Ramp ("Ramp (RGB)", 2D) = "white" {}
		_MainTex ("_MainTex", 2D) = "white" {}
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

	CGPROGRAM
    #pragma surface surf Ramp

    sampler2D _Ramp;
    fixed4 _Color;

    half4 LightingRamp (SurfaceOutput s, half3 lightDir, half atten) {
        half NdotL = dot (s.Normal, lightDir);
        half diff = (NdotL * .5 + .5);
        half3 ramp = tex2D (_Ramp, float2(diff,.5)).rgb;
        half4 c;
        c.rgb = _LightColor0.rgb * atten;
//        c.rgb = (NdotL * .5 + .5) * atten;
//		c. rgb = ramp;
//        c.rgb = .2 * floor(c.rgb / .2);
        c.a = s.Alpha;
        return c;
    }

    struct Input {
        float2 uv_MainTex;
    };
    
    sampler2D _MainTex;
    
    void surf (Input IN, inout SurfaceOutput o) {
//        o.Albedo = tex2D (_MainTex, IN.uv_MainTex).rgb;
    }
    ENDCG
	}
	FallBack "Diffuse"
}
