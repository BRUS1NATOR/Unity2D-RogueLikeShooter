Shader "Custom/Flash"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white"{}
		_Color("Color", Color) = (1,1,1,1)
	}
		SubShader
		{
		    Tags{"Queue" = "Transparent"}
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
			fixed4 _Color;

			float4 frag(v2f i) : SV_Target
			{
				float4 color = tex2D(_MainTex,i.uv);
				if (color.a != 0) {
					color = _Color;
				}

				return color;
			}

			ENDCG
			}
		}
}