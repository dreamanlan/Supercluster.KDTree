using System;
using System.Collections.Generic;


namespace KDTreeTests
{
    using System.Linq;

    using Supercluster.KDTree;

    public static class Utilities
    {
        #region Metrics
        public static Func<float[], float[], double> L2Norm_Squared_Float = (x, y) =>
        {
            float dist = 0f;
            for (int i = 0; i < x.Length; i++)
            {
                dist += (x[i] - y[i]) * (x[i] - y[i]);
            }

            return dist;
        };

        public static Func<double[], double[], double> L2Norm_Squared_Double = (x, y) =>
        {
            double dist = 0f;
            for (int i = 0; i < x.Length; i++)
            {
                dist += (x[i] - y[i]) * (x[i] - y[i]);
            }

            return dist;
        };
        #endregion

        #region Data Generation

        public static double[][] GenerateDoubles(int points, double range, int dimensions)
        {
            var data = new List<double[]>();
            var random = new Random();

            for (var i = 0; i < points; i++)
            {
                var array = new double[dimensions];
                for (var j = 0; j < dimensions; j++)
                {
                    array[j] = random.NextDouble() * range;
                }
                data.Add(array);
            }

            return data.ToArray();
        }

        public static List<double[]> GenerateDoubles(int points, double range)
        {
            var data = new List<double[]>();
            var random = new Random();

            for (int i = 0; i < points; i++)
            {
                data.Add(new double[] { (random.NextDouble() * range), (random.NextDouble() * range) });
            }

            return data;
        }

        public static List<float[]> GenerateFloats(int points, double range)
        {
            var data = new List<float[]>();
            var random = new Random();

            for (int i = 0; i < points; i++)
            {
                data.Add(new float[] { (float)(random.NextDouble() * range), (float)(random.NextDouble() * range) });
            }

            return data;
        }

        public static List<float[]> GenerateFloats(int points, double range, int dimensions)
        {
            var data = new List<float[]>();
            var random = new Random();

            for (var i = 0; i < points; i++)
            {
                var array = new float[dimensions];
                for (var j = 0; j < dimensions; j++)
                {
                    array[j] = (float)(random.NextDouble() * range);
                }
                data.Add(array);
            }

            return data;
        }
        #endregion


        #region Searches

        /// <summary>
        /// Performs a linear search on a given points set to find a nodes that is closest to the given nodes
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="point"></param>
        /// <param name="metric"></param>
        /// <returns></returns>
        public static T[] LinearSearch<T>(List<T[]> data, List<T> point, Func<List<T>, T[], float> metric)
        {
            var bestDist = Double.PositiveInfinity;
            T[] bestPoint = null;

            for (int i = 0; i < data.Count; i++)
            {
                var currentDist = metric(point, data[i]);
                if (bestDist > currentDist)
                {
                    bestDist = currentDist;
                    bestPoint = data[i];
                }
            }

            return bestPoint;
        }

        public static T[] LinearSearch<T>(List<T[]> data, List<T> point, Func<List<T>, T[], double> metric)
        {
            var bestDist = Double.PositiveInfinity;
            T[] bestPoint = null;

            for (int i = 0; i < data.Count; i++)
            {
                var currentDist = metric(point, data[i]);
                if (bestDist > currentDist)
                {
                    bestDist = currentDist;
                    bestPoint = data[i];
                }
            }

            return bestPoint;
        }

        public static KDTree<TPoint, TNode>.TreeNodeInfo LinearSearch<TPoint, TNode>(List<TPoint[]> points, List<TNode> nodes, TPoint[] target, Func<TPoint[], TPoint[], double> metric) where TPoint : IComparable<TPoint>
        {
            var bestIndex = 0;
            var bestDist = Double.MaxValue;

            for (int i = 0; i < points.Count; i++)
            {
                var currentDist = metric(points[i], target);
                if (bestDist > currentDist)
                {
                    bestDist = currentDist;
                    bestIndex = i;
                }
            }

            return new KDTree<TPoint, TNode>.TreeNodeInfo { Coordinates = points[bestIndex], Node = nodes[bestIndex] };
        }


        public static IEnumerable<T[]> LinearRadialSearch<T>(List<T[]> data, List<T> point, Func<List<T>, T[], double> metric, double radius)
        {
            var pointsInRadius = new BoundedPriorityList<T[], double>(data.Count, true);

            for (int i = 0; i < data.Count; i++)
            {
                var currentDist = metric(point, data[i]);
                if (radius >= currentDist)
                {
                    pointsInRadius.Add(data[i], currentDist);
                }
            }

            return pointsInRadius;
        }


        public static IEnumerable<KDTree<TPoint, TNode>.TreeNodeInfo> LinearRadialSearch<TPoint, TNode>(List<TPoint[]> points, List<TNode> nodes, TPoint[] target, Func<TPoint[], TPoint[], double> metric, double radius) where TPoint : IComparable<TPoint>
        {
            var pointsInRadius = new BoundedPriorityList<int, double>(points.Count, true);

            for (int i = 0; i < points.Count; i++)
            {
                var currentDist = metric(target, points[i]);
                if (radius >= currentDist)
                {
                    pointsInRadius.Add(i, currentDist);
                }
            }

            return pointsInRadius.Select(idx => new KDTree<TPoint, TNode>.TreeNodeInfo { Coordinates = points[idx], Node = nodes[idx] });
        }

        #endregion
    }
}
