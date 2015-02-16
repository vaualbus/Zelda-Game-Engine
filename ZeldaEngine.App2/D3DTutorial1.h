#pragma once

#include "stdafx.h"

class CD3DXTutorial1 : public CD3DXBase
{
public:
	CD3DXTutorial1();
	virtual ~CD3DXTutorial1();

	void Update(float dt) override;
	void Render() override;
};

