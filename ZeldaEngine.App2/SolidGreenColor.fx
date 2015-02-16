Texture2D colorMap : register(t0);
SamplerState colorMapSampler : register(s0);


struct VS_Input
{
	float4 pos : POSITION;
	float2 tex0 : TEXCOORD0;
	float4 colorPos : COLORPOS;
};

struct PS_Input
{
	float4 pos : SV_POSITION;
	float2 tex0 : TEXCOORD0;
	float4 colorPos : COLORPOS;
};

PS_Input VS_Main(VS_Input vertex)
{
	PS_Input vsOut = (PS_Input) 0;
	vsOut.pos = vertex.pos;
	vsOut.tex0 = max(((1 -  (vertex.tex0 * vertex.colorPos)) * max((1 - vertex.colorPos), vertex.pos)), vertex.pos - vertex.tex0);
	return vsOut;
}

float4 PS_Main(PS_Input frag) : SV_TARGET
{
	return colorMap.Sample(colorMapSampler, frag.tex0);
}
