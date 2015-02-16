#include "stdafx.h"
#include "DXBase.h"

CD3DXBase::CD3DXBase() :
	m_driverType(D3D_DRIVER_TYPE_NULL),
	m_featureLevel(D3D_FEATURE_LEVEL_11_0),
	m_pDevice(nullptr),
	m_pDeviceContext(nullptr),
	m_pSwapChain(nullptr),
	m_pRenderTargetView(nullptr)
{
}


CD3DXBase::~CD3DXBase()
{
	Shutdown();
}


bool CD3DXBase::Initialize(HINSTANCE hInst, HWND hWnd)
{
	m_hInst = hInst;
	m_hWnd = hWnd;

	RECT dimensions;
	GetClientRect(m_hWnd, &dimensions);

	unsigned int width = dimensions.right - dimensions.left;
	unsigned int height = dimensions.bottom - dimensions.top;

	D3D_DRIVER_TYPE driverTypes [] =
	{
		D3D_DRIVER_TYPE_HARDWARE, D3D_DRIVER_TYPE_WARP, D3D_DRIVER_TYPE_SOFTWARE
	};

	unsigned int totalDriverTypes = ARRAYSIZE(driverTypes);

	D3D_FEATURE_LEVEL featureLevels [] =
	{
		D3D_FEATURE_LEVEL_11_0,
		D3D_FEATURE_LEVEL_10_1,
		D3D_FEATURE_LEVEL_10_0
	};

	unsigned int totalFeatureLevels = ARRAYSIZE(featureLevels);

	DXGI_SWAP_CHAIN_DESC swapChainDesc;
	ZeroMemory(&swapChainDesc, sizeof(swapChainDesc));
	swapChainDesc.BufferCount = 1;
	swapChainDesc.BufferDesc.Width = width;
	swapChainDesc.BufferDesc.Height = height;
	swapChainDesc.BufferDesc.Format = DXGI_FORMAT_R8G8B8A8_UNORM;
	swapChainDesc.BufferDesc.RefreshRate.Numerator = 60;
	swapChainDesc.BufferDesc.RefreshRate.Denominator = 1;
	swapChainDesc.BufferUsage = DXGI_USAGE_RENDER_TARGET_OUTPUT;
	swapChainDesc.OutputWindow = m_hWnd;
	swapChainDesc.Windowed = true;
	swapChainDesc.SampleDesc.Count = 1;
	swapChainDesc.SampleDesc.Quality = 0;

	unsigned int creationFlags = 0;

#ifdef _DEBUG
	creationFlags |= D3D11_CREATE_DEVICE_DEBUG;
#endif

	HRESULT result;
	unsigned int driver = 0;

	for (driver = 0; driver < totalDriverTypes; ++driver)
	{
		result = D3D11CreateDeviceAndSwapChain(0, driverTypes[driver], 0, creationFlags,
			featureLevels, totalFeatureLevels,
			D3D11_SDK_VERSION, &swapChainDesc, &m_pSwapChain,
			&m_pDevice, &m_featureLevel, &m_pDeviceContext);

		if (SUCCEEDED(result))
		{
			m_driverType = driverTypes[driver];
			break;
		}
	}

	if (FAILED(result))
	{
		MessageBox(m_hWnd, "Error creating the render device", "Error", MB_OK);
		return false;
	}

	ID3D11Texture2D* backBufferTexture;

	result = m_pSwapChain->GetBuffer(0, __uuidof(ID3D11Texture2D), (LPVOID*) &backBufferTexture);

	if (FAILED(result))
	{
		MessageBox(m_hWnd, "Error get the swap chain", "Error", MB_OK);
		return false;
	}

	result = m_pDevice->CreateRenderTargetView(backBufferTexture, 0, &m_pRenderTargetView);

	if (backBufferTexture)
		backBufferTexture->Release();

	if (FAILED(result))
	{
		MessageBox(m_hWnd, "Fail to render to the target view", "Error", MB_OK);
		return false;
	}

	m_pDeviceContext->OMSetRenderTargets(1, &m_pRenderTargetView, 0);

	D3D11_VIEWPORT viewport;
	viewport.Width = static_cast<float>(width);
	viewport.Height = static_cast<float>(height);
	viewport.MinDepth = 0.0f;
	viewport.MaxDepth = 1.0f;
	viewport.TopLeftX = 0.0f;
	viewport.TopLeftY = 0.0f;

	m_pDeviceContext->RSSetViewports(1, &viewport);

	return LoadContent();
}

bool CD3DXBase::CompileD3DShader(char *filePath, char *entry, char *shaderModel, ID3DBlob **buffer)
{
	DWORD shaderFlags = D3DCOMPILE_ENABLE_STRICTNESS;

#if defined(DEBUG) || defined(_DEBUG)
	shaderFlags |= D3DCOMPILE_DEBUG;
#endif

	ID3DBlob *errorBuffer = nullptr;
	HRESULT hRes;

	hRes = D3DX11CompileFromFile(filePath, nullptr, 0, entry, shaderModel,
		shaderFlags, 0, nullptr, buffer, &errorBuffer, nullptr);

	if (FAILED(hRes))
	{
		if (errorBuffer != nullptr)
		{
			OutputDebugString((char *) errorBuffer->GetBufferPointer());
			errorBuffer->Release();
		}
		return false;
	}

	SAFE_RELEASE(errorBuffer);
	return true;
}

bool CD3DXBase::LoadContent()
{
	return true;
}

void CD3DXBase::UnloadContent()
{

}

void CD3DXBase::Shutdown()
{
	UnloadContent();

	SAFE_RELEASE(m_pRenderTargetView);
	SAFE_RELEASE(m_pSwapChain);
	SAFE_RELEASE(m_pDeviceContext);
	SAFE_RELEASE(m_pDevice);

	m_pDevice = nullptr;
	m_pDeviceContext = nullptr;
	m_pSwapChain = nullptr;
	m_pRenderTargetView = nullptr;
}
