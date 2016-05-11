using ConsoleApplication1;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestProject
{
    [TestClass]
    public class NodeTest
    {
        [TestMethod]
        public void NodeTest_123()
        {
            Node node = new Node(0, "Hello");
        }
    }
}
