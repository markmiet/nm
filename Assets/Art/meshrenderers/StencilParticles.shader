Shader "Custom/StencilParticles"
{
    Properties
    {
        _MainTex ("Particle Texture", 2D) = "white" {}  // Particle texture
        _Color ("Particle Color", Color) = (1,1,1,1)    // Particle Tint
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha  // Transparency blending
        ZWrite Off
        Cull Off

        // Use stencil buffer to mask particles
        Stencil
        {
            Ref 1       // Match reference from sprite shader
            Comp Equal  // Only render where stencil value is 1 (sprite area)
            Pass Keep
        }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 color : COLOR;
            };

            sampler2D _MainTex;
            fixed4 _Color;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.color = v.color * _Color;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv) * i.color;
                return col;
            }
            ENDCG
        }
    }
}
