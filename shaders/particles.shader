Shader "Custom/particles"
{
    Properties
    {

        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
        _Sat ("Saturation", Range(0.5,10)) = 1
        _Spec ("Specular", Range(0.0,1.0)) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "PerformanceChecks"="False"}
   

        CGPROGRAM
        #define PI 3.14159265359
        #pragma surface surf StandardSpecular vertex:vert addshadow fullforwardshadows
        #pragma instancing_options procedural:setup

 		sampler2D _MainTex;
        float _Sat;
        fixed4 _Color;
        float3 position;
        float3 dir;
        float s;
        half _Spec;

        struct Input {
			float2 uv_MainTex;
			float3 worldPos;
            float3 lightDir;
            float3 viewDir;
		};
  

        #ifdef UNITY_PROCEDURAL_INSTANCING_ENABLED
            struct Particle
            {
                float3 position;
                float dir;
                float scale; 
            };
            StructuredBuffer<Particle> ParticleBuffer; 
        #endif


        
        float3 RotateAroundAxis(float3 position, float3 axis, float angle)
        {    
            float s = sin(angle * (PI/180));
            float c = cos(angle * (PI/180));
            float oneMinusC = 1.0 - c;

            float3x3 rotationMatrix = float3x3(
            c + oneMinusC * axis.x * axis.x,
            oneMinusC * axis.x * axis.y - s * axis.z,
            oneMinusC * axis.x * axis.z + s * axis.y,

            oneMinusC * axis.y * axis.x + s * axis.z,
            c + oneMinusC * axis.y * axis.y,
            oneMinusC * axis.y * axis.z - s * axis.x,

            oneMinusC * axis.z * axis.x - s * axis.y,
            oneMinusC * axis.z * axis.y + s * axis.x,
            c + oneMinusC * axis.z * axis.z
            );

            return mul(rotationMatrix, position);
        }


        void vert(inout appdata_full v, out Input data)
        {
            UNITY_INITIALIZE_OUTPUT(Input, data);

            #ifdef UNITY_PROCEDURAL_INSTANCING_ENABLED
                v.vertex.xyz = RotateAroundAxis(v.vertex.xyz, float3(0,1,0), dir) * s;
                v.vertex.xyz +=  mul(unity_WorldToObject, float4(position, 1)).xyz;
            #endif
        }


        void setup()
        {
            #ifdef UNITY_PROCEDURAL_INSTANCING_ENABLED
                Particle party = ParticleBuffer[unity_InstanceID];
                position = party.position;
                dir = party.dir;
                s = party.scale;
            #endif
        }


        void surf (Input IN, inout SurfaceOutputStandardSpecular o)
        {
            
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Sat;
            fixed dotL = dot(IN.lightDir, o.Normal);
            fixed dotV = dot(IN.viewDir, o.Normal);
            o.Albedo = c.rgb + dotL;
            o.Specular = _Spec * (1-dotV);
        }
        ENDCG
    }
    FallBack "Diffuse"
}
