// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Outline" {
	Properties{
		_Color("Main Color", Color) = (.5,.5,.5,1)
		_OutlineColor("Outline Color", Color) = (0,0,0,1)
		_Outline("Outline width", Range(0, 1)) = .1
		_MainTex("Base (RGB)", 2D) = "white" { }
	}

		SubShader
	{

					ZTest Always
		Cull Off
		ZTest Off
		Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
		CGPROGRAM

		#pragma	vertex vert			//Сначала вызывается vertex функция, в ней задается "Форма объекта"
		#pragma fragment frag		//Затем вызывается fgarment функция, в ней происходит отрисовка пикселей
		#include "UnityCG.cginc"

		struct appdata
		{
			float4 vertex : POSITION;
			float2 uv : TEXCOORD0;
		};

		struct v2f
		{
			float4 vertex : POSITION;
			float2 uv : TEXCOORD0;
		};

		v2f vert(appdata v) {
			v2f o;
			o.vertex = UnityObjectToClipPos(v.vertex);
			o.uv = v.uv;
			return o;
		}

		sampler2D _MainTex;
		fixed4 _OutlineColor;
		fixed4 _Color;
		float _Outline;

		float4 frag(v2f i) : SV_Target
		{
			float4 color = tex2D(_MainTex,i.uv);

			if (i.uv.x > 1 - _Outline || i.uv.y > 1 - _Outline || i.uv.x < _Outline|| i.uv.y < _Outline) {
				color = _OutlineColor;
			}
			else {
				color = _Color;
			}

			return color;
		}

		ENDCG
		}
	}
}