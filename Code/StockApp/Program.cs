using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StockApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var companies = new[] { "ADS.DE", "ALV.DE", "BAS.DE", "BAYN.DE", "BEI.DE", "BMW.DE", "CBK.DE", "DAI.DE", "DBK.DE", "DB1.DE", "LHA.DE", "DPW.DE", "DTE.DE", "EOAN.DE", "FRE.DE", "FME.DE", "HEI.DE", "HEN3.DE", "IFX.DE", "SDF.DE", "LIN.DE", "MAN.DE", "MRK.DE", "MEO.DE", "MUV2.DE", "RWE.DE", "SAP.DE", "SIE.DE", "TKA.DE", "VOW3.DE" };
            var analyzers = StockDll.StockTools.StockAnalyzer.GetAnalyzers(companies, 100);

            foreach (var a in analyzers)
            {
                Console.WriteLine("Company: {0}, Return: {1}, StdDev: {2}", a.Company, a.Return, a.StdDev);
            }

            Console.ReadLine();
        }
    }
}
