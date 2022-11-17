#ifndef CUDA_IMAGE_H_
#define CUDA_IMAGE_H_

#include <Windows.h>
#include <driver_types.h>

//#ifdef CUDA_DLL_EXPORT
//#define CUDA_CLASS extern "C" __declspec(dllexport)
//#else
//#define CUDA_CLASS _declspec(dllimport)
//#endif

struct LabelBuffer
{
	BYTE* pLabel;
	int* pMask;
};

struct CudaImage
{
	CudaImage(int id, int gpuNo);
	~CudaImage();

	int imageId;
	int gpuNo;
	int Width;
	int Height;

	void* pImageBuffer;
	float* pProfile;
	
	LabelBuffer* plabelBuffer;

	LPRECT pRoiRect;

	bool Alloc(int width, int height, int depth);
	void Free();

	// Host -> GPU
	bool SetImage(void* pDstImage);
	bool SetImage(void* pDstImage, int offset);

	bool ClearImage();

	// GPU -> Host
	bool GetImage(void* pDstBuffer);

	bool GetProfile(float* pDstBuffer);

	void SetImageROI(double x, double y, double width, double height);
	void ResetImageROI();

private:
	cudaError_t m_lastError;
	cudaError_t GetLastError() { return m_lastError; }
	bool CheckCudaError(cudaError_t cudaResult);

	int Depth;
};

#endif