#include "stdafx.h"
#include "TriangleDemo.h"

#ifndef USE_INTERNAL_D3DX
#include <xnamath.h>
#else
using namespace DirectX;
#endif

namespace TriangleDemo
{

	CTriangleDemo::CTriangleDemo() :
		m_pSolidColorVS(nullptr),
		m_pSolidColorPS(nullptr),
		m_pInputLayout(nullptr),
		m_pVertexBuffer(nullptr)
	{
	}


	CTriangleDemo::~CTriangleDemo()
	{
	}

	bool CTriangleDemo::LoadContent()
	{
		ID3DBlob *pVsBuffer = nullptr;

		bool compileResult = CompileD3DShader("SolidGreenColor.fx", "VS_Main", "vs_4_0", &pVsBuffer);
		if (compileResult == false)
		{
			MessageBox(m_hWnd, "Error loading the shader", "Error", MB_OK);
			return false;
		}

		HRESULT hRes;
		hRes = m_pDevice->CreateVertexShader(pVsBuffer->GetBufferPointer(), pVsBuffer->GetBufferSize(), 0, &m_pSolidColorVS);
		if (FAILED(hRes))
		{
			MessageBox(m_hWnd, "Error creating the vertex buffer", "Error", MB_OK);
			return false;
		}

		D3D11_INPUT_ELEMENT_DESC solidColorLayout []
		{
			{ "POSITION", 0, DXGI_FORMAT_R32G32B32_FLOAT, 0, 0, D3D11_INPUT_PER_INSTANCE_DATA, 0}
		};

		unsigned int layoutsElement = ARRAYSIZE(solidColorLayout);

		hRes = m_pDevice->CreateInputLayout(solidColorLayout, layoutsElement,
			pVsBuffer->GetBufferPointer(), pVsBuffer->GetBufferSize(),
			&m_pInputLayout);
		if (FAILED(hRes))
		{
			MessageBox(m_hWnd, "Error creating the layout", "Error", MB_OK);
			return false;
		}

		pVsBuffer->Release();

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


		VertexPos verticies [] =
		{
			XMFLOAT3(0.5f, 0.5f, 0.5f),
			XMFLOAT3(0.5f, -0.5f, 0.5f),
			XMFLOAT3(-0.5f, -0.5f, 0.5f),
		};

		D3D11_BUFFER_DESC vertexDesc;
		ZeroMemory(&vertexDesc, sizeof(D3D11_BUFFER_DESC));

		vertexDesc.Usage = D3D11_USAGE_DEFAULT;
		vertexDesc.BindFlags = D3D11_BIND_VERTEX_BUFFER;
		vertexDesc.ByteWidth = sizeof(VertexPos) * 3;

		D3D11_SUBRESOURCE_DATA resourceData;
		ZeroMemory(&resourceData, sizeof(D3D11_SUBRESOURCE_DATA));

		resourceData.pSysMem = verticies;

		hRes = m_pDevice->CreateBuffer(&vertexDesc, &resourceData, &m_pVertexBuffer);
		if (FAILED(hRes))
		{
			MessageBox(m_hWnd, "Error creating the vertex Buffer", "Error", MB_OK);
			return false;
		}

		return true;
	}

	void CTriangleDemo::Update(float dt)
	{
	}

	void CTriangleDemo::Render()
	{
		if (m_pDeviceContext == nullptr)
			return;

		float clearColor[4]{ 0.0f, 0.2f, 0.4f, 1.0f};
		m_pDeviceContext->ClearRenderTargetView(m_pRenderTargetView, clearColor);

		unsigned int stride = sizeof(VertexPos);
		unsigned int offset = 0;

		m_pDeviceContext->IASetInputLayout(m_pInputLayout);
		m_pDeviceContext->IASetVertexBuffers(0, 1, &m_pVertexBuffer, &stride, &offset);
		m_pDeviceContext->IASetPrimitiveTopology(D3D11_PRIMITIVE_TOPOLOGY_TRIANGLELIST);

		m_pDeviceContext->VSSetShader(m_pSolidColorVS, 0, 0);
		m_pDeviceContext->PSSetShader(m_pSolidColorPS, 0, 0);
		m_pDeviceContext->Draw(3, 0);

		m_pSwapChain->Present(0, 0);
	}

	void CTriangleDemo::UnloadContent()
	{
		SAFE_RELEASE(m_pSolidColorVS);
		SAFE_RELEASE(m_pSolidColorPS);
		SAFE_RELEASE(m_pInputLayout);
		SAFE_RELEASE(m_pVertexBuffer);

		m_pSolidColorVS = NULL;
		m_pSolidColorPS = NULL;
		m_pInputLayout = NULL;
		m_pVertexBuffer = NULL;
	}
};