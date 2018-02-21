using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Diagnostics;

namespace MyTools
{
    class Config
    {
        public static void LoadConfig(string file, ContextMenu contextMenu)
        {
            if ((contextMenu == null))
                throw new ArgumentNullException(nameof(contextMenu));
            try
            {
                XmlDocument xmldoc = new XmlDocument();
                XmlNode xmlnode;
                FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read);
                if (fs == null)
                {
                    MessageBox.Show("Es null");
                }
                xmldoc.Load(fs);
                xmlnode = xmldoc.ChildNodes[1];
                contextMenu.Items.Add(new MenuItem() { Name = xmldoc.DocumentElement.Name, Header = xmldoc.DocumentElement.Name });
                MenuItem tNode = contextMenu.Items[0] as MenuItem;
                AddNode(xmlnode, tNode);
            }
            catch (XmlException xmlEx)
            {
                MessageBox.Show(xmlEx.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            { }
        }

        private static void AddNode(XmlNode inXmlNode, MenuItem inTreeNode)
        {
            XmlNode node = null;
            MenuItem nodeTree = null;
            XmlNodeList nodeList = null;
            int i;

            // Loop through the XML nodes until the leaf is reached.
            // Add the nodes to the TreeView during the looping process.
            if (inXmlNode.HasChildNodes)
            {
                nodeList = inXmlNode.ChildNodes;
                for (i = 0; i <= nodeList.Count - 1; i++)
                {
                    node = inXmlNode.ChildNodes[i];
                    if (node.Name.Equals("Category"))
                    {
                        inTreeNode.Items.Add(new MenuItem() { /*name = xnode.name,*/ Header = node.Attributes["Name"].Value ?? "none"});
                        nodeTree = inTreeNode.Items[i] as MenuItem;
                    }
                    else if (node.Name.Equals("Software"))
                    {
                        var subMenuItem = new MenuItem() { /*name = xnode.name,*/ Header = node.Attributes["Name"].Value ?? "none" };
                        inTreeNode.Items.Add(subMenuItem);
                        nodeTree = inTreeNode;

                        foreach (XmlNode subNode in node.ChildNodes)
                        {
                            if (subNode.Name.Equals("PathExe"))
                            {
                                SetPathExe(subMenuItem, subNode.InnerText);
                            }
                        }
                    }
                    AddNode(node, nodeTree);
                }
            }
            else
            {
                // Here you need to pull the data from the XmlNode based on the
                // type of node, whether attribute values are required, and so forth.
                //inTreeNode.Header = (inXmlNode.OuterXml).Trim();

                Console.WriteLine((inXmlNode.OuterXml).Trim());
            }
        }

        private static void SetPathExe(MenuItem menuItem, String filePath)
        {
            // Extract the file ico
            var sysicon = System.Drawing.Icon.ExtractAssociatedIcon(filePath);
            var bmpSrc = System.Windows.Interop.Imaging.CreateBitmapSourceFromHIcon(
                        sysicon.Handle,
                        System.Windows.Int32Rect.Empty,
                        System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());
            sysicon.Dispose();

            menuItem.Icon = new Image { Source = bmpSrc};
            menuItem.Click += delegate { Process.Start(filePath); };

        }

    }
}
