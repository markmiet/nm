Shader "Custom/CubismLine"
{
    Properties
    {
        _MainTex ("Line Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)

        _TileCount ("Tiles (per axis)", Float) = 32
        _Jitter ("Jitter (0..1)", Range(0,1)) = 0.3
        _Rotation ("Rotation (radians)", Range(0,6.28318)) = 1.0
        _ColorNoise ("Color Noise", Range(0,1)) = 0.1
        _Seed ("Seed", Float) = 0.0

        _TileSizeVar ("Tile Size Variation", Range(0,1)) = 0.2
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 100

        Cull Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            float4 _MainTex_ST;
            float4 _Color;

            float _TileCount;
            float _Jitter;
            float _Rotation;
            float _ColorNoise;
            float _Seed;
            float _TileSizeVar;

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = TRANSFORM_TEX(IN.uv, _MainTex);
                OUT.color = IN.color * _Color;
                return OUT;
            }

            float hash12(float2 p)
            {
                return frac(sin(dot(p, float2(12.9898,78.233))) * 43758.5453);
            }

            float2 hash22(float2 p)
            {
                float h1 = frac(sin(dot(p, float2(127.1,311.7))) * 43758.5453);
                float h2 = frac(sin(dot(p, float2(269.5,183.3))) * 43758.5453);
                return float2(h1,h2);
            }

            half4 frag(Varyings IN) : SV_Target
            {
                float2 uv = IN.uv;

                // --- Base tiling ---
                float2 baseTiles = float2(_TileCount, _TileCount);
                float2 cell = floor(uv * baseTiles);
                float2 cellUV = frac(uv * baseTiles);

                // --- Tile size variation ---
                float sizeRand = hash12(cell + _Seed + 1.2345);
                float scale = lerp(1.0 - _TileSizeVar, 1.0 + _TileSizeVar, sizeRand);
                cellUV = frac(cellUV * scale);

                // --- Random offsets/rotation ---
                float2 r2 = hash22(cell + _Seed);
                float r1 = hash12(cell + _Seed + 0.1234);

                float2 jitter = (r2 - 0.5) * _Jitter;
                float angle = (r1 - 0.5) * _Rotation;

                float s = sin(angle), c = cos(angle);
                float2 centered = cellUV - 0.5;
                float2 rotated = float2(centered.x * c - centered.y * s,
                                        centered.x * s + centered.y * c);

                float2 sampleUVinCell = rotated + 0.5 + jitter;
                sampleUVinCell = saturate(sampleUVinCell);

                float2 finalUV = (cell + sampleUVinCell) / baseTiles;

                // --- Sample texture ---
                half4 col = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, finalUV) * IN.color;

                // --- Color noise ---
                float colorVariation = (hash12(cell + _Seed + 0.4321) - 0.5) * _ColorNoise;
                col.rgb *= (1.0 + colorVariation);

                return col;
            }
            ENDHLSL
        }
    }
}
