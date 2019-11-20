using _EDD_ProyectoDeClase.EstructurasDeDatos;
using _EDD_ProyectoDeClase.MyGui;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _EDD_ProyectoDeClase
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-us");
            CreateNecessaryDirectory();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
            //var lista = new DoubleList<string>();
            //lista.AddFirst("Javier0");
            //lista.AddLast("Javier1");
            //lista.AddFirst("Javier2");
            //lista.AddLast("Javier3");
            //lista.AddFirst("Javier4");
            //lista.AddLast("Javier5");
            //foreach (var item in lista)
            //{
            //    Console.WriteLine(item);
            //}

            //var m = new Matrix<string, string, IndexableDePrueba>();
            //m.Add(new IndexableDePrueba("a", "a", "Javier AA"));
            //m.Add(new IndexableDePrueba("b", "b", "Javier BB"));
            //m.Add(new IndexableDePrueba("c", "c", "Javier CC"));
            //m.Add(new IndexableDePrueba("d", "d", "Javier DD"));
            //m.Add(new IndexableDePrueba("e", "e", "Javier EE"));

            //foreach (var item in m)
            //{
            //    Console.WriteLine(item.ToString());
            //}
        }

        private static void CreateNecessaryDirectory()
        {
            Directory.CreateDirectory("res");
        }
    }
}
