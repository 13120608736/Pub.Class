//------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2006 , LiveXY , Ltd. 
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace Pub.Class {
    /// <summary>
    /// ����ͼ������
    /// 
    /// �޸ļ�¼
    ///     2012.06.02 �汾��1.0 livexy �޸�Ϊ��̬��
    ///     2006.05.02 �汾��1.0 livexy ��������
    ///     
    /// <example>
    /// <code>
    /// CutImage.CutImageByHeight(source, desc, 75); //��ͼƬ���ŵ�ָ���ĸ߶�
    /// CutImage.CutImageByWidth(source, desc, 75); //��ͼƬ���ŵ�ָ���Ŀ��
    /// CutImage.CutImageByWidthHeight(source, desc, 75, 75); //����X��������ͼ ������
    /// CutImage.CutImageCustom(source, desc, 75, 75); //����X��������ͼ ����
    /// CutImage.CutImageCustomMin(source, desc, 75, 75); //����X��������ͼ ѹ����С
    /// CutImage.CutImageSquare(source, desc, 75); //��ͼƬ���е�һ��������
    /// CutImage.CutImageByWidthHeight(source, desc, 75, 100, Color.Transparent); //����X��������ͼ ����͸��
    /// </code>
    /// </example>
    /// </summary>
    public class CutImage {
        /// <summary>
        /// ��ȡͼƬ��Ϣ
        /// </summary>
        /// <param name="imagePath">ͼƬ��ַ</param>
        /// <returns>�ɹ�trueʧ��false</returns>
        public static Size GetImage(string imagePath) {
            Size size = new Size();
            if (!imagePath.IsImgFile()) return size;

            System.Drawing.Image image = System.Drawing.Image.FromFile(imagePath);
            size.Width = image.Width;
            size.Height = image.Height;
            image.Dispose();
            return size;
        }
        /// <summary>
        /// ����X��������ͼ
        /// </summary>
        /// <param name="imagePath">Դͼ��ַ</param>
        /// <param name="savePath">��ͼ��ַ</param>
        /// <param name="cutWidth">���</param>
        /// <param name="cutHeight">�߶�</param>
        /// <param name="error">����ʱִ��</param>
        /// <returns>�ɹ�trueʧ��false</returns>
        public static bool CutImageCustomMin(string imagePath, string savePath, int cutWidth, int cutHeight, Action<Exception> error = null) {
            try {
                System.Drawing.Image objImage = System.Drawing.Image.FromFile(imagePath);
                float x = objImage.Width;
                float y = objImage.Height;
                float xPercent = x / cutWidth;
                float yPercent = y / cutHeight;

                int width = 0, height = 0;
                if (xPercent < yPercent) {
                    width = (int)((x * cutHeight) / y);
                    height = cutHeight;
                } else {
                    width = cutWidth;
                    height = (int)((cutWidth * y) / x);
                }

                System.Drawing.Image newimage = new Bitmap(objImage.Width, objImage.Height, PixelFormat.Format32bppRgb);
                Graphics g = Graphics.FromImage(newimage);
                g.DrawImage(objImage, 0, 0, objImage.Width, objImage.Height);
                g.Dispose();
                System.Drawing.Image thumbImage = newimage.GetThumbnailImage(width, height, null, IntPtr.Zero);
                thumbImage.Save(savePath, objImage.RawFormat);

                objImage.Dispose();
                newimage.Dispose();
                thumbImage.Dispose();

                FileStream fs = new FileStream(savePath, FileMode.Open, FileAccess.Read);
                fs.Close();

                return true;
            } catch (Exception ex) {
                if (error.IsNotNull()) error(ex);
                return false;
            }
        }
        /// <summary>
        /// ����X��������ͼ
        /// </summary>
        /// <param name="imagePath">Դͼ��ַ</param>
        /// <param name="savePath">��ͼ��ַ</param>
        /// <param name="cutWidth">���</param>
        /// <param name="cutHeight">�߶�</param>
        /// <param name="error">����ʱִ��</param>
        /// <returns>�ɹ�trueʧ��false</returns>
        public static bool CutImageCustom(string imagePath, string savePath, int cutWidth, int cutHeight, Action<Exception> error = null) {
            try {
                System.Drawing.Image objImage = System.Drawing.Image.FromFile(imagePath);
                float x = objImage.Width;
                float y = objImage.Height;

                float xPercent = x / cutWidth;
                float yPercent = y / cutHeight;

                int width = 0, height = 0;
                if (xPercent < yPercent) {
                    width = (int)((x * cutHeight) / y);
                    height = cutHeight;
                } else {
                    width = cutWidth;
                    height = (int)((cutWidth * y) / x);
                }

                Bitmap newimage = new Bitmap(width, height, PixelFormat.Format32bppRgb);
                newimage.SetResolution(72f, 72f);
                Graphics gdiobj = Graphics.FromImage(newimage);
                gdiobj.CompositingQuality = CompositingQuality.HighQuality;
                gdiobj.SmoothingMode = SmoothingMode.HighQuality;
                gdiobj.InterpolationMode = InterpolationMode.HighQualityBicubic;
#if !MONO40
                gdiobj.PixelOffsetMode = PixelOffsetMode.HighQuality;
#endif
                gdiobj.FillRectangle(new SolidBrush(Color.White), 0, 0, width, height);
                Rectangle destrect = new Rectangle(0, 0, width, height);
                gdiobj.DrawImage(objImage, destrect, 0, 0, objImage.Width, objImage.Height, GraphicsUnit.Pixel);
                gdiobj.Dispose();

                System.Drawing.Imaging.EncoderParameters ep = new System.Drawing.Imaging.EncoderParameters(1);
                ep.Param[0] = new System.Drawing.Imaging.EncoderParameter(System.Drawing.Imaging.Encoder.Quality, (long)100);

                ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
                ImageCodecInfo ici = null;
                foreach (ImageCodecInfo codec in codecs) {
                    if (codec.MimeType == "image/jpeg") { ici = codec; }
                }

                if (ici.IsNotNull()) newimage.Save(savePath, ici, ep); else newimage.Save(savePath, objImage.RawFormat);

                objImage.Dispose();
                newimage.Dispose();

                FileStream fs = new FileStream(savePath, FileMode.Open, FileAccess.Read);
                fs.Close();

                return true;
            } catch (Exception ex) {
                if (error.IsNotNull()) error(ex);
                return false;
            }
        }
        /// <summary>
        /// ��ͼƬ���ŵ�ָ���Ŀ��
        /// </summary>
        /// <param name="imagePath">Դͼ��ַ</param>
        /// <param name="savePath">��ͼ��ַ</param>
        /// <param name="error">����ʱִ��</param>
        /// <param name="square">���</param>
        /// <returns>�ɹ�trueʧ��false</returns>
        public static bool CutImageByWidth(string imagePath, string savePath, int square, Action<Exception> error = null) {
            try {
                int cutWidth = square;

                System.Drawing.Image objImage = System.Drawing.Image.FromFile(imagePath);
                float x = objImage.Width;
                float y = objImage.Height;

                int width = cutWidth;
                int height = (int)((cutWidth * y) / x);

                System.Drawing.Image newimage = new Bitmap(objImage.Width, objImage.Height, PixelFormat.Format32bppRgb);
                Graphics g = Graphics.FromImage(newimage);
                g.DrawImage(objImage, 0, 0, objImage.Width, objImage.Height);
                g.Dispose();
                System.Drawing.Image thumbImage = newimage.GetThumbnailImage(width, height, null, IntPtr.Zero);
                thumbImage.Save(savePath, objImage.RawFormat);

                objImage.Dispose();
                newimage.Dispose();
                thumbImage.Dispose();

                FileStream fs = new FileStream(savePath, FileMode.Open, FileAccess.Read);
                fs.Close();

                return true;
            } catch (Exception ex) {
                if (error.IsNotNull()) error(ex);
                return false;
            }
        }
        /// <summary>
        /// ��ͼƬ���ŵ�ָ���ĸ߶�
        /// </summary>
        /// <param name="imagePath">Դͼ��ַ</param>
        /// <param name="savePath">��ͼ��ַ</param>
        /// <param name="square">�߶�</param>
        /// <param name="error">����ʱִ��</param>
        /// <returns>�ɹ�trueʧ��false</returns>
        public static bool CutImageByHeight(string imagePath, string savePath, int square, Action<Exception> error = null) {
            try {
                int cutHeight = square;

                System.Drawing.Image objImage = System.Drawing.Image.FromFile(imagePath);
                float x = objImage.Width;
                float y = objImage.Height;

                int height = cutHeight;
                int width = (int)((cutHeight * x) / y);

                System.Drawing.Image newimage = new Bitmap(objImage.Width, objImage.Height, PixelFormat.Format32bppRgb);
                Graphics g = Graphics.FromImage(newimage);
                g.DrawImage(objImage, 0, 0, objImage.Width, objImage.Height);
                g.Dispose();
                System.Drawing.Image thumbImage = newimage.GetThumbnailImage(width, height, null, IntPtr.Zero);
                thumbImage.Save(savePath, objImage.RawFormat);

                objImage.Dispose();
                newimage.Dispose();
                thumbImage.Dispose();

                FileStream fs = new FileStream(savePath, FileMode.Open, FileAccess.Read);
                fs.Close();

                return true;
            } catch (Exception ex) {
                if (error.IsNotNull()) error(ex);
                return false;
            }
        }
        /// <summary>
        /// ��ͼƬ���е�һ��������
        /// </summary>
        /// <param name="imagePath">Դͼ��ַ</param>
        /// <param name="savePath">��ͼ��ַ</param>
        /// <param name="square">�����α߳�</param>
        /// <param name="error">����ʱִ��</param>
        /// <returns>�ɹ�trueʧ��false</returns>
        public static bool CutImageSquare(string imagePath, string savePath, int square, Action<Exception> error = null) {
            try {
                int width = square;
                int height = square;
                int cutWidth = square;
                int cutHeight = square;

                System.Drawing.Image objImage = System.Drawing.Image.FromFile(imagePath);

                if (objImage.Width >= objImage.Height) {
                    cutWidth = objImage.Height;
                    cutHeight = objImage.Height;
                } else {
                    cutWidth = objImage.Width;
                    cutHeight = objImage.Width;
                }

                System.Drawing.Image newimage = new Bitmap(cutWidth, cutHeight, PixelFormat.Format32bppRgb);
                Graphics g = Graphics.FromImage(newimage);
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                Rectangle destRect = new Rectangle(0, 0, cutWidth, cutHeight);
                Rectangle srcRect = new Rectangle(0, 0, cutWidth, cutHeight);
                GraphicsUnit units = GraphicsUnit.Pixel;

                g.DrawImage(objImage, destRect, srcRect, units);
                g.Dispose();
                System.Drawing.Image thumbImage = newimage.GetThumbnailImage(width, height, null, IntPtr.Zero);
                thumbImage.Save(savePath, objImage.RawFormat);

                objImage.Dispose();
                newimage.Dispose();
                thumbImage.Dispose();

                FileStream fs = new FileStream(savePath, FileMode.Open, FileAccess.Read);
                fs.Close();

                return true;
            } catch (Exception ex) {
                if (error.IsNotNull()) error(ex);
                return false;
            }
        }
        /// <summary>
        /// ����X��������ͼ ������
        /// </summary>
        /// <param name="imagePath">Դͼ��ַ</param>
        /// <param name="savePath">��ͼ��ַ</param>
        /// <param name="newWidth">���</param>
        /// <param name="newHeight">�߶�</param>
        /// <param name="error">����ʱִ��</param>
        /// <returns>�ɹ�trueʧ��false</returns>
        public static bool CutImageByWidthHeight(string imagePath, string savePath, int newWidth, int newHeight, Action<Exception> error = null) {
            return CutImageByWidthHeight(imagePath, savePath, newWidth, newHeight, Color.Transparent, error);
        }
        /// <summary>
        /// ����X��������ͼ ������
        /// </summary>
        /// <param name="imagePath">Դͼ��ַ</param>
        /// <param name="savePath">��ͼ��ַ</param>
        /// <param name="newWidth">���</param>
        /// <param name="newHeight">�߶�</param>
        /// <param name="error">����ʱִ��</param>
        /// <param name="bgcolor">��䱳��ɫ</param>
        /// <returns>�ɹ�trueʧ��false</returns>
        public static bool CutImageByWidthHeight(string imagePath, string savePath, int newWidth, int newHeight, Color bgcolor, Action<Exception> error = null) {
            //ԭʼͼƬ����ȡԭʼͼƬ�������󣬲�ʹ������Ƕ�����ɫ������Ϣ��  
            System.Drawing.Image originalImage = System.Drawing.Image.FromFile(imagePath);
            int ow = originalImage.Width;//ԭʼ���  
            int oh = originalImage.Height;//ԭʼ�߶�  
            Size toSize = ResizeSite(new Size(newWidth, newHeight), new Size(ow, oh));
            int towidth = toSize.Width;//ԭͼƬ���ź�Ŀ��  
            int toheight = toSize.Height;//ԭͼƬ���ź�ĸ߶�  
            int x = 0;
            int y = 0;
            x = (newWidth - towidth) / 2;
            y = (newHeight - toheight) / 2;
            //�½�һ��bmpͼƬ  
            System.Drawing.Image bitmap = new System.Drawing.Bitmap(towidth, toheight);
            //�½�һ������  
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bitmap);
            //���ø�������ֵ��  
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            //���ø�����,���ٶȳ���ƽ���̶�  
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            //��ջ�������͸������ɫ���  
            g.Clear(bgcolor);
            //��ָ��λ�ò��Ұ�ָ����С����ԭͼƬ��ָ������  
            g.DrawImage(originalImage, new System.Drawing.Rectangle(0, 0, towidth, toheight),
            new System.Drawing.Rectangle(0, 0, ow, oh),
            System.Drawing.GraphicsUnit.Pixel);
            //--------------------------------------------------------------------------------  
            //�½�һ��bmpͼƬ2  
            System.Drawing.Image bitmap2 = new System.Drawing.Bitmap(newWidth, newHeight);
            //�½�һ������2  
            System.Drawing.Graphics g2 = System.Drawing.Graphics.FromImage(bitmap2);
            //���ø�������ֵ��  
            g2.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            //���ø�����,���ٶȳ���ƽ���̶�  
            g2.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            //��ջ�������͸������ɫ���  
            g2.Clear(bgcolor);
            g2.DrawImageUnscaled(bitmap, new Point(x, y));

            try {
                bitmap2.Save(savePath, originalImage.RawFormat);
                return true;
            } catch (System.Exception ex) {
                if (error.IsNotNull()) error(ex);
                return false;
            } finally {
                originalImage.Dispose();
                bitmap.Dispose();
                bitmap2.Dispose();
                g2.Dispose();
                g.Dispose();
            }
        }
        /// <summary>
        /// ����ͼƬ������ʾ
        /// </summary>
        /// <param name="trueWidth"></param>
        /// <param name="trueHeight"></param>
        /// <param name="placeWidth"></param>
        /// <param name="placeHeight"></param>
        /// <param name="showWidth"></param>
        /// <param name="showHeight"></param>
        private static void GetProperSize(int trueWidth, int trueHeight, int placeWidth, int placeHeight, out int showWidth, out int showHeight) {
            if (trueHeight < placeHeight && trueWidth < placeWidth) {
                showHeight = trueHeight;
                showWidth = trueWidth;
            } else {
                float x = (float)trueWidth;
                float y = (float)trueHeight;

                float xPercent = x / placeWidth;
                float yPercent = y / placeHeight;

                if (xPercent < yPercent) {
                    showWidth = (int)((x * placeHeight) / y);
                    showHeight = placeHeight;
                } else {
                    showWidth = placeWidth;
                    showHeight = (int)((placeWidth * y) / x);
                }
            }
        }
        private static Size ResizeSite(Size specifySize, Size originalSize) {
            Size finaSize = new Size();
            float specifyScale = (float)specifySize.Width / (float)specifySize.Height;
            float originalScale = (float)originalSize.Width / (float)originalSize.Height;
            if (specifySize.Width >= originalSize.Width) {
                finaSize.Height = specifySize.Height;
                finaSize.Width = (int)(finaSize.Height * originalScale);
                if (finaSize.Width > specifySize.Width) {
                    finaSize.Width = specifySize.Width;
                    finaSize.Height = (int)(finaSize.Width / originalScale);
                }
            } else {
                finaSize.Width = specifySize.Width;
                finaSize.Height = (int)(finaSize.Width / originalScale);
                if (finaSize.Height > specifySize.Height) {
                    finaSize.Height = specifySize.Height;
                    finaSize.Width = (int)(finaSize.Height * originalScale);
                }
            }
            return finaSize;
        }
    }
}
