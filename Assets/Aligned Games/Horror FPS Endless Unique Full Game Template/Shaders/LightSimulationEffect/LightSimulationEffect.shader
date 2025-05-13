Shader "Custom/MovingLightEffect"
{
    Properties
    {
        _Color("Color", Color) = (1,1,1,1)
        _FresnelPower("Fresnel Power", Range(0.1, 5)) = 2
        _Opacity("Opacity", Range(0,1)) = 0.5
        _NoiseTex1("Noise Texture 1", 2D) = "white" {}
        _NoiseStrength1("Noise Strength 1", Range(0,1)) = 0.3
        _NoiseTex2("Noise Texture 2", 2D) = "white" {}
        _NoiseStrength2("Noise Strength 2", Range(0,1)) = 0.3
        _Speed("Base Speed", Range(0, 5)) = 1
        _NoiseRotation("Rotation Noise Strength", Range(0, 1)) = 0.2
    }
        SubShader
        {
            Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
            Pass
            {
                Blend SrcAlpha OneMinusSrcAlpha
                ZWrite Off
                Cull Back

                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #include "UnityCG.cginc"

                struct appdata_t
                {
                    float4 vertex : POSITION;
                    float3 normal : NORMAL;
                    float2 uv : TEXCOORD0;
                };

                struct v2f
                {
                    float4 pos : SV_POSITION;
                    float3 viewDir : TEXCOORD0;
                    float3 normal : TEXCOORD1;
                    float2 uv : TEXCOORD2;
                    float2 noiseUV : TEXCOORD3;
                };

                float4 _Color;
                float _FresnelPower;
                float _Opacity;
                sampler2D _NoiseTex1;
                float _NoiseStrength1;
                sampler2D _NoiseTex2;
                float _NoiseStrength2;
                float _Speed;
                float _NoiseRotation;

                // Rotate UV function
                float2 RotateUV(float2 uv, float angle)
                {
                    float s = sin(angle);
                    float c = cos(angle);
                    float2x2 rotationMatrix = float2x2(c, -s, s, c);
                    return mul(rotationMatrix, uv - 0.5) + 0.5;
                }

                v2f vert(appdata_t v)
                {
                    v2f o;
                    o.pos = UnityObjectToClipPos(v.vertex);
                    o.viewDir = normalize(WorldSpaceViewDir(v.vertex));
                    o.normal = UnityObjectToWorldNormal(v.normal);

                    // Generate slight variation per object
                    float instanceVariation = frac(dot(v.vertex.xyz, float3(0.31, 0.47, 0.89))) * 0.5 + 0.75;
                    float adjustedSpeed = _Speed * instanceVariation;

                    // Sample noise for rotation variation
                    o.noiseUV = v.uv * 2.0; // Scale noise sampling
                    float noiseValue = tex2Dlod(_NoiseTex1, float4(o.noiseUV, 0, 0)).r * 2.0 - 1.0;
                    float noiseAngle = _NoiseRotation * noiseValue;

                    // Apply rotation with noise
                    float angle = (_Time.y * adjustedSpeed) + noiseAngle;
                    o.uv = RotateUV(v.uv, angle);

                    return o;
                }

                fixed4 frag(v2f i) : SV_Target
                {
                    // Compute Fresnel Effect
                    float fresnel = pow(1.0 - saturate(dot(i.viewDir, i.normal)), _FresnelPower);

                // Sample first Noise Texture
                float noise1 = tex2D(_NoiseTex1, i.uv).r * _NoiseStrength1;

                // Sample second Noise Texture
                float noise2 = tex2D(_NoiseTex2, i.uv).r * _NoiseStrength2;

                // Blend both noise textures into Fresnel effect
                fresnel = smoothstep(0.0, 1.0, fresnel + noise1 + noise2);

                // Calculate final alpha
                float alpha = fresnel * _Opacity;

                return float4(_Color.rgb, alpha);
            }
            ENDCG
        }
        }
}