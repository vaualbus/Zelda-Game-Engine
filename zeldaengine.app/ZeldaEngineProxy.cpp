#include "stdafx.h"
#include "ZeldaEngineProxy.h"



#pragma unmanaged

bool ZeldaEngineVector2Proxy::Equals(ZeldaEngineVector2Proxy obj)
{
	return X == obj.GetX() && Y == obj.GetY();
}

bool ZeldaEngineVector2Proxy::Equals(void *obj)
{
	auto vec2 = (ZeldaEngineVector2Proxy*) obj;
	if (vec2 == NULL)
		return false;

	return Equals(vec2);
}

#pragma managed
#using <mscorlib.dll>
#using "..\\ZeldaEngine.ScriptEngine\\bin\\Debug\\ZeldaEngine.ScriptEngine.dll"

using namespace ZeldaEngine::ScriptEngine::ValueObjects;
using namespace ZeldaEngine::ScriptEngine;

using namespace System;
using namespace System::Runtime::InteropServices;

void ZeldaEngineVector2Proxy::CVector2()
{
	GCHandle gck;
	Vector2 ^vec;

	vec = gcnew Vector2();
	gck = GCHandle::Alloc(vec);
	ptr = GCHandle::ToIntPtr(gck).ToPointer();

	return;
}


void ZeldaEngineVector2Proxy::CVector2(int x, int y)
{
	GCHandle gch;
	Vector2 ^vec;

	vec = gcnew Vector2(x, y);
	gch = GCHandle::Alloc(vec);

	ptr = GCHandle::ToIntPtr(gch).ToPointer();

	return;
}

void ZeldaEngineVector2Proxy::free()
{
	IntPtr temp(ptr);
	GCHandle gck;

	gck = static_cast<GCHandle>(temp);
	gck.Free();
}

int ZeldaEngineVector2Proxy::GetX() const
{
	IntPtr temp(ptr);
	Int32 ^data;
	GCHandle gch;
	Vector2 ^vec;

	gch = static_cast<GCHandle>(temp);
	vec = static_cast<Vector2^>(gch.Target);

	return vec->X;
}

int ZeldaEngineVector2Proxy::GetY() const
{
	IntPtr temp(ptr);
	Int32 ^data;
	GCHandle gch;
	Vector2 ^vec;

	gch = static_cast<GCHandle>(temp);
	vec = static_cast<Vector2^>(gch.Target);

	return vec->Y;
}

