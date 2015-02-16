#pragma once
#include "stdafx.h"

class CGameSpriteDemo : CD3DXBase
{
public:
	CGameSpriteDemo();
	~CGameSpriteDemo();

	bool LoadContent() override;
	void UnloadContent() override;

	void Update(float dt) override;
	void Render() override;

private:
	ID3D11VertexShader *m_pSolidColorVS;
	ID3D11PixelShader  *m_pSolidColorPS;

	ID3D11InputLayout  *m_pInputLayout;
	ID3D11Buffer       *m_pVertexBuffer;


	struct VertexPos
	{
		XMFLOAT3 pos;
	};
};

