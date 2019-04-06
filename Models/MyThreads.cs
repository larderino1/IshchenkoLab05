using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace IshchenkoLab05.Models
{
    internal class MyThreads
    {
        private readonly ProcessThread _thread;

        public int ThreadId => _thread.Id;
        public ThreadState ThreadState => _thread.ThreadState;
        public DateTime ThreadLaunchTime => _thread.StartTime;

        internal MyThreads([NotNull] ProcessThread thread)
        {
            this._thread = thread;
        }
    }
}
