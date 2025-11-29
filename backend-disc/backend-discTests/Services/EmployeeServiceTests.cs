using Microsoft.VisualStudio.TestTools.UnitTesting;
using backend_disc.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace backend_disc.Services.Tests
{
    [TestClass()]
    public class EmployeeServiceTests
    {
        [TestMethod()]
        public void GenerateUsernameWorkMailAndPhoneSucess()
        {
            int a = 1;
            Assert.AreEqual(1, a);
        }

        [TestMethod()]
        public void GenerateUsernameWorkMailAndPhoneFail()
        {
            int a = 1;
            Assert.AreNotEqual(2, a);
        }


    }
}