using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab4
{
    class Program
    {

        public class VectorSystem
        {
            public VectorSystem(double[][] vectors)
            {
                numbersOfVectors = new int[vectors.Length];
                for (int i = 0; i < vectors.Length; ++i)
                    numbersOfVectors[i] = i;

                numbersOfVariables = new int[vectors[0].Length];
                for (int i = 0; i < numbersOfVariables.Length; ++i)
                    numbersOfVariables[i] = i;

                matrix = vectors;
            }

            public static VectorSystem Parse(string inputString)
            {
                string[] strings = inputString.Split('\n');
                string[][] input = new string[strings.Length][];

                int length = 0;

                for (int i = 0; i < strings.Length; ++i)
                {
                    strings[i] = strings[i].Trim();
                    input[i] = strings[i].Split(' ');
                    if (length == 0)
                        length = input[i].Length;
                }

                double[][] matrix = new double[strings.Length][];

                for (int i = 0; i < strings.Length; ++i)
                    matrix[i] = new double[length];

                for (int i = 0; i < strings.Length; ++i)
                    for (int j = 0; j < length; ++j)
                        matrix[i][j] = Convert.ToDouble(input[i][j]);

                return new VectorSystem(matrix);
            }

            public override string ToString()
            {
                string result = "";

                for (int i = 0; i < matrix.Length - 1; ++i)
                {
                    for (int j = 0; j < matrix[i].Length - 1; ++j)
                        result += matrix[i][j].ToString() + " ";

                    result += matrix[i][matrix[i].Length - 1].ToString() + '\n';
                }

                for (int j = 0; j < matrix[matrix.Length - 1].Length - 1; ++j)
                    result += matrix[matrix.Length - 1][j].ToString() + " ";

                result += matrix[matrix.Length - 1][matrix[matrix.Length - 1].Length - 1].ToString();

                return result;
            }

            private void RowMinusRow(int deminishing, int subtrahend, double coefficient)
            {
                for (int i = 0; i < matrix[deminishing].Length; ++i)
                    matrix[deminishing][i] -= matrix[subtrahend][i] * coefficient;
            }

            private void ReplaceRows(int row1, int row2)
            {
                object temp = matrix[row1];
                matrix[row1] = matrix[row2];
                matrix[row2] = (double[])temp;

                int tmp = numbersOfVectors[row1];
                numbersOfVectors[row1] = numbersOfVectors[row2];
                numbersOfVectors[row2] = tmp;
            }

            private void ReplaceColumns(int column1, int column2)
            {
                int tmp = numbersOfVariables[column1];
                numbersOfVariables[column1] = numbersOfVariables[column2];
                numbersOfVariables[column2] = tmp;

                for (int i = 0; i < matrix.Length; ++i)
                {
                    double temp = matrix[i][column1];
                    matrix[i][column1] = matrix[i][column2];
                    matrix[i][column2] = temp;
                }
            }

            private void SortColumns()
            {
                for (int i = 0; i < numbersOfVariables.Length - 1; ++i)
                    for (int j = i + 1; j < numbersOfVariables.Length; ++j)
                        if (numbersOfVariables[i] > numbersOfVariables[j])
                            ReplaceColumns(i, j);
            }

            private void DivideRow(int row, double coefficient)
            {
                for (int i = 0; i < matrix[row].Length; ++i)
                    if (matrix[row][i] != 0)
                        matrix[row][i] = matrix[row][i] / coefficient;
            }

            public void ToTrapezoidal()
            {
                int row = 0, column = 0;

                while (row < matrix.Length - 1 && column < matrix[row].Length)
                {
                    if (matrix[row][column] == 0)
                    {
                        int i = 0;
                        for (i = row + 1; i < matrix.Length; ++i)
                            if (matrix[i][column] != 0)
                            {
                                ReplaceRows(row, i);
                                break;
                            }
                        if (i >= matrix.Length)
                        {
                            ++column;
                            continue;
                        }
                    }

                    for (int i = row + 1; i < matrix.Length; ++i)
                        if (matrix[i][column] != 0)
                        {
                            double coefficient = matrix[i][column] / matrix[row][column];
                            RowMinusRow(i, row, coefficient);
                        }

                    ++row;
                    ++column;
                }
            }

            private int ContainsOneElement(int row)
            {
                int pos = -1;

                for (int i = 0; i < matrix[row].Length; ++i)
                    if (pos == -1)
                    {
                        if (matrix[row][i] != 0)
                            pos = i;
                    }
                    else if (matrix[row][i] != 0)
                        return -1;

                return pos;
            }

            public void ToFundamental()
            {
                ToTrapezoidal();

                for (int i = 0; i < matrix.Length; ++i)
                {
                    int pos = ContainsOneElement(i);
                    if (pos > -1 && pos != i)
                        ReplaceColumns(i, pos);
                }

                for (int i = 0; i < matrix.Length; ++i)
                    if (matrix[i][i] != 1)
                        DivideRow(i, matrix[i][i]);

                for (int i = matrix.Length - 1; i > 0; --i)
                    for (int j = i - 1; j >= 0; --j)
                        if (matrix[j][i] != 0)
                        {
                            double coefficient = matrix[j][i] / matrix[i][i];
                            RowMinusRow(j, i, coefficient);
                        }

                SortColumns();
            }

            public bool IsZero(int row)
            {
                for (int i = 0; i < matrix[row].Length; ++i)
                    if (matrix[row][i] != 0)
                        return false;

                return true;
            }

            public VectorSystem TransitionMatrix()
            {
                ToTrapezoidal();
                for (int i = 0; i < matrix.Length; ++i)
                    if (matrix[i][i] != 1)
                        DivideRow(i, matrix[i][i]);

                for (int i = matrix.Length - 1; i > 0; --i)
                    for (int j = i - 1; j >= 0; --j)
                        if (matrix[j][i] != 0)
                        {
                            double coefficient = matrix[j][i] / matrix[i][i];
                            RowMinusRow(j, i, coefficient);
                        }

                double[][] m = new double[matrix.Length][];
                for (int i = 0; i < matrix.Length; ++i)
                    m[i] = new double[matrix[i].Length / 2];

                for (int i = 0; i < m.Length; ++i)
                    for (int j = 0; j < matrix[i].Length / 2; ++j)
                        m[i][j] = matrix[i][matrix[i].Length / 2 + j];

                return new VectorSystem(m);
            }

            public VectorSystem Multiply(double[][] system1, double[][] system2)
            {
                double[][] r = new double[system1.Length][];
                for (int i = 0; i < system1.Length; ++i)
                    r[i] = new double[system2[i].Length];

                for (int i = 0; i < r.Length; ++i)
                    for (int j = 0; j < r[i].Length; ++j)
                    {
                        r[i][j] = 0;

                        for (int k = 0; k < system1[i].Length; ++k)
                            r[i][j] += system1[i][k] * system2[k][j];
                    }

                return new VectorSystem(r);
            }

            public string Task1()      
            {
                string result = "";
                ToTrapezoidal();

                int dimension = 0;

                for (int i = 0; i < matrix.Length; ++i)
                    if (!IsZero(i)) ++dimension;

                result += "dimension space: " + dimension + "\n";
                result += "Basis vectors can form: ";

                for (int i = 0; i < dimension; ++i)
                    result += numbersOfVectors[i] + 1 + ", ";

                return result.Remove(result.Length - 2);
            }

            public string Task2()
            {
                ToTrapezoidal();
                int dimensions = 0;

                for (int i = 0; i < matrix.Length; ++i)
                    if (!IsZero(i))
                        ++dimensions;

                if (dimensions == 3)
                    return ToString() + "\nThe target system is the basis of three-dimensional vector space.";
                else
                    return ToString() + "\nThe target system is the basis vectors are not three-dimensional space.";
            }

            public string Task3()
            {
                return TransitionMatrix().ToString();
            }

            public string Task4()
            {
                double[][] basis = new double[matrix.Length - 1][];
                for (int i = 0; i < basis.Length; ++i)
                    basis[i] = new double[matrix[i].Length];
                double[][] vector = new double[matrix[matrix.Length - 1].Length][];
                for (int i = 0; i < vector.Length; ++i)
                    vector[i] = new double[1];

                for (int i = 0; i < matrix.Length - 1; ++i)
                    for (int j = 0; j < matrix[i].Length; ++j)
                        basis[i][j] = matrix[i][j];

                for (int i = 0; i < vector.Length; ++i)
                    vector[i][0] = matrix[matrix.Length - 1][i];

                return Multiply(basis, vector).ToString();
            }

            public static string Task5(VectorSystem s1, VectorSystem s2)
            {
                string result = "";
                double[][] sum = new double[s1.matrix.Length + s2.matrix.Length][];
                double[][] transposed = new double[s1.matrix[0].Length][];
                for (int i = 0; i < sum.Length; ++i)
                    sum[i] = new double[s1.matrix[0].Length];

                for (int i = 0; i < transposed.Length; ++i)
                    transposed[i] = new double[s1.matrix.Length + s2.matrix.Length];

                for (int i = 0; i < s1.matrix.Length; ++i)
                    for (int j = 0; j < s1.matrix[i].Length; ++j)
                        transposed[j][i] = sum[i][j] = s1.matrix[i][j];

                for (int i = 0; i < s2.matrix.Length; ++i)
                    for (int j = 0; j < s2.matrix[i].Length; ++j)
                        transposed[j][i + s1.matrix.Length] = -(sum[i + s1.matrix.Length][j] = s2.matrix[i][j]);

                VectorSystem summ = new VectorSystem(sum);
                summ.ToTrapezoidal();

                int dimension = 0;

                for (int i = 0; i < sum.Length; ++i)
                    if (!summ.IsZero(i)) ++dimension;

                result += "The amount of linear dimension shells vectors: " + dimension + "\n";
                result += "The basis of the amount of linear vectors can form membranes vectors: ";

                for (int i = 0; i < dimension; ++i)
                    result += summ.numbersOfVectors[i] + 1 + ", ";

                VectorSystem transp = new VectorSystem(transposed);
                transp.ToFundamental();

                return result.Remove(result.Length - 2) + "\n\n" + transp.ToString();
            }

            public double[][] matrix;
            public int[] numbersOfVariables;
            public int[] numbersOfVectors;
        }
        static void Main(string[] args)
        {
            Console.WriteLine("Enter the number of lines");
            int N = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("enter the line");
            string s = "";
            for (int i = 0; i < N; i++)
            {
                s += Console.ReadLine();
                if (i < N - 1) s += '\n';
            }
            VectorSystem v = VectorSystem.Parse(s);
            Console.WriteLine("Enter the item of laboratory (1, 2, 3, 4, 5):");
            switch (Convert.ToInt32(Console.ReadLine()))
            {
                case 1:
                    {
                        Console.WriteLine("\n");
                        Console.WriteLine(v.Task1());
                        break;
                    }

                case 2:
                    {
                        Console.WriteLine("\n");
                        Console.WriteLine(v.Task2());
                        break;
                    }

                case 3:
                    {
                        Console.WriteLine("\nAnswer:\n");
                        Console.WriteLine(v.Task3());
                        break;
                    }

                case 4:
                    {
                        Console.WriteLine("\nAnswer:\n");
                        Console.WriteLine(v.Task4());
                        break;
                    }
                case 5:
                    {
                        Console.WriteLine("\n");
                        Console.WriteLine("Enter the number of lines");
                        N = Convert.ToInt32(Console.ReadLine());
                        Console.WriteLine("enter the line");
                        s = "";
                        for (int i = 0; i < N; i++)
                        {
                            s += Console.ReadLine();
                            if (i < N - 1) s += '\n';
                        }
                        VectorSystem v2 = VectorSystem.Parse(s);
                        Console.WriteLine("\n");
                        Console.WriteLine(VectorSystem.Task5(v, v2).ToString());
                        break;
                    }
            }
            Console.ReadKey();
        }
    }
}
