Shader "Custom/Fire"
{
	Properties
	{
	_NoiseTex("Noise Texture", 2D) = "white" {}

	_Offset("FireOffset", Range(0,1)) = 0

	_GradientTex("Gradient Texture", 2D) = "white" {}

	_GradientWidthHeight("GradientWidthHeight", Int) = 1

	_PositionX("PositionX", Int) = 1
	_PositionY("PositionY", Int) = 1

	_BrighterCol("Brighter Color", Color) = (1,1,1,1)
	_MiddleCol("Middle Color", Color) = (.7,.7,.7,1)
	_DarkerCol("Darker Color", Color) = (.4,.4,.4,1)
	}

		SubShader
	{
		//The shader is transparent
		Tags
		{
		"RenderType" = "Transparent"
		}
		 Blend SrcAlpha OneMinusSrcAlpha
		 ZTest Always
		 ZWrite off
		 Cull off

		Pass
		{

		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		#include "UnityCG.cginc"
		#include "UnityShaderVariables.cginc" //to use _Time


		sampler2D _NoiseTex;

		float4 _BrighterCol;
		float4 _MiddleCol;
		float4 _DarkerCol;

		sampler2D _GradientTex;
		float4 _GradientTex_ST;

		//Input for the vertex
		struct appdata {
		float4 vertex : POSITION;
		float4 texcoord : TEXCOORD0;
		float2 uv2 : TEXCOORD1;
		};

		//Output for the fragment
		struct v2f {
		float4 pos : SV_POSITION;
		float2 uv : TEXCOORD0;
		float2 uv2 : TEXCOORD1;
		};

		int _PositionX;
		int _PositionY;
		float _GradientWidthHeight;

		v2f vert(appdata v) {
		v2f o;
		o.pos = UnityObjectToClipPos(v.vertex);
		o.uv = v.texcoord.xy;
		o.uv2 = TRANSFORM_TEX(v.uv2, _GradientTex);
		o.uv2 = v.uv2 / _GradientWidthHeight;
		o.uv2 += float2(_PositionX / _GradientWidthHeight, _PositionY / _GradientWidthHeight);

		return o;
		}


		fixed _Offset;

		float4 frag(v2f IN) : SV_Target 
			{
			float noiseValue = tex2D(_NoiseTex, IN.uv - float2(0, _Offset+_Time.x*4)).x; //fire with scrolling

			float gradientValue = tex2D(_GradientTex, IN.uv2).x;

			float step1 = step(noiseValue, gradientValue);
			float step2 = step(noiseValue, gradientValue - 0.2);
			float step3 = step(noiseValue, gradientValue - 0.4);

				//The entire fire color
				float4 c = float4
				(
				//Calculates where to place the darker color instead of the brighter one
				lerp
				(
				_BrighterCol.rgb,
				_DarkerCol.rgb,
				step1 - step2 //Corresponds to "L1" in my GIF
				),
	
				step1 //This is the alpha of our fire, which is the "outer" color, i.e. the step1
				);

				c.rgb = lerp //Calculates where to place the middle color
				(
				c.rgb,
				_MiddleCol.rgb,
				step2 - step3 //Corresponds to "L2" in my GIF
				);

			return c;
			}
			ENDCG
			}

	}
}