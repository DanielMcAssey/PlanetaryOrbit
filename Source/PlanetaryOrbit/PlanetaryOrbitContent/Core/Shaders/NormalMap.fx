float4x4 xWorld;
float4x4 xView;
float4x4 xProjection;
float4x4 xWorldInverseTranspose;
float3 xCameraPosition;

float4 xAmbientColor = float4(1, 1, 1, 1);
float xAmbientIntensity;

float4 xDiffuseColor = float4(1, 1, 1, 1);
float xDiffuseIntensity;

float3 xLightDirection = float3(1, 0, 0);

float xSpecularIntensity = 32;
float4 xSpecularColor = float4(1, 1, 1, 1);

texture ModelTexture;
sampler2D ColorMapSampler = sampler_state {
    Texture = (ModelTexture);
    MinFilter = Anisotropic;
    MagFilter = Anisotropic;
	MipFilter = Linear;
    AddressU = Clamp;
    AddressV = Clamp;
	MaxAnisotropy = 16;
};

texture NormalMap;
sampler2D NormalMapSampler = sampler_state {
    Texture = (NormalMap);
    MinFilter = Anisotropic;
    MagFilter = Anisotropic;
	MipFilter = Linear; 
    AddressU = Clamp;
    AddressV = Clamp;
	MaxAnisotropy = 16;
};

struct VertexShaderInput
{
    float4 Position : POSITION0;
	float2 TextureCoordinate : TEXCOORD0;
	float3 Normal : NORMAL0;
	float3 Binormal : BINORMAL0;
    float3 Tangent : TANGENT0;
};

struct VertexShaderOutput
{
    float4 Position : POSITION0;
    float2 TextureCoordinate : TEXCOORD0;
    float3 View : TEXCOORD2;
	float3x3 WorldToTangentSpace : TEXCOORD4;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
	VertexShaderOutput output;
     
    float4 worldPosition = mul(input.Position, xWorld);
    float4 viewPosition = mul(worldPosition, xView);
    output.Position = mul(viewPosition, xProjection);
    output.TextureCoordinate = input.TextureCoordinate;
 
    output.WorldToTangentSpace[0] = mul(normalize(input.Tangent), xWorld);
    output.WorldToTangentSpace[1] = mul(normalize(input.Binormal), xWorld);
    output.WorldToTangentSpace[2] = mul(normalize(input.Normal), xWorld);
     
    output.View = normalize(float4(xCameraPosition,1.0) - worldPosition);
 
    return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR
{
	float4 color = tex2D(ColorMapSampler, input.TextureCoordinate);
    float3 NormalMap = (2 *(tex2D(NormalMapSampler, input.TextureCoordinate))) - 1.0;
    NormalMap = normalize(mul(NormalMap, input.WorldToTangentSpace));
    float4 Normal = float4(NormalMap,1.0);

    float4 diffuse = saturate(dot(xLightDirection,Normal));
    float4 reflect = normalize(2*diffuse*Normal-float4(-xLightDirection,1.0));
    float4 specular = pow(saturate(dot(reflect,input.View)),xSpecularIntensity);
 
    return  color * xAmbientColor * xAmbientIntensity + 
            color * xDiffuseIntensity * xDiffuseColor * diffuse + 
            color * xSpecularColor * specular;
}

technique Normal
{
    pass Pass0
    {
        VertexShader = compile vs_2_0 VertexShaderFunction();
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}
