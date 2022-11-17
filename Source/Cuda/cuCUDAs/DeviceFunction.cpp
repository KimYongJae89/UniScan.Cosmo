#include "DeviceFunction.h"

__global__ void devProfile(BYTE* pImageBuffer, float* pProfile, const int width, const int height)
{
	float dev_Profile = 0;
	int x = blockIdx.x * blockDim.x + threadIdx.x;

	if (x >= width)
		return;

	int i = x;

	for (int y = 0; y < height; y++)
	{
		dev_Profile += (float)pImageBuffer[i];
		i += width;
	}

	pProfile[x] = dev_Profile / (float)height;
}

__global__ void devEdgeFinderX(
	BYTE* pImageBuffer, 
	float* pProfile, 
	double threshold, 
	int range, 
	int* startPos, 
	int* endPos, 
	CudaLock lock)
{
	int x = blockIdx.x * blockDim.x + threadIdx.x;

	if (x >= range)
		return;

	if (pProfile[x] > threshold)
	{
		lock.lock();

		if (*startPos > x)
			*startPos = x;

		if (*endPos < x)
			*endPos = x;

		lock.unlock();
	}
}

__global__ void devLowerBinarize(
	BYTE* pSrcBuffer,
	BYTE* pDstBuffer,
	const int width,
	const int height,
	const float threshold,
	bool inverse,
	LPRECT lpRoiRect)
{
	int idx = blockIdx.x * blockDim.x + blockIdx.y * blockDim.x * gridDim.x + threadIdx.x;

	if (idx >= width * height)
		return;

	int x = idx % width;
	int y = idx / width;

	if (lpRoiRect && (x < lpRoiRect->left || x > lpRoiRect->right || y < lpRoiRect->top || y > lpRoiRect->bottom))
		return;

	int target = inverse ? 0 : 255;
	int background = inverse ? 255 : 0;

	if (pSrcBuffer[idx] < threshold)
		pDstBuffer[idx] = target;
	else
		pDstBuffer[idx] = background;
}

__global__ void devUpperBinarize(
	BYTE* pSrcBuffer,
	BYTE* pDstBuffer,
	const int width,
	const int height,
	const float threshold,
	bool inverse,
	LPRECT lpRoiRect)
{
	int idx = blockIdx.x * blockDim.x + blockIdx.y * blockDim.x * gridDim.x + threadIdx.x;

	if (idx >= width * height)
		return;

	int x = idx % width;
	int y = idx / width;

	if (lpRoiRect && (x < lpRoiRect->left || x > lpRoiRect->right || y < lpRoiRect->top || y > lpRoiRect->bottom))
		return;

	int target = inverse ? 0 : 255;
	int background = inverse ? 255 : 0;

	if (pSrcBuffer[idx] > threshold)
		pDstBuffer[idx] = target;
	else
		pDstBuffer[idx] = background;
}

__global__ void devBinarize(
	BYTE* pSrcBuffer,
	BYTE* pDstBuffer,
	const int width,
	const int height,
	const float lower,
	const float upper,
	bool inverse,
	LPRECT lpRoiRect)
{
	int idx = blockIdx.x * blockDim.x + blockIdx.y * blockDim.x * gridDim.x + threadIdx.x;

	if (idx >= width * height)
		return;

	int x = idx % width;
	int y = idx / width;

	if (lpRoiRect && (x < lpRoiRect->left || x > lpRoiRect->right || y < lpRoiRect->top || y > lpRoiRect->bottom))
		return;

	int target = inverse ? 0 : 255;
	int background = inverse ? 255 : 0;

	if (pSrcBuffer[idx] < lower ||
		pSrcBuffer[idx] > upper)
	{
		pDstBuffer[idx] = target;
	}
	else
	{
		pDstBuffer[idx] = background;
	}
}

__global__ void devLowerAdaptiveBinarize(
	BYTE* pSrcBuffer, 
	BYTE* pDstBuffer, 
	float* pProfile, 
	const int width, 
	const int height, 
	const float threshold,
	bool inverse,
	LPRECT lpRoiRect)
{
	int idx = blockIdx.x * blockDim.x + blockIdx.y * blockDim.x * gridDim.x + threadIdx.x;

	if (idx >= width * height)
		return;

	int x = idx % width;
	int y = idx / width;

	if (lpRoiRect && (x < lpRoiRect->left || x > lpRoiRect->right || y < lpRoiRect->top || y > lpRoiRect->bottom))
		return;

	int target = inverse ? 0 : 255;
	int background = inverse ? 255 : 0;

	float line_threshold = pProfile[x] - threshold;

	if (pSrcBuffer[idx] < line_threshold)
		pDstBuffer[idx] = target;
	else
		pDstBuffer[idx] = background;
}

__global__ void devUpperAdaptiveBinarize(
	BYTE* pSrcBuffer,
	BYTE* pDstBuffer,
	float* pProfile,
	const int width,
	const int height,
	const float threshold,
	bool inverse,
	LPRECT lpRoiRect)
{
	int idx = blockIdx.x * blockDim.x + blockIdx.y * blockDim.x * gridDim.x + threadIdx.x;

	if (idx >= width * height)
		return;

	int x = idx % width;
	int y = idx / width;

	if (lpRoiRect && (x < lpRoiRect->left || x > lpRoiRect->right || y < lpRoiRect->top || y > lpRoiRect->bottom))
		return;

	int target = inverse ? 0 : 255;
	int background = inverse ? 255 : 0;
	
	float line_threshold = pProfile[x] + threshold;

	if (pSrcBuffer[idx] > line_threshold)
		pDstBuffer[idx] = target;
	else
		pDstBuffer[idx] = background;
}

__global__ void devAdaptiveBinarize(
	BYTE* pSrcBuffer,
	BYTE* pDstBuffer,
	float* pProfile,
	const int width,
	const int height,
	const float lower,
	const float upper,
	bool inverse,
	LPRECT lpRoiRect)
{
	int idx = blockIdx.x * blockDim.x + blockIdx.y * blockDim.x * gridDim.x + threadIdx.x;

	if (idx >= width * height)
		return;

	int x = idx % width;
	int y = idx / width;

	if (lpRoiRect && (x < lpRoiRect->left || x > lpRoiRect->right || y < lpRoiRect->top || y > lpRoiRect->bottom))
		return;

	int target = inverse ? 0 : 255;
	int background = inverse ? 255 : 0;

	float line_Lower = pProfile[x] - lower;
	float line_Upper = pProfile[x] + upper;

	if (pSrcBuffer[idx] < line_Lower ||
		pSrcBuffer[idx] > line_Upper)
	{
		pDstBuffer[idx] = target;
	}
	else
	{
		pDstBuffer[idx] = background;
	}
}

__device__ int gx[3][3] = { {-1, 0, 1}, {-2, 0, 2}, {-1, 0, 1} };
__device__ int gy[3][3] = { {-1, -2, -1}, {0, 0, 0}, {1, 2, 1} };

__global__ void devSobel(BYTE* pSrcBuffer, BYTE* pDstBuffer, int width, int height)
{
	int idx = blockIdx.x * blockDim.x + blockIdx.y * blockDim.x * gridDim.x + threadIdx.x;

	if (idx >= width * height)
		return;

	int x = idx % width;
	int y = idx / width;

	if (x == 0 || x == width - 1
		|| y == 0 || y == height - 1)
		return;

	double sumX = 0;
	double sumY = 0;
	for (int yIndex = 0; yIndex < 3; yIndex++)
	{
		for (int index = (yIndex + y - 1) * width + x - 1, xIndex = 0; xIndex < 3; xIndex++)
		{
			sumX += pSrcBuffer[index] * gx[yIndex][xIndex];
			sumY += pSrcBuffer[index] * gy[yIndex][xIndex];
			index++;
		}
	}

	pDstBuffer[idx] = (sumX > 0 ? sumX : -sumX + sumY > 0 ? sumY : -sumY) / 2;
}