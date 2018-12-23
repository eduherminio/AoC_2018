using System;
using System.Collections.Generic;
using System.Text;

namespace AoC_2018.Model
{
    public class Node<TKey>
    {
        public TKey ParentId { get; set; }
        public TKey Id { get; set; }

        public HashSet<Node<TKey>> Children { get; set; }

        public Node(TKey id)
        {
            Id = id;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (!(obj is Node<TKey>))
            {
                return false;
            }

            return Equals((Node<TKey>)obj);
        }

        public bool Equals(Node<TKey> other)
        {
            if (other == null)
            {
                return false;
            }

            if (Id is string _id && other is Node<string> _other)
            {
                return _id == _other.Id;
            }
            else if (Id is int _id2 && other is Node<int> _other2)
            {
                return _id2 == _other2.Id;
            }
            else
            {
                throw new Exception("Wrong TKey in Node<TKey>");
            }
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public static bool operator ==(Node<TKey> node1, Node<TKey> node2)
        {
            if (ReferenceEquals(node1, null))
            {
                return ReferenceEquals(node2, null);
            }

            return node1.Equals(node2);
        }

        public static bool operator !=(Node<TKey> node1, Node<TKey> node2)
        {
            if (ReferenceEquals(node1, null))
            {
                return !ReferenceEquals(node2, null);
            }

            return !node1.Equals(node2);
        }
    }
}
