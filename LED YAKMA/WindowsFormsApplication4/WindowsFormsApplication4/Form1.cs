using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;
using AForge;
using AForge.Imaging;
using AForge.Imaging.Filters;
using System.Drawing.Imaging;
using System.IO.Ports;

namespace WindowsFormsApplication4
{
    public partial class Form1 : Form
    {
        int R;
        int G;
        int B;

        private FilterInfoCollection VideoDe;
        private VideoCaptureDevice Video;

        

        public Form1()
        {
            InitializeComponent();
            {
                comboBox2.DataSource = SerialPort.GetPortNames();
                VideoDe = new FilterInfoCollection(FilterCategory.VideoInputDevice);
                foreach (FilterInfo VideoCaptureDevice in VideoDe)
                {
                    comboBox1.Items.Add(VideoCaptureDevice.Name);
                }
                comboBox1.SelectedIndex = 0;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Video = new VideoCaptureDevice(VideoDe[comboBox1.SelectedIndex].MonikerString);
            Video.NewFrame += new NewFrameEventHandler(Video_NewFrame);
            Video.Start();
        }
        void Video_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            Bitmap video = (Bitmap)eventArgs.Frame.Clone();
            pictureBox1.Image = video;
            Bitmap image = (Bitmap)eventArgs.Frame.Clone();
            Bitmap image1 = (Bitmap)eventArgs.Frame.Clone();
            pictureBox1.Image = image;

           if (Seç.Checked)
            {
                EuclideanColorFiltering filter = new EuclideanColorFiltering();
                filter.CenterColor = new RGB(Color.FromArgb(R, G, B));
                filter.Radius = 100;
                filter.ApplyInPlace(image1);
                nesne(image1);
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Video.Stop();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            R = trackBar1.Value;
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            G = trackBar2.Value;
        }

        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            B = trackBar3.Value;
        }
        private System.Drawing.Point[] ToPointsArray(List<IntPoint> points)
        {
            System.Drawing.Point[] array = new System.Drawing.Point[points.Count];

            for (int i = 0, n = points.Count; i < n; i++)
            {
                array[i] = new System.Drawing.Point(points[i].X, points[i].Y);
            }
            return array;
        }
        public void nesne (Bitmap image)
        {
            BlobCounter blobCounter = new BlobCounter();
            blobCounter.MinWidth = 5;
            blobCounter.MinHeight = 5;
            blobCounter.FilterBlobs = true;
            blobCounter.ObjectsOrder = ObjectsOrder.Size;

            BitmapData objectsData = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadOnly, image.PixelFormat);
            Grayscale grayscaleFilter = new Grayscale(0.2125, 0.7154, 0.0721);
            UnmanagedImage grayImage = grayscaleFilter.Apply(new UnmanagedImage(objectsData));
            image.UnlockBits(objectsData);

            blobCounter.ProcessImage(image);
            Rectangle[] rects = blobCounter.GetObjectsRectangles();
            Blob[] blobs = blobCounter.GetObjectsInformation();
            try
            {
                pictureBox2.Image = image;
            }
            catch { }

            if (Dikdörtgen.Checked)
            {
                foreach (Rectangle recs in rects)
                {
                    if (rects.Length > 0)
                    {
                        Rectangle objectRect = rects[0];
                        Graphics g = pictureBox1.CreateGraphics();
                        using (Pen pen = new Pen(Color.FromArgb(252, 3, 26), 2))
                        {
                            g.DrawRectangle(pen, objectRect);
                        }
                        int objectX = objectRect.X + (objectRect.Width / 2);
                        int objectY = objectRect.Y + (objectRect.Height / 2);

                        g.Dispose();
                        {
                            Invoke((MethodInvoker)delegate
                            {
                            });

                            if (objectX > 0 && objectX < 105 && objectY < 80)
                            {
                                serialPort1.Write("1");
                            }
                            if (objectX > 105 && objectX < 215 && objectY < 80)
                            {
                                serialPort1.Write("2");
                            }
                            if (objectX > 215 && objectX < 325 && objectY < 80)
                            {
                                serialPort1.Write("3");
                            }
                            if (objectX > 0 && objectX < 105 && objectY > 80 && objectY < 160)
                            {
                                serialPort1.Write("4");
                            }
                            if (objectX > 105 && objectX < 215 && objectY > 80 && objectY < 160)
                            {
                                serialPort1.Write("5");
                            }
                            if (objectX > 215 && objectX < 325 && objectY > 80 && objectY < 160)
                            {
                                serialPort1.Write("6");
                            }
                            if (objectX > 0 && objectX < 105 && objectY > 160 && objectY < 240)
                            {
                                serialPort1.Write("7");
                            }
                            if (objectX > 105 && objectX < 215 && objectY > 160 && objectY < 240)
                            {
                                serialPort1.Write("8");
                            }
                            if (objectX > 215 && objectX < 325 && objectY > 160 && objectY < 240)
                            {
                                serialPort1.Write("9");
                            }
                        }
                    }
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            serialPort1.BaudRate = 9600;
            serialPort1.PortName = comboBox2.SelectedItem.ToString();
            serialPort1.Open();
            if (serialPort1.IsOpen)
            {
                MessageBox.Show("Port Açık");
            }
    }

        private void button4_Click(object sender, EventArgs e)
        {
            serialPort1.Close();
            MessageBox.Show("Port Kapalı");
        }
    }
}

