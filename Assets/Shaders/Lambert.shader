Shader "Custom/Lambert" {
	Properties {
		_Color ("Color", Color) = (1.0, 1.0, 1.0, 1.0)
	}
	Subshader {
		pass {
			Tags { "LightMode" = "ForwardBase" }
			CGPROGRAM

			// pragmas
			#pragma vertex vert
			#pragma fragment frag

			// User defined variables
			uniform float4 _Color;
			uniform float4 _LightColor0;

			struct vertexInput {
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float4 color: COLOR;
			};
			struct vertexOutput {
				float4 pos : SV_POSITION;
				float4 col : COLOR;
			};

			// Vertex Program
			vertexOutput vert(vertexInput v) {
				vertexOutput o;

				float3 normalDirection = normalize(mul(float4(v.normal, 0.0), _World2Object).xyz);
				float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
				float attenuation = 1.0;

				float3 diffuseReflection = attenuation * _LightColor0.rgb
					* max(0.0, dot(normalDirection, lightDirection));
				float3 lightFinal = diffuseReflection * UNITY_LIGHTMODEL_AMBIENT.xyz;

				o.col = float4(lightFinal * v.color.rgb, 1.0);
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
				return o;
			}

			float4 frag(vertexOutput v) : COLOR {
				return v.col;
			}

			ENDCG
		}
	}
	Fallback "Diffuse"
}