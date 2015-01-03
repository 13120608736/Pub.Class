//------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2006 , LiveXY , Ltd. 
//------------------------------------------------------------

using System;
using System.Data;
using System.Web;
using System.Configuration;
using System.Text;
using System.Data.Common;
using System.Web.UI;
using System.Collections.Generic;
using System.IO;

namespace Pub.Class {
    /// <summary>
    /// ģ��ҳ�̳���
    /// 
    /// �޸ļ�¼
    ///     2006.05.08 �汾��1.0 livexy ��������
    /// 
    /// </summary>
    public partial class MasterPageBase : MasterPage {
        //#region ˽��
        /// <summary>
        /// ����CACHE
        /// </summary>
        private static readonly ISafeDictionary<string, ISafeDictionary<string, string>> langList = new SafeDictionary<string, ISafeDictionary<string, string>>();
        /// <summary>
        /// ����
        /// </summary>
        protected string lang = string.Empty;
        /// <summary>
        /// Ƥ��
        /// </summary>
        protected string skin = "default";
        /// <summary>
        /// ����
        /// </summary>
        protected string title = string.Empty;
        /// <summary>
        /// ����
        /// </summary>
        protected string description = string.Empty;
        /// <summary>
        /// �ؼ���
        /// </summary>
        protected string keywords = string.Empty;
        /// <summary>
        /// ����ֵ
        /// </summary>
        protected int index = 0;
        /// <summary>
        /// ����JS
        /// </summary>
        protected StringBuilder js = new StringBuilder();
        /// <summary>
        /// ����CSS
        /// </summary>
        protected StringBuilder css = new StringBuilder();
        /// <summary>
        /// ��Ը�·�� /��ͷ
        /// </summary>
        public string RootPath = Request2.GetRelativeRoot();
        //#endregion
        /// <summary>
        /// ����
        /// </summary>
        public string Lang { get { return lang; } set { lang = value; } }
        /// <summary>
        /// Ƥ��
        /// </summary>
        public string Skin { get { return skin; } set { skin = value; } }
        /// <summary>
        /// ����
        /// </summary>
        public string Title { get { return title; } set { description = value + ", "; keywords = value + ", "; title = value + " - "; } }
        /// <summary>
        /// ����
        /// </summary>
        public string Description { get { return description; } set { description = value.Length > 0 ? value + ", " : value; } }
        /// <summary>
        /// �ؼ���
        /// </summary>
        public string Keywords { get { return keywords; } set { keywords = value.Length > 0 ? value + ", " : value; } }
        /// <summary>
        /// ����ֵ
        /// </summary>
        public int Index { get { return index; } set { index = value; } }
        /// <summary>
        /// ����JS
        /// </summary>
        public string JS { get { return js.ToString(); } set { value.Split(';').Do((s, i) => { js.AppendFormat("<script language=\"JavaScript\" type=\"text/javascript\" src=\"{0}\"></script>", s); }); } }
        /// <summary>
        /// ����CSS
        /// </summary>
        public string CSS { get { return css.ToString(); } set { value.Split(';').Do((s, i) => { js.AppendFormat("<link rel=\"stylesheet\" type=\"text/css\" href=\"{0}\" />", s); }); } }
        /// <summary>
        /// ȡ��������
        /// </summary>
        /// <returns></returns>
        private ISafeDictionary<string, string> GetLang() {
            if (lang.IsNullEmpty()) Msg.WriteEnd("����δ���ã�");
            string path = "".GetMapPath() + "\\lang\\{0}.lang".FormatWith(lang);
            if (!FileDirectory.FileExists(path)) Msg.WriteEnd("�����ļ�{0}.lang�����ڣ�".FormatWith(lang));

            string lineText = string.Empty; ISafeDictionary<string, string> list = new SafeDictionary<string, string>();
            using (StreamReader reader = new StreamReader(path, System.Text.Encoding.UTF8)) {
                while ((lineText = reader.ReadLine()).IsNotNull()) {
                    int len = lineText.IndexOf('=');
                    if (lineText.IsNullEmpty() || len == -1) continue;
                    string key = lineText.Substring(0, len).Trim();
                    string value = lineText.Substring(len + 1).Trim();
                    if (!list.ContainsKey(key)) list.Add(key, value); else list[key] = value;
                }
            }
            return list;
        }
        /// <summary>
        /// ȡ����
        /// </summary>
        /// <param name="key">key</param>
        /// <returns></returns>
        public string GetLang(string key) {
            if (!langList.ContainsKey(lang)) langList[lang] = GetLang();
            if (!langList[lang].ContainsKey(key)) return string.Empty;
            return langList[lang][key];
        }
    }
}
