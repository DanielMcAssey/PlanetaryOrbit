float4x4 xWVP;
float3 xColour;

struct VertexShaderInput
{
    float4 Position : POSITION0;
};

struct VertexShaderOutput
{
    float4 Position : POSITION0;

};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output;

	output.Position = mul(input.Position,xWVP);

    return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
    return float4(xColour, 1);
}

technique StarColour
{
    pass Pass0
    {
        VertexShader = compile vs_2_0 VertexShaderFunction();
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}