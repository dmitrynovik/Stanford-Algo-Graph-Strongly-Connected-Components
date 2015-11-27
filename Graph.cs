using System.Collections.Generic;
using System.Linq;

namespace Mincut
{
    public class Graph<T>
    {
        public IDictionary<T, Vertex<T>> Vertices { get; private set; }

        public Graph()
        {
            Vertices = new Dictionary<T, Vertex<T>>();
        }

        public Graph(Graph<T> other) : this()
        {
            foreach (var v in other.Vertices.Values)
            {
                var v1 = GetOrCreateVertex(v.Key);
                foreach (var vAdj in v.Vertices)
                {
                    var v2 = GetOrCreateVertex(vAdj.Key);
                    v1.Add(v2);
                }
                Vertices[v1.Key] = v1;
            }
        }

        public Vertex<T> GetOrCreateVertex(T label, int timestamp = 0)
        {
            return GetOrCreateVertex(new Vertex<T>(this, label, timestamp));
        }

        public Vertex<T> GetOrCreateVertex(Vertex<T> v)
        {
            if (Vertices.ContainsKey(v.Key))
            {
                return Vertices[v.Key];
            }

            var v1 = new Vertex<T>(this, v.Key) {Timestamp = v.Timestamp};
            Vertices[v1.Key] = v1;
            return v1;
        }

        public void ContractEdge(Vertex<T> v1, Vertex<T> v2)
        {
            //if (Equals(v1, v2))
            //    throw new ArgumentException("an edge between same vertex");

            Vertices.Remove(v2.Key);
            v2.Vertices.ToList().ForEach(kvp =>
            {
                var v = kvp.Value;
                if (!Equals(v, v1))
                {
                    v.Vertices.Remove(v2.Key);
                    v.Add(v1);
                    v1.Add(v);
                }
            });
        }

        public Graph<T> Reverse()
        {
            var g = new Graph<T>();
            foreach (var v in Vertices.Values)
            {
                foreach (var vlink in v.Vertices.Values)
                {
                    var from = g.GetOrCreateVertex(vlink.Key, vlink.Timestamp);
                    var to = g.GetOrCreateVertex(v.Key, v.Timestamp);
                    from.Add(to);
                }
            }
            return g;
        }
    }
}
