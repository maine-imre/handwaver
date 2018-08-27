Shader "Custom/Equirectangular" {
	Properties {
		_Color ("Tint", Color ) = (1,1,1,1)
		_MainTex ("Base (RGB)", 2D) = "white" {}
	}
	SubShader {
		Tags { "RenderType"="Opaque" "Queue"="Overlay+50" }
		LOD 200
		ZTest Always
		Fog { Mode Off }
		
		Cull Off
		
		CGPROGRAM
		#pragma target 3.0
#pragma surface surf None exclude_path:prepass nolightmap noforwardadd novertexlights

		sampler2D _MainTex;

		half4 _Color;

		struct Input {
			float2 uv_MainTex;
			float3 viewDir;
		};

		inline fixed4 LightingNone(SurfaceOutput s, fixed3 lightDir, fixed atten)
		{
			fixed4 c;
			c.rgb = s.Albedo;
			c.a = s.Alpha;
			return c;
		}


		void surf (Input IN, inout SurfaceOutput o) {

			const float PI = 3.1415926;

			float3 n;

			n = -IN.viewDir;

			n = normalize( n );
			float vA = acos( n.y );

			float hA;
			
			hA = asin( n.x / ( sin( vA ) ) );

			if( abs( n.z ) > abs( n.x ) )
			{
				if( n.z >= 0 )
				{
					hA = PI - asin( n.x / ( sin( vA ) ) );
				}
				else
				{
					hA = asin( n.x / ( sin( vA ) ) );
				}
			}
			else
			{
				hA = acos( n.z / ( sin( vA ) ) );

				if( n.x >= 0 )
				{
					hA = PI - hA;
				}
				else
				{
					hA = PI + hA;
				}
			}

			float2 uv;

			uv.x = 1.0 - ( hA / ( 2 * PI ) );
			uv.y = 1.0 - ( hA / PI );

			uv = fmod( uv, 1.0 );
			
			half4 c = tex2D ( _MainTex, uv ) * _Color;

			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
				