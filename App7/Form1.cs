using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace App7
{
    public partial class Form1 : Form
    {
        private BackgroundWorker matrixMultiplicationWorker;
        private int matrixSize = 0; // Variable to store the matrix size

        public Form1()
        {
            InitializeComponent();
            InitializeBackgroundWorker();
        }

        private void InitializeBackgroundWorker()
        {
            matrixMultiplicationWorker = new BackgroundWorker();
            matrixMultiplicationWorker.DoWork += MatrixMultiplicationWorker_DoWork;
            matrixMultiplicationWorker.ProgressChanged += MatrixMultiplicationWorker_ProgressChanged;
            matrixMultiplicationWorker.RunWorkerCompleted += MatrixMultiplicationWorker_RunWorkerCompleted;

            matrixMultiplicationWorker.WorkerReportsProgress = true;
            matrixMultiplicationWorker.WorkerSupportsCancellation = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (matrixMultiplicationWorker.IsBusy)
            {
                // If the worker is busy, cancel the current operation and reset the progress bar
                matrixMultiplicationWorker.CancelAsync();
            }

            
            matrixSize = new Random().Next(1, 1000);

            // Reset the progress bar
            progressBar1.Value = 0;

            // Start the background worker with the new matrix size
            matrixMultiplicationWorker.RunWorkerAsync();
        }

        private int[,] GenerateRandomMatrix(int size)
        {
            Random random = new Random();
            int[,] matrix = new int[size, size];

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    matrix[i, j] = random.Next(1, 10); // Adjust the range as needed
                }
            }

            return matrix;
        }

        private void MatrixMultiplicationWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            // Sample matrix multiplication operations
            int[,] matrixA = GenerateRandomMatrix(matrixSize);
            int[,] matrixB = GenerateRandomMatrix(matrixSize);
            int[,] resultMatrix = new int[matrixSize, matrixSize];

            for (int i = 0; i < matrixSize; i++)
            {
                if (worker.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }

                for (int j = 0; j < matrixSize; j++)
                {
                    resultMatrix[i, j] = 0;

                    for (int k = 0; k < matrixSize; k++)
                    {
                        resultMatrix[i, j] += matrixA[i, k] * matrixB[k, j];
                    }
                }

                // Report progress for each iteration
                int progressPercentage = (int)((float)(i + 1) / matrixSize * 100);
                worker.ReportProgress(progressPercentage, resultMatrix); // Pass the resultMatrix for additional handling
                Thread.Sleep(600); // Simulate some work being done
            }

            e.Result = resultMatrix;
        }

        private void MatrixMultiplicationWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
        }

        private void MatrixMultiplicationWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show("Error : " + e.Error.Message);
            }
            else if (!e.Cancelled)
            {
                MessageBox.Show("100%");
            }
        }
    }
}