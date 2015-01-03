//------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2006 , LiveXY , Ltd. 
//------------------------------------------------------------

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace Pub.Class {
    /// <summary>
    /// ���ˮӡ�� ֻ֧�����ͼƬˮӡ
    /// 
    /// �޸ļ�¼
    ///     2006.05.15 �汾��1.0 livexy ��������
    /// 
    /// <example>
    /// <code>
    ///     //ͼƬˮӡ
    ///     Watermark wm = new Watermark();
    ///     wm.DrawedImagePath= Server.MapPath("") + "/upfile/" + "backlogo.gif";
    ///     wm.ModifyImagePath=path; 
    ///     wm.RightSpace=184;
    ///     wm.BottoamSpace=81;
    ///     wm.LucencyPercent=50;
    ///     wm.OutPath=Server.MapPath("") + "/upfile/" + fileName + "_new" + extension;
    ///     wm.DrawImage();
    ///     //�����ˮӡ�����ͼƬ,ɾ��ԭʼͼƬ 
    ///     mFileName=fileName + "_new" + extension;
    ///     if(File.Exists(path)) {  File.Delete(path); } 
    ///     
    ///     //����ˮӡ
    ///     Watermark wm = new Watermark();
    ///     wm.ModifyImagePath=path; 
    ///     wm.OutPath=Server.MapPath("") + "/upfile/" + fileName + "_new" + extension;
    ///     wm.DrawText("����", 1, 100);
    /// </code>
    /// </example>
    /// </summary>
    public class Watermark {
        //#region ˽�г�Ա
        private string modifyImagePath = null;
        private string drawedImagePath = null;
        private int rightSpace;
        private int bottoamSpace;
        private int lucencyPercent = 70;
        private string outPath = null;
        //#endregion
        //#region ������
        /// <summary>
        /// ���캯��
        /// </summary>
        public Watermark() { }
        //#endregion
        //#region ����
        /// <summary>
        /// ��ȡ������Ҫ�޸ĵ�ͼ��·��
        /// </summary>
        public string ModifyImagePath {
            get { return this.modifyImagePath; }
            set { this.modifyImagePath = value; }
        }
        /// <summary>
        /// ��ȡ�������ڻ���ͼƬ·��(ˮӡͼƬ)
        /// </summary>
        public string DrawedImagePath {
            get { return this.drawedImagePath; }
            set { this.drawedImagePath = value; }
        }
        /// <summary>
        /// ��ȡ������ˮӡ���޸�ͼƬ�е��ұ߾�
        /// </summary>
        public int RightSpace {
            get { return this.rightSpace; }
            set { this.rightSpace = value; }
        }
        /// <summary>
        /// ��ȡ������ˮӡ���޸�ͼƬ�о�ײ��ĸ߶�
        /// </summary>
        public int BottoamSpace {
            get { return this.bottoamSpace; }
            set { this.bottoamSpace = value; }
        }
        /// <summary>
        /// ��ȡ������Ҫ����ˮӡ��͸����,ע����ԭ��ͼƬ͸���ȵİٷֱ�
        /// </summary>
        public int LucencyPercent {
            get { return this.lucencyPercent; }
            set { if (value >= 0 && value <= 100) this.lucencyPercent = value; }
        }
        /// <summary>
        /// ��ȡ������Ҫ���ͼ���·��
        /// </summary>
        public string OutPath {
            get { return this.outPath; }
            set { this.outPath = value; }
        }
        //#endregion
        //#region ��ʼ����ˮӡ DrawImage
        /// <summary>
        /// ��ʼ����ͼƬˮӡ
        /// </summary>
        /// <example>
        /// <code>
        ///     Watermark wm = new Watermark();
        ///     wm.DrawedImagePath= Server.MapPath("") + "/upfile/" + "backlogo.gif";
        ///     wm.ModifyImagePath=path; 
        ///     wm.RightSpace=184;
        ///     wm.BottoamSpace=81;
        ///     wm.LucencyPercent=50;
        ///     wm.OutPath=Server.MapPath("") + "/upfile/" + fileName + "_new" + extension;
        ///     wm.DrawImage();
        ///     
        ///     //�����ˮӡ�����ͼƬ,ɾ��ԭʼͼƬ 
        ///     mFileName=fileName + "_new" + extension;
        ///     if(File.Exists(path)) {  File.Delete(path); } 
        /// </code>
        /// </example>
        public void DrawImage() {
            Image modifyImage = null;
            Image drawedImage = null;
            Graphics g = null;
            try {
                modifyImage = Image.FromFile(this.ModifyImagePath);//����ͼ�ζ���
                drawedImage = Image.FromFile(this.DrawedImagePath);
                g = Graphics.FromImage(modifyImage);

                int x = modifyImage.Width - this.rightSpace;//��ȡҪ����ͼ������
                int y = modifyImage.Height - this.BottoamSpace;

                float[][] matrixItems ={//������ɫ����
									   new float[] {1, 0, 0, 0, 0},
									   new float[] {0, 1, 0, 0, 0},
									   new float[] {0, 0, 1, 0, 0},
									   new float[] {0, 0, 0, (float)this.LucencyPercent/100f, 0},
									   new float[] {0, 0, 0, 0, 1}};

                ColorMatrix colorMatrix = new ColorMatrix(matrixItems);
                ImageAttributes imgAttr = new ImageAttributes();
                imgAttr.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

                g.DrawImage(//������Ӱͼ��
                    drawedImage,
                    new Rectangle(x, y, drawedImage.Width, drawedImage.Height),
                    0, 0, drawedImage.Width, drawedImage.Height,
                    GraphicsUnit.Pixel, imgAttr);

                string[] allowImageType = { ".jpg", ".gif", ".png", ".bmp", ".tiff", ".wmf", ".ico" };//�����ļ�
                FileInfo file = new FileInfo(this.ModifyImagePath);
                ImageFormat imageType = ImageFormat.Gif;
                switch (file.Extension.ToLower()) {
                    case ".jpg": imageType = ImageFormat.Jpeg; break;
                    case ".gif": imageType = ImageFormat.Gif; break;
                    case ".png": imageType = ImageFormat.Png; break;
                    case ".bmp": imageType = ImageFormat.Bmp; break;
                    case ".tif": imageType = ImageFormat.Tiff; break;
                    case ".wmf": imageType = ImageFormat.Wmf; break;
                    case ".ico": imageType = ImageFormat.Icon; break;
                    default: break;
                }
                MemoryStream ms = new MemoryStream();
                modifyImage.Save(ms, imageType);
                byte[] imgData = ms.ToArray();
                modifyImage.Dispose();
                drawedImage.Dispose();
                g.Dispose();
                FileStream fs = null;
                if (this.OutPath.IsNull() || this.OutPath == "") {
                    File.Delete(this.ModifyImagePath);
                    fs = new FileStream(this.ModifyImagePath, FileMode.Create, FileAccess.Write);
                } else {
                    fs = new FileStream(this.OutPath, FileMode.Create, FileAccess.Write);
                }
                if (fs.IsNotNull()) {
                    fs.Write(imgData, 0, imgData.Length);
                    fs.Close();
                }
            } finally {
                try {
                    drawedImage.Dispose();
                    modifyImage.Dispose();
                    g.Dispose();
                } catch { }
            }
        }
        /// <summary>
        /// ��ʼ��������ˮӡ
        /// </summary>
        /// <example>
        /// <code>
        /// Watermark wm = new Watermark();
        /// wm.ModifyImagePath=path; 
        /// wm.OutPath=Server.MapPath("") + "/upfile/" + fileName + "_new" + extension;
        /// wm.DrawText("����", 1, 100);
        /// </code>
        /// </example>
        /// <param name="watermarkText">����</param>
        /// <param name="watermarkStatus">1-9</param>
        /// <param name="quality">����</param>
        /// <param name="fontname">����</param>
        /// <param name="fontsize">��С</param>
        public void DrawText(string watermarkText, int watermarkStatus, int quality, string fontname="����", int fontsize=12) {
            if (!FileDirectory.FileExists(this.ModifyImagePath)) return;

            Image img = Image.FromFile(this.ModifyImagePath);
            string filename = this.OutPath.IsNullEmpty() ? this.ModifyImagePath : this.OutPath;

            Graphics g = Graphics.FromImage(img);
            Font drawFont = new Font(fontname, fontsize, FontStyle.Regular, GraphicsUnit.Pixel);
            SizeF crSize;
            crSize = g.MeasureString(watermarkText, drawFont);

            float xpos = 0;
            float ypos = 0;

            switch (watermarkStatus) {
                case 1:
                    xpos = (float)img.Width * (float).01;
                    ypos = (float)img.Height * (float).01;
                    break;
                case 2:
                    xpos = ((float)img.Width * (float).50) - (crSize.Width / 2);
                    ypos = (float)img.Height * (float).01;
                    break;
                case 3:
                    xpos = ((float)img.Width * (float).99) - crSize.Width;
                    ypos = (float)img.Height * (float).01;
                    break;
                case 4:
                    xpos = (float)img.Width * (float).01;
                    ypos = ((float)img.Height * (float).50) - (crSize.Height / 2);
                    break;
                case 5:
                    xpos = ((float)img.Width * (float).50) - (crSize.Width / 2);
                    ypos = ((float)img.Height * (float).50) - (crSize.Height / 2);
                    break;
                case 6:
                    xpos = ((float)img.Width * (float).99) - crSize.Width;
                    ypos = ((float)img.Height * (float).50) - (crSize.Height / 2);
                    break;
                case 7:
                    xpos = (float)img.Width * (float).01;
                    ypos = ((float)img.Height * (float).99) - crSize.Height;
                    break;
                case 8:
                    xpos = ((float)img.Width * (float).50) - (crSize.Width / 2);
                    ypos = ((float)img.Height * (float).99) - crSize.Height;
                    break;
                case 9:
                    xpos = ((float)img.Width * (float).99) - crSize.Width;
                    ypos = ((float)img.Height * (float).99) - crSize.Height;
                    break;
            }

            g.DrawString(watermarkText, drawFont, new SolidBrush(Color.White), xpos + 1, ypos + 1);
            g.DrawString(watermarkText, drawFont, new SolidBrush(Color.Black), xpos, ypos);

            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
            ImageCodecInfo ici = null;
            foreach (ImageCodecInfo codec in codecs) {
                if (codec.MimeType.IndexOf("jpeg") > -1)
                    ici = codec;
            }
            EncoderParameters encoderParams = new EncoderParameters();
            long[] qualityParam = new long[1];
            if (quality < 0 || quality > 100)
                quality = 80;

            qualityParam[0] = quality;

            EncoderParameter encoderParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, qualityParam);
            encoderParams.Param[0] = encoderParam;

            if (FileDirectory.FileExists(this.ModifyImagePath)) FileDirectory.FileDelete(this.ModifyImagePath);
            if (ici.IsNotNull())
                img.Save(filename, ici, encoderParams);
            else
                img.Save(filename);

            g.Dispose();
            img.Dispose();
        }
        //#endregion
    }
}
