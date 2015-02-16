// stdafx.h : file di inclusione per file di inclusione di sistema standard
// o file di inclusione specifici del progetto utilizzati di frequente, ma
// modificati raramente
//

#pragma once

#include "targetver.h"

#define WIN32_LEAN_AND_MEAN             // Escludere gli elementi utilizzati di rado dalle intestazioni di Windows
// File di intestazione di Windows:
#include <windows.h>

// File di intestazione Runtime C
#include <stdlib.h>
#include <malloc.h>
#include <memory.h>
#include <tchar.h>
#include <memory>

#include <d3d11.h>
#include <d3dcompiler.h>

#ifdef USE_INTERNAL_D3DX
#include <DirectXColors.h>
#include <DirectXMath.h>
#include <d3d9types.h>
#else
#include <D3DX11.h>
#include <DxErr.h>
#include <xnamath.h>
#endif

#include "DXBase.h"
#include "GameSprite.h"

#define SAFE_RELEASE(x) \
	if(x != nullptr) x->Release()
// TODO: fare riferimento qui alle intestazioni aggiuntive richieste dal programma
