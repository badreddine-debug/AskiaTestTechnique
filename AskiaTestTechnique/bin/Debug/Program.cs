using AskiaModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.IO;
using log4net;
using System.Threading;
using System.Diagnostics;

namespace AskiaTestTechnique
{
    public class Program
    {
        //Declare an instance for log4net
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static void Main(string[] args)
        {
            //Start execute application
            Log.Info("----------------------------------------------------");
            Log.Info("--------------Début de traitement-------------------");
            Log.Info("----------------------------------- ----------------");
            //Thread declaration
            Thread thread;
            Stopwatch sw = Stopwatch.StartNew();
            //get file Csv
            string[] Values = ConfigurationManager.AppSettings["SimpleExampleHeader"].Split(',');
            //get path file csv
            string CSVpath = Values[0];
            //get header csv
            bool IsHeader = Convert.ToBoolean(Values[1]);

            //Check File Exists or not exists
            if (ConvertCsvFileToJson.IsCheckFileExists(CSVpath))
            {
                //get file output json
                string OutputFile = ConfigurationManager.AppSettings["outputFile"];
                //instance thread, calls it method
                thread = new Thread(() => ConvertCsvFileToJson.GenerateJsonFile(CSVpath, OutputFile, IsHeader));
                //start thread
                thread.Start();
            }
            //Fin execution
            sw.Stop();
            Log.Info(Constant.MsgTime + sw.Elapsed);
            //End execute application
            Log.Info("----------------------------------------------------");
            Log.Info("--------------Fin de traitement---------------------");
            Log.Info("----------------------------------- ----------------");
        }
    }
}
