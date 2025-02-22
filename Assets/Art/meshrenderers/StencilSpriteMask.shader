Shader "Custom/StencilSpriteMask"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}  // Sprite texture
        _Color ("Tint", Color) = (1,1,1,1)           // Tint color
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha  // Transparency support
        ZWrite Off
        Cull Off

        // Stencil buffer setup
        Stencil
        {
            Ref 1           // Reference value
            Comp Always     // Always write to stencil buffer
            Pass Replace    // Replace stencil value with 1
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
                fixed4 texColor = tex2D(_MainTex, i.uv) * i.color;
                return texColor;
            }
            ENDCG
        }
    }
}
