using ImageProcessor;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKinEditer
{
    class BitmapUtil
    {
        // 按比例缩放图片
        public static Image ZoomPicture(string file, Image SourceImage, int destWidth, int destHeight)
        {



            // System.Drawing.Imaging.ImageFormat format = SourceImage.RawFormat;


            int IntWidth = 0;
            int IntHeight = 0;
            //按比例缩放           
            int sourWidth = SourceImage.Width;
            int sourHeight = SourceImage.Height;
            if (sourHeight > destHeight || sourWidth > destWidth)
            {
                if ((sourWidth * destHeight) > (sourHeight * destWidth))
                {
                    IntWidth = destWidth;
                    IntHeight = (destWidth * sourHeight) / sourWidth;
                }
                else
                {
                    IntHeight = destHeight;
                    IntWidth = (sourWidth * destHeight) / sourHeight;
                }
            }
            else
            {
                IntWidth = sourWidth;
                IntHeight = sourHeight;
            }


            System.Drawing.Bitmap SaveImage = new System.Drawing.Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage(SaveImage);
            g.Clear(Color.Transparent);

            // Console.WriteLine(file + $"原圖{SourceImage.Width}x{SourceImage.Height} -- {IntWidth}x{IntHeight}");

            g.DrawImage(SourceImage, (destWidth - IntWidth) / 2, (destHeight - IntHeight) / 2, IntWidth, IntHeight);
            SourceImage.Dispose();


            return SaveImage;

        }
     
        public static void ResizeImageAndSave(string file, string savePath, int width, int height)
        {
            if (string.IsNullOrEmpty(file))
            {
                throw new ArgumentNullException(file);
            }
            byte[] photoBytes = System.IO.File.ReadAllBytes(file);
            // 检测格式
          //   ISupportedImageFormat format = new JpegFormat { Quality = 70 };
            Size size = new Size(width, height);
            using (MemoryStream inStream = new MemoryStream(photoBytes))
            {
                using (MemoryStream outStream = new MemoryStream())
                {
                    // 使用重载初始化ImageFactory以保留EXIF元数据。
                    using (ImageFactory imageFactory = new ImageFactory(true))
                    {
                        // 加载，调整大小，设置格式和质量并保存图像。
                        imageFactory.Load(inStream)
                                    .Resize(size)
                                    .Save(outStream);
                        
                        //对获取的imageFactory对象进行对应的操作
                    }
                    //对获取的数据流进行操作
                    File.WriteAllBytes(savePath, outStream.ToArray());
                }
            }

        }
    }
}
