//-----------------------------------------------------------------------------
//Constant buffers
cbuffer arguments                  : register(c0)
{
    float4x4  WorldViewProj;
}

//-----------------------------------------------------------------------------
//PixelShader input
struct PS_IN
{
    float4 pos    : SV_POSITION;
    float4 col    : COLOR0;
};

//-----------------------------------------------------------------------------
//PixelShader implementation
float4 main( PS_IN input ) : SV_Target
{
    return input.col;
}