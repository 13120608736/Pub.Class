//------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2006 , LiveXY , Ltd. 
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Data;
using System.Data.Common;
using System.Web.Script.Serialization;
using System.IO;
using System.Security.Cryptography;
using System.Drawing;
using System.Drawing.Imaging;

namespace Pub.Class {
    /// <summary>
    /// Image��չ
    /// 
    /// �޸ļ�¼
    ///     2009.06.25 �汾��1.0 livexy ��������
    /// 
    /// </summary>
    public static class ImageExtensions {
        /// <summary>
        /// ��ӱ߿�
        /// </summary>
        /// <param name="image">Image��չ</param>
        /// <param name="borderWidth">�߿��</param>
        /// <param name="color">�߿���ɫ</param>
        /// <returns></returns>
        public static Image AppendBorder(this Image image, int borderWidth, Color color) {
            var newSize = new Size(image.Width + (borderWidth * 2), image.Height + (borderWidth * 2));
            var img = new Bitmap(newSize.Width, newSize.Height);
            var g = Graphics.FromImage(img);
            g.Clear(color);
            g.DrawImage(image, new Point(borderWidth, borderWidth));
            g.Dispose();
            return img;
        }
        /// <summary>
        /// ������Ч�� ԭ��ȷ��ͼ������λ�õ��ȷ�������˿�Ĵ�С,Ȼ�������˿�ͼ�񸲸�����㼴��.
        /// </summary>
        /// <param name="original">Bitmap��չ</param>
        /// <param name="scale">�ָ��val*val���ص�С����</param>
        /// <returns></returns>
        public static Bitmap ToMosaic(this Bitmap original, int scale) {
            Bitmap result = new Bitmap(original.Width * scale, original.Height * scale);
            using (Graphics g = Graphics.FromImage(result)) {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
                g.DrawImage(original, 0, 0, result.Width, result.Height);
            }
            return result;
        }
    }
}
