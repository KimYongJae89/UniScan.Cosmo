#include "Defines.h"
#include "CudaImage.h"

#define _ATL_NO_EXCEPTIONS
#define _ATL_NO_HOSTING
#define _ATL_CSTRING_EXPLICIT_CONSTRUCTORS      // �Ϻ� CString �����ڴ� ��������� ����˴ϴ�.

bool hostEdgeDetectX(
	float* pProfile,
	int rangeMin, int rangeMax,
	int maxSize,
	const float threshold,
	int* pStartPos, int* pEndPos,
	int searchRange);