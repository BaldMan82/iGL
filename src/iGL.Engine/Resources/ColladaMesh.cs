using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iGL.Engine.Import;
using iGL.Engine.Math;

namespace iGL.Engine.Resources
{
    public class ColladaMesh : Resource
    {
        public Vector3[] Vertices { get; private set; }
        public Vector3[] Normals { get; private set; }
        public short[] Indices { get; private set; }

        protected override bool InternalLoad()
        {
            var resourceAsm = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(asm => asm.GetManifestResourceNames().Contains(base.ResourceName));
            {
                if (resourceAsm == null) throw new Exception(base.ResourceName + " not found.");

                using (var stream = resourceAsm.GetManifestResourceStream(base.ResourceName))
                {
                    COLLADA collada = COLLADA.Load(stream);

                    foreach (var item in collada.Items)
                    {
                        var geoms = item as library_geometries;
                        if (geoms == null) continue;

                        foreach (var geom in geoms.geometry)
                        {
                            var mesh = geom.Item as mesh;
                            if (mesh == null) continue;

                            Vector3[] importedNormals = null;

                            foreach (var source in mesh.source)
                            {
                                var float_array = source.Item as float_array;
                                if (float_array == null)
                                    continue;

                                /* vertices */

                                if (float_array.id.Contains("positions"))
                                {
                                    if (float_array.count % 3 != 0) throw new NotSupportedException("Need 3 doubles per vertex");

                                    Vertices = new Vector3[float_array.count / 3];

                                    for (ulong i = 0; i < float_array.count; i += 3)
                                    {
                                        Vertices[i / 3] = new Vector3((float)float_array.Values[i],
                                                                  (float)float_array.Values[i + 1],
                                                                  (float)float_array.Values[i + 2]);
                                    }
                                }
                                else if (float_array.id.Contains("normals"))
                                {
                                    if (float_array.count % 3 != 0) throw new NotSupportedException("Need 3 doubles per normal");

                                    importedNormals = new Vector3[float_array.count / 3];

                                    for (ulong i = 0; i < float_array.count; i += 3)
                                    {
                                        importedNormals[i / 3] = new Vector3((float)float_array.Values[i],
                                                                  (float)float_array.Values[i + 1],
                                                                  (float)float_array.Values[i + 2]);
                                    }
                                }
                            }

                            foreach (var meshItem in mesh.Items)
                            {
                                if (meshItem is vertices)
                                {
                                    var vertices = meshItem as vertices;
                                }
                                else if (meshItem is triangles)
                                {
                                    var triangles = meshItem as triangles;
                                }
                                else if (meshItem is polylist)
                                {
                                    var polylist = meshItem as polylist;
                                    Indices = new short[polylist.count * 3];
                                    var normalFaces = new short[polylist.count * 3];

                                    var numbers = polylist.p.Split(' ');

                                    for (ulong i = 0; i < polylist.count; i++)
                                    {
                                        ulong vertexIndex = i * 6;

                                        Indices[i * 3] = short.Parse(numbers[vertexIndex]);
                                        Indices[(i * 3) + 1] = short.Parse(numbers[vertexIndex + 2]);
                                        Indices[(i * 3) + 2] = short.Parse(numbers[vertexIndex + 4]);

                                        normalFaces[i * 3] = short.Parse(numbers[vertexIndex + 1]);
                                        normalFaces[(i * 3) + 1] = short.Parse(numbers[vertexIndex + 3]);
                                        normalFaces[(i * 3) + 2] = short.Parse(numbers[vertexIndex + 5]);
                                    }

                                    if (importedNormals != null)
                                    {
                                        /* calc normals from input to match vertex count */
                                        Normals = new Vector3[Vertices.Length];

                                        for (ulong i = 0; i < polylist.count*3; i++)
                                        {
                                            Normals[Indices[i]] = importedNormals[normalFaces[i]];
                                        }

                                        for (ulong i = 0; i < polylist.count * 3; i++)
                                        {
                                            Normals[Indices[i]].Normalize();
                                        }
                                    }

                                }
                            }
                        }

                    }
                }
            }
            return true;
        }
    }
}
