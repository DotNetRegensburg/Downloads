//*****************************************************************************
// SimpleDisplacedRendering.fx effect file
//*****************************************************************************

float4x4  WorldViewProj : WorldViewProjection;
float4x4  World;
float3    LightPosition;
float     LightPower;
float     Ambient;
float     DisplaceFactor;
Texture2D ObjectTexture;
Texture2D DisplacementTexture;

//-----------------------------------------------------------------------------
//SamplerState for the texture
SamplerState LinearSampler
{
	Filter = MIN_MAG_MIP_LINEAR;
	AddressU = Wrap;
	AddressV = Wrap;
};

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
//Calculates the dot product
float DotProduct(float3 lightPos, float3 pos3D, float3 normal)
{
    float3 lightDir = normalize(pos3D - lightPos);
    return dot(-lightDir, normal);    
}

//-----------------------------------------------------------------------------
//VertexShader implementation
PS_IN VS(VS_IN input) 
{
	PS_IN output = (PS_IN)0;
	
	//Calculate displacement
	float displaceHeight = DisplacementTexture.SampleLevel(LinearSampler, input.tex, 0).x;
	float3 position = input.pos + input.normal * displaceHeight * DisplaceFactor;

	//Calculate standard values
	output.pos = mul(float4(position.xyz, 1.0), WorldViewProj);
	output.col = input.col;
	output.tex = input.tex;
	
	//Calculate values needed for per-pixel lighting
	output.normal = normalize(mul(input.normal, (float3x3)World));
	output.pos3D = mul(float4(input.pos.xyz, 1.0), World);
     
	return output;
}

//PixelShader implementation
float4 PSColored( PS_IN input ) : SV_Target
{
	return input.col;
}

//PixelShader implementation
float4 PSTextured( PS_IN input ) : SV_Target
{
	float diffuseLightingFactor = DotProduct(LightPosition, input.pos3D, input.normal);
	diffuseLightingFactor = saturate(diffuseLightingFactor);
	diffuseLightingFactor *= LightPower;
	
	float4 baseColor = ObjectTexture.Sample(LinearSampler, input.tex);
	float4 result = baseColor * clamp(diffuseLightingFactor + Ambient, 0.0, 1.0);
	
	//clip(result.a < 0.1 ? -1:1 );
	return result;
}

//-----------------------------------------------------------------------------
//Simple technique declaration
technique10 Render
{
	//Rendering using diffuse colors
	pass P0
	{
		SetGeometryShader( 0 );
		SetVertexShader( CompileShader( vs_4_0, VS() ) );
		SetPixelShader( CompileShader( ps_4_0, PSColored() ) );
	}
	
	//Rendering using textures
	pass P1
	{
		SetGeometryShader( 0 );
		SetVertexShader( CompileShader( vs_4_0, VS() ) );
		SetPixelShader( CompileShader( ps_4_0, PSTextured() ) );
	}
}