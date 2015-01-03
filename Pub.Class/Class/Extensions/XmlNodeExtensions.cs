//------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2006 , LiveXY , Ltd. 
//------------------------------------------------------------

using System;
using System.Collections.Generic;
#if NET20
using Pub.Class.Linq;
#else
using System.Linq;
using System.Web.Script.Serialization;
using System.Xml.Linq;
#endif
using System.Text;
using System.Reflection;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Security.Cryptography;
using System.Xml;
using System.Xml.Serialization;

namespace Pub.Class {
    /// <summary>
    /// XDocument XmlNode
    /// 
    /// �޸ļ�¼
    ///     2009.06.25 �汾��1.0 livexy ��������
    /// 
    /// </summary>
    public static class XmlNodeExtensions {
#if !NET20
        /// <summary>
        /// XDocument ת����
        /// </summary>
        /// <typeparam name="T">Դ����</typeparam>
        /// <param name="xdoc">XDocument��չ</param>
        /// <returns></returns>
        public static T FromXDoc<T>(this XDocument xdoc) {
            XmlSerializer xs = new XmlSerializer(typeof(T));
            using (XmlReader r = xdoc.CreateReader()) return (T)xs.Deserialize(r);
        }
#endif
        /// <summary>
        /// ����ӽڵ�
        /// </summary>
        /// <param name="parentNode">XmlNode��չ</param>
        /// <param name="name">�ڵ���</param>
        /// <returns></returns>
        public static XmlNode CreateChildNode(this XmlNode parentNode, string name) {
            XmlDocument document = parentNode is XmlDocument ? (XmlDocument)parentNode : parentNode.OwnerDocument;
            XmlNode node = document.CreateElement(name);
            parentNode.AppendChild(node);
            return node;
        }
        /// <summary>
        /// ����ӽڵ�
        /// </summary>
        /// <param name="parentNode">XmlNode��չ</param>
        /// <param name="name">�ڵ���</param>
        /// <param name="namespaceUri"></param>
        /// <returns></returns>
        public static XmlNode CreateChildNode(this XmlNode parentNode, string name, string namespaceUri) {
            XmlDocument document = parentNode is XmlDocument ? (XmlDocument)parentNode : parentNode.OwnerDocument;
            XmlNode node = document.CreateElement(name, namespaceUri);
            parentNode.AppendChild(node);
            return node;
        }
        /// <summary>
        /// ���CData�ڵ�
        /// </summary>
        /// <param name="parentNode">XmlNode��չ</param>
        /// <returns></returns>
        public static XmlCDataSection CreateCDataSection(this XmlNode parentNode) {
            return parentNode.CreateCDataSection(string.Empty);
        }
        /// <summary>
        /// ���CData�ڵ�
        /// </summary>
        /// <param name="parentNode">XmlNode��չ</param>
        /// <param name="data">DATA</param>
        /// <returns></returns>
        public static XmlCDataSection CreateCDataSection(this XmlNode parentNode, string data) {
            XmlDocument document = parentNode is XmlDocument ? (XmlDocument)parentNode : parentNode.OwnerDocument;
            XmlCDataSection node = document.CreateCDataSection(data);
            parentNode.AppendChild(node);
            return node;
        }
        /// <summary>
        /// ȡCData�ڵ�
        /// </summary>
        /// <param name="parentNode">XmlNode��չ</param>
        /// <returns></returns>
        public static string GetCDataSection(this XmlNode parentNode) {
            foreach (var node in parentNode.ChildNodes) {
                if (node is XmlCDataSection) return ((XmlCDataSection)node).Value;
            }

            return null;
        }
        /// <summary>
        /// ȡ�ڵ�����
        /// </summary>
        /// <param name="node">XmlNode��չ</param>
        /// <param name="attributeName">������</param>
        /// <returns></returns>
        public static string GetAttribute(this XmlNode node, string attributeName) {
            return GetAttribute(node, attributeName, null);
        }
        /// <summary>
        /// ȡ�ڵ�����
        /// </summary>
        /// <param name="node">XmlNode��չ</param>
        /// <param name="attributeName">������</param>
        /// <param name="defaultValue">Ĭ��ֵ</param>
        /// <returns></returns>
        public static string GetAttribute(this XmlNode node, string attributeName, string defaultValue) {
            XmlAttribute attribute = node.Attributes[attributeName];
            return attribute.IsNotNull() ? attribute.InnerText : defaultValue;
        }
        /// <summary>
        /// ȡ�ڵ�����
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node">XmlNode��չ</param>
        /// <param name="attributeName">������</param>
        /// <returns></returns>
        public static T GetAttribute<T>(this XmlNode node, string attributeName) {
            return GetAttribute<T>(node, attributeName, default(T));
        }
        /// <summary>
        /// ȡ�ڵ�����
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node">XmlNode��չ</param>
        /// <param name="attributeName">������</param>
        /// <param name="defaultValue">Ĭ��ֵ</param>
        /// <returns></returns>
        public static T GetAttribute<T>(this XmlNode node, string attributeName, T defaultValue) {
            var value = GetAttribute(node, attributeName);

            return !value.IsNullEmpty() ? value.ConvertTo<T>(defaultValue) : defaultValue;
        }
        /// <summary>
        /// ���ýڵ�����ֵ
        /// </summary>
        /// <param name="node">XmlNode��չ</param>
        /// <param name="name">������</param>
        /// <param name="value">����ֵ</param>
        public static void SetAttribute(this XmlNode node, string name, object value) {
            SetAttribute(node, name, value.IsNotNull() ? value.ToString() : null);
        }
        /// <summary>
        /// ���ýڵ�����ֵ
        /// </summary>
        /// <param name="node">XmlNode��չ</param>
        /// <param name="name">������</param>
        /// <param name="value">����ֵ</param>
        public static void SetAttribute(this XmlNode node, string name, string value) {
            if (node.IsNotNull()) {
                var attribute = node.Attributes[name, node.NamespaceURI];

                if (attribute.IsNull()) {
                    attribute = node.OwnerDocument.CreateAttribute(name, node.OwnerDocument.NamespaceURI);
                    node.Attributes.Append(attribute);
                }

                attribute.InnerText = value;
            }
        }
    }
}
