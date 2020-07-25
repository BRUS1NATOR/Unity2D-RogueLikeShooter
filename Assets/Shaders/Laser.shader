Shader "Custom/Laser"
{
    Properties
    {
		_MainTex("Texture", 2D) = "white"{}
		_InsideColor("Inside", Color) = (1,1,1,1)
		_OutsideColor("OutSide", Color) = (1,1,1,0.5)
	}
		SubShader
		{
			Tags
			{
				"Queue" = "Transparent"
			}

			Pass{
				Cull Off
				Blend SrcAlpha OneMinusSrcAlpha
				CGPROGRAM

				#pragma	vertex vertexFunc			//Сначала вызывается vertex функция, в ней задается "Форма объекта"
				#pragma fragment fragmentFunc		//Затем вызывается fgarment функция, в ней происходит отрисовка пикселей
				#include "UnityCG.cginc"

			sampler2D _MainTex;

			struct v2f {
				float4 pos : SV_POSITION;
				half2 uv : TEXCOORD0;
			};

			v2f vertexFunc(appdata_base v) {
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = v.texcoord;
				return o;
			}

			fixed4 _InsideColor;
			fixed4 _OutsideColor;

			fixed4 fragmentFunc(v2f i) : COLOR
			{
				float4 c = tex2D(_MainTex, i.uv);
				if (c.a == 0) {
					discard;
				}
				else if (c.b == 1) {
					c = _OutsideColor;
					c += (0.1, 0.1, 0.1);
				}
				else if (c.g == 1) {
					c = _OutsideColor;
				}
				else if (c.r == 1) {
					c = _OutsideColor;
					c.a = 0.4f;
				}
				else {
					c = _InsideColor;
				}

				return c;
			}

			ENDCG
		} 
	}
}
