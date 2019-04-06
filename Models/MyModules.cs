using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace IshchenkoLab05.Models
{
    internal class MyModules
    {
        private readonly ProcessModule _module;

        public string ModuleName => _module.ModuleName;
        public string ModulePath => _module.FileName;

        internal MyModules([NotNull] ProcessModule module)
        {
            this._module = module;
        }
    }
}
