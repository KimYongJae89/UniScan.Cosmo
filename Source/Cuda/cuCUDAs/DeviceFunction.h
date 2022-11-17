//#ifndef _CUDA_FUNCTION_H_
//#define _CUDA_FUNCTION_H_
//#endif

#include "Defines.h"
#include "device_launch_parameters.h"
#include "cuda_runtime.h"
#include "CudaLock.h"

__global__ void devProfile(BYTE* pImageBuffer, float* pProfile, const int width, const int height);

__global__ void devEdgeFinderX(
	BYTE* pImageBuffer, 
	float* pProfile, 
	double threshold, 
	int range, 
	int* startPos, 
	int* endPos, 
	CudaLock lock);

__global__ void devLowerBinarize(
	BYTE* pSrcBuffer,
	BYTE* pDstBuffer,
	const int width,
	const int height,
	const float threshold,
	bool inverse = false,
	LPRECT lpRoiRect = NULL);

__global__ void devUpperBinarize(
	BYTE* pSrcBuffer,
	BYTE* pDstBuffer,
	const int width,
	const int height,
	const float threshold,
	bool inverse = false,
	LPRECT lpRoiRect = NULL);

__global__ void devBinarize(
	BYTE* pSrcBuffer,
	BYTE* pDstBuffer,
	const int width,
	const int height,
	const float lower,
	const float upper,
	bool inverse = false,
	LPRECT lpRoiRect = NULL);

__global__ void devLowerAdaptiveBinarize(
	BYTE* pSrcBuffer,
	BYTE* pDstBuffer,
	float* pProfile,
	const int width,
	const int height,
	const float threshold,
	bool inverse = false,
	LPRECT lpRoiRect = NULL);

__global__ void devUpperAdaptiveBinarize(
	BYTE* pSrcBuffer,
	BYTE* pDstBuffer,
	float* pProfile,
	const int width,
	const int height,
	const float threshold,
	bool inverse = false,
	LPRECT lpRoiRect = NULL);

__global__ void devAdaptiveBinarize(
	BYTE* pSrcBuffer,
	BYTE* pDstBuffer,
	float* pProfile,
	const int width,
	const int height,
	const float lower,
	const float upper,
	bool inverse = false,
	LPRECT lpRoiRect = NULL);

__global__ void devLabelMarker(
	BYTE* pImageBuffer,
	const int width,
	const int height,
	int** profileLabel,
	int** profilePosX,
	int** profilePosY,
	LPRECT lpRoiRect = NULL);

__global__ void devSobel(BYTE* pSrcBuffer, BYTE* pDstBuffer, int width, int height);
//__global__ void devLabelMerge(
//	const int width,
//	const int height,
//	int** profileLabel,
//	int** dstProfileLabel,
//	int mergeSize,
//	bool* mergeResult);

//__global__ void PseudoLabelingKernel1(
//	BYTE* pSrcBuffer,
//	BYTE* pDstBuffer);
