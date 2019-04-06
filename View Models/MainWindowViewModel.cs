using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using IshchenkoLab05.Models;
using IshchenkoLab05.Tools.Managers;

namespace IshchenkoLab05.View_Models
{
    internal class MainWindowViewModel : INotifyPropertyChanged

    {
        private Task _task;
        private CancellationToken _token;
        private Thread _workingThread;
        private CancellationTokenSource _tokenSource;

        public List<MyProcess> ProcessesList { get; set; }

        internal MainWindowViewModel()
        {
            Process[] processes = Process.GetProcesses();
            ProcessesList = new List<MyProcess>();
            _tokenSource = new CancellationTokenSource();
            _token = _tokenSource.Token;
            BeginWorkingThread();
            StationManager.StopThreads += StopWorkingThread;
            StartBarckgroundTask();

            foreach (var process in processes)
            {
                try
                {
                    ProcessesList.Add(new MyProcess(process));
                }
                catch (Exception e)
                { 
                    
                }
                
            }
            
        }
        private async void StartBarckgroundTask()
        {
            await Task.Run(() =>
            {
                _task = Task.Factory.StartNew(BackgroundTaskProcess, TaskCreationOptions.LongRunning);
                
            });
        }
        
        private void BackgroundTaskProcess()
        {
            Process[] processes = Process.GetProcesses();
            while (!_token.IsCancellationRequested) 
            {
                
                for (int i = 0; i < 5; i++)
                {
                    Thread.Sleep(1000);
                    if (_token.IsCancellationRequested) break;
                }

                ProcessesList = new List<MyProcess>();
                foreach (Process process in Process.GetProcesses())
                {
                    try
                    {
                        ProcessesList.Add(new MyProcess(process));
                    }
                    catch (Exception e)
                    {

                    }
                }
                if (_token.IsCancellationRequested)
                    break;
                                
            }
        }
        private void BeginWorkingThread()
        {
            _workingThread = new Thread(StartBarckgroundTask);
            _workingThread.Start();
        }
        private Process _selectedProcess;
        public Process SelectedProcess
        {
            get => _selectedProcess;
            set
            {
                _selectedProcess = value;
                OnPropertyChanged();
            }
        }

        internal void StopWorkingThread()
        {
            _tokenSource.Cancel();
            _workingThread.Join(2000);
            _workingThread.Abort();
            _workingThread = null;
        }
        public async void KillSelectedProcess()
        {
            await Task.Run(() => { SelectedProcess.Kill(); });
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
