using System;
using System.IO;
using System.Linq;

namespace Mincut
{
    public static class GraphBuilder
    {
        public static Graph<string> FromFile(string path)
        {
            var g = new Graph<string>();
            using (var stream = File.OpenRead(path))
            {
                using (var reader = new StreamReader(stream))
                {
                    string line;
                    while ((line = reader.ReadLine() ) != null)
                    {
                        var vertices = line.Split(new[] {' ', '\t'}, StringSplitOptions.RemoveEmptyEntries);
                        if (vertices.Any())
                        {
                            var v = g.GetOrCreateVertex(vertices[0]);
                            foreach (var adj in vertices.Skip(1))
                            {
                                v.Add(g.GetOrCreateVertex(adj));
                            }
                        }
                    }
                }
            }
            return g;
        }
    }
}
