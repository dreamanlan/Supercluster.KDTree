// <copyright file="BinaryTreeNavigator.cs" company="Eric Regina">
// Copyright (c) Eric Regina. All rights reserved.
// </copyright>

namespace Supercluster.KDTree.Utilities
{
    using System;
    using System.Collections.Generic;

    using static BinaryTreeNavigation;

    /// <summary>
    /// Allows one to navigate a binary tree stored in an <see cref="Array"/> using familiar
    /// tree navigation concepts.
    /// </summary>
    /// <typeparam name="TPoint">The type of the individual points.</typeparam>
    /// <typeparam name="TNode">The type of the individual nodes.</typeparam>
    public class BinaryTreeNavigator<TPoint, TNode>
    {
        /// <summary>
        /// A reference to the pointArray in which the binary tree is stored in.
        /// </summary>
        private readonly List<TPoint> pointList;
        private readonly List<TNode> nodeList;
        private BinaryTreeNavigator<TPoint, TNode> left;
        private BinaryTreeNavigator<TPoint, TNode> right;
        private BinaryTreeNavigator<TPoint, TNode> parent;

        /// <summary>
        /// The index in the pointArray that the current node resides in.
        /// </summary>
        public int Index { get; private set; }

        /// <summary>
        /// The left child of the current node.
        /// </summary>
        public BinaryTreeNavigator<TPoint, TNode> Left
        {
            get {
                if(LeftChildIndex(this.Index) < this.pointList.Count - 1) {
                    if (null == left) {
                        left = new BinaryTreeNavigator<TPoint, TNode>(this.pointList, this.nodeList, LeftChildIndex(this.Index));
                        left.parent = this;
                    }
                    else {
                        left.Index = LeftChildIndex(this.Index);
                        left.parent.Index = this.Index;
                    }
                    return left;
                }
                else {
                    return null;
                }
            }
        }
        /// <summary>
        /// The right child of the current node.
        /// </summary>
        public BinaryTreeNavigator<TPoint, TNode> Right
        {
            get {
                if(RightChildIndex(this.Index) < this.pointList.Count - 1) {
                    if (null == right) {
                        right = new BinaryTreeNavigator<TPoint, TNode>(this.pointList, this.nodeList, RightChildIndex(this.Index));
                        right.parent = this;
                    }
                    else {
                        right.Index = RightChildIndex(this.Index);
                        right.parent.Index = this.Index;
                    }
                    return right;
                }
                else {
                    return null;
                }
            }
        }
        /// <summary>
        /// The parent of the current node.
        /// </summary>
        public BinaryTreeNavigator<TPoint, TNode> Parent
        {
            get {
                if(this.Index == 0) {
                    return null;
                }
                else {
                    if (null == parent)
                        parent = new BinaryTreeNavigator<TPoint, TNode>(this.pointList, this.nodeList, ParentIndex(this.Index));
                    else
                        parent.Index = ParentIndex(this.Index);
                    return parent;
                }
            }
        }

        /// <summary>
        /// The current <typeparamref name="TPoint"/>.
        /// </summary>
        public TPoint Point => this.pointList[this.Index];

        /// <summary>
        /// The current <typeparamref name="TNode"/>
        /// </summary>
        public TNode Node => this.nodeList[this.Index];

        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryTreeNavigator{TPoint, TNode}"/> class.
        /// </summary>
        /// <param name="pointArray">The point array backing the binary tree.</param>
        /// <param name="nodeArray">The node array corresponding to the point array.</param>
        /// <param name="index">The index of the node of interest in the pointArray. If not given, the node navigator start at the 0 index (the root of the tree).</param>
        public BinaryTreeNavigator(List<TPoint> pointArray, List<TNode> nodeArray, int index = 0)
        {
            this.Index = index;
            this.pointList = pointArray;
            this.nodeList = nodeArray;
        }
    }
}
