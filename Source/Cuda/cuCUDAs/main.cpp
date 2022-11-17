#include <GL/freeglut.h>
#include <chrono>

#include "helper_image.h"
#include "CudaFunction.h"


void initializeData(char *file, unsigned char **pixels, unsigned int *width, unsigned int *height)
{
	GLint bsize;
	size_t file_length = strlen(file);
	unsigned int g_Bpp;

	if (!strcmp(&file[file_length - 3], "pgm"))
	{
		if (sdkLoadPGM<unsigned char>(file, pixels, width, height) != true)
		{
			printf("Failed to load PGM image file: %s\n", file);
			exit(EXIT_FAILURE);
		}

		g_Bpp = 1;
	}
}

bool SaveData(char *file, BYTE* pixels, unsigned int width, unsigned int height)
{
	return sdkSavePGM<BYTE>(file, pixels, width, height);
}

chrono::time_point<chrono::steady_clock> startTime;
void FunctionStart()
{
	startTime = std::chrono::high_resolution_clock::now();//::GetTickCount();
}

void FunctionStop(string s)
{
	cout << std::chrono::duration_cast<std::chrono::nanoseconds>(std::chrono::high_resolution_clock::now() - startTime).count() / 1000 << " us - " << s <<"\n";
}

int main()
{
	unsigned char *pixels = NULL;
		BYTE *dest_Pixels1 = NULL;
	int *dest_Pixels = NULL;
	unsigned int width = 100;
	unsigned int height = 100;

	//initializeData("N1.pgm", &pixels, &width, &height);
	
	int gpu = 0;
	CUDA_INITIALIZE(gpu);
	//CUDA_THREAD_NUM(1024);

	//for (int i = 0; i < 5; i++)
	//{
		initializeData("c:\\image.pgm", &pixels, &width, &height);
		//initializeData("D:\\Image_C00_S000_L00.pgm", &pixels, &width, &height);

		
		//initializeData("D:\\CUDA_TEST\\TEST3.pgm", &pixels, &width, &height);


		/*int image = CUDA_CREATE_IMAGE(width, height, sizeof(BYTE));
		CUDA_SET_IMAGE(image, pixels);*/

		int count = 0;
		for (int y = 0, index = 0; y < height; y++)
		{
			for (int x = 0; x < width; x++)
			{
				if (pixels[index++] > 0)
					count++;
			}
		}

		double* xArray = new double[count];
		double* yArray = new double[count];

		for (int y = 0, index = 0, cur = 0; y < height; y++)
		{
			for (int x = 0; x < width; x++)
			{
				if (pixels[index++] > 0)
				{
					xArray[cur] = x;
					yArray[cur++] = y;
				}
			}
		}

		double cost;
		double gradient;
		double centerX;
		double centerY;
		CUDA_RANSAC(width, height, xArray, yArray, count, &cost, &gradient, &centerX, &centerY, 10);

}
