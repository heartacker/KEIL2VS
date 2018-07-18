#include "string.h"
#include "reg51.h"
#include "example.h"

#if defined(VS)
#include "math.h"
#endif
#define PI (3.14)
BOOL data islonger;
void wait(WORD us)
{
	WORD i = 0, j = 0;
	for (; i < us; i++)
	{
		for (j = 0; j < 1000; i++)
	}
}

void main()
{
	if (islonger)
	{
		wait(2048);
	}
	else
	{
		wait(1024);
	}
}