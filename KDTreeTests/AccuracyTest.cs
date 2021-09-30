using KDTreeTests;
using System.Collections.Generic;

using NUnit.Framework;

namespace KDTreeTests
{
    using System.Linq;

    using NUnit.Framework;

    using Supercluster.KDTree;
    using Supercluster.KDTree.Utilities;

    using static Supercluster.KDTree.Utilities.BinaryTreeNavigation;

    [TestFixture]
    public class AccuracyTest
    {

        /// <summary>
        /// Should build the tree displayed in the article:
        /// https://en.wikipedia.org/wiki/K-d_tree
        /// </summary>
        [Test]
        public void WikipediaBuildTests()
        {
            // Should generate the following tree:
            //             7,2
            //              |
            //       +------+-----+
            //      5,4          9,6
            //       |            |
            //   +---+---+     +--+
            //  2,3     4,7   8,1 


            var points = new List<double[]>
                             {
                                 new double[] { 7, 2 }, new double[] { 5, 4 }, new double[] { 2, 3 },
                                 new double[] { 4, 7 }, new double[] { 9, 6 }, new double[] { 8, 1 }
                             };

            var nodes = new List<string> { "Eric", "Is", "A", "Really", "Stubborn", "Ferret" };
            var tree = new KDTree<double, string>(
                2,
                Utilities.L2Norm_Squared_Double,
                double.MinValue,
                double.MaxValue);
            tree.Build(points, nodes);

            Assert.That(tree.InternalPointList[0], Is.EqualTo(points[0]));
            Assert.That(tree.InternalPointList[LeftChildIndex(0)], Is.EqualTo(points[1]));
            Assert.That(tree.InternalPointList[LeftChildIndex(LeftChildIndex(0))], Is.EqualTo(points[2]));
            Assert.That(tree.InternalPointList[RightChildIndex(LeftChildIndex(0))], Is.EqualTo(points[3]));
            Assert.That(tree.InternalPointList[RightChildIndex(0)], Is.EqualTo(points[4]));
            Assert.That(tree.InternalPointList[LeftChildIndex(RightChildIndex(0))], Is.EqualTo(points[5]));
        }



        /// <summary>
        /// Should build the tree displayed in the article:
        /// https://en.wikipedia.org/wiki/K-d_tree
        /// </summary>
        [Test]
        public void NodeNavigatorTests()
        {
            // Should generate the following tree:
            //             7,2
            //              |
            //       +------+-----+
            //      5,4          9,6
            //       |            |
            //   +---+---+     +--+
            //  2,3     4,7   8,1 


            var points = new List<double[]>
                             {
                                 new double[] { 7, 2 }, new double[] { 5, 4 }, new double[] { 2, 3 },
                                 new double[] { 4, 7 }, new double[] { 9, 6 }, new double[] { 8, 1 }
                             };

            var nodes = new List<string> { "Eric", "Is", "A", "Really", "Stubborn", "Ferret" };

            var tree = new KDTree<double, string>(2, Utilities.L2Norm_Squared_Double);
            tree.Build(points, nodes);

            var nav = tree.Navigator;

            Assert.That(nav.Point, Is.EqualTo(points[0]));
            Assert.That(nav.Left.Point, Is.EqualTo(points[1]));
            Assert.That(nav.Left.Left.Point, Is.EqualTo(points[2]));
            Assert.That(nav.Left.Right.Point, Is.EqualTo(points[3]));
            Assert.That(nav.Right.Point, Is.EqualTo(points[4]));
            Assert.That(nav.Right.Left.Point, Is.EqualTo(points[5]));



            Assert.That(nav.Node, Is.EqualTo(nodes[0]));
            Assert.That(nav.Left.Node, Is.EqualTo(nodes[1]));
            Assert.That(nav.Left.Left.Node, Is.EqualTo(nodes[2]));
            Assert.That(nav.Left.Right.Node, Is.EqualTo(nodes[3]));
            Assert.That(nav.Right.Node, Is.EqualTo(nodes[4]));
            Assert.That(nav.Right.Left.Node, Is.EqualTo(nodes[5]));
        }




        [Test]
        public void FindNearestNeighborTest()
        {
            var dataSize = 10000;
            var testDataSize = 100;
            var range = 1000;

            var treePoints = Utilities.GenerateDoubles(dataSize, range);
            var treeNodes = new List<string>(Utilities.GenerateDoubles(dataSize, range).Select(d => d.ToString()));
            var testData = Utilities.GenerateDoubles(testDataSize, range);


            var tree = new KDTree<double, string>(2, Utilities.L2Norm_Squared_Double);
            tree.Build(treePoints, treeNodes);

            for (int i = 0; i < testDataSize; i++)
            {
                List<KDTree<double, string>.TreeNodeInfo> treeNearest = new List<KDTree<double, string>.TreeNodeInfo>();
                bool ret = tree.NearestNeighbors(testData[i], 1, (coords, str)=> { return new KDTree<double, string>.TreeNodeInfo { Coordinates = coords, TagInfo = str }; }, treeNearest);
                var linearNearest = Utilities.LinearSearch(treePoints, treeNodes, testData[i], Utilities.L2Norm_Squared_Double);

                Assert.That(Utilities.L2Norm_Squared_Double(testData[i], linearNearest.Coordinates), Is.EqualTo(Utilities.L2Norm_Squared_Double(testData[i], treeNearest[0].Coordinates)));

                // TODO: wrote linear search for both node and point arrays
                Assert.That(treeNearest[0].TagInfo, Is.EqualTo(linearNearest.TagInfo));
            }
        }

        [Test]
        public void RadialSearchTest()
        {
            var dataSize = 10000;
            var testDataSize = 100;
            var range = 1000;
            var radius = 100;

            var treeData = Utilities.GenerateDoubles(dataSize, range);
            var treeNodes = new List<string>(Utilities.GenerateDoubles(dataSize, range).Select(d => d.ToString()));
            var testData = Utilities.GenerateDoubles(testDataSize, range);
            var tree = new KDTree<double, string>(2, Utilities.L2Norm_Squared_Double);
            tree.Build(treeData, treeNodes);

            for (int i = 0; i < testDataSize; i++)
            {
                List<KDTree<double, string>.TreeNodeInfo> treeRadial = new List<KDTree<double, string>.TreeNodeInfo>();
                bool ret = tree.RadialSearch(testData[i], radius, -1, (coords, str) => new KDTree<double, string>.TreeNodeInfo { Coordinates = coords, TagInfo = str }, treeRadial);
                var linearRadial = new List<KDTree<double, string>.TreeNodeInfo>(Utilities.LinearRadialSearch(
                    treeData,
                    treeNodes,
                    testData[i],
                    Utilities.L2Norm_Squared_Double,
                    radius));

                for (int j = 0; j < treeRadial.Count; j++)
                {
                    Assert.That(treeRadial[j].Coordinates, Is.EqualTo(linearRadial[j].Coordinates));
                    Assert.That(treeRadial[j].TagInfo, Is.EqualTo(linearRadial[j].TagInfo));
                }
            }
        }
    }
}
