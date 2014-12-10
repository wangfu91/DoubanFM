using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoubanFM.Common
{
    public abstract class ViewModelBase : BindableBase, IDisposable
    {
        private bool disposed;

        protected ViewModelBase()
        {

        }
        public void Dispose()
        {
            throw new NotImplementedException();
        }


        private void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    //
                }
                disposed = true;
            }
        }
    }

}
