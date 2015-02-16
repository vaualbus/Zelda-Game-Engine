#include "stdafx.h"
#include "TextureDemo.h"

#include "stdafx.h"

#ifndef USE_INTERNAL_D3DX
#include <xnamath.h>
#else
using namespace DirectX;
#endif

namespace TextureDemo
{
	CTextureDemo::CTextureDemo() :
		m_pSolidColorVS(nullptr),
		m_pSolidColorPS(nullptr),
		m_pInputLayout(nullptr),
		m_pVertexBuffer(nullptr),
		m_pColorMap(nullptr),
		m_pColorMapSampler(nullptr)
	{
	}


	CTextureDemo::~CTextureDemo()
	{
	}

	bool CTextureDemo::LoadContent()
	{
		ID3DBlob *pVsBuffer = nullptr;

		bool compileResult = CompileD3DShader("SolidGreenColor.fx", "VS_Main", "vs_4_0", &pVsBuffer);
		if (compileResult == false)
		{
			MessageBox(m_hWnd, "Error loading the vertex shader", "Error", MB_OK);
			return false;
		}

		HRESULT hRes;
		hRes = m_pDevice->CreateVertexShader(pVsBuffer->GetBufferPointer(), pVsBuffer->GetBufferSize(), 0, &m_pSolidColorVS);
		if (FAILED(hRes))
		{
			MessageBox(m_hWnd, "Error creating the vertex buffer", "Error", MB_OK);
			return false;
		}

		D3D11_INPUT_ELEMENT_DESC solidColorLayout [] =
		{
			{ "POSITION", 0, DXGI_FORMAT_R32G32B32_FLOAT, 0, 0, D3D11_INPUT_PER_VERTEX_DATA, 0 },
			{ "TEXCOORD", 0, DXGI_FORMAT_R32G32_FLOAT, 0, 12, D3D11_INPUT_PER_VERTEX_DATA, 0 },
			{ "COLORPOS", 0, DXGI_FORMAT_R32G32B32A32_FLOAT, 0, 0, D3D11_INPUT_PER_VERTEX_DATA, 0 }
		};
	

		unsigned int layoutElementCount = ARRAYSIZE(solidColorLayout);

		hRes = m_pDevice->CreateInputLayout(solidColorLayout, layoutElementCount, 
			                               pVsBuffer->GetBufferPointer(),
										   pVsBuffer->GetBufferSize(),
			                               &m_pInputLayout);
		if (FAILED(hRes))
		{
			MessageBox(m_hWnd, "Error creating the layout", "Error", MB_OK);
			return false;
		}

		SAFE_RELEASE(pVsBuffer);

		ID3DBlob *psBuffer = nullptr;
		compileResult = CompileD3DShader("SolidGreenColor.fx", "PS_Main", "ps_4_0", &psBuffer);
		if (compileResult == false)
		{
			MessageBox(m_hWnd, "Error loading the pixel shader", "Error", MB_OK);
			return false;
		}

		hRes = m_pDevice->CreatePixelShader(psBuffer->GetBufferPointer(), psBuffer->GetBufferSize(), nullptr, &m_pSolidColorPS);
		SAFE_RELEASE(psBuffer);

		if (FAILED(hRes))
		{
			MessageBox(m_hWnd, "Error creating the pixel shader", "Error", MB_OK);
			return false;
		}

		VertexPos vertices [] =
		{
			{ XMFLOAT3(1.0f, 1.0f, 1.0f), XMFLOAT2(1.0f, 1.0f) },
			{ XMFLOAT3(1.0f, -1.0f, 1.0f), XMFLOAT2(1.0f, 0.0f) },
			{ XMFLOAT3(-1.0f, -1.0f, 1.0f), XMFLOAT2(0.0f, 0.0f) },

			{ XMFLOAT3(-1.0f, -1.0f, 1.0f),  XMFLOAT2(0.0f, 0.0f) },
			{ XMFLOAT3(-1.0f, 1.0f, 1.0f), XMFLOAT2(0.0f, 1.0f) },
			{ XMFLOAT3(1.0f, 1.0f, 1.0f), XMFLOAT2(1.0f, 1.0f) },
		};

		D3D11_BUFFER_DESC vertexDesc;
		ZeroMemory(&vertexDesc, sizeof(vertexDesc));

		vertexDesc.Usage = D3D11_USAGE_DEFAULT;
		vertexDesc.BindFlags = D3D11_BIND_VERTEX_BUFFER;
		vertexDesc.ByteWidth = sizeof(VertexPos) * 6;

		D3D11_SUBRESOURCE_DATA resourceData;
		ZeroMemory(&resourceData, sizeof(resourceData));
		resourceData.pSysMem = vertices;

		hRes = m_pDevice->CreateBuffer(&vertexDesc, &resourceData, &m_pVertexBuffer);
		if (FAILED(hRes))
		{
			MessageBox(m_hWnd, "Error creating the vertex Buffer", "Error", MB_OK);
			return false;
		}

		hRes = D3DX11CreateShaderResourceViewFromFile(m_pDevice, "decal.dds", nullptr, nullptr, &m_pColorMap, nullptr);
		if (FAILED(hRes))
		{
			MessageBox(m_hWnd, "Error creating the texture", "Error", MB_OK);
			return false;
		}

		D3D11_SAMPLER_DESC colorMapDesc;
		ZeroMemory(&colorMapDesc, sizeof(colorMapDesc));
		colorMapDesc.AddressU = D3D11_TEXTURE_ADDRESS_WRAP;
		colorMapDesc.AddressV = D3D11_TEXTURE_ADDRESS_WRAP;
		colorMapDesc.AddressW = D3D11_TEXTURE_ADDRESS_WRAP;
		colorMapDesc.ComparisonFunc = D3D11_COMPARISON_NEVER;
		colorMapDesc.Filter = D3D11_FILTER_MIN_MAG_MIP_LINEAR;
		colorMapDesc.MaxLOD = D3D11_FLOAT32_MAX;

		hRes = m_pDevice->CreateSamplerState(&colorMapDesc, &m_pColorMapSampler);
		if (FAILED(hRes))
		{
			MessageBox(m_hWnd, "Error creating color map sampler", "Error", MB_OK);
			return false;
		}

		return true;
	}

	bool CTextureDemo::CompileD3DShader(char *filePath, char *entry, char *shaderModel, ID3DBlob **buffer)
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

	void CTextureDemo::Update(float dt)
	{
	}

	void CTextureDemo::Render()
	{
		if (m_pDeviceContext == 0)
			return;

		float clearColor[4] = { 0.0f, 0.0f, 0.25f, 1.0f };
		m_pDeviceContext->ClearRenderTargetView(m_pRenderTargetView, clearColor);

		unsigned int stride = sizeof(VertexPos);
		unsigned int offset = 0;

		m_pDeviceContext->IASetInputLayout(m_pInputLayout);
		m_pDeviceContext->IASetVertexBuffers(0, 1, &m_pVertexBuffer, &stride, &offset);
		m_pDeviceContext->IASetPrimitiveTopology(D3D11_PRIMITIVE_TOPOLOGY_TRIANGLELIST);

		m_pDeviceContext->VSSetShader(m_pSolidColorVS, 0, 0);
		m_pDeviceContext->PSSetShader(m_pSolidColorPS, 0, 0);
		m_pDeviceContext->PSSetShaderResources(0, 1, &m_pColorMap);
		m_pDeviceContext->PSSetSamplers(0, 1, &m_pColorMapSampler);
		m_pDeviceContext->Draw(6, 0);

		m_pSwapChain->Present(0, 0);
	}

	void CTextureDemo::UnloadContent()
	{
		SAFE_RELEASE(m_pColorMap);
		SAFE_RELEASE(m_pColorMapSampler);
		SAFE_RELEASE(m_pSolidColorVS);
		SAFE_RELEASE(m_pSolidColorPS);
		SAFE_RELEASE(m_pInputLayout);
		SAFE_RELEASE(m_pVertexBuffer);

		m_pColorMap = nullptr;
		m_pColorMapSampler = nullptr;
		m_pSolidColorVS = nullptr;
		m_pSolidColorPS = nullptr;
		m_pInputLayout = nullptr;
		m_pVertexBuffer = nullptr;
	}
};