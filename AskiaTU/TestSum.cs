using AskiaModel;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using  Moq;

namespace AskiaTU
{
    [TestFixture]
    public class TestSum
    {
        [TestCase(10, 5, 5)]
        public void CheckSum(int res, int a, int b)
        {
            Mock<Sum> mock = new Mock<Sum>();
            mock.Setup(x=>x.Somme(a, b)).Returns(5);
            Assert.AreEqual(res, mock.Object.Somme(a, b));     
         }
    }
}
