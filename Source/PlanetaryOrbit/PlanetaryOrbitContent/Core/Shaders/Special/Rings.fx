float4x4 xWorld;
float4x4 xView;
float4x4 xProjection;
float4x4 xWorldInverseTranspose;

float3 xLightDirection;
float3 xCameraPosition;

float4 xAmbientColor = float4(1.0, 1.0, 1.0, 1.0);
float xAmbientIntensity;

float4 xDiffuseColor = float4(1.0, 1.0, 1.0, 1.0);
float xDiffuseIntensity;

texture ColourMap;
sampler ColourMapSampler = sampler_state 
{
    texture = <ColourMap>;
    MinFilter = Anisotropic;
    MagFilter = Anisotropic;
	MipFilter = Linear;
	AddressU	= Clamp;  
	AddressV	= Clamp;
	MaxAnisotropy = 16;
};

texture AlphaMap;
sampler AlphaMapSampler = sampler_state 
{
    texture = <AlphaMap>;
    MinFilter = Anisotropic;
    MagFilter = Anisotropic;
	MipFilter = Linear;
	AddressU	= Clamp;
	AddressV	= Clamp;
	MaxAnisotropy = 16;
};


struct VertexShaderInput
{
    float4 Position : POSITION0;
    float2 TexCoord : TEXCOORD0;
    float3 Normal : NORMAL;
};

struct VertexShaderOutput
{
    float4 Position : POSITION0;
	float2 TexCoord : TEXCOORD0;
	float3 Light    : TEXCOORD1;
	float3 Normal    : TEXCOORD2;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output;

    float4 worldPosition = mul(input.Position, xWorld);
    float4 viewPosition = mul(worldPosition, xView);
    output.Position = mul(viewPosition, xProjection);
    output.TexCoord = input.TexCoord * 5;
	output.Light = normalize(xLightDirection);
	output.Normal = normalize(mul(xWorldInverseTranspose, input.Normal));

    return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	float2 NewTexCoord = input.TexCoord;
	NewTexCoord = (NewTexCoord-0.5)/2;
	NewTexCoord.x = length(NewTexCoord)*4;
	NewTexCoord.y = 0;

	float4 Color = tex2D(ColourMapSampler, NewTexCoord);
	//float diffuse = saturate(dot(input.Light,input.Normal));
	
	//Color = (Color * xAmbientIntensity * xAmbientColor) + (Color * xDiffuseIntensity * xDiffuseColor * diffuse);
	//Color.a = tex2D(AlphaMapSampler, NewTexCoord).r;
	
	return Color;
}

technique RingShader
{
    pass Pass0
    {
        AlphaBlendEnable = true;
		SrcBlend = SrcAlpha;
		DestBlend = InvSrcAlpha;

		Sampler[0] = (ColourMapSampler);
		Sampler[1] = (AlphaMapSampler);

        VertexShader = compile vs_2_0 VertexShaderFunction();
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}
