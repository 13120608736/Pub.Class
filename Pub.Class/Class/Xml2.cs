//------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2006 , LiveXY , Ltd. 
//------------------------------------------------------------

using System;
using System.Data;
using System.Xml;
using System.IO;

namespace Pub.Class {
    /// <summary>
    /// Xmls������
    /// 
    /// �޸ļ�¼
    ///     2006.05.18 �汾��1.0 livexy ��������
    /// 
    /// <example>
    /// <code>
    ///     string strXmlFile = Server.MapPath("~/web.config");
    ///     Xml2 _xml = new Xml2(strXmlFile);
    ///     _xml.AddNode("configuration//appSettings","add", "key|value", "12|1111111");
    ///     _xml.AddNode("configuration//appSettings", "add", "key|value", "12|1111111", "cexo255");
    ///     Response.Write(_xml.getNodeText("configuration//appSettings//add[@key='12']"));
    ///     _xml.SetAttr("configuration//appSettings//add[@key='']", "value|providerName", "aaaaaaaaaaaa3|System.Data.SqlClient3");
    ///     _xml.AddAttr("configuration//appSettings//add[@key='']", "value|providerName","aaaaaaaaaaaa|System.Data.SqlClient");
    ///     Response.Write(_xml.getAttr("configuration//appSettings//add[@key='']", "value|providerName"));
    ///     _xml.Save();
    ///     switch (_xml.State) { 
    ///         case 0:
    ///             Js.Alert(this, "�����ɹ���");
    ///             break;
    ///         case 1:
    ///             Js.Alert(this, "�޷�����XML�ļ�");
    ///             break;
    ///         case 2:
    ///             Js.Alert(this, "����ʧ��");
    ///             break;
    ///         case 3:
    ///             Js.Alert(this, "������Ӧ����ȷ");
    ///             break;
    ///         case 4:
    ///             Js.Alert(this, "��������");
    ///             break;
    ///     }
    ///     string xmlText = _xml.ToXmlText();
    /// </code>
    /// </example>
    /// </summary>
    public class Xml2 {
        //#region ˽�г�Ա
        private readonly string strXmlFile = string.Empty;
        private XmlDocument objXmlDoc = new XmlDocument();
        private int state = 0;
        //#endregion
        //#region ����
        /// <summary>
        /// ���ز���״̬
        /// </summary>
        public int State { get { return state; } }
        //#endregion
        //#region ������
        /// <summary>
        /// ������
        /// </summary>
        /// <param name="xmlFile"></param>
        public Xml2(string xmlFile) {
            strXmlFile = xmlFile;
            try { objXmlDoc.Load(xmlFile); } catch { state = 1; } //�޷�����XML�ļ�
        }
        //#endregion
        //#region GetData
        /// <summary>
        /// ����XML�ļ���������
        /// </summary>
        /// <returns>DataSet</returns>
        public DataSet GetData() {
            DataSet ds = new DataSet();
            ds.ReadXml(@strXmlFile);
            return ds;
        }
        /// <summary>
        /// ����ָ��������������
        /// </summary>
        /// <param name="node">���</param>
        /// <returns>DataSet</returns>
        public DataSet GetData(string node) {
            string mainNode = node.TrimEnd('/');
            DataSet ds = new DataSet();
            StringReader read = new StringReader(objXmlDoc.SelectSingleNode(mainNode).OuterXml);
            ds.ReadXml(read);
            return ds;
        }
        //#endregion
        //#region Node/DelNode/GetNodeText/SetNodeText/AddNode
        /// <summary>
        /// ȡ��������
        /// </summary>
        /// <param name="node">���</param>
        /// <returns>����</returns>
        public string GetNodeText(string node) {
            string mainNode = node.TrimEnd('/'), _value = "";
            XmlNode objNode = objXmlDoc.SelectSingleNode(mainNode);
            _value = objNode.InnerText;
            return _value;
        }
        /// <summary>
        /// �������ý�������
        /// </summary>
        /// <param name="node">���</param>
        /// <param name="content">����</param>
        public void SetNodeText(string node, string content) {
            string mainNode = node.TrimEnd('/');
            XmlNode objNode = objXmlDoc.SelectSingleNode(mainNode);
            objNode.InnerText = content;
        }
        /// <summary>
        /// ��������ݵĽ�㡣������������ֵ֧���á�|���ֿ����ַ���
        /// </summary>
        /// <param name="mainNode">��ǰ���</param>
        /// <param name="node">�½����</param>
        /// <param name="attributeName">������</param>
        /// <param name="attributeValue">����ֵ</param>
        /// <param name="content">����</param>
        public void AddNode(string mainNode, string node, string attributeName, string attributeValue, string content) {
            string _mainNode = mainNode.TrimEnd('/');
            string[] attributeNameArr = attributeName.Split('|'), attributeValueArr = attributeValue.Split('|');
            if (attributeValueArr.Length != attributeNameArr.Length) { state = 3; return; }//��������ȷ
            XmlNode objNode = objXmlDoc.SelectSingleNode(_mainNode);
            XmlElement objElement = objXmlDoc.CreateElement(node);
            if (attributeName.Trim() != "") {
                for (int i = 0; i <= attributeNameArr.Length - 1; i++) {
                    if (objNode.Attributes[attributeNameArr[i]].IsNull()) {
                        objElement.SetAttribute(attributeNameArr[i], attributeValueArr[i]);
                    }
                }
            }
            objElement.InnerText = content;
            objNode.AppendChild(objElement);
        }
        /// <summary>
        /// ��������ݵĽ�㡣������������ֵ֧���á�|���ֿ����ַ���
        /// </summary>
        /// <param name="mainNode">��ǰ���</param>
        /// <param name="node">�½����</param>
        /// <param name="attributeName">������</param>
        /// <param name="attributeValue">����ֵ</param>
        public void AddNode(string mainNode, string node, string attributeName, string attributeValue) {
            string _mainNode = mainNode.TrimEnd('/');
            string[] attributeNameArr = attributeName.Split('|'), attributeValueArr = attributeValue.Split('|');
            if (attributeValueArr.Length != attributeNameArr.Length) { state = 3; return; }//��������ȷ
            XmlNode objNode = objXmlDoc.SelectSingleNode(_mainNode);
            XmlElement objElement = objXmlDoc.CreateElement(node);
            if (attributeName.Trim() != "") {
                for (int i = 0; i <= attributeNameArr.Length - 1; i++) {
                    if (objNode.Attributes[attributeNameArr[i]].IsNull()) {
                        objElement.SetAttribute(attributeNameArr[i], attributeValueArr[i]);
                    }
                }
            }
            objNode.AppendChild(objElement);
        }
        /// <summary>
        /// ɾ�����
        /// </summary>
        /// <param name="mainNode"></param>
        public void DelNode(string mainNode) {
            string _mainNode = mainNode.TrimEnd('/');
            XmlNode objNode = objXmlDoc.SelectSingleNode(_mainNode);
            if (objNode.IsNotNull()) objNode.ParentNode.RemoveChild(objNode);
        }
        //#endregion
        //#region Attr
        /// <summary>
        /// ȡָ����������ֵ��������֧���á�|���ֿ����ַ���
        /// </summary>
        /// <param name="node">���</param>
        /// <param name="attributeName">������</param>
        /// <returns></returns>
        public string GetAttr(string node, string attributeName) {
            string mainNode = node.TrimEnd('/'), _value = "";
            string[] attributeNameArr = attributeName.Split('|');
            XmlNode objNode = objXmlDoc.SelectSingleNode(mainNode);
            for (int i = 0; i <= attributeNameArr.Length - 1; i++) {
                try {
                    _value += objNode.Attributes[attributeNameArr[i]].Value.ToString() + "|";
                } catch { _value += "|"; }
            }
            return _value.Substring(0, _value.Length - 1);
        }
        /// <summary>
        /// Ϊָ���������µ�����ֵ�������������ӡ�������������ֵ֧���á�|���ֿ����ַ���
        /// </summary>
        /// <param name="node">���</param>
        /// <param name="attributeName">������</param>
        /// <param name="attributeValue">����ֵ</param>
        public void AddAttr(string node, string attributeName, string attributeValue) {
            string _mainNode = node.TrimEnd('/');
            string[] attributeNameArr = attributeName.Split('|'), attributeValueArr = attributeValue.Split('|');
            if (attributeValueArr.Length != attributeNameArr.Length) { state = 3; return; }//��������ȷ
            XmlElement objElement = (XmlElement)objXmlDoc.SelectSingleNode(_mainNode);
            try {
                for (int i = 0; i <= attributeNameArr.Length - 1; i++) {
                    if (objElement.Attributes[attributeNameArr[i]].IsNull()) {
                        objElement.SetAttribute(attributeNameArr[i], attributeValueArr[i]);
                    }
                }
            } catch { state = 4; }//��������
        }
        /// <summary>
        /// ����ָ��������ֵ,�����Ա�����ڡ�������������ֵ֧���á�|���ֿ����ַ���
        /// </summary>
        /// <param name="node">���</param>
        /// <param name="attributeName">������</param>
        /// <param name="attributeValue">����ֵ</param>
        public void SetAttr(string node, string attributeName, string attributeValue) {
            string mainNode = node.TrimEnd('/');
            string[] attributeNameArr = attributeName.Split('|'), attributeValueArr = attributeValue.Split('|');
            if (attributeValueArr.Length != attributeNameArr.Length) { state = 3; return; }//��������ȷ
            XmlNode objNode = objXmlDoc.SelectSingleNode(mainNode);
            for (int i = 0; i <= attributeNameArr.Length - 1; i++) {
                try {
                    objNode.Attributes[attributeNameArr[i]].Value = attributeValueArr[i];
                } catch { }
            }
        }
        //#endregion
        //#region ����XML�ļ�
        /// <summary>
        /// ����XML�ļ�
        /// </summary>
        /// <example>
        /// <code>
        ///     string strXmlFile = Server.MapPath("~/web.config");
        ///     Xml2 _xml = new Xml2(strXmlFile);
        ///     _xml.AddNode("configuration//appSettings","add", "key|value", "12|1111111");
        ///     _xml.AddNode("configuration//appSettings", "add", "key|value", "12|1111111", "cexo255");
        ///     Response.Write(_xml.getNodeText("configuration//appSettings//add[@key='12']"));
        ///     _xml.SetAttr("configuration//appSettings//add[@key='']", "value|providerName", "aaaaaaaaaaaa3|System.Data.SqlClient3");
        ///     _xml.AddAttr("configuration//appSettings//add[@key='']", "value|providerName","aaaaaaaaaaaa|System.Data.SqlClient");
        ///     Response.Write(_xml.getAttr("configuration//appSettings//add[@key='']", "value|providerName"));
        ///     _xml.Save();
        ///     switch (_xml.State) { 
        ///         case 0:
        ///             Js.Alert(this, "�����ɹ���");
        ///             break;
        ///         case 1:
        ///             Js.Alert(this, "�޷�����XML�ļ�");
        ///             break;
        ///         case 2:
        ///             Js.Alert(this, "����ʧ��");
        ///             break;
        ///         case 3:
        ///             Js.Alert(this, "������Ӧ����ȷ");
        ///             break;
        ///         case 4:
        ///             Js.Alert(this, "��������");
        ///             break;
        ///     }
        ///     string xmlText = _xml.ToXmlText();
        /// </code>
        /// </example>
        public void Save() {
            try { if (state == 0) objXmlDoc.Save(strXmlFile); } catch { state = 2; }//����ʧ��
            objXmlDoc = null;
        }
        /// <summary>
        /// �ر�XML����
        /// </summary>
        public void Close() {
            if (objXmlDoc.IsNotNull()) { objXmlDoc = null; }
        }
        /// <summary>
        /// תXML�ַ���
        /// </summary>
        /// <returns></returns>
        public string ToXmlText() {
            return objXmlDoc.OuterXml;
        }
        //#endregion
        //#region ȫ�ַ���Create
        /// <summary>
        /// �½�һ��XML�ļ�
        /// </summary>
        /// <example>
        /// <code>
        ///     Xml2.Create("c:\\rss\\rss.xml", "", "", "utf-8", "&lt;root>&lt;/root>")
        /// </code>
        /// </example>
        /// <param name="xmlFile">XML�ļ�·��</param>
        /// <param name="cssFile">CSS�ļ�·��</param>
        /// <param name="xlsFile">XLS�ļ�·��</param>
        /// <param name="encoding">����</param>
        /// <param name="node">�����</param>
        /// <returns>�Ƿ�����ɹ�</returns>
        public static bool Create(string xmlFile, string cssFile, string xlsFile, string encoding, string node) {
            if (node.Trim().Equals("")) return false;
            if (encoding.Trim().Equals("")) encoding = "utf-8";
            string _str = "<?xml version=\"1.0\" encoding=\"" + encoding + "\"?>";
            if (!cssFile.Trim().Equals("")) _str += Environment.NewLine + "<?xml-stylesheet type=\"text/css\" href=\"" + cssFile + "\"?>";
            if (!xlsFile.Trim().Equals("")) _str += Environment.NewLine + "<?xml-stylesheet type=\"text/xsl\" href=\"" + xlsFile + "\" media=\"screen\"?>";
            _str += Environment.NewLine + node;
            return FileDirectory.FileWrite(xmlFile, _str, encoding);
        }
        //#endregion
    }
}


