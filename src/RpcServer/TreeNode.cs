// Copyright (C) 2015-2023 The Neo Project.
//
// The Neo.Network.RPC is free software distributed under the MIT software license,
// see the accompanying file LICENSE in the main directory of the
// project or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

using System.Collections.Generic;

namespace Neo.Plugins
{
    class TreeNode<T>
    {
        private readonly List<TreeNode<T>> _children = new();

        public T Item { get; }
        public TreeNode<T> Parent { get; }
        public IReadOnlyList<TreeNode<T>> Children => _children;

        internal TreeNode(T item, TreeNode<T> parent)
        {
            Item = item;
            Parent = parent;
        }

        public TreeNode<T> AddChild(T item)
        {
            TreeNode<T> child = new(item, this);
            _children.Add(child);
            return child;
        }

        internal IEnumerable<T> GetItems()
        {
            yield return Item;
            foreach (var child in _children)
                foreach (T item in child.GetItems())
                    yield return item;
        }
    }
}
