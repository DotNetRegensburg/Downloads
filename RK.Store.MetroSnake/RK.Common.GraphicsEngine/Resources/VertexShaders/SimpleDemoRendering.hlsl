//-----------------------------------------------------------------------------
//Constant buffers
cbuffer arguments                  : register(c0)
{
    float4x4  WorldViewProj;
}

//-----------------------------------------------------------------------------
//VertexShader input
struct VS_IN
{
    float3 pos    : POSITION;
    float4 col    : COLOR0;
};

//PixelShader input
struct PS_IN
{
    float4 pos    : SV_POSITION;
    float4 col    : COLOR0;
};

//-----------------------------------------------------------------------------
//VertexShader implementation
PS_IN main(VS_IN input) 
{
    PS_IN output = (PS_IN)0;
    
    //Calculate standard values
    output.pos = mul(float4(input.pos.xyz, 1.0), WorldViewProj);
    output.col = input.col;

    return output;
}
