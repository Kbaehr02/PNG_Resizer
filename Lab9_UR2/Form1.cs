using Emgu.CV;
using Emgu.CV.CvEnum;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab9_UR2
{
    public partial class Form1 : Form
    {
        //begins thread and instantiates a file explorer dialogue box
        //Thread resizeThread;
        OpenFileDialog openFileDialog;
        //initiates string at no characters will hold file path later
        string path = "";
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
            //Gives basic instruction for use during runtime.
            beginLabel.Text = "Please enter file name, width, and height\nin the fields below before pressing convert";
            Invoke(new Action(() =>
            {
                FilenameTextBox.Location = new Point(beginLabel.Right+50, 25);
                WidthTextBox.Location = new Point(FilenameTextBox.Right+50, 25);
                HeightTextBox.Location = new Point(WidthTextBox.Right+50, 25);
                convert_PNG_Button.Location = new Point(HeightTextBox.Right+50, 25);
                SF_Width_Label.Location = new Point(beginLabel.Left, beginLabel.Bottom+35);
                SF_Height_Label.Location = new Point(beginLabel.Left, SF_Width_Label.Location.Y+50);
                RS_Width_Label.Location = new Point(beginLabel.Left, SF_Height_Label.Location.Y+50);
                RS_Height_Label.Location = new Point(beginLabel.Left, RS_Width_Label.Location.Y+50);
                OGPicBox.Location = new Point(SF_Width_Label.Location.X+(SF_Width_Label.Width+20), SF_Width_Label.Bottom);
                ResizedPicBox.Location = new Point(OGPicBox.Right + 50, OGPicBox.Top);
            }));
        }

        private void ImageResizer()
        {
            // declares string and stores the desired final filename for altered picture
            string fileName;
            fileName = FilenameTextBox.Text;
            //declaring mat and a clone to safely operate on
            Mat sourceFrame = new Mat();
            sourceFrame = CvInvoke.Imread(path, ImreadModes.AnyColor);
            Mat resizedFrame = sourceFrame.Clone();
            //Converts to integer from textbox string
            int width = Convert.ToInt32(WidthTextBox.Text);
            int height = Convert.ToInt32(HeightTextBox.Text);
            //displays unaltered picture in first picturebox for easy storage
            OGPicBoxChanges(sourceFrame);
            //inputs source image outputs the resized version to the resizedFrame clone, displays altered picture in second picturebox
            CvInvoke.Resize(sourceFrame, resizedFrame, new Size(width, height), 0, 0, Inter.Linear);    //This resizes the image into your specified width and height
            ResizedPicBoxChanges(resizedFrame);
            if (fileName!= "")
            {
                //calls the resizing function
                SaveBmpAsPNG(resizedFrame, fileName);
            }
            FilenameTextBox.Text = "Please enter desired filename";
            WidthTextBox.Text = "Please enter desired width(pix)";
            HeightTextBox.Text = "Please enter desired height (pix)";
        }
        private void OGPicBoxChanges(Mat sourceFrame)
        {
            Mat temp = sourceFrame.Clone();
            if (sourceFrame.Size.Width <= 750 && sourceFrame.Size.Height <= 750)
            {
                Size OGSize = new Size(sourceFrame.Size.Width, sourceFrame.Size.Height);
                Invoke(new Action(() =>
                {
                    OGPicBox.Size = OGSize;
                }));
            }
            else
            {
                int newHeight = ((sourceFrame.Size.Width * OGPicBox.Height) / sourceFrame.Size.Height);
                CvInvoke.Resize(sourceFrame, temp, new Size(OGPicBox.Width, newHeight), 0, 0, Inter.Linear);
                Invoke(new Action(() =>
                {
                    OGPicBox.Size = new Size(500, 500);
                }));
            }
            Invoke(new Action(() =>
            {
                SF_Height_Label.Text = $"Original picture pixel width is {sourceFrame.Size.Width}";
                SF_Width_Label.Text = $"Original picture pixel width is {sourceFrame.Size.Height}";
            }));
            OGPicBox.Image = temp.ToBitmap();
        }
        private void ResizedPicBoxChanges(Mat resizedFrame)
        {
            Point ResizedPicBoxLoaction = new Point(OGPicBox.Right + 100, OGPicBox.Top);
            Invoke(new Action(() =>
            {
                ResizedPicBox.Size = new Size(resizedFrame.Size.Width, resizedFrame.Size.Height);
                ResizedPicBox.Location = ResizedPicBoxLoaction;
                RS_Height_Label.Text = $"Resized picture pixel width is {resizedFrame.Size.Width}";
                RS_Width_Label.Text = $"Resized picture pixel width is {resizedFrame.Size.Height}";
            }));
            ResizedPicBox.Image = resizedFrame.ToBitmap();
        }
        private void convert_PNG_Button_Click(object sender, EventArgs e)
        {
            openFileDialog = new OpenFileDialog();
            //prevents moving further until user selects "OK" in file explorer
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                path = openFileDialog.FileName;
            }
            ImageResizer();
        }
        private void SaveBmpAsPNG(Mat resizedFrame, string var)
        {
            //Instantiates a new image object matching the altered picture box data
            Image copy = ResizedPicBox.Image;
            //saves altered image to specified folder path with user determined file name
            copy.Save($"C:\\Users\\Kurt\\Desktop\\ConvertedPics\\{var}.png", ImageFormat.Png);
        }
    }
    
}
