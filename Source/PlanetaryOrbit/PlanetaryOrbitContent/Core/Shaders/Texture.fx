float4x4 xWorld;
float4x4 xView;
float4x4 xProjection;
float4x4 xWorldInverseTranspose;
float3 xCameraPosition;
float4 xAmbientColor = float4(0.1f, 0.1f, 0.1f, 0.0f);
float3 xLightDirection = float3(-1.0f, 0.0f, 0.0f);
float3 xLightColor		= float3(0.9, 0.9, 0.9);
float4 xDiffuseColor = float4(1.0f, 1.0f, 1.0f, 1.0f);
float xSpecularIntensity = 32;
float4 xSpecularColor = float4(1, 1, 1, 1);    

texture ModelTexture;
sampler2D TextureSampler = sampler_state {
    Texture = (ModelTexture);
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
    float4 Normal : NORMAL0;
    float2 TextureCoordinate : TEXCOORD0;
};
 
struct VertexShaderOutput
{
    float4 Position : POSITION0;
    float2 TextureCoordinate : TEXCOORD0;
	float3 Normal : TEXCOORD1;
	float3 ViewDirection : TEXCOORD2;
};
 
VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output;
 
    float4 worldPosition = mul(input.Position, xWorld);

	float4x4 viewProjection = mul(xView, xProjection);
    output.Position = mul(worldPosition, viewProjection);

	output.TextureCoordinate = input.TextureCoordinate;

	output.Normal = mul(input.Normal, xWorldInverseTranspose);
	output.ViewDirection = worldPosition - xCameraPosition;

    return output;
}
 
float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	float3 color = xDiffuseColor;

	color *= tex2D(TextureSampler, input.TextureCoordinate);

	float3 lighting = xAmbientColor;
	float3 lightDir = normalize(xLightDirection);
	float3 normal = normalize(input.Normal);
	float3 view = normalize(input.ViewDirection);
	lighting += saturate(dot(lightDir, normal)) * xLightColor;
	float3 refl = reflect(lightDir, normal);
	lighting += pow(saturate(dot(refl, view)), xSpecularIntensity) * xSpecularColor;
	float3 output = saturate(lighting) * color;

	return float4(output, 1);
}
 
technique Textured
{
    pass Pass0
    {
        VertexShader = compile vs_2_0 VertexShaderFunction();
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}