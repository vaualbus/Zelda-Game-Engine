#pragma once

#include "stdafx.h"

class CGameSprite
{
public:
	CGameSprite();
	virtual ~CGameSprite();

	XMMATRIX GetWorldMatrix() const;

	void SetPosition(const XMFLOAT2& pos);
	void SetRotation(const float rotation);
	void SetScale(const XMFLOAT2& scale);

private:
	XMFLOAT2 m_position;
	XMFLOAT2 m_rotation;
	float m_scale;
};

