//*****************************************************************************
// SimpleNormalMappedRendering.fx effect file
//*****************************************************************************

float4x4  WorldViewProj     : WORLDVIEWPROJ;
float4x4  World             : WORLD;
float3    LightPosition;
float     LightPower;
float     Ambient;
float     StrongLightFactor;
Texture2D ObjectTexture;
Texture2D NormalMapTexture;

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
	float3 pos      : POSITION;
	float3 normal   : NORMAL0;
	float3 tangent  : TANGENT0;
	float3 binormal : BINORMAL0;
	float4 col      : COLOR0;
	float2 tex      : TEXCOORD0;
};

//PixelShader input
struct PS_IN
{
	float4 pos         : SV_POSITION;
	float4 col         : COLOR0;
	float2 tex         : TEXCOORD0;
	float3 lightVector : TEXCOORD1;
	float  att         : TEXCOORD2;
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
	
	//Calculate position of the vertex in the world
	float4 posInWorld = mul(float4(input.pos.xyz, 1.0), World);
	
	//Get vertex's light vector
	float3 vertexLightVector = normalize(LightPosition - posInWorld);
	
	//Calculate texture space matrix
	float3x3 TextureMatrix = float3x3(input.tangent, input.binormal, input.normal);

	//Calculate output values
	output.att = 1 / (1 + (0.005 * distance(LightPosition.xyz, posInWorld) ) );
	output.pos = mul(float4(input.pos.xyz, 1.0), WorldViewProj);
	output.col = input.col;
	output.tex = input.tex;
    output.lightVector = mul(TextureMatrix, vertexLightVector);
    
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
	//Get color from diffuse texture
	float4 baseColor = ObjectTexture.Sample(LinearSampler, input.tex);
	
	//Get normal from normal map
	float3 normal = NormalMapTexture.Sample(LinearSampler, input.tex).xyz * 2.0 - 1.0;
	
	//Normalize given transformed light vector
	float3 light = normalize(input.lightVector);
	
	//Caluclate diffuse component
	float diffuse = saturate(dot(normal, light));
	diffuse = 1.0 - diffuse / StrongLightFactor;
	
	//Calculate result color
	float4 result = input.att * baseColor * diffuse * Ambient;
	
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