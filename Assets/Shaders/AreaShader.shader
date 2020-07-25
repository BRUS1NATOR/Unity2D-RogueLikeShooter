// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Area" {
	Properties{
		_MainTex("Base (RGB)", 2D) = "white" {}
		_Color("Color", Color) = (1,1,1,0.5)
	}

		SubShader{
		   Tags { "Queue" = "Transparent"}

		   Pass {
			  Stencil {
				Ref 1
				Comp NotEqual
				Pass Replace
			   }

				   Blend SrcAlpha OneMinusSrcAlpha
		   ZTest Always
				   ZWrite off
				   Cull off


				   CGPROGRAM
				   #pragma vertex vert
				   #pragma fragment frag
				   #include "UnityCG.cginc"

				   uniform sampler2D _MainTex;

				   struct v2f {
					   half4 pos : POSITION;
					   half2 uv : TEXCOORD0;
				   };

				   v2f vert(appdata_img v) {
					   v2f o;
					   o.pos = UnityObjectToClipPos(v.vertex);
					   half2 uv = MultiplyUV(UNITY_MATRIX_TEXTURE0, v.texcoord);
					   o.uv = uv;
					   return o;
				   }

				   half4 _Color;

				   half4 frag(v2f i) : COLOR
				   {
					   half4 color = tex2D(_MainTex, i.uv);

						   color.a *= _Color.a;
						   if (color.a == 0) {
							   discard;
						   }
					   return color;
				   }
				   ENDCG
			  }
			
	   }

		Fallback off
}