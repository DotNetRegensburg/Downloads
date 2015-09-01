//-----------------------------------------------------------------------------
//Constant buffers
cbuffer arguments                  : register(c0)
{
	float4x4  WorldViewProj;
	float4x4  World;
	float3    LightPosition;
	float     LightPower;
	float     StrongLightFactor;
	float     Ambient;
	float     Dummy1;
	float     Dummy2;
}

//-----------------------------------------------------------------------------
//VertexShader input
struct VS_IN
{
	float3 pos    : POSITION;
	float3 normal : NORMAL0;
	float4 col    : COLOR0;
	float2 tex    : TEXCOORD0;
};

//PixelShader input
struct PS_IN
{
	float4 pos    : SV_POSITION;
	float4 col    : COLOR0;
	float2 tex    : TEXCOORD0;
	float3 normal : TEXCOORD1;
	float3 pos3D  : TEXCOORD2;
};

//-----------------------------------------------------------------------------
//VertexShader implementation
PS_IN main(VS_IN input) 
{
	PS_IN output = (PS_IN)0;
	
	//Calculate standard values
	output.pos = mul(float4(input.pos.xyz, 1.0), WorldViewProj);
	output.col = input.col;
	output.tex = input.tex;
	
	//Calculate values needed for per-pixel lighting
	output.normal = normalize(mul(input.normal, (float3x3)World));
	output.pos3D = mul(float4(input.pos.xyz, 1.0), World);
     
	return output;
}
