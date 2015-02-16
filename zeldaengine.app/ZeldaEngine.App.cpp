// ZeldaEngine.App.cpp : definisce il punto di ingresso dell'applicazione.
//

#include "stdafx.h"
#include "ZeldaEngine.App.h"

#pragma comment (lib, "d3d11.lib")

#import "ZeldaScript.tlb" named_guids

#define MAX_LOADSTRING 100

void Draw(HDC hDC, HWND hWnd);

// Variabili globali:
HINSTANCE hInst;								// istanza corrente
TCHAR szTitle[MAX_LOADSTRING];					// Testo della barra del titolo
TCHAR szWindowClass[MAX_LOADSTRING];			// nome della classe di finestre principale

using namespace ZeldaEngine_ScriptEngine;

IVector2Ptr vec2Ptr;
IVector2Ptr vec22Ptr;
IZeldaScriptPtr scriptPtr;
IScriptEnginePtr enginePtr;


IDXGISwapChain *pSwapChain;
ID3D11Device *pDevice;
ID3D11DeviceContext *pDeviceContext;
ID3D11RenderTargetView *pRenderTargetView;


// Dichiarazioni con prototipo delle funzioni incluse in questo modulo di codice:
ATOM				CreateAndRegisterWindow(HINSTANCE hInstance);
BOOL				InitInstance(HINSTANCE, int);
LRESULT CALLBACK	WndProc(HWND, UINT, WPARAM, LPARAM);
INT_PTR CALLBACK	About(HWND, UINT, WPARAM, LPARAM);
HRESULT CreateCOMObjects(void);

std::string ExePath() {
	char buffer[MAX_PATH];
	GetModuleFileName(NULL, buffer, MAX_PATH);
	std::string::size_type pos = std::string(buffer).find_last_of("\\/");
	return std::string(buffer).substr(0, pos);
}

void RenderFrame(void);

void CleanD3D11();

bool InitD3D11(HWND hWnd);

int APIENTRY _tWinMain(_In_ HINSTANCE hInstance,
                     _In_opt_ HINSTANCE hPrevInstance,
                     _In_ LPTSTR    lpCmdLine,
                     _In_ int       nCmdShow)
{
 	// TODO: inserire qui il codice.
	MSG msg;
	HACCEL hAccelTable;
	HRESULT hRes = NULL;

	CreateCOMObjects();

	//enginePtr->CurrentScriptName = "Test";
	//enginePtr->EnginePath = ExePath().c_str();
	//enginePtr->ProjectFolder = "";
	//enginePtr->ProjectName = "Test";
	//enginePtr->InitializeEngine();
	//auto params = new int[4] { 32 };
	//enginePtr->AddScriptParams("Test", (SAFEARRAY*) params);

	// Inizializzare le stringhe globali
	LoadString(hInstance, IDS_APP_TITLE, szTitle, MAX_LOADSTRING);
	LoadString(hInstance, IDC_ZELDAENGINEAPP, szWindowClass, MAX_LOADSTRING);
	
	CreateAndRegisterWindow(hInstance);

	// Eseguire l'inizializzazione dall'applicazione:
	if (!InitInstance (hInstance, nCmdShow))
	{
		MessageBox(NULL, _T("Error!!"), _T("Error!!"), NULL);
		return FALSE;
	}

	hAccelTable = LoadAccelerators(hInstance, MAKEINTRESOURCE(IDC_ZELDAENGINEAPP));

	// Ciclo di messaggi principale:
	while (GetMessage(&msg, NULL, 0, 0))
	{
		if (!TranslateAccelerator(msg.hwnd, hAccelTable, &msg))
		{
			TranslateMessage(&msg);
			DispatchMessage(&msg);
		}
	}

	CoUninitialize();

	CleanD3D11();

	//enginePtr->Dispose();

	return (int) msg.wParam;
}

ATOM CreateAndRegisterWindow(HINSTANCE hInstance)
{
	WNDCLASSEX wcex;

	wcex.cbSize = sizeof(WNDCLASSEX);

	wcex.style			= CS_HREDRAW | CS_VREDRAW;
	wcex.lpfnWndProc	= WndProc;
	wcex.cbClsExtra		= 0;
	wcex.cbWndExtra		= 0;
	wcex.hInstance		= hInstance;
	wcex.hIcon			= LoadIcon(hInstance, MAKEINTRESOURCE(IDI_ZELDAENGINEAPP));
	wcex.hCursor		= LoadCursor(NULL, IDC_ARROW);
	wcex.hbrBackground	= (HBRUSH)(COLOR_WINDOW+1);
	wcex.lpszMenuName	= MAKEINTRESOURCE(IDC_ZELDAENGINEAPP);
	wcex.lpszClassName	= szWindowClass;
	wcex.hIconSm		= LoadIcon(wcex.hInstance, MAKEINTRESOURCE(IDI_SMALL));

	return RegisterClassEx(&wcex);
}

BOOL InitInstance(HINSTANCE hInstance, int nCmdShow)
{
   HWND hWnd;

   hInst = hInstance; // Memorizzare l'handle di istanza nella variabile globale

   hWnd = CreateWindow(szWindowClass, szTitle, WS_OVERLAPPEDWINDOW,
      CW_USEDEFAULT, 0, CW_USEDEFAULT, 0, NULL, NULL, hInstance, NULL);

   if (!hWnd)
   {
      return FALSE;
   }

   if (!InitD3D11(hWnd))
	   return false;

   ShowWindow(hWnd, nCmdShow);
   UpdateWindow(hWnd);

   return TRUE;
}

LRESULT CALLBACK WndProc(HWND hWnd, UINT message, WPARAM wParam, LPARAM lParam)
{
	int wmId, wmEvent;
	PAINTSTRUCT ps;
	HDC hdc;

	switch (message)
	{
	case WM_COMMAND:
		wmId    = LOWORD(wParam);
		wmEvent = HIWORD(wParam);
		// Analizzare le selezioni di menu:
		switch (wmId)
		{
		case IDM_ABOUT:
			DialogBox(hInst, MAKEINTRESOURCE(IDD_ABOUTBOX), hWnd, About);
			break;
		case IDM_EXIT:
			DestroyWindow(hWnd);
			break;
		default:
			return DefWindowProc(hWnd, message, wParam, lParam);
		}
		break;
	case WM_PAINT:
		hdc = BeginPaint(hWnd, &ps);
		Draw(hdc, hWnd);
		// TODO: aggiungere qui il codice per il disegno...
		EndPaint(hWnd, &ps);
		break;
	case WM_DESTROY:
		PostQuitMessage(0);
		break;
	default:
		return DefWindowProc(hWnd, message, wParam, lParam);
	}
	return 0;
}

INT_PTR CALLBACK About(HWND hDlg, UINT message, WPARAM wParam, LPARAM lParam)
{
	UNREFERENCED_PARAMETER(lParam);
	switch (message)
	{
	case WM_INITDIALOG:
		return (INT_PTR)TRUE;

	case WM_COMMAND:
		if (LOWORD(wParam) == IDOK || LOWORD(wParam) == IDCANCEL)
		{
			EndDialog(hDlg, LOWORD(wParam));
			return (INT_PTR)TRUE;
		}
		break;
	}
	return (INT_PTR)FALSE;
}

void Draw(HDC hDC, HWND hWnd)
{
	RenderFrame();
}

bool InitD3D11(HWND hWnd)
{
	DXGI_SWAP_CHAIN_DESC desc;
	ZeroMemory(&desc, sizeof(DXGI_SWAP_CHAIN_DESC));

	desc.BufferCount = 1;
	desc.BufferDesc.Format = DXGI_FORMAT_R8G8B8A8_UNORM;
	desc.BufferUsage = DXGI_USAGE_RENDER_TARGET_OUTPUT;
	desc.BufferDesc.Width = 800;
	desc.BufferDesc.Height = 600;
	desc.OutputWindow = hWnd;
	desc.SampleDesc.Count = 4;
	desc.Windowed = TRUE;
	desc.Flags = DXGI_SWAP_CHAIN_FLAG_ALLOW_MODE_SWITCH;

	//Create the d3d11 window
	HRESULT hr = D3D11CreateDeviceAndSwapChain(NULL, D3D_DRIVER_TYPE_HARDWARE, 
		                                       NULL, NULL, NULL, NULL,
											   D3D11_SDK_VERSION, &desc,
											   &pSwapChain, &pDevice, 
											   NULL, &pDeviceContext);

	if (hr != S_OK)
	{
		MessageBox(hWnd, _T("Error Creating the D3D11 Device"), _T("Error Creating the D3D11 Device"), NULL);
		return FALSE;
	}

	//Now we begin the drawing
	ID3D11Texture2D *pBackBuffer;
	pSwapChain->GetBuffer(0, __uuidof(ID3D11Texture2D), (LPVOID*) &pBackBuffer);

	pDevice->CreateRenderTargetView(pBackBuffer, NULL, &pRenderTargetView);

	pDeviceContext->OMSetRenderTargets(1, &pRenderTargetView, NULL);

	//Create the view port
	RECT rect{ 0 };
	GetClientRect(hWnd, &rect);

	D3D11_VIEWPORT viewPort;
	ZeroMemory(&viewPort, sizeof(D3D11_VIEWPORT));

	viewPort.TopLeftX = 0;
	viewPort.TopLeftY = 0;
	viewPort.Width = 800;//rect.left - rect.right;
	viewPort.Height = 600;//rect.top - rect.bottom;

	pDeviceContext->RSSetViewports(1, &viewPort);

	return true;
}

// Inizializza gli oggetti com
HRESULT CreateCOMObjects()
{
	HRESULT hRes = CoInitialize(NULL);
	if (hRes != S_OK)
	{
		MessageBox(NULL, _T("Error CoUnitilize()"), _T("Error"), NULL);
		return FALSE;
	}

	hRes = enginePtr.CreateInstance(__uuidof(ScriptEngine));
	if (hRes != S_OK)
	{
		MessageBox(NULL, _T("Error ScriptEngine"), _T("Error ScriptEngine"), NULL);
		return FALSE;
	}

	//enginePtr->InitializeEngine();

	return hRes;
}

void RenderFrame()
{
	pDeviceContext->ClearRenderTargetView(pRenderTargetView, new float[4]{ 0.0f, 0.2f, 0.4f, 1.0f });

	pSwapChain->Present(0, 0);

}

void CleanD3D11()
{
	pSwapChain->SetFullscreenState(FALSE, NULL);

	pSwapChain->Release();
	pRenderTargetView->Release();
	pDevice->Release();
	pDeviceContext->Release();
	pRenderTargetView->Release();
}

void Update(float dt)
{
}