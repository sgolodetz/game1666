/***
 * game1666proto4: TerrainMultitexture.fx
 * Copyright 2011. All rights reserved.
 ***/

float4x4 World;
float4x4 View;
float4x4 Projection;

texture Texture0;
texture Texture1;

sampler TextureSampler0 = sampler_state {
	Texture = <Texture0>;
	MagFilter = Linear;
	MinFilter = Linear;
	MipFilter = Linear;
	AddressU = Wrap;
	AddressV = Wrap;
};

sampler TextureSampler1 = sampler_state {
	Texture = <Texture1>;
	MagFilter = Linear;
	MinFilter = Linear;
	MipFilter = Linear;
	AddressU = Wrap;
	AddressV = Wrap;
};

struct VertexShaderInput
{
    float4 Position : POSITION0;
	float2 TexCoords : TEXCOORD0;
};

struct VertexShaderOutput
{
    float4 ProjectedPosition : POSITION0;
	float2 TexCoords : TEXCOORD0;
	float4 WorldPosition : TEXCOORD1;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output;

	output.WorldPosition = mul(input.Position, World);
    float4 viewPosition = mul(output.WorldPosition, View);
    output.ProjectedPosition = mul(viewPosition, Projection);
	output.TexCoords = input.TexCoords;

    return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	float transitionHeight = 10.0f;
	float transitionHalfWidth = 4.0f;
	float transitionWidth = 2 * transitionHalfWidth;
	float transitionLow = transitionHeight - transitionHalfWidth;
	float transitionHigh = transitionHeight + transitionHalfWidth;

	float height = input.WorldPosition.z;
	float weight0, weight1;
	if(height < transitionLow)
	{
		weight0 = 1.0f;
		weight1 = 0.0f;
	}
	else if(height > transitionHigh)
	{
		weight0 = 0.0f;
		weight1 = 1.0f;
	}
	else
	{
		weight0 = (transitionHigh - height) / transitionWidth;
		weight1 = 1.0f - weight0;
	}

	return saturate(weight0 * tex2D(TextureSampler0, input.TexCoords) + weight1 * tex2D(TextureSampler1, input.TexCoords));
}

technique TerrainMultitexture
{
    pass Pass1
    {
        VertexShader = compile vs_2_0 VertexShaderFunction();
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}
