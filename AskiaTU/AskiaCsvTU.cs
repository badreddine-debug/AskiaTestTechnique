using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;
using AskiaModel;
using Moq;

namespace AskiaTU
{
    [TestClass]
    public class AskiaCsvTU
    {
        private string OutputFile;

        [TestInitialize]
        public void Init()
        {
            //get file output json
            OutputFile = ConfigurationManager.AppSettings["outputFile"];
        }

        /// <summary>
        /// File exists
        /// </summary>
        [TestMethod]
        public void CheckFileExists_WhenCheckFileExists_ThenFileExists()
        {
            string[] Values = ConfigurationManager.AppSettings["SimpleExamplewithoutHeader"].Split(',');
            string CSVpath = Values[0];

            bool isExists = ConvertCsvFileToJson.IsCheckFileExists(CSVpath);
            Assert.AreEqual(true, isExists);
            Assert.IsTrue(isExists);
        }

        /// <summary>
        /// File is not exist
        /// </summary>
        [TestMethod]
        public void CheckFileExists_WhenCheckFileIsNotExists_ThenFileIsNotExists()
        {
            string[] Values = ConfigurationManager.AppSettings["fileNotExist"].Split(',');
            string CSVpath = Values[0];

            bool isNotExists = ConvertCsvFileToJson.IsCheckFileExists(CSVpath);
            Assert.AreEqual(false, isNotExists);
            Assert.IsFalse(isNotExists);
        }

        /// <summary>
        /// File is Empty null
        /// </summary>
        [TestMethod]
        public void GenerateJsonFile_WhenCheckFileIsEmpty_ThenFileIsEmpty()
        {
            string[] Values = ConfigurationManager.AppSettings["EmtyFile"].Split(',');
            string CSVpath = Values[0];
            bool IsHeader = Convert.ToBoolean(Values[1]);

            bool isNullEmpty = ConvertCsvFileToJson.GenerateJsonFile(CSVpath, OutputFile, IsHeader);
            Assert.AreEqual(false, isNullEmpty);
            Assert.IsFalse(isNullEmpty);
        }

        /// <summary>
        /// ReadFile simple Example header
        /// </summary>
        [TestMethod]
        public void ReadFile_WhenReadFileSimpleExampleHeader_ThenListData()
        {
            string[] Values = ConfigurationManager.AppSettings["SimpleExampleHeader"].Split(',');
            string CSVpath = Values[0];

            var listData = ConvertCsvFileToJson.ReadFile(CSVpath);
            Assert.IsNotNull(listData);
            Assert.AreEqual(6, listData.Count);
        }

        /// <summary>
        /// ReadFile simple Example header
        /// </summary>
        [TestMethod]
        public void GetHeaders_WhenReadFileSimpleExampleHeader_ThenGetDataHeader()
        {
            string[] Values = ConfigurationManager.AppSettings["SimpleExampleHeader"].Split(',');
            string CSVpath = Values[0];

            var listData = ConvertCsvFileToJson.ReadFile(CSVpath);
            var getHeader = ConvertCsvFileToJson.GetHeaders(listData);
            Assert.IsNotNull(getHeader);
            Assert.AreEqual(4, getHeader.Length);
            Assert.AreEqual("Year", getHeader[0]);
            Assert.AreEqual("Car", getHeader[1]);
            Assert.AreEqual("Model", getHeader[2]);
            Assert.AreEqual("Description", getHeader[3]);
        }
        /// <summary>
        /// Det Data CSV
        /// </summary>
        [TestMethod]
        public void ReadFile_WhenReadFileSimpleExampleHeader_ThenGetDataCSV()
        {
            string[] Values = ConfigurationManager.AppSettings["SimpleExampleHeader"].Split(',');
            string CSVpath = Values[0];

            var listData = ConvertCsvFileToJson.ReadFile(CSVpath);
            var getData = listData[1];
            Assert.IsNotNull(getData);
            Assert.AreEqual(4, getData.Length);
            Assert.AreEqual("1991", getData[0]);
            Assert.AreEqual("Ford", getData[1]);
            Assert.AreEqual("E350", getData[2]);
            Assert.AreEqual("Super", getData[3]);
        }

        /// <summary>
        /// Det Data CSV Commas And Quoted
        /// </summary>
        [TestMethod]
        public void ReadFile_WhenReadFileExampleCommasAndQuoted_ThenGetDataCSV()
        {
            string[] Values = ConfigurationManager.AppSettings["ExampleCommasAndQuoted"].Split(',');
            string CSVpath = Values[0];

            var listData = ConvertCsvFileToJson.ReadFile(CSVpath);
            var getData = listData[1];
            Assert.IsNotNull(getData);
            Assert.AreEqual(4, getData.Length);
            Assert.AreEqual("1991", getData[0]);
            Assert.AreEqual("Ford \"\"test ; test1\"\"", getData[1]);
            Assert.AreEqual("E350", getData[2]);
            Assert.AreEqual("Test \"\"Super\"\"", getData[3]);
        }

        /// <summary>
        /// Det Data CSV Commas And Double Quoted
        /// </summary>
        [TestMethod]
        public void ReadFile_WhenReadFileExampleCommasAndDoubleQuoted_ThenGetDataCSV()
        {
            string[] Values = ConfigurationManager.AppSettings["ExampleCommasAndDoubleQuoted"].Split(',');
            string CSVpath = Values[0];

            var listData = ConvertCsvFileToJson.ReadFile(CSVpath);
            var getData = listData[5];
            Assert.IsNotNull(getData);
            Assert.AreEqual(4, getData.Length);
            Assert.AreEqual("1995", getData[0]);
            Assert.AreEqual("Kia", getData[1]);
            Assert.AreEqual("Rio", getData[2]);
            Assert.AreEqual("\"\"Super;\"\"\"\"luxurious\"\"\"\" truck\"\"", getData[3]);
        }

        /// <summary>
        /// Det Data CSV Commas And Quoted breaks Line
        /// </summary>
        [TestMethod]
        public void ReadFile_WhenReadFileExampleCommasAndQuotedAndbreaksLine_ThenGetDataCSV()
        {
            string[] Values = ConfigurationManager.AppSettings["ExampleCommasAndQuotedAndbreaksLine"].Split(',');
            string CSVpath = Values[0];

            var listData = ConvertCsvFileToJson.ReadFile(CSVpath);
            var getData = listData[5];
            Assert.IsNotNull(getData);
            Assert.AreEqual(4, getData.Length);
            Assert.AreEqual("1995", getData[0]);
            Assert.AreEqual("Kia", getData[1]);
            Assert.AreEqual("Rio", getData[2]);
            Assert.AreEqual("\"\"Go getone nowthey are going fast\"\"", getData[3]);
        }
        /// <summary>
        /// ReadFile Exemple Empty
        /// </summary>
        [TestMethod]
        public void ReadFile_WhenReadFileExampleEmpty_ThenListEmpty()
        {
            string[] Values = ConfigurationManager.AppSettings["EmtyFile"].Split(',');
            string CSVpath = Values[0];

            var listData = ConvertCsvFileToJson.ReadFile(CSVpath);
            Assert.IsNotNull(listData);
            Assert.AreEqual(0, listData.Count);
        }

        /// <summary>
        /// Convert Csv File To Json Object Simple Example Header
        /// </summary>
        [TestMethod]
        public void ConvertCsvFileToJsonObject_WhenConvertCsvFileToJsonObject_ThenIsNotNull()
        {
            string[] Values = ConfigurationManager.AppSettings["SimpleExampleHeader"].Split(',');
            string CSVpath = Values[0];

            string listData = ConvertCsvFileToJson.ConvertCsvFileToJsonObject(CSVpath);
            Assert.IsNotNull(listData);
        }

        /// <summary>
        /// Convert Csv File To Json Object Example empty
        /// </summary>
        [TestMethod]
        public void ConvertCsvFileToJsonObject_WhenConvertCsvFileToJsonObject_ThenExampleEmpty()
        {
            string[] Values = ConfigurationManager.AppSettings["EmtyFile"].Split(',');
            string CSVpath = Values[0];

            string listData = ConvertCsvFileToJson.ConvertCsvFileToJsonObject(CSVpath);
            Assert.IsNotNull(listData);
            Assert.AreEqual(string.Empty, listData);
        }

        [TestMethod]
        public void CheckSumTest()
        {
            int res = 10, a = 5, b = 5;
            Mock<Sum> mock = new Mock<Sum>();
            mock.SetupGet(x => x.Somme(a, b)).Returns(res);
            Assert.AreEqual(res, mock.Object.Somme(a, b));
        }
    }
}
