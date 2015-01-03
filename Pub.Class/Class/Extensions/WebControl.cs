//------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2006 , LiveXY , Ltd. 
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;

namespace Pub.Class {
    /// <summary>
    /// DataGrid GridView��չ
    /// 
    /// �޸ļ�¼
    ///     2009.06.25 �汾��1.0 livexy ��������
    /// 
    /// </summary>
    public static class WebControlExtensions {
        /// <summary>
        /// �������ʱ����ɫ
        /// </summary>
        /// <param name="dg">DataGrid��չ</param>
        /// <param name="color">Ĭ����ɫ</param>
        /// <param name="hoverColor">hover��ɫ</param>
        public static void HoverScript(this DataGrid dg, string color, string hoverColor) {
            //�������ʱ����ɫ
            for (int i = 0; i < dg.Items.Count; i++) {
                if (dg.Items[i].ItemType.ToString() == "Item" || dg.Items[i].ItemType.ToString() == "AlternatingItem") {
                    TableRow tr = (TableRow)dg.Items[i].Cells[0].Parent;
                    Js.AddAttr(tr, "onmouseover", "this.bgColor='" + hoverColor + "'");
                    Js.AddAttr(tr, "onmouseout", "this.bgColor='" + color + "'");
                }
            }
        }
        /// <summary>
        /// �������ʱ����ɫ
        /// </summary>
        /// <param name="dv">GridView��չ</param>
        /// <param name="color">Ĭ����ɫ</param>
        /// <param name="hoverColor">hover��ɫ</param>
        public static void HoverScript(this GridView dv, string color, string hoverColor) {
            //�������ʱ����ɫ
            for (int i = 0; i < dv.Rows.Count; i++) {
                if (dv.Rows[i].RowType.ToString() == "DataRow") {
                    TableRow tr = (TableRow)dv.Rows[i].Cells[0].Parent;
                    Js.AddAttr(tr, "onmouseover", "gvBgColor = this.bgColor; this.bgColor='" + hoverColor + "'");
                    Js.AddAttr(tr, "onmouseout", "this.bgColor=" + (color == "" ? "gvBgColor;" : "'" + color + "'"));
                }
            }
        }
        /// <summary>
        /// ǿ�������ݰ�
        /// </summary>
        /// <example>
        /// <code>
        /// this.Eval&lt;Student>(p => p.Age)
        /// </code>
        /// </example>
        /// <typeparam name="TEntity">����</typeparam>
        /// <param name="page">Page��չ</param>
        /// <param name="func">����</param>
        /// <returns></returns>
        public static string Eval<TEntity>(this Page page, Func<TEntity, string> func) {
            return func((TEntity)page.GetDataItem()).ToStr();
        }
        /// <summary>
        /// ǿ�������ݰ�
        /// </summary>
        /// <example>
        /// <code>
        /// this.Eval&lt;Student, int>(p => p.Age)
        /// </code>
        /// </example>
        /// <typeparam name="TEntity">����</typeparam>
        /// <typeparam name="TResult">����ֵ����</typeparam>
        /// <param name="page">Page��չ</param>
        /// <param name="func">����</param>
        /// <returns></returns>
        public static TResult Eval<TEntity, TResult>(this Page page, Func<TEntity, TResult> func) {
            return func((TEntity)page.GetDataItem());
        }
        /// <summary>
        /// ȡ�ؼ���ֵ
        /// </summary>
        /// <param name="page">Page��չ</param>
        /// <param name="ctrlID">�ؼ�ID</param>
        /// <returns></returns>
        public static string GetControlValue(this Page page, string ctrlID) {
            Control control = page.FindControl(ctrlID);
            if (control is TextBox) return ((TextBox)control).Text;
            if (control is DropDownList) return ((DropDownList)control).SelectedItem.Value;
            return "";
        }
        /// <summary>
        /// ���ÿؼ���ֵ
        /// </summary>
        /// <param name="page">Page��չ</param>
        /// <param name="ctrlID">�ؼ�ID</param>
        /// <param name="value">ֵ</param>
        public static void SetControlValue(this Page page, string ctrlID, string value) {
            Control control = page.FindControl(ctrlID);
            if (control is TextBox) ((TextBox)control).Text = value;
            if (control is DropDownList) {
                DropDownList list = (DropDownList)control;
                foreach (ListItem item in list.Items) {
                    if (item.Value == value) {
                        item.Selected = true;
                        break;
                    }
                }
            }
        }
    }
}
