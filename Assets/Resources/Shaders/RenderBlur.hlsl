#ifndef SAMPLESCREEN_INCLUDED
#define SAMPLESCREEN_INCLUDED

// #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
// #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/API/D3D11.hlsl"

void LinearBlur_float(UnityTexture2D tex, float2 uv, UnitySamplerState ss,
                      float r, float n, out float3 color)
{
    color = float3(0, 0, 0);
    const float step_size = r / n;
    for (int x_i = 0; x_i < n; x_i++)
    {
        for (int y_i = 0; y_i < n; y_i++)
        {
            float2 position = float2(
                uv.x + step_size * (x_i - n / 2),
                uv.y + step_size * (y_i - n / 2)
            );
            color += SAMPLE_TEXTURE2D(tex, ss, position) / pow(n, 2);
        }
    }
}

#endif
