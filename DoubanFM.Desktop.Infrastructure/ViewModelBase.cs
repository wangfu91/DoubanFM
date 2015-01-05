using Microsoft.Practices.Prism.Mvvm;
using System;

namespace DoubanFM.Desktop.Infrastructure
{
    public abstract class ViewModelBase : BindableBase, IDisposable
    {
        private bool disposed;

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
                    //TODO:Implement Dispose method.
                }
                disposed = true;
            }
        }
    }


}
