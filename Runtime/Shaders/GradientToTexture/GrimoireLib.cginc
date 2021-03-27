
#ifndef INTERPOLATION     
#define INTERPOLATION

/* //hlsl supports linear interpolation intrinsically so this isn't needed
float lerp(float from, float to, float rel){
  return ((1 - rel) * from) + (rel * to);
}
*/

// -----------------------------------------------------
// INVERSE LERP
// -----------------------------------------------------

float invLerp(float from, float to, float value)
{
    return (value - from) / (to - from);
}

float2 invLerp(float2 from, float2 to, float2 value)
{
    return float2(
        invLerp(from.x, to.x, value.x),
        invLerp(from.y, to.y, value.y)
    );
}

float3 invLerp(float3 from, float3 to, float3 value)
{
    return float3(
        invLerp(from.x, to.x, value.x),
        invLerp(from.y, to.y, value.y),
        invLerp(from.z, to.z, value.z)
    );
}

float4 invLerp(float4 from, float4 to, float4 value)
{
    return float4(
        invLerp(from.x, to.x, value.x),
        invLerp(from.y, to.y, value.y),
        invLerp(from.z, to.z, value.z),
        invLerp(from.w, to.w, value.w)
    );
}

// -----------------------------------------------------
// REMAP
// -----------------------------------------------------

float remap(float inFrom, float inTo, float outFrom, float outTo, float value)
{
    value = invLerp(inFrom, outFrom, value);
    return lerp(outFrom, outTo, value);
}

float4 remap(float4 inFrom, float4 inTo, float4 outFrom, float4 outTo, float4 value)
{
    value = invLerp(inFrom, outFrom, value);
    return lerp(outFrom, outTo, value);
}

#endif