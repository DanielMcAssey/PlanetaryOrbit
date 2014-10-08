float4x4 wvp : WorldViewProjection;
float4x4 xWorld;
float3 xCameraPosition; 

float xAmbientIntensity = 1;
float4 xAmbientColor : AMBIENT = float4(0,0,0,1);

float3 xLightDirection : Direction = float3(0,1,1);

float time : Time;

float cloudSpeed = .0025;
float cloudHeight = .005;
float cloudShadowIntensity = 1;

texture ModelTexture : Diffuse;
sampler ColorMapSampler = sampler_state 
{
    texture = <ModelTexture>;
	MinFilter = Anisotropic;
    MagFilter = Anisotropic;
	MipFilter = Linear;
    AddressU = Clamp;
    AddressV = Clamp;
    MaxAnisotropy = 16;
};

texture GlowMap : Diffuse;
sampler GlowMapSampler = sampler_state 
{
    texture = <GlowMap>;
	MinFilter = Anisotropic;
    MagFilter = Anisotropic;
	MipFilter = Linear;
    AddressU = Clamp;
    AddressV = Clamp;
	MaxAnisotropy = 16;
};

texture NormalMap;
sampler BumpMapSampler = sampler_state
{
	texture = <NormalMap>;
	MinFilter = Anisotropic;
    MagFilter = Anisotropic;
	MipFilter = Linear;
    AddressU = Clamp;
    AddressV = Clamp;
	MaxAnisotropy = 16;
};

texture SpecularMap : Diffuse;
sampler ReflectionMapSampler = sampler_state 
{
    texture = <SpecularMap>;
	MinFilter = Anisotropic;
    MagFilter = Anisotropic;
	MipFilter = Linear;
    AddressU = Clamp;
    AddressV = Clamp;
	MaxAnisotropy = 16;
};

texture CloudMap;
sampler CloudMapSampler = sampler_state
{
	texture = <CloudMap>;
	MinFilter = Anisotropic;
    MagFilter = Anisotropic;
	MipFilter = Linear;
    AddressU = Wrap;
    AddressV = Wrap;
	MaxAnisotropy = 16;
};

texture WaveMap;
sampler WaveMapSampler = sampler_state
{
	texture = <WaveMap>;
	MinFilter = Anisotropic;
    MagFilter = Anisotropic;
	MipFilter = Linear;
    AddressU = Clamp;
    AddressV = Clamp;
	MaxAnisotropy = 16;
};

texture AtmosMap;
sampler AtmosMapSampler = sampler_state
{
	texture = <AtmosMap>;
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
	float2 TexCoord : TEXCOORD0;
	float3 Normal : NORMAL;
	float3 Tangent : TANGENT;
};

struct VertexShaderOutput
{
    float4 Position : POSITION0;
	float2 TexCoord : TEXCOORD0;
	float3 Light : TEXCOORD1;
	float3 CamView : TEXCOORD2;
	float4 posS : TEXCOORD3;
	float3 Normal : TEXCOORD4;
};

struct VertexShaderOutput2
{
	float4 Position : POSITION;
	float2 TexCoord : TEXCOORD0;
	float3 Normal : TEXCOORD1;
	float4 pos : TEXCOORD2;
};

struct PixelShaderOutput
{
	float4 Color : COLOR;
};

VertexShaderOutput2 VS_OuterAtmoshpere(VertexShaderInput input,uniform float size)
{
	VertexShaderOutput2 output = (VertexShaderOutput2)0;
	output.Normal = mul(input.Normal, xWorld);
	output.Position = mul(input.Position, wvp) + (mul(size, mul(input.Normal, wvp)));
	output.TexCoord = input.TexCoord;
	output.pos = mul(input.Position,xWorld);
	
	return output;
}

float2 RotateRight(float2 coord,float time)
{
	coord.x -= time;
		
	return coord;
}

float2 MoveInCircle(float2 texCoord,float time,float speed)
{
	float2 texRoll = texCoord;
	texRoll.x += cos(time*speed);
	texRoll.y += sin(time*speed);
	
	return texRoll;
}

VertexShaderOutput VS_Color(VertexShaderInput input)
{
	VertexShaderOutput output = (VertexShaderOutput)0;
	
	output.Position = mul(input.Position,wvp);
	
	float3x3 worldToTangentSpace;
	worldToTangentSpace[0] = mul(input.Tangent,xWorld);
	worldToTangentSpace[1] = mul(cross(input.Tangent,input.Normal),xWorld);
	worldToTangentSpace[2] = mul(input.Normal,xWorld);
	
	float4 PosWorld = mul(input.Position,xWorld);
	
	output.Light = mul(worldToTangentSpace,xLightDirection);	
	output.CamView = xCameraPosition - mul(input.Position,xWorld);
	
	output.posS = input.Position;
	
	output.TexCoord = input.TexCoord;
	
	output.Normal = mul(input.Normal,xWorld);
	
	return output;
}

PixelShaderOutput PS_Color(VertexShaderOutput input)
{
	PixelShaderOutput output = (PixelShaderOutput)0;
	
	float3 Normal = (2 * (tex2D(BumpMapSampler,input.TexCoord))) - 1.0;
	
	float3 LightDir = normalize(input.Light);
	float Diffuse = saturate(dot(LightDir,Normal));
		
	float4 texCol = tex2D(ColorMapSampler,input.TexCoord);
	float4 glowCol = tex2D(GlowMapSampler,input.TexCoord);
	
	float4 Ambient = xAmbientIntensity * xAmbientColor;
	
	float4 glow = glowCol * saturate(1-Diffuse);	
	
	texCol *= Diffuse;
	float3 regN = normalize(input.Normal);
	float3 Half = normalize(normalize(xLightDirection) + normalize(input.CamView));	
	float specular = pow(saturate(dot(regN,Half)),32);
	float4 specCol = .75 * tex2D(ReflectionMapSampler,input.TexCoord) * (specular * Diffuse);
	
	float shadowHeight = cloudHeight * dot(regN,normalize(input.Light));
	
	float2 shadowAngle = input.TexCoord - float2(shadowHeight,shadowHeight);
	float2 cloudCoordS = RotateRight(shadowAngle,time*cloudSpeed);	
	
	float cloudShadow = (-tex2D(CloudMapSampler,cloudCoordS).a) * cloudShadowIntensity;
	cloudShadow *= Diffuse;	
	
	output.Color =  (Ambient + texCol + 0 + cloudShadow +  glow + specCol);
	
	return output;
}

PixelShaderOutput PS_Water(VertexShaderOutput input)
{
	PixelShaderOutput output = (PixelShaderOutput)0;
	
	float3 LightDir = normalize(input.Light);
	
	float3 Normal = (2 * (tex2D(WaveMapSampler,MoveInCircle(input.TexCoord,time,.001) * 50))) - 1.0;	
	float Diffuse = saturate(dot(LightDir,Normal));
	float4 wav = tex2D(ReflectionMapSampler,input.TexCoord) * Diffuse;
	
	output.Color = wav * .5;
	
	return output;
}

PixelShaderOutput PS_Cloud(VertexShaderOutput2 input)
{
	PixelShaderOutput output = (PixelShaderOutput)0;

	float3 Normal = normalize(input.Normal);
	
	float3 LightDir = normalize(xLightDirection);
	float Diffuse = saturate(dot(LightDir,Normal));

	float2 cloudCoord = RotateRight(input.TexCoord,time*cloudSpeed);
	
	float4 clouds = tex2D(CloudMapSampler,cloudCoord);	
	float4 cloudsN = tex2D(CloudMapSampler,cloudCoord);
		
	clouds *= Diffuse;	
	cloudsN *= saturate(.25-Diffuse);
	
	output.Color =  (clouds + cloudsN) * 2;
	
	return output;
}

PixelShaderOutput PS_OuterAtmoshpere(VertexShaderOutput2 input, uniform bool flip)
{
	PixelShaderOutput output = (PixelShaderOutput)0;

	float4 atmos = tex2D(AtmosMapSampler,input.TexCoord);

	float3 regN = normalize(input.Normal);
	float3 Half = normalize(normalize(xCameraPosition-input.pos) + normalize(xCameraPosition - input.pos));	
	float specular = 0;
	
	//float Diffuse = 1-saturate(dot(normalize(xLightDirection),-regN))*4; //Need to Fix
		
	if(flip) {		
		specular = (saturate(1-dot(regN,Half))*.125);
		atmos.a *= specular;
	} else {
		specular = 1-saturate(1.1 + dot(regN,Half));
		atmos *= specular;
	}
	
	output.Color = atmos;// * Diffuse;
	
	return output;
}

technique EarthShader
{
	pass Colors
	{
		AlphaBlendEnable = False;
		CullMode = CCW;
		VertexShader = compile vs_2_0 VS_Color();
		PixelShader = compile ps_2_0 PS_Color();		
	}

	pass Waves
	{
		AlphaBlendEnable = True;
        SrcBlend = SrcAlpha;
        DestBlend = One;
        
		PixelShader = compile ps_2_0 PS_Water();
	}

	pass Clouds
	{
        VertexShader = compile vs_2_0 VS_OuterAtmoshpere(.02);
		PixelShader = compile ps_2_0 PS_Cloud();
	}

	pass OuterAtmosphere
	{
		DestBlend = InvSrcAlpha;
		PixelShader = compile ps_2_0 PS_OuterAtmoshpere(true);
	}

	pass UpperOuterAtmosphere
	{
		VertexShader = compile vs_2_0 VS_OuterAtmoshpere(.2);
		PixelShader = compile ps_2_0 PS_OuterAtmoshpere(false);
		CullMode = CW;
	}
}
