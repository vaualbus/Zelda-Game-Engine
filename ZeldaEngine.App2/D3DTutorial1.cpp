#include "stdafx.h"
#include "D3DTutorial1.h"


CD3DXTutorial1::CD3DXTutorial1()
{
}


CD3DXTutorial1::~CD3DXTutorial1()
{
}

void CD3DXTutorial1::Update(float dt)
{

}

void CD3DXTutorial1::Render()
{
	if (m_pDeviceContext == nullptr)
		return;

	float clearColor[4]{ 0.0f, 0.2f, 0.4f, 1.0f};
	m_pDeviceContext->ClearRenderTargetView(m_pRenderTargetView, clearColor);

	m_pSwapChain->Present(0, 0);
}
