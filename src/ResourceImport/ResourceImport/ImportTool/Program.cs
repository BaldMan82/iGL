using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Linq;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace ImportTool
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 3)
            {
                Console.WriteLine("Invalid args");
                return;

            }

            try
            {
                string projectFile = args[0];
                string sourceFolder = args[1];
                string destinationFolder = args[2];

                var files = Directory.GetFiles(sourceFolder);

                FileStream stream = new FileStream(projectFile, FileMode.Open);

                XDocument doc = XDocument.Load(stream);
                var groups = doc.Root.Elements().Where(e => e.Name.LocalName == "ItemGroup" && e.Elements().Any(c => c.Name.LocalName == "EmbeddedResource")).ToList();

              
                var newGroups = new List<XElement>();

                foreach (var file in files)
                {                  
                    string fileName = file.Split('\\').Last();
                    string sourceFile = string.Format("{0}\\{1}", sourceFolder, fileName);
                    string destFile = string.Format("{0}\\{1}", destinationFolder, fileName);

                    File.Delete(destFile);

                    string ext = file.Split('.').Last();
                    if (ext.ToLower() == "bmp" || ext.ToLower() == "png" || ext.ToLower() == "jpg" || ext.ToLower() == "jpeg")
                    {
                        destFile = destFile.Replace(ext, "text");
                        ConvertTexture(sourceFile, destFile);
                    }
                    else
                    {
                        File.Copy(sourceFile, destFile);
                    }

                    Console.WriteLine("Adding: " + destFile);

                    //var existingGroup = groups[0].Elements().FirstOrDefault();
                    //if (existingGroup != null && existingGroup.Attribute("Include").Value == destFile)
                    //{
                    //    /* already exists */
                    //}

                    var group = new XElement(XName.Get("ItemGroup", "http://schemas.microsoft.com/developer/msbuild/2003"));
                    newGroups.Add(group);

                    var child = new XElement(XName.Get("EmbeddedResource", "http://schemas.microsoft.com/developer/msbuild/2003"));
                    child.SetAttributeValue(XName.Get("Include"), destFile);

                    group.Add(child);
                }

                stream.Close();

                if (newGroups.Count == groups.Count && groups.All(g => newGroups.Exists(ng => ng.Elements().First().Attribute("Include").Value == g.Elements().First().Attribute("Include").Value)))
                {
                    /* extactly same project resources */
                    return;
                }
                else
                {
                    groups.ForEach(g => g.Remove());
                    newGroups.ForEach(g => doc.Root.Add(g));

                    File.Delete(projectFile + ".backup");
                    File.Copy(projectFile, projectFile + ".backup");

                    FileStream save = new FileStream(projectFile, FileMode.Create);

                    doc.Save(save);
                }                
                
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }

        private static void ConvertTexture(string fileName, string destinationFileName)
        {
            using (Bitmap bmp = new Bitmap(fileName))
            {
                BitmapData data = bmp.LockBits(new System.Drawing.Rectangle(0, 0, bmp.Width, bmp.Height),
                                    ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppRgb);


                using (var stream = new FileStream(destinationFileName, FileMode.Create))
                {
                    unsafe
                    {
                        int[] size = new int[2];
                        size[0] = bmp.Width;
                        size[1] = bmp.Height;

                        var bytes = new byte[4 * bmp.Width * bmp.Height];

                        stream.Write(BitConverter.GetBytes(size[0]), 0, 4);
                        stream.Write(BitConverter.GetBytes(size[1]), 0, 4);

                        Marshal.Copy(data.Scan0, bytes, 0, bytes.Length);

                        for (int i = 0; i < bytes.Length; i += 4)
                        {
                            var r = bytes[i + 2];
                            var g = bytes[i + 1];
                            var b = bytes[i];

                            bytes[i] = r;
                            bytes[i + 1] = g;
                            bytes[i + 2] = b;
                        }

                        stream.Write(bytes, 0, bytes.Length);
                    }
                }
            }
        }
    }
}
