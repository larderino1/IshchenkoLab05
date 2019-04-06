using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using IshchenkoLab05.Models;

namespace IshchenkoLab05.Tools.Managers
{
    internal static class StationManager
    {
        
            public static event Action StopThreads;
            
            internal static MyProcess CurretnProcess { get; set; }

            internal static void CloseApp()
            {
                MessageBox.Show("ShutDown");
                StopThreads?.Invoke();
                Environment.Exit(1);
            }
        
    }
}
