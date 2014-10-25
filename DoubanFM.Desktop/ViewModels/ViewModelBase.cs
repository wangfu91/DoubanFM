using Microsoft.Practices.Prism.Mvvm;
using System;

namespace DoubanFM.Desktop.ViewModels
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
