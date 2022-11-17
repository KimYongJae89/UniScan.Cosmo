#include "Defines.h"
#include <cmath>

#define EXPORT_DLL extern "C" __declspec(dllexport)

EXPORT_DLL bool CUDA_INITIALIZE(int& gpuNo);
EXPORT_DLL void CUDA_RELEASE();

EXPORT_DLL void CUDA_THREAD_NUM(int threadNum);

// 이미지를 생성하고 이미지에 대한 고유 식별 번호를 리턴한다.
EXPORT_DLL UINT CUDA_CREATE_IMAGE(int width, int height, int depth);
EXPORT_DLL bool CUDA_SET_IMAGE(UINT image, void* pImageBuffer);

EXPORT_DLL bool CUDA_CLEAR_IMAGE(UINT image);
EXPORT_DLL void CUDA_FREE_IMAGE(UINT image);
EXPORT_DLL bool CUDA_GET_IMAGE(UINT image, void* pDstBuffer);

EXPORT_DLL void CUDA_SET_ROI(UINT image, double x, double y, double width, double height);
EXPORT_DLL void CUDA_RESET_ROI(UINT image);

// 생성된 이미지의 프로파일을 계산한다.
EXPORT_DLL bool CUDA_CREATE_PROFILE(UINT srcImage);

// 라벨 버퍼 미리 할당
EXPORT_DLL bool CUDA_CREATE_LABEL_BUFFER(UINT srcImage);

EXPORT_DLL bool CUDA_BINARIZE(UINT srcImage, UINT dstImage, float lower, float upper, bool inverse = false);
EXPORT_DLL bool CUDA_BINARIZE_LOWER(UINT srcImage, UINT dstImage, float lower, bool inverse = false);
EXPORT_DLL bool CUDA_BINARIZE_UPPER(UINT srcImage, UINT dstImage, float upper, bool inverse = false);

EXPORT_DLL bool CUDA_ADAPTIVE_BINARIZE(UINT srcImage, UINT dstImage, float lower, float upper, bool inverse = false);
EXPORT_DLL bool CUDA_ADAPTIVE_BINARIZE_LOWER(UINT srcImage, UINT dstImage, float lower, bool inverse = false);
EXPORT_DLL bool CUDA_ADAPTIVE_BINARIZE_UPPER(UINT srcImage, UINT dstImage, float upper, bool inverse = false);
EXPORT_DLL bool CUDA_EDGE_DETECT(UINT srcImage, EdgeSearchDirection dir, int threshold, int* startPos, int* endPos);

EXPORT_DLL bool CUDA_SOBEL(UINT srcImage, UINT dstImage);

// Blob
EXPORT_DLL int CUDA_LABELING(UINT binImage);

EXPORT_DLL bool CUDA_BLOBING(UINT binImage, UINT srcImage, int count,
	UINT* areaArray, UINT* xMinArray, UINT* xMaxArray, UINT* yMinArray, UINT* yMaxArray, 
	UINT* vMinArray, UINT* vMaxArray, float* vMeanArray);

// MORPHOLOGY
EXPORT_DLL bool CUDA_MORPHOLOGY_ERODE(UINT srcImage, UINT dstImage, int maskSize);
EXPORT_DLL bool CUDA_MORPHOLOGY_DILATE(UINT srcImage, UINT dstImage, int maskSize);
EXPORT_DLL bool CUDA_MORPHOLOGY_OPEN(UINT srcImage, UINT dstImage, int maskSize);
EXPORT_DLL bool CUDA_MORPHOLOGY_CLOSE(UINT srcImage, UINT dstImage, int maskSize);
EXPORT_DLL bool CUDA_MORPHOLOGY_THINNING(UINT srcImage);

// MATH
EXPORT_DLL bool CUDA_MATH_AND(UINT srcImage1, UINT srcImage2, UINT dstImage);
EXPORT_DLL bool CUDA_MATH_OR(UINT srcImage1, UINT srcImage2, UINT dstImage);
EXPORT_DLL bool CUDA_MATH_XOR(UINT srcImage1, UINT srcImage2, UINT dstImage);
EXPORT_DLL bool CUDA_MATH_MUL(UINT srcImage, float* profile);

// Ransac
EXPORT_DLL bool CUDA_RANSAC(int width, int height, double* xArray, double* yArray, int size, double* cost, double* gradient, double* centerX, double* centerY, double threshold);
