using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DeftLib;

namespace DeftTesting
{
    /// <summary>
    /// Test harness for "DeftLib.Entity" class.
    /// Goal is to establish security around component manipulation. 
    /// </summary>
    [TestClass]
    public class Tests_Entity
    {
        // Declare all variables needed for testing.
        // NOTE: Complete test harness must declare a variable for ALL DeftLib component types.
        public List<Entity> entities;
        public SpatialComponent spatial;

        public Entity First { get => entities[0]; }

        [TestInitialize]
        public void SetupTests()
        {
            entities = new List<Entity>();
            entities.Add(new Entity());

            spatial = new SpatialComponent();
        }

        /// <summary>
        /// Tests that each component type can be successfully added to and retrieved from an Entity.
        /// NOTE : Complete test method must provide a test for ALL DeftLib component types.
        /// </summary>
        [TestMethod]
        public void TestAddComponent()
        {
            First.AddComponent<SpatialComponent>(spatial);
            Assert.IsTrue(First.HasComponent<SpatialComponent>());
        }

        [TestMethod]
        public void TestRetrieveComponent()
        {
            First.AddComponent<SpatialComponent>(spatial);
            Assert.IsNotNull(First.GetComponent<SpatialComponent>());
        }

        [TestMethod]
        public void TestRemoveComponent()
        {
            First.AddComponent<SpatialComponent>(spatial);
            First.RemoveComponent<SpatialComponent>();
            Assert.IsFalse(First.HasComponent<SpatialComponent>());
        }
    }
}
