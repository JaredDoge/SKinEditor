using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SKinEditer
{
    public abstract class SKinRow
    {

        public string imgName { get; set; }

   

    
    }
    

    public class SkinRowEmpty : SKinRow
    {

        public SkinRowEmpty() { }

        public SkinRowEmpty(string imgName, string folderPath) {
            this.folderPath = folderPath;
            this.imgName=imgName;
        }
        public string folderPath { get; set; }

        public static implicit operator SkinRowEmpty(SKinRowImg v)
        {
            throw new NotImplementedException();
        }
    }

    public class SKinRowHeader : SKinRow
    {
        public int width { get; set; }
        public int height { get; set; }

        public string filePath { get; set; }

        public bool isWhite { get; set; }

        public string showText { get; set; }

        public SKinRowHeader() { }
        public SKinRowHeader(string imgName, string filePath, int width, int height,string showText,bool isWhite=false)
        {
            this.imgName = imgName;
            this.filePath = filePath;
            this.width = width;
            this.height = height;
            this.showText = showText;
            this.isWhite = isWhite;
        }
    }

    public class SKinRowImg : SKinRow
    {
        public SKinRowImg() { }

        public int width { get; set; }
        public int height { get; set; }

        public string filePath { get; set; }

        public SKinRowImg(/*Image image,*/string imgName, string filePath, int width, int height)
        {
           // this.image = image;
            this.imgName = imgName;
            this.filePath = filePath;
            this.width = width;
            this.height = height;
        }

       // public Image image { get; set; }
    }
     

}
