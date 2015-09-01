//-----------------------------------------------------------------------------
//Constant buffers
cbuffer arguments                  : register(c0)
{
	float     Scaling;
	float     Dummy1;
	float     Dummy2;
	float	  Dummy3;
}

//-----------------------------------------------------------------------------
//VertexShader input
struct VS_IN
{
	//float3 pos      : POSITION;
	//float2 texcoord : TEXCOORD0;
	float3 pos      : POSITION;
	float3 normal   : NORMAL0;
	float4 col      : COLOR0;
	float2 texcoord : TEXCOORD0;
};

//PixelShader input
struct PS_IN
{
	float4 pos      : SV_POSITION;
	float2 texcoord : TEXCOORD0;
};

//-----------------------------------------------------------------------------
//VertexShader implementation
PS_IN main(VS_IN input) 
{
	PS_IN output = (PS_IN)0;
	
	output.pos = float4(input.pos.xyz * Scaling, 1.0);
	output.texcoord = input.texcoord;

	return output;
}