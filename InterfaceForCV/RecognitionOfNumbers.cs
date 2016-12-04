using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace InterfaceForCV
{
    static class RecognitionOfNumbers
    {
        static public int [] Recognize(Image image)
        {
            int[] numbers = { 1, 2, 3 };
            return numbers;
        }
        static double [] getStatistic(int [,] image,int N,int M)
        {
            int [] partialCount = new int[4];
            int ci = N%2;
            int cj = M%2;
            for(int i = 0;i<N/2;++i)
            {
                for(int j = 0;j<M/2;++j)
                {
                    if(image[i,j] == 1)
                    {
                        ++partialCount[0];
                    }
                    if(image[i,j+M/2+cj] == 1)
                    {
                        ++partialCount[1];
                    }
                    if(image[i+N/2+ci,j+M/2+cj] == 1)
                    {
                        ++partialCount[2];
                    }
                    if(image[i+N/2+ci,j] == 1)
                    {
                        ++partialCount[3];
                    }
                }
            }
            int generalCount = 0;
            for(int i = 0; i<4;++i)
            {
                generalCount += partialCount[i];
            }
            double [] freq = new double[4];
            for(int i = 0; i<4;++i)
            {
                freq[i] = double(partialCount[i])/generalCount;
            }
            return freq;
        }
    }
}
