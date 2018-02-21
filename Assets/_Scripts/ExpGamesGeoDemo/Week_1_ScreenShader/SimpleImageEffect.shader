Shader "OurCoolShaders/SimpleImageEffect"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			//two shaders, a function called vert and a function called frag
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			//These structs contain data, structs are similar to classes but fro exposed data
			struct appdata
			{
				float4 vertex : POSITION; //position of the current vertex
				float2 uv : TEXCOORD0; //first UV coordinate set
				//float4 color : COLOR; NORMAL TANGENT etc.
				float4 normal : NORMAL;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0; //first UV coordinate set
				float4 vertex : SV_POSITION;
				float4 normal : NORMAL;
			};
			//two functions a vert function...
			//called once for each vertecy in the object
			v2f vert (appdata v)
			{
				v2f o; //craete an instance of v2f named o
				//save our vertex data in O
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.normal = v.normal;
				//save our UV data in O, save it for later
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;
			//...and a frag function
			//runs once for every pixel
			//1920 * 1080 = 2,073,600... thats alot

			fixed4 frag (v2f i) : SV_Target
			{
				//This function takes in our texture and our current UV coordinates
				//although we have a texture component in our shader/material
				//the interceptor script will be replacing Maintex with our screen render
				
			
				fixed4 col = tex2D(_MainTex, i.uv + float2(0,(i.vertex.x*-i.vertex.x)/3000000));
				
				// just invert the colors
				//col = 1 - col;
				//col.b = 1;
				return col;
			}
			ENDCG
		}
	}
}
