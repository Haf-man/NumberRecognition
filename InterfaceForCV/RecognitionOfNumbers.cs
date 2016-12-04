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
        //эталонные значения частот
        int [][] standardFrequance = 
        {{0.25, 0.25, 0.25, 0.25},
         {0.2,  0.5,  0.3,  0},
         {0.2,  0.3,  0.19, 0.31},
         {0.15, 0.35, 0.35, 0.15},
         {0.33, 0.33, 0.33, 0},
         {0.36, 0.15, 0.35, 0.14},
         {0.29, 0.12, 0.29, 0.3},
         {0.19, 0.49, 0.02, 0.3},
         {0.25, 0.25, 0.25, 0.25},
         {0.29, 0.3,  0.29, 0.12},
        };
        int [] oneClosedArea = {0, 6, 9};
        int [] noClosedArea = {1,2,3,4,5,7};
        //евклидова метрика
        double distance(double [] x,double [] y, int N)
        {
            double sum = 0;
            for(int i = 0;i<N;++i)
            {
                sum += (x[i] - y[i])*(x[i] - y[i]);
            }
            return Math.Sqrt(sum);
        }
        //
        int selectSelectedDigit(double [] freq,int [] possibleDigits,int N)
        {
            int resDig = possibleDigits[0];
            double minDist = distance(freq,standardFrequance[resDig],4);
            for(int i = 1;i < N; ++i)
            {
                int currDig = possibleDigits[i];
                double currDist =  distance(freq,standardFrequance[currDig],4);
                if(currDist < minDist)
                {
                    resDig = currDig;
                    minDist = currDist;
                }
            }
            return resDig;
        }
        //наиболее похожая цифра
        int selectDigit(double [] freq,int numOfClosedArea)
        {
            int res = -1;
            if(numOfClosedArea == 2)
            {
                res = 8;
            }
            else
            {
                if(numOfClosedArea == 1)
                {
                    res = selectSelectedDigit(freq, oneClosedArea, 3);
                }
                else
                {
                    if(numOfClosedArea == 0)
                    {
                        res = selectSelectedDigit(freq, noClosedArea, 6);
                    }
                }
            }
            return res;
        }
        
        //вычисление относительных частот
        static double [] getStatistic(int [,] image,int N,int M)
        {
            int [] partialCount = new int[4];
            //константы для корректировки в случае нечетного количества пикселей
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
