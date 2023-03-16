#ifndef SAMPLESCREEN_INCLUDED
#define SAMPLESCREEN_INCLUDED

// #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
// #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/API/D3D11.hlsl"

// #define PI 3.141592654

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

float sqr(float x)
{
    return x * x;
}

// float Gaussian1D(float x, float stdev)
// {
//     return rsqrt(2 * PI * sqr(stdev)) * exp(-sqr(x) / (2 * sqr(stdev)));
// }

float Gaussian2D(float x, float y, float stdev)
{
    // return Gaussian1D(x, stdev) * Gaussian1D(y, stdev);
    return exp(-(sqr(x) + sqr(y)) / (2 * sqr(stdev))) / (2 * PI * sqr(stdev));
}

void GaussianBlur_float(UnityTexture2D tex, float2 uv, UnitySamplerState ss,
                        float stdev, float k, float n, out float3 color)
{
    color = float3(0, 0, 0);
    const float step_size = k * stdev / n;
    float weight_sum = 0;
    for (int x_i = 0; x_i < n; x_i++)
    {
        for (int y_i = 0; y_i < n; y_i++)
        {
            const float delta_x = step_size * (x_i - n / 2);
            const float delta_y = step_size * (y_i - n / 2);
            float2 position = float2(uv.x + delta_x, uv.y + delta_y);
            float weight = Gaussian2D(delta_x, delta_y, stdev);
            color += SAMPLE_TEXTURE2D(tex, ss, position) * weight;
            weight_sum += weight;
        }
    }
    color /= weight_sum;
}

#endif
