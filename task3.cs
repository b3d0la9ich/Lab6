using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static int FindPivot(double[,] A, int col, int startRow, int n)
    {
        double maxVal = Math.Abs(A[startRow, col]);
        int maxRow = startRow;

        for (int i = startRow + 1; i < n; i++)
        {
            if (Math.Abs(A[i, col]) > maxVal)
            {
                maxVal = Math.Abs(A[i, col]);
                maxRow = i;
            }
        }

        return maxRow;
    }

    static void GaussElimination(double[,] A, List<double> b, List<double> x, int n)
    {
        // Прямой ход
        for (int k = 0; k < n - 1; k++)
        {
            // Выбор ведущего элемента
            int maxRow = FindPivot(A, k, k, n);

            // Прямой ход
            for (int i = k + 1; i < n; i++)
            {
                double factor = A[i, k] / A[k, k];
                for (int j = k; j < n; j++)
                {
                    A[i, j] -= factor * A[k, j];
                }
                b[i] -= factor * b[k];
            }
        }

        // Обратный ход
        x[n - 1] = b[n - 1] / A[n - 1, n - 1];
        for (int i = n - 2; i >= 0; i--)
        {
            double sum = b[i];
            for (int j = i + 1; j < n; j++)
            {
                sum -= A[i, j] * x[j];
            }
            x[i] = sum / A[i, i];
        }
    }

    static void FormCanonicalSystem(double[,] A, List<double> b, double[,] C, List<double> f, int n)
    {
        for (int i = 0; i < n; i++)
        {
            f[i] = b[i] / A[i, i];
            for (int j = 0; j < n; j++)
            {
                if (j != i)
                {
                    C[i, j] = -A[i, j] / A[i, i];
                }
                else
                {
                    C[i, j] = 0.0;
                }
            }
        }
    }

    static void SimpleIteration(double[,] C, List<double> f, List<double> x, int n, double epsilon)
    {
        List<double> xNew = new List<double>(Enumerable.Repeat(0.0, n));
        int k = 0;
        double maxDiff = 0.0;

        // Задаем начальное приближение
        for (int i = 0; i < n; i++)
        {
            x[i] = 0.0;
        }

        Console.WriteLine("N" + "      x1" + "      x2" + "      x3" + "      x4" + "      εn");

        do
        {
            // Вычисляем новое приближение
            for (int i = 0; i < n; i++)
            {
                double sum = f[i];
                for (int j = 0; j < n; j++)
                {
                    sum += C[i, j] * x[j];
                }
                xNew[i] = sum;
            }

            // Проверяем условие остановки
            maxDiff = 0.0;
            for (int i = 0; i < n; i++)
            {
                if (Math.Abs(xNew[i] - x[i]) > maxDiff)
                {
                    maxDiff = Math.Abs(xNew[i] - x[i]);
                }
                x[i] = xNew[i];
            }
            k++;

            // Вывод результатов в таблицу
            Console.WriteLine($"{k}     {x[0]}     {x[1]}     {x[2]}     {x[3]}     {maxDiff}");
        } while (maxDiff > epsilon);

        Console.WriteLine("Число итераций: " + k);

        // Вывод сообщения о сходимости или расходимости
        if (maxDiff <= epsilon)
        {
            Console.WriteLine("Метод сходится.");
        }
        else
        {
            Console.WriteLine("Метод расходится.");
        }
    }

    static void Main(string[] args)
    {
        double[,] A = {
            {0.89, -0.04, 0.21, -18.0},
            {0.25, -1.23, 0.12, -0.09},
            {-0.21, 0.12, 0.8,  -0.13},
            {0.15, -1.31, 0.06, -1.15}
        };
        List<double> b = new List<double> { -1.24, -1.15, 2.56, 0.89 };
        List<double> x = new List<double>(Enumerable.Repeat(0.0, 4));
        double[,] C = new double[4, 4];
        List<double> f = new List<double>(Enumerable.Repeat(0.0, 4));

        // Решение методом Гаусса
        Console.WriteLine("Решение методом Гаусса:");
        GaussElimination(A, b, x, 4);
        Console.WriteLine("x1 = " + x[0]);
        Console.WriteLine("x2 = " + x[1]);
        Console.WriteLine("x3 = " + x[2]);
        Console.WriteLine("x4 = " + x[3]);

        // Преобразуем систему к каноническому виду
        FormCanonicalSystem(A, b, C, f, 4);

        // Решение методом простых итераций
        Console.WriteLine("\nРешение методом простых итераций:");
        SimpleIteration(C, f, x, 4, 0.001);
        Console.WriteLine("x1 = " + x[0]);
        Console.WriteLine("x2 = " + x[1]);
        Console.WriteLine("x3 = " + x[2]);
        Console.WriteLine("x4 = " + x[3]);
    }
}
