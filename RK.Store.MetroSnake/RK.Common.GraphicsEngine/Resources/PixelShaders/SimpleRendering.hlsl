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
//Calculates the dot product
float DotProduct(float3 lightPos, float3 pos3D, float3 normal)
{
    float3 lightDir = normalize(pos3D - lightPos);
    return dot(-lightDir, normal);    
}

//-----------------------------------------------------------------------------
//PixelShader implementation
float4 main( PS_IN input ) : SV_Target
{
	//Calculate simple lightning effect
	float diffuseLightingFactor = DotProduct(LightPosition, input.pos3D, input.normal);
	diffuseLightingFactor = saturate(diffuseLightingFactor);
	diffuseLightingFactor *= LightPower;
	diffuseLightingFactor = 1.0 - (1.0 - diffuseLightingFactor) / StrongLightFactor;
	
	//Calculate output color 
	float lastAlpha = input.col.a;
	float4 result = input.col * clamp(diffuseLightingFactor + Ambient, 0.0, 1.0);
	result.a = 1;

	//clip(result.a < 0.1 ? -1:1 );
	
	return result;
}