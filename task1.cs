using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static void Eratosthenes(List<int> primeNums, int k)
    {
        for (int i = 2; i < k; i++)
        {
            primeNums.Add(i);
        }

        for (int i = 0; i <= Math.Sqrt(primeNums.Count); i++)
        {
            int j = i + 1;
            while (j < primeNums.Count)
            {
                if (primeNums[j] % primeNums[i] == 0)
                {
                    primeNums.RemoveAt(j);
                }
                else
                {
                    j++;
                }
            }
        }
    }

    static (int, int) FindMaxFrequentPrime(List<int> counter)
    {
        (int maxFreq, int maxFreqPrime) = (0, 0);

        for (int i = 0; i < counter.Count; i++)
        {
            if (counter[i] > maxFreq)
            {
                maxFreq = counter[i];
                maxFreqPrime = i;
            }
        }

        return (maxFreq, maxFreqPrime);
    }

    static void Input(out int M, out int N)
    {
        Console.Write("\nВведите число M: ");
        while (!int.TryParse(Console.ReadLine(), out M) || M <= 0)
        {
            Console.Write("Введены неправильные данные, попробуйте еще раз: ");
        }

        Console.Write("\nВведите число N: ");
        while (!int.TryParse(Console.ReadLine(), out N) || N <= 0)
        {
            Console.Write("Введены неправильные данные, попробуйте еще раз: ");
        }
        Console.WriteLine();
    }

    static List<int> FindSaddlePoints(List<List<int>> newArray)
    {
        List<int> saddlePoints = new List<int>();

        int M = newArray.Count;
        int N = newArray[0].Count;

        List<int> rowMin = new List<int>(new int[M]);
        for (int i = 0; i < M; ++i)
        {
            rowMin[i] = newArray[i].Min();
        }

        List<int> colMax = new List<int>(new int[N]);
        for (int j = 0; j < N; ++j)
        {
            colMax[j] = newArray.Max(row => row[j]);
        }

        for (int i = 0; i < M; ++i)
        {
            for (int j = 0; j < N; ++j)
            {
                if (newArray[i][j] == rowMin[i] && newArray[i][j] == colMax[j])
                {
                    saddlePoints.Add(newArray[i][j]);
                }
            }
        }

        return saddlePoints;
    }

    static void Main(string[] args)
    {
        // пункт 1: определяем матрицу и заполняем случайными числами
        Random gen = new Random();
        int M = 5, N = 5;
        int[,] matrix = new int[M, N];
        for (int i = 0; i < M; i++)
        {
            for (int j = 0; j < N; j++)
            {
                matrix[i][j] = gen.Next(0, 51);
            }
        }

        List<int> primeNums = new List<int>();
        int k = 50;
        Eratosthenes(primeNums, k);
        int maxPrime = primeNums.Max();
        List<int> counter = new List<int>(new int[maxPrime + 1]);

        for (int i = 0; i < M; i++)
        {
            for (int j = 0; j < N; j++)
            {
                if (primeNums.Contains(matrix[i, j]))
                {
                    counter[matrix[i, j]]++;
                }
            }
        }

        var maxFrequentPrime = FindMaxFrequentPrime(counter);
        Console.WriteLine($"Число {maxFrequentPrime.Item1} встречается {maxFrequentPrime.Item2} раз");

        // 2 пункт
        Input(out M, out N);
        int[,] newMatrix = new int[M, N];
        for (int i = 0; i < M; i++)
        {
            for (int j = 0; j < N; j++)
            {
                if (i == j)
                {
                    newMatrix[i, j] = 0;
                }
                else if (i > j)
                {
                    newMatrix[i, j] = 100 + j;
                }
                else
                {
                    newMatrix[i, j] = 100 + N - j - 1;
                }
            }
        }

        for (int i = 0; i < M; i++)
        {
            for (int j = 0; j < N; j++)
            {
                Console.Write($"{newMatrix[i, j]:D3} ");
            }
            Console.WriteLine();
        }

        // 3 пункт
        k = 500;
        Eratosthenes(primeNums, k);
        List<int> uniqueNums = primeNums.Take(M * N).ToList();
        uniqueNums = uniqueNums.OrderBy(x => gen.Next()).ToList();

        List<List<int>> newArray = new List<List<int>>(M);
        for (int i = 0; i < M; i++)
        {
            newArray.Add(new List<int>(new int[N]));
        }

        int idx = 0;
        for (int i = 0; i < M; i++)
        {
            for (int j = 0; j < N; j++)
            {
                newArray[i][j] = uniqueNums[idx++];
            }
        }

        List<int> saddlePoints = FindSaddlePoints(newArray);
        if (saddlePoints.Count == 0)
        {
            Console.WriteLine("\nВ матрице нет седловых точек.");
        }
        else
        {
            Console.WriteLine("\nНайденные седловые точки:");
            foreach (int point in saddlePoints)
            {
                Console.Write(point + " ");
            }
            Console.WriteLine();
        }
    }
}
