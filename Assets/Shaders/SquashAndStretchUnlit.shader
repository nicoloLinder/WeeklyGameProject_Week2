// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "PicoTanks/SquashAndStretchUnlit"
{
	Properties
	{
		_Color("Main color", Color) = (1,1,1,1)
		_Squash("Squash", Float) = 0
		_Radius("Radius", Float) = 1
		_SquashEffect("SquashEffect", Float) = 1
		_SquashCurve("SquashCurve", Float) = 0
		_StretchEffect("StretchEffect", Float) = 1
		_StretchCurve("StretchCurve", Float) = 0
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		struct Input
		{
			float3 worldNormal;
		};

		uniform fixed _Radius;
		uniform fixed _StretchCurve;
		uniform fixed _StretchEffect;
		uniform fixed _Squash;
		uniform fixed _SquashEffect;
		uniform fixed _SquashCurve;
		uniform float4 _Color;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float3 ase_vertex3Pos = v.vertex.xyz;
			float3 break49_g1 = ase_vertex3Pos;
			float xPos52_g1 = break49_g1.x;
			float yPos50_g1 = break49_g1.y;
			float Radius329 = _Radius;
			float3 ase_objectScale = float3( length( unity_ObjectToWorld[ 0 ].xyz ), length( unity_ObjectToWorld[ 1 ].xyz ), length( unity_ObjectToWorld[ 2 ].xyz ) );
			float temp_output_22_0_g1 = ( 1.0 - ( ( sin( ( ( ( abs( yPos50_g1 ) / ( Radius329 / ase_objectScale.x ) ) * ( 0.5 * UNITY_PI ) ) - ( 0.5 * UNITY_PI ) ) ) + 1.0 ) / 2.0 ) );
			float StretchCurve333 = _StretchCurve;
			float StretchEffect332 = _StretchEffect;
			float Squash328 = _Squash;
			float SquashInput77_g1 = Squash328;
			float clampResult18_g1 = clamp( SquashInput77_g1 , -10.0 , 1.0 );
			float Squas23_g1 = clampResult18_g1;
			float lerpResult41_g1 = lerp( 1.0 , ( ( 1.0 - ( temp_output_22_0_g1 * StretchCurve333 ) ) * StretchEffect332 ) , ( atan( ( abs( Squas23_g1 ) * 2.0 ) ) / ( 0.5 * UNITY_PI ) ));
			float StretchMultiplierXZZ45_g1 = lerpResult41_g1;
			float SquashEffect330 = _SquashEffect;
			float SquashCurve331 = _SquashCurve;
			float lerpResult38_g1 = lerp( 0.0 , ( SquashEffect330 * ( 1.0 - ( SquashCurve331 * temp_output_22_0_g1 ) ) ) , Squas23_g1);
			float SquashMultiplierXZZ43_g1 = ( lerpResult38_g1 + 1.0 );
			float clampResult66_g1 = clamp( ( ( Squas23_g1 * 1000.0 ) + 0.5 ) , 0.0 , 1.0 );
			float lerpResult69_g1 = lerp( StretchMultiplierXZZ45_g1 , SquashMultiplierXZZ43_g1 , clampResult66_g1);
			float SquashMultiplierYY47_g1 = ( ( 1.0 - Squas23_g1 ) * yPos50_g1 );
			float zPos51_g1 = break49_g1.z;
			float3 appendResult75_g1 = (fixed3(( xPos52_g1 * lerpResult69_g1 ) , SquashMultiplierYY47_g1 , ( zPos51_g1 * lerpResult69_g1 )));
			v.vertex.xyz = appendResult75_g1;
		}

		inline fixed4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return fixed4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			o.Emission = _Color.rgb;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Unlit keepalpha fullforwardshadows noambient novertexlights nolightmap  nodynlightmap nodirlightmap nofog nometa noforwardadd vertex:vertexDataFunc 

		ENDCG
		
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}