using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Dynamic;
using System.Threading;
using JetBrains.Annotations;

namespace IshchenkoLab05.Models
{
    internal class MyProcess : ContextBoundObject
    {
        private readonly Process _process;
        private readonly int _processID;
        private readonly string _processName;
        private bool _isActive;
        private PerformanceCounter theCPUCounter =
            new PerformanceCounter("Processor", "% Processor Time", Process.GetCurrentProcess().ProcessName, true);
        private float _cpu;
        private float _ram;
       private readonly int _threadAmount;
        private readonly string _user;
        private readonly string _filePath;
        private readonly DateTime _processTime;


        





        public int ProcessID
        {
            get => _processID;
        }

        public string ProcessName
        {
            get => _processName;
        }

        public bool IsActive
        {
            get => _isActive;
        }

        public float ProcessCPU
        {
            
           get => _cpu;
        } 

        public float ProcessRAM
        {
            get => _ram;
        }
        

        public int ThreadsAmount
        {
            get => _threadAmount;
        }
        
        public string User
        {
            get => _user;
        }

        public string FilePath
        {
            get => _filePath;
        }

        public DateTime ProcessTime
        {
            get => _processTime;
        }
        

        
       

       



        private List<MyModules> _modules;
        private List<MyThreads> _threads;
        

        public List<MyModules> Modules
        {
            get
            {
                if (_modules == null)
                {
                    RefreshModules(); 
                }
                return _modules;
            }
        }

        public List<MyThreads> Threads
        {
            get
            {
                if (_threads == null)
                {
                    RefreshThreads();
                }
                return _threads;
            }
        }

        internal async void RefreshThreads()
        {
            try
            {
                await Task.Run(() =>
                    {
                        _threads = new List<MyThreads>();
                        foreach (ProcessThread thread in _process.Threads)
                        {
                            _threads.Add(new MyThreads(thread));
                        }

                       
                    }
                );
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        internal async void RefreshModules()
        {

            await Task.Run(() =>
                    {
                        _modules = new List<MyModules>();
                        foreach (ProcessModule module in _process.Modules)
                        {
                            try
                            {
                                _modules.Add(new MyModules(module));

                            }
                            catch (Exception e)
                            {
                                _modules.Add(null);
                            }
                        }
                        
                    }
                
                );
                
        }

    

        

        internal MyProcess([NotNull] Process process)
        {
            this._process = process;
            this._processID = process.Id;
            this._processName = process.ProcessName;
            this._isActive = process.Responding;
            
            try
            {

                this._cpu = theCPUCounter.NextValue();
                this._ram = this._process.PrivateMemorySize64 / 2048;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            this._threadAmount = process.Threads.Count;
            this._user = process.MachineName;
            try
            {
                this._filePath = process.MainModule.FileName;
            }
            catch (Exception e)
            {
                this._filePath = "access denied";
            }

            try
            {
                this._processTime = process.StartTime;
            }
            catch (Exception e)
            {
                this._processTime = DateTime.Now;
            }
        }
    }
}
