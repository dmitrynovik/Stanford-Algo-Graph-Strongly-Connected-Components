using System;
using System.IO;
using System.Linq;
using System.Text;

namespace Mincut
{
    public class EdgeGraphParser
    {
        public static Graph<int> FromFile(string path)
        {
            var g = new Graph<int>();
            using (var stream = File.OpenRead(path))
            {
                using (var reader = new StreamReader(stream, Encoding.ASCII))
                {
                    var edges = reader.ReadToEnd().Split(new[] {"\n"}, StringSplitOptions.None);
                    foreach (var edge in edges)
                    {
                        var nodes = edge.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries).Select(i => new Vertex<int>(g, int.Parse(i))).ToArray();
                        var from = nodes[0];
                        var to = nodes[1];

                        if (!g.Vertices.ContainsKey(from.Key))
                        {
                            g.Vertices[from.Key] = from;
                        }
                        else
                        {
                            from = g.GetOrCreateVertex(from);
                        }

                        if (!g.Vertices.ContainsKey(to.Key))
                        {
                            g.Vertices[to.Key] = to;
                        }
                        else
                        {
                            to = g.GetOrCreateVertex(to);
                        }

                        from.Add(to);
                    }
                }
            }
            return g;
        }
    }
}
