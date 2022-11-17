#include "HostFunction.h"
#include <algorithm>

using namespace std;

bool hostEdgeDetectX(
	float* pProfile,
	int rangeMin, int rangeMax,
	int maxSize,
	const float threshold, 
	int* pStartPos, int* pEndPos, 
	int searchRange)
{
	bool isFindStart = pStartPos == NULL;
	bool isFindEnd = pEndPos == NULL;

	if (rangeMin < 0)
		rangeMin = 0;

	for (int x = rangeMin; x < rangeMax; x += searchRange)
	{
		if (!isFindStart && pStartPos != NULL)
		{
			if (pProfile[x] > threshold && *pStartPos > x)
			{
				isFindStart = true;

				if (searchRange == 1)
					*pStartPos = x;
				else
				{
					int newSearchRange = searchRange / 5;
					if (newSearchRange == 0)
						newSearchRange = 1;

					int findMin = x - searchRange;
					int findMax = x;

					//if (findMin < rangeMin)
					//	findMin = rangeMin;

					if (findMin < rangeMin)
					{
						int diff = rangeMin - findMin;
						findMin += diff;
						findMax += diff;
					}

					hostEdgeDetectX(pProfile, findMin, findMax, maxSize, threshold, pStartPos, NULL, newSearchRange);
				}
			}
		}

		if (!isFindEnd && pEndPos != NULL)
		{
			int revX = maxSize - x - 1;

			if (pProfile[revX] > threshold && *pEndPos < revX)
			{
				isFindEnd = true;

				if (searchRange == 1)
					*pEndPos = revX;
				else
				{
					int newSearchRange = searchRange / 5;
					if (newSearchRange == 0)
						newSearchRange = 1;

					int findMin = x - searchRange;
					int findMax = x;

					if (findMin < rangeMin)
					{
						int diff = rangeMin - findMin;
						findMin += diff;
						findMax += diff;
					}

					hostEdgeDetectX(pProfile, findMin, findMax, maxSize, threshold, NULL, pEndPos, newSearchRange);
				}
			}
		}
	}

	return isFindStart && isFindEnd;
}
