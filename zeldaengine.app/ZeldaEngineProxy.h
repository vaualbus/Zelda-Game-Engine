#pragma once

#include <string>

class ZeldaEngineVector2Proxy
{
public:
	ZeldaEngineVector2Proxy()
	{
		CVector2();
	}

	ZeldaEngineVector2Proxy(int x, int y)
	{
		CVector2(x, y);
	}

	~ZeldaEngineVector2Proxy() { free(); }

	bool Equals(ZeldaEngineVector2Proxy other);

	bool Equals(void *obj);

	int GetX() const;
	int GetY() const;

	void SetX(long x) { X = x;  }
	void SetY(long y) { Y = y;  }
private:
	int X;
	int Y;
	void *ptr;

	void CVector2();
	void CVector2(int x, int y);

	void free();
};

class ZeldaEngineProxy
{
public:
	ZeldaEngineProxy();

private:
	void *ref;
};

