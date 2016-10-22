using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImagePreprocessing
{
    public class Preprocessing
    {
        const int IMAGE_WIDTH = 22;
        const int IMAGE_HEIGHT = 22;

        private int[,] image;
        private int[,] finalImage;

        private int imageHeight;
        private int imageWidth;

        private int numAreas;

        public int[,] preprocessing(int[,] _image, int _imageWidth, int _imageHeight)
        {
            resize();
            thickening();
            this.numAreas = countAreas();

            return finalImage;
        }

        public void resize()
        {
            double stepX = (double)IMAGE_HEIGHT / imageHeight;
            double stepY = (double)IMAGE_WIDTH / imageWidth;

            finalImage = new int[IMAGE_HEIGHT, IMAGE_WIDTH];

            for (int i = 0; i < IMAGE_HEIGHT; i++)
                for (int j = 0; j < IMAGE_WIDTH; j++)
                    finalImage[i, j] = 0;

            for (int i = 0; i < imageHeight; i++)
                for (int j = 0; j < imageWidth; j++)
                    if (image[i, j] == 1)
                        finalImage[(int)(i * stepX), (int)(j * stepY)] = 1;
        }

        //утолщение цифр 
        public void thickening()
        {
            bool isAlone;

            int[] dx = { -1, 1, 0, 0, -1, 1, -1, 1 };
            int[] dy = { -1, 1, 0, 0, -1, 1, -1, 1 };

            for (int i = 1; i < imageWidth - 1; i++)
                for (int j = 1; j < imageHeight-1; j++)
                {
                    if (image[i, j] == 1)
                    {
                        finalImage[i, j] = 1;
                        for (int k = 0; k < 8; k++)
                            finalImage[i + dx[k], j + dy[k]] = 1;
                    }
                }

            //убирает "одинокие" нули
            for (int i = 1; i < imageWidth - 1; i++)
                for (int j = 1; j < imageHeight - 1; j++)
                {
                    isAlone = true;
                    if (image[i, j] == 0)
                        for (int k = 0; k < 8; k++)
                            if (finalImage[i + dx[k], j + dy[k]] == 0)
                            {
                                isAlone = false;
                                break;
                            }
                    if (isAlone)
                        image[i, j] = 1;
                }
        }

        private int countAreas()
        {
            return 0;
        }
    }
}
