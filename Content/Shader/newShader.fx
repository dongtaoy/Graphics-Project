float4x4 lwvp;
float4x4 wvp;
float4x4 xWorld;
float3 xLightPos;
float xLightPower = float(2);
float xAmbient = float(0.2);

texture2D meshTexture;
texture2D shadowTexture;

struct VS_IN
{
	float4 Position : SV_POSITION;
};

struct PS_IN
{
	float4 Position: SV_POSITION;
	float4 Position2D : TEXCOORD0;
};

sampler MeshTextureSampler =
sampler_state
{
	Texture = <g_MeshTexture>;
	MipFilter = LINEAR;
	MinFilter = LINEAR;
	MagFilter = LINEAR;
};

sampler ShadowMapSampler =
sampler_state
{
	Texture = <g_ShadowMapTexture>;
	MinFilter = POINT;
	MagFilter = POINT;
	MipFilter = POINT;
	AddressU = Clamp;
	AddressV = Clamp;
};



PS_IN VS(VS_IN input)
{
	PS_IN output = (PS_IN)0;
	output.Position = mul(input.Position, lwvp);
	output.Position2D = output.Position;
	return output;
}

float4 PS(PS_IN input) : SV_TARGET
{
	return input.Position2D.z / input.Position2D.w;
}


struct PS_INSCENE
{
	float4 Position : SV_POSITION;
	float4 Pos2DAsSeenByLight : TEXCOORD0;
	float3 Normal : TEXCOORD1;
	float4 Position3D : TEXCOORD2;
	float2 TexCoords : TEXCOORD3;
};

PS_INSCENE ShadowedSceneVertexShader( float4 inPos : SV_POSITION, float2 inTexCoords : TEXCOORD0, float3 inNormal : NORMAL)
{
    PS_INSCENE output = (PS_INSCENE)0;

    output.Position = mul(inPos, wvp);    
    output.Pos2DAsSeenByLight = mul(inPos, lwvp);    
    output.Normal = normalize(mul(inNormal, (float3x3)xWorld));    
    output.Position3D = mul(inPos, xWorld);
    output.TexCoords = inTexCoords;    

    return output;
}


float DotProduct(float3 lightPos, float3 pos3D, float3 normal)
{
    float3 lightDir = normalize(pos3D - lightPos);
    return dot(-lightDir, normal);    
}

float4 ShadowedScenePixelShader(PS_INSCENE PSIn) : SV_TARGET
{
    float2 ProjectedTexCoords;
    ProjectedTexCoords[0] = PSIn.Pos2DAsSeenByLight.x/PSIn.Pos2DAsSeenByLight.w/2.0f +0.5f;
    ProjectedTexCoords[1] = -PSIn.Pos2DAsSeenByLight.y/PSIn.Pos2DAsSeenByLight.w/2.0f +0.5f;
    
    float diffuseLightingFactor = 0;
    if ((saturate(ProjectedTexCoords).x == ProjectedTexCoords.x) && (saturate(ProjectedTexCoords).y == ProjectedTexCoords.y))
    {
        float depthStoredInShadowMap = meshTexture.Sample(ShadowMapSampler, ProjectedTexCoords).r;
        float realDistance = PSIn.Pos2DAsSeenByLight.z/PSIn.Pos2DAsSeenByLight.w;
        if ((realDistance - 1.0f/100.0f) <= depthStoredInShadowMap)
        {
            diffuseLightingFactor = DotProduct(xLightPos, PSIn.Position3D, PSIn.Normal);
            diffuseLightingFactor = saturate(diffuseLightingFactor);
            diffuseLightingFactor *= xLightPower;            
        }
    }
        
    float4 baseColor = shadowTexture.Sample(MeshTextureSampler, PSIn.TexCoords);                
    return baseColor*(diffuseLightingFactor + xAmbient);

}


technique ShadowMap
{
	pass Pass1
	{
		Profile = 9.3;
		VertexShader = VS;
		PixelShader = PS;
	}
}

technique ShaderRender
{
	pass Pass1
	{
		VertexShader = ShadowedSceneVertexShader;
		PixelShader = ShadowedScenePixelShader;
	}
}