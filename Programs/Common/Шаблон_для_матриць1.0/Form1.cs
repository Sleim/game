using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Шаблон_для_матриць1._0
{
    public partial class Form1 : Form {
        public static int rowsGl = 0;
        public Form1() {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e) {
            DataTable dt = new DataTable();
            int columns = Convert.ToInt32(textBox2.Text);
            int lines = Convert.ToInt32(textBox1.Text);
            for (int i = 0; i < columns; i++)
                dt.Columns.Add();
            for (int j = 0; j < lines; j++)
                dt.Rows.Add();
            dataGridView1.DataSource = dt;
            for (int i = 0; i < columns; i++)
                dataGridView1.Columns[i].Width = 50;
            for (int i = 0; i < lines; i++)
                dataGridView1.Rows[i].Height = 50;
            dataGridView1.Width = 50 * columns + 5;
            dataGridView1.Height = 50 * lines + 5;
            button2.Visible = true;
            richTextBox1.Visible = true;
        }

        private void Form1_Load(object sender, EventArgs e) {

        }

        private void button2_Click(object sender, EventArgs e) {
            int columns = Convert.ToInt32(textBox2.Text)-1;
            int lines = Convert.ToInt32(textBox1.Text);
            double[,] matrix = new double[lines, columns];
            for (int i = 0; i < lines; i++) {
                for (int j = 0; j < columns; j++) {
                    matrix[i, j] = Convert.ToDouble(dataGridView1[j, i].Value);
                }
            }
            double[] vector = new double[lines];
            for (int j = 0; j < columns; j++) {
                vector[j] = Convert.ToDouble(dataGridView1[columns,j].Value);
            }
            
            matrix = UpperDiagonalForm(matrix, lines, columns);
            matrix = Improve(matrix, lines, columns);
            richTextBox1.Text += "Rank: " + rowsGl;
            int solves = columns - rowsGl;
            richTextBox1.Text += "\nСистема має " + solves + " вільних членів\n\n";
            for(int i = 0; i < rowsGl; i++) {
                for(int j = 0; j < columns; j++) {
                    richTextBox1.Text += (matrix[i, j] + " ");
                }
                richTextBox1.Text += "\n";
            }
            richTextBox1.Text += "\n";
            for(int i = 0; i < rowsGl; i++) {
                for(int j = 0; j < columns; j++) {
                    if (j == rowsGl) {
                        richTextBox1.Text += "= ";
                    }
                    if (j >= rowsGl)
                        matrix[i, j] *= -1;
                    if (matrix[i, j] > 0)
                        richTextBox1.Text += ("+" + matrix[i, j] + "x" + (j+1) + " ");
                    else if (matrix[i, j] < 0)
                        richTextBox1.Text += (matrix[i,j]+"x"+(j+1)+" ");
                }
                richTextBox1.Text += "\n";
            }

            richTextBox1.Text += "\nЧастинний розв'язок\n";
            for(int i = 0; i < lines; i++) {
                if (i >= rowsGl)
                    richTextBox1.Text += "0";
                else
                    richTextBox1.Text += vector[i]/matrix[i,i];
                richTextBox1.Text += "\n";
            }
        
        }

        static double DeterminantGaussElimination(double[,] matrix, int rows, int columns) {
            matrix = UpperDiagonalForm(matrix, rows, columns);
            double det = 1;
            for (int i = 0; i < rows; i++)
                det = det * matrix[i, i];
            return det;
        }
        
        static double[,] UpperDiagonalForm(double[,] matrix, int rows, int columns) {
            int nm1 = rows - 1;
            int kp1;
            double p;
            for (int k = 0; k < nm1; k++) {
                kp1 = k + 1;
                for (int i = kp1; i < rows; i++) {
                    p = matrix[i, k] / matrix[k, k];
                    for (int j = 0; j < columns; j++)
                        matrix[i, j] = matrix[i, j] - p * matrix[k, j];
                }
            }

            return matrix;
        }
        
        static double[,] Improve(double[,] matrix, int rows, int columns) {
            int ind = 0;
            int counter = 0;
            for (int i = 0; i < rows; i++) {
                counter = 0;
                for (int j = 0; j < columns; j++) {
                    if (matrix[i, j] != 0)
                        counter++;
                }
                if (counter == 0) {
                    ind = i;
                    break;
                }

                
            }
            if (ind == 0)
                ind = rows;  
            rowsGl = ind;
            double[,] result = new double[ind, columns];
            for (int i = 0; i < ind; i++)
                for (int j = 0; j < columns; j++)
                    result[i, j] = matrix[i, j];
            return result;
        }
    }
}
