using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LR3_med_filter
{
    public partial class Form1 : Form
    {
        private Image image;
        Bitmap image1;
        public Form1()
        {
            InitializeComponent();
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e) // загрузка изображения
        {
            openFileDialog1.Filter = "Файлы изображений|*.bmp";
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            // получаем выбранный файл
            string filename = openFileDialog1.FileName;
            // читаем файл в объект класса Image
            try
            {
                image = Image.FromFile(filename);
                pictureBox1.Image = image;
                MessageBox.Show("Файл открыт","Оповещение");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка чтения картинки"+ex);
                return;
            }
        }

        private void button3_Click(object sender, EventArgs e) // сохранение изображения
        {
            if (image != null)
            {
                saveFileDialog1.Filter = "Файлы изображений|*.bmp";
                saveFileDialog1.ShowDialog();
                if (saveFileDialog1.FileName != "")
                {
                    saveFileDialog1.FileName = saveFileDialog1.FileName.ToUpper(); // название в верхнем регистре
                    System.IO.FileStream fs = (System.IO.FileStream)saveFileDialog1.OpenFile();
                    image.Save(fs, System.Drawing.Imaging.ImageFormat.Bmp);
                    fs.Close();
                }
            }
            else
            {
                MessageBox.Show("Нет изображения", "Ошибка");
                return;
            }
        }

        private void button2_Click(object sender, EventArgs e) // фильтрация
        {
            image1 = new Bitmap(image);
            Bitmap window;
            Rectangle rectangle;
            for(int i=1;i<image1.Height-1;i++)
            {
                for(int j=1;j<image1.Width-1;j++)
                {
                    rectangle = new Rectangle(j-1,i-1,3,3);
                    window = image1.Clone(rectangle, 0);
                    int R = getMedian(window, "R");
                    int G = getMedian(window, "G");
                    int B = getMedian(window, "B");
                    var set = Color.FromArgb(R, G, B); //Red, Green, Blue
                    image1.SetPixel(j, i, set);

                }
            }
            image = image1;
            pictureBox1.Image = image;

        }
        private int getMedian(Bitmap window,string mode)
        {
            int[] array = new int[9];
            int c = 0;
            switch(mode)
            {
                case "R":
                    for (int i = 0; i < 3; i++)//заполняем одномерный массив R значениями пискселей
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            array[c] = Convert.ToInt32(window.GetPixel(j, i).R);
                            c++;
                        }
                    }
                    int s = array.Length;
                    for (int i=0;i<array.Length;i++) // сортируем массив по возрастанию
                    {                        
                        for(int j=0;j<s-1;j++)
                        {
                            if(array[j]>array[j+1])
                            {
                                int k = array[j];
                                array[j] = array[j + 1];
                                array[j + 1] = k;
                            }
                        }
                        s--;
                    }
                    return array[4];
                case "G":
                    for (int i = 0; i < 3; i++)//заполняем одномерный массив R значениями пискселей
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            array[c] = Convert.ToByte(window.GetPixel(j, i).G);
                            c++;
                        }
                    }
                    s = array.Length;
                    for (int i = 0; i < array.Length; i++) // сортируем массив по возрастанию
                    {
                        for (int j = 0; j < s-1; j++)
                        {
                            if (array[j] > array[j + 1])
                            {
                                int k = array[j];
                                array[j] = array[j + 1];
                                array[j + 1] = k;
                            }
                        }
                        s--;
                    }
                    return array[4];
                case "B":
                    for (int i = 0; i < 3; i++)//заполняем одномерный массив R значениями пискселей
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            array[c] = Convert.ToByte(window.GetPixel(j, i).B);
                            c++;
                        }
                    }
                    s = array.Length;
                    for (int i = 0; i < array.Length; i++) // сортируем массив по возрастанию
                    {
                        for (int j = 0; j < s-1; j++)
                        {
                            if (array[j] > array[j + 1])
                            {
                                int k = array[j];
                                array[j] = array[j + 1];
                                array[j + 1] = k;
                            }
                        }
                        s--;
                    }
                    return array[4];
                default:
                    MessageBox.Show("Ошибка", "Ошибка");
                    throw new NullReferenceException();

            }
            
        }
    }
}
