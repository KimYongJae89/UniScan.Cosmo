#include "DeviceMorphologyFunction.h"

__global__ void kernel_Erode(BYTE * pSrcBuffer, BYTE * pDstBuffer, int W, int H, int mask)
{
	int id = (blockIdx.x * blockDim.x) + (blockIdx.y * blockDim.x * gridDim.x) + threadIdx.x;

	if (id >= W * H)
		return;

	if (pSrcBuffer[id] > 0)
		return;

	int x = id % W;
	int y = id / W;

	for (int rangeY = y - mask; rangeY <= y + mask; rangeY++)
	{
		if (rangeY < 0 || rangeY >= H)
			continue;

		for (int rangeX = x - mask; rangeX <= x + mask; rangeX++)
		{
			if (rangeX < 0 || rangeX >= W)
				continue;

			pDstBuffer[rangeY * W + rangeX] = 0;
		}
	}
}

__global__ void kernel_Dilate(BYTE * pSrcBuffer, BYTE * pDstBuffer, int W, int H, int mask)
{
	int id = (blockIdx.x * blockDim.x) + (blockIdx.y * blockDim.x * gridDim.x) + threadIdx.x;

	if (id >= W * H)
		return;

	if (pSrcBuffer[id] == 0)
		return;

	int x = id % W;
	int y = id / W;

	for (int rangeY = y - mask; rangeY <= y + mask; rangeY++)
	{
		if (rangeY < 0 || rangeY >= H)
			continue;

		for (int rangeX = x - mask; rangeX <= x + mask; rangeX++)
		{
			if (rangeX < 0 || rangeX >= W)
				continue;

			pDstBuffer[rangeY * W + rangeX] = 255;
		}
	}
}

__global__ void kernel_Thinning(BYTE * pSrcBuffer, BYTE * pDstBuffer, int W, int H, bool* pResult)
{
	int id = (blockIdx.x * blockDim.x) + (blockIdx.y * blockDim.x * gridDim.x) + threadIdx.x;

	if (id >= W * H)
		return;

	if (pSrcBuffer[id] == 0)
		return;

	int x = id % W;
	int y = id / W;

	if (x + 1 >= W || y + 1 >= H ||
		x - 1 < 0 || y - 1 < 0)
		return;

	int filter[2][3][3] = 
	{ 
		{
			{ 0,  0,  0},
			{-1,  1, -1},
			{ 1,  1,  1}
		},
		{
			{-1,  0,  0},
			{ 1,  1,  0},
			{-1,  1, -1}
		}
	};

	for (int rot = 0; rot < 4; rot++)
	{
		for (int i = 0; i < 2; i++)
		{
			bool isFind = true;
			
			for (int rangeY = 0; rangeY < 3; rangeY++)
			{
				for (int rangeX = 0; rangeX < 3; rangeX++)
				{
					int filterX = rangeX;
					int filterY = rangeY;

					int nFilterValue = 0;

					switch (rot)
					{
					case 1:
						nFilterValue = filter[i][filterX][2 - filterY] * 255;
						break;
					case 2:
						nFilterValue = filter[i][2 - filterY][2 - filterX] * 255;
						break;
					case 3:
						nFilterValue = filter[i][2 - filterX][filterY] * 255;
						break;
					case 0:
					default:
						nFilterValue = filter[i][filterY][filterX] * 255;
						break;
					}

					if (nFilterValue < 0)
						continue;

					int maskY = y + rangeY - 1;
					int maskX = x + rangeX - 1;

					if (pSrcBuffer[maskY * W + maskX] != nFilterValue)
					{
						isFind = false;
						break;
					}
				}

				if (!isFind)
					break;
			}

			if (isFind)
			{
				pDstBuffer[id] = 0;
				*pResult = true;
				return;
			}
		}
	}
}

__global__ void kernel_EdgeTrim(BYTE * pSrcBuffer, BYTE * pDstBuffer, int W, int H, bool* pResult)
{
	int id = (blockIdx.x * blockDim.x) + (blockIdx.y * blockDim.x * gridDim.x) + threadIdx.x;

	if (id >= W * H)
		return;

	if (pSrcBuffer[id] == 0)
		return;

	int x = id % W;
	int y = id / W;

	if (x + 1 >= W || y + 1 >= H ||
		x - 1 < 0 || y - 1 < 0)
		return;

	int filter[2][3][3] =
	{
		{
			{ 0,  0,  0},
			{ 0,  1,  0},
			{ 0, -1, -1}
		},
		{
			{ 0,  0,  0},
			{ 0,  1,  0},
			{-1, -1,  0}
		}
	};

	for (int rot = 0; rot < 4; rot++)
	{
		for (int i = 0; i < 2; i++)
		{
			bool isFind = true;

			for (int rangeY = 0; rangeY < 3; rangeY++)
			{
				for (int rangeX = 0; rangeX < 3; rangeX++)
				{
					int filterX = rangeX;
					int filterY = rangeY;

					int nFilterValue = 0;

					switch (rot)
					{
					case 1:
						nFilterValue = filter[i][filterX][2 - filterY] * 255;
						break;
					case 2:
						nFilterValue = filter[i][2 - filterY][2 - filterX] * 255;
						break;
					case 3:
						nFilterValue = filter[i][2 - filterX][filterY] * 255;
						break;
					case 0:
					default:
						nFilterValue = filter[i][filterY][filterX] * 255;
						break;
					}

					if (nFilterValue < 0)
						continue;

					int maskY = y + rangeY - 1;
					int maskX = x + rangeX - 1;

					if (pSrcBuffer[maskY * W + maskX] != nFilterValue)
					{
						isFind = false;
						break;
					}
				}

				if (!isFind)
					break;
			}

			if (isFind)
			{
				pDstBuffer[id] = 0;
				*pResult = true;
				return;
			}
		}
	}
}
