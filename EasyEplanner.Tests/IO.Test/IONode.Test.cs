﻿using System;
using System.Collections.Generic;
using System.Reflection;
using EasyEPlanner;
using EplanDevice;
using IO;
using Moq;
using NUnit.Framework;

namespace IOTests
{
    class IONodeTest
    {
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            typeof(IOManager).GetField("instance", BindingFlags.NonPublic | BindingFlags.Static)
                .SetValue(null, null);
            IOManager.GetInstance(); // Load description from file.
        }

        [TestCaseSource(nameof(TypeTestCaseSource))]
        public void Type_NewNode_CorrectGetAndSet(string typeStr,
            IONode.TYPES expectedType)
        {
            var testNode = new IONode(typeStr, IntStub, IntStub, StrStub, StrStub, StrStub, StrStub);

            Assert.AreEqual(expectedType, testNode.Type);
        }

        private static object[] TypeTestCaseSource()
        {
            var testData = new List<object[]>();

            IONode.TYPES internal750_86x = IONode.TYPES.T_INTERNAL_750_86x;
            testData.Add(new object[] { "750-863", internal750_86x });

            IONode.TYPES ethernet = IONode.TYPES.T_ETHERNET;
            testData.Add(new object[] { "750-341", ethernet });
            testData.Add(new object[] { "750-841", ethernet });
            testData.Add(new object[] { "750-352", ethernet });
            testData.Add(new object[] { "750-362", ethernet });

            IONode.TYPES internal750_820x = IONode.TYPES.T_INTERNAL_750_820x;
            testData.Add(new object[] { "750-8202", internal750_820x });
            testData.Add(new object[] { "750-8203", internal750_820x });
            testData.Add(new object[] { "750-8204", internal750_820x });
            testData.Add(new object[] { "750-8206", internal750_820x });

            IONode.TYPES pxcCoupler = IONode.TYPES.T_PHOENIX_CONTACT;
            testData.Add(new object[] { "AXL F BK ETH", pxcCoupler });
            testData.Add(new object[] { "AXL F BK ETH NET2", pxcCoupler });

            IONode.TYPES pxcController = IONode.TYPES.T_PHOENIX_CONTACT_MAIN;
            testData.Add(new object[] { "AXC F 1152", pxcController });
            testData.Add(new object[] { "AXC F 2152", pxcController });
            testData.Add(new object[] { "AXC F 3152", pxcController });

            IONode.TYPES emptyType = IONode.TYPES.T_EMPTY;
            testData.Add(new object[] { "", emptyType });
            testData.Add(new object[] { default, emptyType });
            testData.Add(new object[] { "Wrong type", emptyType });

            return testData.ToArray();
        }

        [TestCase("750-863", false)]
        [TestCase("750-341", true)]
        [TestCase("750-841", false)]
        [TestCase("750-352", true)]
        [TestCase("750-362", true)]
        [TestCase("750-8202", false)]
        [TestCase("750-8203", false)]
        [TestCase("750-8204", false)]
        [TestCase("750-8206", false)]
        [TestCase("AXL F BK ETH", true)]
        [TestCase("AXL F BK ETH NET2", true)]
        [TestCase("AXC F 1152", false)]
        [TestCase("AXC F 2152", false)]
        [TestCase("AXC F 3152", false)]
        [TestCase("", false)]
        [TestCase(null, false)]
        [TestCase("Wrong type", false)]
        public void IsCoupler_NewNode_ReturnsTrueOrFalse(string typeStr,
            bool expectedValue)
        {
            var testNode = new IONode(typeStr, IntStub, IntStub, StrStub, StrStub, StrStub, StrStub);

            Assert.AreEqual(expectedValue, testNode.IsCoupler);
        }

        [TestCase("NodeName", "NodeName")]
        [TestCase(null, null)]
        [TestCase("", "")]
        [TestCase("Имя узла", "Имя узла")]
        public void Name_NewNode_CorrectGetAndSet(string expected,
            string actual)
        {
            var testNode = new IONode(StrStub, IntStub, IntStub, StrStub, actual, StrStub, StrStub);
            Assert.AreEqual(expected, testNode.Name);
        }

        [TestCase("", "")]
        [TestCase(null, null)]
        [TestCase("255.0.0.0", "255.0.0.0")]
        [TestCase("12.12.12.12", "12.12.12.12")]
        public void IP_NewNode_CorrectGetAndSet(string expected, string actual)
        {
            var testNode = new IONode(StrStub, IntStub, IntStub, actual, StrStub, StrStub, StrStub);
            Assert.AreEqual(expected, testNode.IP);
        }

        [TestCase("TypeStrSetted", "TypeStrSetted")]
        [TestCase(null, null)]
        [TestCase("", "")]
        [TestCase("Строковый тип", "Строковый тип")]
        public void Type_NewNode_CorrectGetAndSet(string expected, string actual)
        {
            var testNode = new IONode(actual, IntStub, IntStub, StrStub, StrStub, StrStub, StrStub);
            Assert.AreEqual(expected, testNode.TypeStr);
        }

        [TestCase(0, 0)]
        [TestCase(1, 1)]
        [TestCase(2, 2)]
        [TestCase(10, 10)]
        public void N_NewNode_CorrectGetAndSet(int expected, int actual)
        {
            var testNode = new IONode(StrStub, actual, IntStub, StrStub, StrStub, StrStub, StrStub);
            Assert.AreEqual(expected, testNode.N);
        }

        [TestCase(0, 0)]
        [TestCase(1, 1)]
        [TestCase(100, 100)]
        [TestCase(900, 900)]
        public void FullN_NewNode_CorrectGetAndSet(int expected, int actual)
        {
            var testNode = new IONode(StrStub, IntStub, actual, StrStub, StrStub, StrStub, StrStub);
            Assert.AreEqual(expected, testNode.NodeNumber);
        }

        [TestCase(0, 0)]
        [TestCase(4, 4)]
        public void AIcount_NewNode_CorrectGetAndSetSignalsCount(int expected,
            int actual)
        {
            var testNode = new IONode(StrStub, IntStub, IntStub, StrStub, StrStub, StrStub, StrStub);

            testNode.AI_count += actual;

            Assert.AreEqual(expected, testNode.AI_count);
        }

        [TestCase(0, 0)]
        [TestCase(4, 4)]
        public void AOcount_NewNode_CorrectGetAndSetSignalsCount(int expected,
            int actual)
        {
            var testNode = new IONode(StrStub, IntStub, IntStub, StrStub, StrStub, StrStub, StrStub);

            testNode.AO_count += actual;

            Assert.AreEqual(expected, testNode.AO_count);
        }

        [TestCase(0, 0)]
        [TestCase(4, 4)]
        public void DIcount_NewNode_CorrectGetAndSetSignalsCount(int expected,
            int actual)
        {
            var testNode = new IONode(StrStub, IntStub, IntStub, StrStub, StrStub, StrStub, StrStub);

            testNode.DI_count += actual;

            Assert.AreEqual(expected, testNode.DI_count);
        }

        [TestCase(0, 0)]
        [TestCase(4, 4)]
        public void DOcount_NewNode_CorrectGetAndSetSignalsCount(int expected,
            int actual)
        {
            var testNode = new IONode(StrStub, IntStub, IntStub, StrStub, StrStub, StrStub, StrStub);

            testNode.DO_count += actual;

            Assert.AreEqual(expected, testNode.DO_count);
        }


        [TestCase("+MCC1", "+MCC1")]
        [TestCase(null, null)]
        [TestCase("", "")]
        [TestCase("location", "location")]
        public void Location_NewNode_CorrectGetAndSet(string expected,
            string actual)
        {
            var testNode = new IONode(StrStub, IntStub, IntStub, StrStub, StrStub, actual, StrStub);
            Assert.AreEqual(expected, testNode.Location);
        }

        [Test]
        public void SetModule_CorrectSet()
        {
            const string moduleName = "A101";
            const string nodeType = "AXC F 1152";
            const int lastIndex = 63;

            var EmptyIOModuleMock = new Mock<IIOModule>();
            EmptyIOModuleMock.Setup(m => m.Name).Returns(string.Empty);

            var IOModuleMock = new Mock<IIOModule>();
            IOModuleMock.Setup(m => m.Name).Returns(moduleName);

            var testNodeMock = new Mock<IONode>(nodeType, 1, 1, string.Empty, string.Empty, string.Empty, string.Empty);
            testNodeMock.Setup(n => n.StubIOModule).Returns(EmptyIOModuleMock.Object);

            var testNode = testNodeMock.Object;

            testNode.SetModule(IOModuleMock.Object, lastIndex);

            Assert.IsTrue(testNode[lastIndex - 1].Name == moduleName);
        }

        [TestCase("AXC F 1152", 64)]
        [TestCase("750-863", 65)]
        public void SetModule_IndexOutOfRangeException(string type, int index)
        {
            var testNode = new IONode(type, 1, 1, string.Empty, string.Empty, string.Empty, string.Empty);

            var IOModuleMock = new Mock<IIOModule>();

            Assert.Throws<IONode.ModulesPerNodeOutOfRageException>(() => { testNode.SetModule(IOModuleMock.Object, index); });
        }

        [TestCase("AXC F 1152", 1)]
        public void SetModule_AddressAreaOutOfRangeException(string type, int index)
        {
            var testNode = new IONode(type, 1, 1, string.Empty, string.Empty, string.Empty, string.Empty);

            var currentAddressArea = typeof(IONode).GetField("currentAddressArea",
                BindingFlags.NonPublic | BindingFlags.Instance);
            currentAddressArea.SetValue(testNode, IONode.IOAddressArea.PHOENIX_CONTACT.AddressAreaMax - 4);

            var IOModuleMock = new Mock<IIOModule>();
            IOModuleMock.Setup(m => m.AddressArea).Returns(6);

            Assert.Throws<IONode.AddressAreaOutOfRangeException>(() => { testNode.SetModule(IOModuleMock.Object, index); });
        }

        [Test]
        public void SetModule_AddressAreaNullReferenceException()
        {
            var testNode = new IONode("AXC F 1152", 1, 1, string.Empty, string.Empty, string.Empty, string.Empty);
            typeof(IONode).GetProperty("AddressArea", BindingFlags.Public | BindingFlags.Instance)
                .SetValue(testNode, null);

            Assert.Throws<IONode.AddressAreaNullReferenceException>(() => { testNode.SetModule(null, 0); });
        }

        const string StrStub = "";
        const int IntStub = 0;
    }
}
