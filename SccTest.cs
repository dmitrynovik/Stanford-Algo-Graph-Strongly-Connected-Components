using System;
using System.Linq;
using System.Diagnostics;
using NUnit.Framework;

namespace Mincut
{
    [TestFixture]
    public class SccTest
    {
        private Graph<int> _graph;

        public Graph<int> Graph
        {
            get
            {
                if (_graph == null)
                {
                    var watch = Stopwatch.StartNew();
                    _graph = EdgeGraphParser.FromFile("SCC.txt");
                    watch.Stop();
                    Console.WriteLine("TestParser, elapsed = {0}, vertices: {1}", watch.Elapsed, Graph.Vertices.Count);                                
                }
                return _graph;
            }
        }

        [Test]
        public void TestParser()
        {
            var vtx = Graph.Vertices.FirstOrDefault(v => v.Key == 24007).Value.Vertices.OrderBy(v => v.Key).ToArray();
            Assert.IsTrue(vtx.Any(v => v.Key == 23999));
            Assert.IsTrue(vtx.Any(v => v.Key == 24000));
            Assert.IsTrue(vtx.Any(v => v.Key == 24001));
        }

        [Test]
        public void TestReverse()
        {
            var nodes = Graph.Vertices.Count;
            var edges = Graph.Vertices.Sum(v => v.Value.Vertices.Count);

            var g = Graph.Reverse();
            Assert.AreEqual(nodes, g.Vertices.Count);
            Assert.AreEqual(edges, g.Vertices.Sum(v => v.Value.Vertices.Count));

            var vtx = g.Vertices.FirstOrDefault(v => v.Key == 23999).Value.Vertices.OrderBy(v => v.Key).ToArray();
            Assert.IsTrue(vtx.Any(v => v.Key == 24007));
        }

        [Test]
        public void ComputeScc()
        {
            var scc = new Scc<int>();
            foreach (var i in scc.Compute(Graph).Take(5))
            {
                Console.WriteLine(i);
            }
        }

        [Test]
        public void ComputeSmall()
        {
            var g = new Graph<int>();

            var v1 = g.GetOrCreateVertex(1);
            var v2 = g.GetOrCreateVertex(2);
            var v3 = g.GetOrCreateVertex(3);
            var v4 = g.GetOrCreateVertex(4);
            var v5 = g.GetOrCreateVertex(5);

            v1.Add(v2);
            v2.Add(v3);
            v3.Add(v1);
            v4.Add(v5);
            v5.Add(v4);

            var scc = new Scc<int>();
            var a = scc.Compute(g).ToArray();
            Assert.AreEqual(2, a.Length);
            Assert.AreEqual(3, a[0]);
            Assert.AreEqual(2, a[1]);
        }
    }
}
