using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace AskiaModel
{
    public class ConvertCsvFileToJson
    {
        //Declare an instance for log4net
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Check File Exists is not exist
        /// </summary>
        /// <param name="filePath">file path csv</param>
        /// <returns>value bool</returns>
        public static bool IsCheckFileExists(string filePath)
        {
            if (!string.IsNullOrWhiteSpace(filePath) && File.Exists(filePath) && Path.GetExtension(filePath).Equals(".csv", StringComparison.InvariantCultureIgnoreCase))
            {
                //Check File Exists
                Log.Info(Constant.fileExists);
                return true;
            } else
            {
                //Check File is not Exists
                Log.Info(Constant.fileIsNotExists);
            }

            return false;
        }
        /// <summary>
        /// Read File csv
        /// </summary>
        /// <param name="filePath">file path csv</param>
        /// <returns>list of Csv data</returns>
        public static List<string[]> ReadFile(string filePath)
        {
            try
            {
                var csvData = new List<string[]>();
                //Reaf file CSV
                var csvFileContent = File.ReadAllText(filePath);
                //Delete espace
                var fixedCsvFileContent = Regex.Replace(csvFileContent, @"(?!(([^""]*""){2})*[^""]*$)\n+", string.Empty);
                //Split of line
                string[] stringSeparators = new string[] { "\r\n" };
                string[] lines = fixedCsvFileContent.Split(stringSeparators, StringSplitOptions.None);

                foreach (string line in lines)
                {
                    if (line.Length != 0)
                    {
                        string[] values = ParseCsvRow(line);
                        csvData.Add(values);
                    }
                }

                return csvData;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// generate csv file line
        /// </summary>
        /// <param name="line">line of Csv</param>
        /// <returns>list of Csv data</returns>
        /// 
        public static string[] ParseCsvRow(string line)
        {
            try
            {
                char separator = ';';
                List<string> parsed = new List<string>();
                string[] temp = line.Split(separator);
                bool isLast = false;
                string data = "";

                foreach (string item in temp)
                {
                    string value = item;

                    if (isLast)
                    {
                        // End of field
                        if (value.EndsWith("\""))
                        {
                            data += separator + value.Substring(0, value.Length - 1);
                            parsed.Add(data);
                            data = "";
                            isLast = false;
                            continue;

                        }
                        else
                        {
                            // Field still not ended
                            data += separator + value;
                            continue;
                        }
                    }

                    // Fully encapsulated with no comma within
                    if (value.StartsWith("\"") && value.EndsWith("\""))
                    {
                        if ((value.EndsWith("\"\"") && !value.EndsWith("\"\"\"")) && value != "\"\"")
                        {
                            isLast = true;
                            data = value;
                            continue;
                        }

                        parsed.Add(value.Substring(1, value.Length - 2));
                        continue;
                    }

                    // Start of encapsulation but comma has split it into at least next field
                    if (value.StartsWith("\"") && !value.EndsWith("\""))
                    {
                        isLast = true;
                        data += value.Substring(1);
                        continue;
                    }

                    // Non encapsulated complete field
                    parsed.Add(value);
                }

                return parsed.ToArray();
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// Get header csv
        /// </summary>
        /// <param name="dataFromTable">list of Csv data</param>
        /// <returns>data header</returns>
        public static string[] GetHeaders(List<string[]> dataFromTable)
        {
            try
            {
                string[] headers = null;

                if (dataFromTable.Any())
                {
                    headers = dataFromTable[0];
                }
                return headers;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// Convert Csv File To Json Object
        /// </summary>
        /// <param name="filePath">file path csv</param>
        /// <returns>return Json Object</returns>
        public static string ConvertCsvFileToJsonObject(string filePath)
        {
            try
            {
                List<string[]> dataFromTable = ReadFile(filePath);
                if (dataFromTable.Count() == 0)
                    return String.Empty;

                //Get header
                var properties = GetHeaders(dataFromTable);

                //list Objet
                var listObjResult = new List<Dictionary<string, string>>();

                for (int i = 1; i < dataFromTable.Count; i++)
                {
                    var objResult = new Dictionary<string, string>();
                    for (int j = 0; j < properties.Length; j++)
                        objResult.Add(properties[j], dataFromTable[i][j]);

                    //rempli list objet
                    listObjResult.Add(objResult);
                }

                return JsonConvert.SerializeObject(listObjResult);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// Convert Csv File without Header To Json Object
        /// </summary>
        /// <param name="filePath">file path csv</param>
        /// <returns>return Json Object</returns>
        public static string ConvertCsvFilewithoutHeaderToJsonObject(string filePath)
        {
            try
            {
                var lines = ReadFile(filePath);
                if (lines.Count() == 0)
                    return String.Empty;
                return JsonConvert.SerializeObject(lines);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// Generate Json File
        /// </summary>
        /// <param name="filePath">file path csv</param>
        /// <param name="outputFile">file path json</param>
        /// <param name="IsHeader">check csv is header</param>
        /// <returns></returns>
        public static bool GenerateJsonFile(string filePath, string outputFile, bool IsHeader)
        {
            try
            {
                string dataFromTable = string.Empty;
                // retrive the data from table 
                if (IsHeader)
                {
                    dataFromTable = ConvertCsvFileToJsonObject(filePath);
                    if(string.IsNullOrEmpty(dataFromTable))
                    {
                        Log.Info(Constant.fileEmpty);
                        return false;
                    }
                }
                else
                {
                    dataFromTable = ConvertCsvFilewithoutHeaderToJsonObject(filePath);
                    if (string.IsNullOrEmpty(dataFromTable))
                    {
                        Log.Info(Constant.fileEmpty);
                        return false;
                    }
                }
                // Pass the data object for conversion object to JSON string  
                string jsondata = new JavaScriptSerializer().Serialize(dataFromTable);
                //Get name file Json
                string filename = Path.GetFileNameWithoutExtension(filePath);
                // Write that JSON to txt file,
                File.WriteAllText(outputFile + filename + ".json", jsondata);
                Log.Info(Constant.fileJsonManage);

                return true;
            }
            catch (Exception ex)
            {
                Log.Info(Constant.fileJsonIsNotManage);
                Log.Error(ex.Message);
                return false;
            }
        }
    }
}
