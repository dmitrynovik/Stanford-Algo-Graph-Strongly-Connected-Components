using System.Collections.Generic;

namespace Mincut
{
    public class Vertex<T>
    {
        private readonly Graph<T> _g;

        public T Key { get; set; }

        public IDictionary<T, Vertex<T>> Vertices { get; private set; }

        public Vertex(Graph<T> g)
        {
            Vertices = new Dictionary<T, Vertex<T>>();
            _g = g;
            //if (_g != null)
            //    _g.Add(this);
        }

        public Vertex(Graph<T> g, T key, int timestamp = 0) : this(g)
        {
            Key = key;
            Timestamp = timestamp;
        }

        public Vertex<T> Get(T key)
        {
            return Vertices.ContainsKey(key) ? Vertices[key] : null;
        }

        public bool IsExplored { get; set; }
        public int  Timestamp { get; set; }
        public Vertex<T> Leader { get; set; }

        public void Add(Vertex<T> v)
        {
            Vertices[v.Key] = v;
        }

        public override int GetHashCode()
        {
            return Key == null ? 0 : Key.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var other = obj as Vertex<T>;
            return other != null && other.Key.Equals(Key);
        }

        public override string ToString()
        {
            return Key.ToString();
        }
    }
}
