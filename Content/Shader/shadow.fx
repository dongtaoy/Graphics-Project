float4x4 lwvp;

float3 g_LightPos;					// Position of light
float3 g_LightDir = float3(0, -4, 0);					// Direction of light (temp)
float4x4 g_mLightView;				// View matrix of light
float4x4 g_mLightProj;				// Projection matrix of light

float4 g_LightDiffuse = float4(0.5, 0.5, 0.5, 1.0);;				// Light's diffuse color
float4 g_LightAmbient = float4(0.3, 0.3, 0.3, 1.0);              // Light's ambient color

float4x4 g_mWorld;                  // World matrix for object
float3 g_CameraPos;				    // Camera position for scene View 
float4x4 g_mCameraView;				// Camera's view matrix
float4x4 g_mCameraProj;				// Projection matrix

texture2D g_MeshTexture;              // Color texture for mesh
texture2D g_ShadowMapTexture;			// Shadow map texture for lighting


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





struct ShadowMapVS_IN
{
	float4 Position : SV_POSITION;
};

struct ShadowMapPS_IN
{
	float4 Position : SV_POSITION;
	float4 Depth: TEXCOORD;
};




float4 GetPositionFromLight(float4 position)
{
	float4x4 WorldViewProjection =
		mul(mul(g_mWorld, g_mLightView), g_mLightProj);
	return mul(position, WorldViewProjection);
}

ShadowMapPS_IN ShadowMapVS(ShadowMapVS_IN input)
{

	ShadowMapPS_IN output = (ShadowMapPS_IN)0;
	
	output.Position = GetPositionFromLight(input.Position);
	output.Depth.x = 1 - output.Position.z / output.Position.w;
	return output;
}

float4 ShadowMapPS(ShadowMapPS_IN input) : SV_Target
{
	return input.Depth.x;
	
}


struct ShadowRenderVS_IN
{
	float4 Position : SV_POSITION;
	float3 Normal : NORMAL;
	float2 vTexCoord0 : TEXCOORD0;
};

struct ShadowRenderPS_IN
{
	float4 Position : SV_POSITION;
	float2 TextureUV  : TEXCOORD0;
	float4 vNormal : TEXCOORD1;
	float4 vPos : TEXCOORD2;
};


ShadowRenderPS_IN ShadowRenderVS(ShadowRenderVS_IN input)
{
	ShadowRenderPS_IN output;
	//generate the world-view-projection matrix
	float4x4 wvp = mul(mul(g_mWorld, g_mCameraView), g_mCameraProj);

	//transform the input position to the output
	output.Position = mul(input.Position, wvp);

	//transform the normal to world space
	output.vNormal = mul(input.Normal, g_mWorld);
	
	//do not transform the position needed for the
	//shadow map determination
	output.vPos = input.Position;

	//pass the texture coordinate as-is
	output.TextureUV = input.vTexCoord0;

	return output;
}

float4 ShadowRenderPS(ShadowRenderPS_IN input) : SV_Target
{
	// Standard lighting equation
	float4 vTotalLightDiffuse = float4(0, 0, 0, 1);
	float3 lightDir = normalize(g_LightPos - input.vPos);  // direction of light
	vTotalLightDiffuse += g_LightDiffuse * max(0, dot(input.vNormal, lightDir));
	vTotalLightDiffuse.a = 1.0f;
	
	// Now, consult the ShadowMap to see if we're in shadow
	float4 lightingPosition
		= GetPositionFromLight(input.vPos);// Get our position on the shadow map

	// Get the shadow map depth value for this pixel   
	float2 ShadowTexC =
		0.5 * lightingPosition.xy / lightingPosition.w + float2(0.5, 0.5);
	ShadowTexC.y = 1.0f - ShadowTexC.y;

	float shadowdepth = g_ShadowMapTexture.Sample(ShadowMapSampler, ShadowTexC).r;

	// Check our value against the depth value
	float ourdepth = 1 - (lightingPosition.z / lightingPosition.w);

	// Check the shadowdepth against the depth of this pixel
	// a fudge factor is added to account for floating-point error
	if (shadowdepth - 0.05 > ourdepth )
	{
			vTotalLightDiffuse = float4(0, 0, 0, 1);
	};

	
	return g_MeshTexture.Sample(MeshTextureSampler, input.TextureUV) *
		(vTotalLightDiffuse + g_LightAmbient);
}


technique ShadowMap
{
    pass Pass1
    {
		CullMode = NONE;
		ZEnable = TRUE;
		ZWriteEnable = TRUE;
		AlphaBlendEnable = FALSE;
		Profile = 9.3;
        VertexShader = ShadowMapVS;
        PixelShader = ShadowMapPS;
    }
}

technique ShadowRender
{
	pass Pass1
	{
		VertexShader = ShadowRenderVS;
		PixelShader = ShadowRenderPS;
	}
}
