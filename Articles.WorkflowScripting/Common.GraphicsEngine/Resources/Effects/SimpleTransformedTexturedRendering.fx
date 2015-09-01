//*****************************************************************************
// SimpleRendering.fx effect file
//*****************************************************************************

float     Scaling;
texture2D ObjectTexture;

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
PS_IN VS(VS_IN input) 
{
	PS_IN output = (PS_IN)0;
	
	output.pos = float4(input.pos.xyz * Scaling, 1.0);
	output.texcoord = input.texcoord;

	return output;
}

//PixelShader implementation
float4 PS( PS_IN input ) : SV_Target
{
	//return tex2D(LinearSampler, input.texcoord);
	float4 result = ObjectTexture.Sample(LinearSampler, input.texcoord);
	
	//clip(result.a < 0.1 ? -1:1 );
	return result;
}

//-----------------------------------------------------------------------------
//Simple technique declaration
technique10 Render
{
	pass P0
	{
		SetGeometryShader( 0 );
		SetVertexShader( CompileShader( vs_4_0, VS() ) );
		SetPixelShader( CompileShader( ps_4_0, PS() ) );
		//VertexShader = compile vs_2_0 VS(); //( CompileShader( vs_3_0, VS() ) );
		//PixelShader  = compile ps_2_0 PS(); //( CompileShader( ps_3_0, PSColored() ) );
	}
}