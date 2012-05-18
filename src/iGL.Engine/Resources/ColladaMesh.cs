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
        public Vector2[] UVs { get; private set; }
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
                            Vector2[] importedUVs = null;

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
                                                                  (float)float_array.Values[i + 2],
                                                                  -(float)float_array.Values[i + 1]);
                                    }
                                }
                                else if (float_array.id.Contains("normals"))
                                {
                                    if (float_array.count % 3 != 0) throw new NotSupportedException("Need 3 doubles per normal");

                                    importedNormals = new Vector3[float_array.count / 3];

                                    for (ulong i = 0; i < float_array.count; i += 3)
                                    {
                                        importedNormals[i / 3] = new Vector3((float)float_array.Values[i],
                                                                  (float)float_array.Values[i + 2],
                                                                  -(float)float_array.Values[i + 1]);
                                    }
                                }
                                else if (float_array.id.Contains("map"))
                                {
                                    if (float_array.count % 2 != 0) throw new NotSupportedException("Need 2 doubles per uv");

                                    importedUVs = new Vector2[float_array.count / 2];


                                    for (ulong i = 0; i < float_array.count; i += 2)
                                    {
                                        importedUVs[i / 2] = new Vector2((float)float_array.Values[i],
                                                                  1.0f - (float)float_array.Values[i + 1]);
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
                                    var uvIndices = new short[polylist.count * 3];
                                    var numbers = polylist.p.Split(' ');

                                    if (polylist.vcount.Contains("4"))
                                    {
                                        throw new NotSupportedException("Expecting 3 parts per face, not 4");
                                    }                                

                                    for (int i = 0; i < numbers.Length; i += 3)
                                    {
                                        int vertexIndex = i;

                                        Indices[i/3] = short.Parse(numbers[vertexIndex++]);
                                        normalFaces[i/3] = short.Parse(numbers[vertexIndex++]);
                                        uvIndices[i/3] = short.Parse(numbers[vertexIndex++]);                                       
                                    }
                                  

                                    //if (importedNormals != null)
                                    //{
                                    //    /* calc normals from input to match vertex count */
                                    //    Normals = new Vector3[Vertices.Length];
                                    //    UVs = new Vector2[Vertices.Length];

                                    //    for (ulong i = 0; i < polylist.count * 3; i++)
                                    //    {
                                    //        Normals[Indices[i]] = importedNormals[normalFaces[i]];
                                    //        UVs[Indices[i]] = importedUVs[uvIndices[i]];
                                    //    }
                                        
                                    //}

                                    /* each uv coordinate defines a new point/pixel (vertex)
                                     * so create vertices per uv point, instead of following the vertex indices
                                     * opengl cannot have separate uv index buffers ...
                                     */

                                    var vertices = new Vector3[importedUVs.Length];
                                    var normals = new Vector3[importedUVs.Length];
                                    var uvs = new Vector3[importedUVs.Length];

                                    for (ulong i = 0; i < polylist.count * 3; i++)
                                    {
                                        vertices[uvIndices[i]] = Vertices[Indices[i]];
                                        normals[uvIndices[i]] = importedNormals[normalFaces[i]];
                                    }

                                    Vertices = vertices;
                                    Normals = normals;
                                    UVs = importedUVs;
                                    Indices = uvIndices;

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
