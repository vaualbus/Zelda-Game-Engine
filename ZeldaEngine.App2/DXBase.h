#pragma once
#include "stdafx.h"

class CD3DXBase
{
public:
	CD3DXBase();
	virtual ~CD3DXBase();

	bool Initialize(HINSTANCE hInst, HWND hWnd);
	
	void Shutdown();

	virtual bool LoadContent();
	virtual void UnloadContent();

	virtual void Update(float dt) = 0;
	virtual void Render() = 0;

	virtual bool CompileD3DShader(char *filePath, char *entry, char *shaderModel, ID3DBlob **buffer);

protected:
	HINSTANCE m_hInst;
	HWND m_hWnd;

	D3D_DRIVER_TYPE m_driverType;
	D3D_FEATURE_LEVEL m_featureLevel;

protected:
	ID3D11Device *m_pDevice;
	ID3D11DeviceContext *m_pDeviceContext;
	IDXGISwapChain *m_pSwapChain;
	ID3D11RenderTargetView *m_pRenderTargetView;

};

