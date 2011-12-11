/***
 * game1666proto4: TerrainMultitexture.fx
 * Copyright 2011. All rights reserved.
 ***/

/*
Description:

This shader is used to multitexture terrains. Lower altitudes are rendered using one texture,
higher altitudes using another. The transition between the two regions is smoothly blended.
*/

//#################### INPUT VARIABLES ####################

// The current world matrix.
float4x4 World;

// The current view matrix.
float4x4 View;

// The current projection matrix.
float4x4 Projection;

// The texture to use for lower altitudes (e.g. "grass").
texture Texture0;

// The texture to use for higher altitudes (e.g. "snow").
texture Texture1;

// Half the width of the transition zone between the two altitude bands.
float TransitionHalfWidth;

// The altitude of the middle of the transition zone.
float TransitionHeight;

//#################### OTHER VARIABLES ####################

// The sampler for the lower altitude texture.
sampler TextureSampler0 = sampler_state {
	Texture = <Texture0>;
	MagFilter = Linear;
	MinFilter = Linear;
	MipFilter = Linear;
	AddressU = Wrap;
	AddressV = Wrap;
};

// The sampler for the higher altitude texture.
sampler TextureSampler1 = sampler_state {
	Texture = <Texture1>;
	MagFilter = Linear;
	MinFilter = Linear;
	MipFilter = Linear;
	AddressU = Wrap;
	AddressV = Wrap;
};

//#################### DATA STRUCTURES ####################

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

//#################### VERTEX SHADER ####################

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output;

	output.WorldPosition = mul(input.Position, World);
    float4 viewPosition = mul(output.WorldPosition, View);
    output.ProjectedPosition = mul(viewPosition, Projection);
	output.TexCoords = input.TexCoords;

    return output;
}

//#################### PIXEL SHADER ####################

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	float transitionLow = TransitionHeight - TransitionHalfWidth;
	float transitionHigh = TransitionHeight + TransitionHalfWidth;

	float height = input.WorldPosition.z;
	float weight0, weight1;
	if(height < transitionLow)
	{
		// If we're below the transition zone, use the lower altitude texture only.
		weight0 = 1.0f;
		weight1 = 0.0f;
	}
	else if(height > transitionHigh)
	{
		// If we're above the transition zone, use the higher altitude texture only.
		weight0 = 0.0f;
		weight1 = 1.0f;
	}
	else
	{
		// If we're in the transition zone, blend the two textures together depending on our height.
		float transitionWidth = 2 * TransitionHalfWidth;
		weight0 = (transitionHigh - height) / transitionWidth;
		weight1 = 1.0f - weight0;
	}

	return saturate(weight0 * tex2D(TextureSampler0, input.TexCoords) + weight1 * tex2D(TextureSampler1, input.TexCoords));
}

//#################### TECHNIQUES ####################

technique TerrainMultitexture
{
    pass Pass1
    {
        VertexShader = compile vs_2_0 VertexShaderFunction();
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}
