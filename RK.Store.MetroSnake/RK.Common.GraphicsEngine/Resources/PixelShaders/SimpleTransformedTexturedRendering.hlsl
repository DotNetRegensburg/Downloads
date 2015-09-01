//-----------------------------------------------------------------------------
//Constant buffers
cbuffer arguments                  : register(c0)
{
	float     Scaling;
	float     Dummy1;
	float     Dummy2;
	float	  Dummy3;
}

//Additional argumetns
Texture2D     ObjectTexture        : register(t0);
SamplerState  ObjectTextureSampler : register(s0);

//-----------------------------------------------------------------------------
//VertexShader input
struct VS_IN
{
	float3 pos      : POSITION;
	float2 texcoord : TEXCOORD0;
};

//PixelShader input
struct PS_IN
{
	float4 pos      : SV_POSITION;
	float2 texcoord : TEXCOORD0;
};

//-----------------------------------------------------------------------------
//PixelShader implementation
float4 main( PS_IN input ) : SV_Target
{
	//return tex2D(LinearSampler, input.texcoord);
	float4 result = ObjectTexture.Sample(ObjectTextureSampler, input.texcoord);
	
	//clip(result.a < 0.1 ? -1:1 );
	return result;
}