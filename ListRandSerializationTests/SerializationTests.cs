using System;
using System.IO;
using ListRandSerialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ListRandSerializationTests
{
    [TestClass]
    public class SerializationTests
    {
        private static Random random = new Random();

        [TestMethod]
        public void EmptyListTest()
        {
            var oldEmptyList = new ListRand();
            var newEmptyList = ProcessSerialization(oldEmptyList);

            Assert.IsTrue(Compare(oldEmptyList, newEmptyList));
        }

        [TestMethod]
        public void SingleElementListTest()
        {
            var oldList = GenerateListRand(1);
            var newList = ProcessSerialization(oldList);

            Assert.IsTrue(Compare(oldList, newList));
        }

        [TestMethod]
        public void ShortListsTest()
        {
            for (var i = 2; i < 10; i++)
            {
                var oldList = GenerateListRand(i);
                var newList = ProcessSerialization(oldList);

                Assert.IsTrue(Compare(oldList, newList));
            }
        }

        [TestMethod]
        public void LongListsTest()
        {
            for (var i = 100; i < 1000; i += 100)
            {
                var oldList = GenerateListRand(i);
                var newList = ProcessSerialization(oldList);

                Assert.IsTrue(Compare(oldList, newList));
            }
        }

        private static bool Compare(ListRand oldList, ListRand newList)
        {
            if (oldList.Count != newList.Count)
            {
                return false;
            }

            var currentNodeOld = oldList.Head;
            var currentNodeNew = newList.Head;
            for (var i = 0; i < oldList.Count; i++)
            {
                if (!CompareNodes(currentNodeOld, currentNodeNew))
                {
                    return false;
                }

                currentNodeOld = currentNodeOld.Next;
                currentNodeNew = currentNodeNew.Next;
            }

            currentNodeOld = oldList.Tail;
            currentNodeNew = newList.Tail;
            for (var i = 0; i < oldList.Count; i++)
            {
                if (!CompareNodes(currentNodeOld, currentNodeNew))
                {
                    return false;
                }

                currentNodeOld = currentNodeOld.Prev;
                currentNodeNew = currentNodeNew.Prev;
            }

            return true;
        }

        private static bool CompareNodes(ListNode node1, ListNode node2)
        {
            return node1.Data == node2.Data && node1.Rand.Data == node2.Rand.Data;
        }

        private ListRand ProcessSerialization(ListRand listRand)
        {
            var newListRand = new ListRand();
            var filepath = Path.GetTempFileName();

            var fileSteam = new FileStream(filepath, FileMode.Create);
            listRand.Serialize(fileSteam);

            fileSteam = new FileStream(filepath, FileMode.Open);
            newListRand.Deserialize(fileSteam);

            return newListRand;
        }

        private ListRand GenerateListRand(int count)
        {
            var listRand = new ListRand()
            {
                Count = count
            };

            var nodes = new ListNode[count];
            for (int i = 0; i < count; i++)
            {
                nodes[i] = new ListNode();
            }

            for (int i = 0; i < count; i++)
            {
                if (i > 0)
                {
                    nodes[i].Prev = nodes[i - 1];
                }

                if (i != count - 1)
                {
                    nodes[i].Next = nodes[i + 1];
                }

                nodes[i].Rand = nodes[random.Next(0, count - 1)];

                nodes[i].Data = GenerateString();
            }

            listRand.Head = nodes[0];
            listRand.Tail = nodes[count - 1];

            return listRand;
        }

        private string GenerateString() => Guid.NewGuid().ToString();
    }
}
