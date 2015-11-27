using System;
using System.Collections.Generic;
using System.Linq;

namespace Mincut
{
    public class Scc<T>
    {
        private Vertex<T> _leader;
        private int _timestamp;

        private void Dfs(Graph<T> graph, Vertex<T> i)
        {
            var stack = new Stack<Vertex<T>>();
            stack.Push(i);
            while (stack.Any())
            {
                var v = stack.Pop();
                v.IsExplored = true;
                v.Leader = _leader;
                foreach (var j in v.Vertices.Values)
                {
                    if (!j.IsExplored)
                        stack.Push(j);
                }

                if (v.Timestamp == 0)
                    v.Timestamp = _timestamp++;
            }
        }

        //private void Dfs(Graph<T> graph, Vertex<T> i)
        //{
        //    i.IsExplored = true;
        //    i.Leader = _leader;
        //    foreach (var j in i.Vertices.Values)
        //    {
        //        if (!j.IsExplored)
        //            Dfs(graph, j);

        //        if (i.Timestamp == 0)
        //            i.Timestamp = _timestamp++;
        //    }
        //}

        private void DfsLoop(Graph<T> graph)
        {
            foreach (var i in graph.Vertices.Values.OrderByDescending(i => i.Timestamp))
            {
                if (!i.IsExplored)
                {
                    _leader = i;
                    Dfs(graph, i);
                }
            }
        }

        public IEnumerable<int> Compute(Graph<T> graph)
        {
            var gRev = graph.Reverse();
            _timestamp = 0;
            DfsLoop(gRev);

            graph = gRev.Reverse();
            if (!graph.Vertices.Any(v => v.Value.Timestamp > 0))
                throw new Exception("Timestamp not computed.");

            _leader = null;
            DfsLoop(graph);

            var result = graph.Vertices.GroupBy(v => v.Value.Leader);
            return result.Select(g => g.Count()).OrderByDescending(i => i);
        }
    }
}
