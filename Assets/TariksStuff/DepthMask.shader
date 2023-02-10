// "Invisible" Unity Occlusion Shader. Useful for AR, Masking, etc
// Mark Johns / Doomlaser - https://twitter.com/Doomlaser

Shader "DepthMask"
{
    Properties
    {
    }
    SubShader
    {
        Tags
        {
            "RenderType" = "Opaque"
            "Queue" = "Geometry-1"
        }
        Pass
        {
            ColorMask 0

            CGPROGRAM
// Upgrade NOTE: excluded shader from DX11; has structs without semantics (struct v2f members pos)
#pragma exclude_renderers d3d11
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct v2f
            {
                float3 pos;
            };

            v2f vert(appdata_base v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }

            half4 frag(v2f i) : COLOR
            {
                return float4(1,1,1,1);
            }
            ENDCG
        }
    }
}
