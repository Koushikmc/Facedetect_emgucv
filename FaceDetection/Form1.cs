using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.CV.UI;
using Emgu.CV.CvEnum;
using Emgu.CV.Cuda;

namespace FaceDetection
{
    public partial class Form1 : Form
    {
        private Capture cap;
        private CascadeClassifier classify;
        private CascadeClassifier classify1;
        private Boolean progress;




        public Form1()
        {

            InitializeComponent();
        }

        private void Process(object sender, EventArgs arg)
        {
            classify = new CascadeClassifier("haarcascade_frontalface_default.xml");
            classify1 = new CascadeClassifier("haarcascade_eye.xml");

            Image<Bgr, Byte> img = cap.QueryFrame().ToImage<Bgr, Byte>();
            if (img != null)
            {
                Image<Gray,byte> grayscaleframe = img.Convert<Gray, byte>();
                var faces = classify.DetectMultiScale(grayscaleframe, 1.1,3,Size.Empty,Size.Empty);
                var eyes = classify1.DetectMultiScale(grayscaleframe, 1.1, 10,Size.Empty,Size.Empty);

                foreach(var face in faces)
                {
                    
                   img.Draw(face, new Bgr(Color.Blue));
                }

               foreach(var eye in eyes)
                {
                    img.Draw(eye, new Bgr(Color.Red), 2);
                }
            }

            imgCap.Image = img;

        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (cap == null)
            {
                try
                {
                    cap = new Capture();
                }
                catch (NullReferenceException excpt)
                {
                    MessageBox.Show(excpt.Message);
                }
            }
            if (cap != null)
            {
                if (progress)
                {  //if camera is getting frames then stop the capture and set button Text
                    // "Start" for resuming capture
                    button1.Text = "Start!"; //
                    Application.Idle -= Process;
                }
                else
                {
                    //if camera is NOT getting frames then start the capture and set button
                    // Text to "Stop" for pausing capture
                    button1.Text = "Stop";
                    Application.Idle += Process;
                }

                progress = !progress;
            }
        }
        private void ReleaseData()
        {
            if (cap != null)
                cap.Dispose();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
                    }
    }
}
