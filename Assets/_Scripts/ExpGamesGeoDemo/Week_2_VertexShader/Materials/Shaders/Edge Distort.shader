// Upgrade NOTE: replaced 'PositionFog()' with transforming position into clip space.

Shader "Custom/EdgeDistort"
{
	Properties
	{
		_Color("Main Color", Color) = (1,1,1,1)
		_RimColor("Rim Color", Color) = (1, 1, 1, 1)
		_RimWidth("Rim Width", Float) = 0.7
		_MainTex("Base (RGB)", 2D) = "white" {}
		_NoiseTex("Noise Texture", 2D) = "white" {}
		_DistortMod("Distortion Modifier", Float) = 50
	
	}
    SubShader
    {
        // Draw ourselves after all opaque geometry
        Tags { "Queue" = "Transparent" }

        // Grab the screen behind the object into _BackgroundTexture
        GrabPass
        {
            "_BackgroundTexture"
        }

        // Render the object with the texture generated above, and invert the colors
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

			struct appdata {
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float2 texcoord : TEXCOORD0;
			};

            struct v2f
            {
				//V2F_POS_FOG;
                float4 grabPos : TEXCOORD1;
                float4 pos : SV_POSITION;
				float3 worldNormal : TEXCOORD2;
				float2 uv : TEXCOORD3;
				float3 color : COLOR;


            };
			uniform float4 _MainTex_ST;
			uniform float4 _RimColor;
			uniform float _RimWidth;

            v2f vert(appdata_base v, float3 normal : NORMAL){
                v2f o;
				o.pos = UnityObjectToClipPos (v.vertex);
				float3 viewDir = normalize(ObjSpaceViewDir(v.vertex));
				float dotProduct = 1 - dot(v.normal, viewDir);
				float rimWidth = _RimWidth;
				o.color = smoothstep(1 - rimWidth, 1.0, dotProduct);

				o.color *= _RimColor;

				o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
                // use UnityObjectToClipPos from UnityCG.cginc to calculate 
                // the clip-space of the vertex
                o.pos = UnityObjectToClipPos(v.vertex);
                // use ComputeGrabScreenPos function from UnityCG.cginc
                // to get the correct texture coordinate
                o.grabPos = ComputeGrabScreenPos(o.pos);
				o.worldNormal = UnityObjectToWorldNormal(normal);

				
                return o;
            }

            sampler2D _BackgroundTexture;
			sampler2D _NoiseTex;
			float _DistortMod;
			uniform sampler2D _MainTex;
			uniform float4 _Color;

            float4 frag(v2f i) : SV_Target
            {
			
				float offset = float (tex2D(_NoiseTex, float2(i.pos.x / _DistortMod,i.pos.y/ _DistortMod) ).r);
				float4 bgcolor = tex2Dproj(_BackgroundTexture, i.grabPos+ i.color.r );
				float4 texcol = tex2D(_MainTex, i.uv);
				
				
                return bgcolor;
            }
            ENDCG
        }

    }
}