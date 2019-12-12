//using System;
//using AutoUi.Core.ViewModels;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Xunit;

//namespace AutoUi.Core.Test
//{
//    [TestClass]
//    public class CustomerVmTest
//    {
//        [TestMethod]
//        public void CustomerNameCorrectlyReflectsFirstNameAndLastName()
//        {
//            // Triple-A-Vorgehen:
//            // 1. (A)rrange
//            // 2. (A)ct
//            // 3. (A)ssert

//            // 1. (A)rrange
//            var vm = new CustomerVm()
//            {
//                Vorname = "Max",
//                Nachname = "Muster"
//            };

//            // 2. (A)ct
//            vm.Vorname = "Bea";

//            // 3. (A)ssert
//            Assert.AreEqual("Bea Muster", vm.Name);
//        }
//    }
//}
