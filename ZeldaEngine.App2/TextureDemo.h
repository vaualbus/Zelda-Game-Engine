#pragma once

#include "stdafx.h"
#include <xnamath.h>

namespace TextureDemo
{
	class CTextureDemo : public CD3DXBase
	{
	public:
		CTextureDemo();
		~CTextureDemo();

		bool LoadContent() override;
		void UnloadContent() override;

		void Update(float dt) override;
		void Render() override;

	private:
		bool CompileD3DShader(char *filePath, char *entry, char *shaderModel, ID3DBlob **buffer);

	private:
		ID3D11VertexShader *m_pSolidColorVS;
		ID3D11PixelShader  *m_pSolidColorPS;

		ID3D11InputLayout  *m_pInputLayout;
		ID3D11Buffer       *m_pVertexBuffer;

		ID3D11ShaderResourceView *m_pColorMap;
		ID3D11SamplerState *m_pColorMapSampler;
	};

	struct VertexPos
	{
		XMFLOAT3 pos;
		XMFLOAT2 tex0;
	};
};
