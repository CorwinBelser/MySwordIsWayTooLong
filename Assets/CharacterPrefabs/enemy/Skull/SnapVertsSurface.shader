// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/SnapVertsSurface" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_Ramp ("Ramp (RGB)", 2D) = "white" {}
		_MainTex ("_MainTex", 2D) = "white" {}
		_PixelWidth("VertIntervalPixels", Range(0,1)) = .2
		_PixelHeight("PixelIntervalPixels", float) = 1.0
		_ScreenWidth("ScreenWidthPixels", Range(0,10)) = 5.0
		_ScreenHeight("ScreenHeightPixels", float) = 1.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

	CGPROGRAM
    #pragma surface surf Ramp vertex:vert

    sampler2D _Ramp;
    fixed4 _Color;
    struct Input {
        float2 uv_MainTex;
    };
    
    sampler2D _MainTex;
    float _PixelWidth;
    float _PixelHeight;
    float _ScreenWidth;
    float _ScreenHeight;

    half4 LightingRamp (SurfaceOutput s, half3 lightDir, half atten) {
        half NdotL = dot (s.Normal, lightDir);
        half diff = (NdotL * .5 + .5);
        half3 ramp = tex2D (_Ramp, float2(diff,.5)).rgb;
       	half4 c = float4(_LightColor0.rgb * ramp * atten * _Color, 1);
        return c;
    }

    void vert (inout appdata_full v) {
        float dx = _PixelWidth / _ScreenWidth;
        v.vertex.xyz = mul(unity_ObjectToWorld, v.vertex.xyz);
        v.vertex.xyz = dx *  floor(v.vertex.xyz / dx);
        v.vertex.xyz = mul(unity_WorldToObject, v.vertex.xyz);
    }

    void surf (Input IN, inout SurfaceOutput o) {
        o.Albedo = tex2D (_MainTex, IN.uv_MainTex).rgb;
    }
    ENDCG
	}
	FallBack "Diffuse"
}
