using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ListRandSerialization
{
    public class ListRand
    {
        public ListNode Head;
        public ListNode Tail;
        public int Count;

        public void Serialize(FileStream s)
        {
            var nodeIndexes = new Dictionary<ListNode, int>();

            var currentNode = Head;
            for (int i = 0; i < Count; i++)
            {
                nodeIndexes.Add(currentNode, i);

                currentNode = currentNode.Next;
            }

            using (var binaryWriter = new BinaryWriter(s))
            {
                binaryWriter.Write(Count);

                currentNode = Head;
                for (int i = 0; i < Count; i++)
                {
                    binaryWriter.Write(currentNode.Data);
                    binaryWriter.Write(nodeIndexes[currentNode.Rand]);

                    currentNode = currentNode.Next;
                }
            }
        }

        public void Deserialize(FileStream s)
        {
            using (var binaryReader = new BinaryReader(s))
            {
                Count = binaryReader.ReadInt32();

                var nodes = new ListNode[Count];
                for (int i = 0; i < Count; i++)
                {
                    nodes[i] = new ListNode();
                }

                for (int i = 0; i < Count; i++)
                {
                    nodes[i].Data = binaryReader.ReadString();

                    if (i > 0)
                    {
                        nodes[i].Prev = nodes[i - 1];
                    }

                    if (i != Count - 1)
                    {
                        nodes[i].Next = nodes[i + 1];
                    }

                    var randIndex = binaryReader.ReadInt32();
                    nodes[i].Rand = nodes[randIndex];
                }

                if (Count > 0)
                {
                    Head = nodes[0];
                    Tail = nodes[Count - 1];
                }
            }
        }
    }
}
