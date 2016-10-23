using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RecordLabel.Data.ok;
using System.Collections.Generic;
using System.Linq;
using RecordLabel;

namespace RepositoryTests
{
    [TestClass]
    public class CollectionMergerTests
    {
        ICollectionMerger merger;

        [TestInitialize]
        public void Setup()
        {
            merger = new CollectionMerger();
        }

        private List<TestClass> GenerateOriginalCollection()
        {
            return new List<TestClass>()
            {
                new TestClass { Id = 1, Value = "target 1" },
                new TestClass { Id = 2, Value = "target 2" },
                new TestClass { Id = 3, Value = "target 3" }
            };
        }

        /// <summary>
        /// Performs a simple collection merge and returns the resulting collection
        /// </summary>
        /// <param name="removedEntries">Collection of entries that have been removed from the Original collection</param>
        /// <returns>New collection which is a result of a merge</returns>
        private List<TestClass> Merge(IList<TestClass> original, IList<TestClass> newCollection, ref IList<TestClass> removedEntries)
        {
            IList<TestClass> removed = null;
            var result = merger.MergeCollections<TestClass>(original, newCollection,
                (origEntity, newEntity) => {
                    origEntity.Value = newEntity.Value;
                    return origEntity;
                },
                newEntity => newEntity,
                toRemove => removed = toRemove);

            removedEntries = removed;
            return result;
        }

        private IList<TestClass> DefaultTest(IList<TestClass> originalCollection, IList<TestClass> newCollection,
            int expectedNumberOfRemovedItems)
        {
            IList<TestClass> removedEntries = null;
            var resultingCollection = Merge(originalCollection, newCollection, ref removedEntries);
            if (removedEntries == null) removedEntries = new List<TestClass>();

            Assert.IsTrue(resultingCollection.Count == newCollection.Count, "Resulting.Count != NewState.Count");
            Assert.IsTrue(removedEntries.Count == expectedNumberOfRemovedItems, "Removed wrong number of entries");

            // Make sure each entry in Resulting collection has no more than one match in the New or Original collection
            foreach (var item in resultingCollection)
            {
                Assert.IsFalse(newCollection.Count(x => x == item) > 1, "Entry in Resulting collection has more than 1 match in New collection");
                Assert.IsFalse(originalCollection.Count(x => x == item) > 1, "Entry in Resulting collection has more than 1 match in Original collection");

                Assert.IsNotNull(newCollection.SingleOrDefault(x => x == item)
                    ?? originalCollection.SingleOrDefault(x => x == item),
                    "Resulting collection doesn't contain items that aren't present in the new collection");
            }

            // Make sure removed entries are not present in the Resulting collection
            foreach (var item in removedEntries)
            {
                Assert.IsTrue(resultingCollection.Count(x => x == item) == 0, "Removed collection contains entries from the Resulting collection");
            }

            // Make sure entities are only removed from the Target collection (not removed from the New collection)
            foreach (var item in removedEntries)
            {
                Assert.IsTrue(newCollection.Count(x => x == item) == 0, "Removed collection contains entries from the New collection");
            }

            return resultingCollection;
        }

        [TestMethod]
        public void Remove2Items()
        {
            var newCollection = new List<TestClass>()
            {
                new TestClass { Id = 1,Value = "target 1" }
            };
            DefaultTest(GenerateOriginalCollection(), newCollection, 2);
        }

        [TestMethod]
        public void RemoveAll()
        {
            var newCollection = new List<TestClass>();
            var resultingCollection = DefaultTest(GenerateOriginalCollection(), newCollection, 3);
        }

        [TestMethod]
        public void RemoveAllAndAdd1()
        {
            var newCollection = new List<TestClass>()
            {
                new TestClass { Id = 0,Value = "target 2" }
            };
            var resultingCollection = DefaultTest(GenerateOriginalCollection(), newCollection, 3);
        }

        [TestMethod]
        public void Update1()
        {
            var newCollection = GenerateOriginalCollection();
            newCollection[1].Value = "New value";
            var resultingCollection = DefaultTest(GenerateOriginalCollection(), newCollection, 0);
        }

        [TestMethod]
        public void Add1()
        {
            var newCollection = GenerateOriginalCollection();
            newCollection.Add(new TestClass { Value = "New entry" });
            var resultingCollection = DefaultTest(GenerateOriginalCollection(), newCollection, 0);

            Assert.IsTrue(resultingCollection.Count == 4);
        }

        [TestMethod]
        public void Add2()
        {
            var newCollection = GenerateOriginalCollection();
            newCollection.Add(new TestClass { Value = "New entry" });
            newCollection.Add(new TestClass { Value = "New entry 2" });
            var resultingCollection = DefaultTest(GenerateOriginalCollection(), newCollection, 0);

            Assert.IsTrue(resultingCollection.Count == 5);
        }
    }

    internal class TestClass : IHasId
    {
        public int Id { get; set; }
        public string Value { get; set; }
    }
    
}
