using Prism.Mvvm;
using System;

namespace DoubanFM.Desktop.Infrastructure
{
    public abstract class ViewModelBase : BindableBase, IDisposable
    {
        private bool disposed;

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (this.disposed)
            {
                return;
            }

            if (disposing)
            {
                //TODO: release some common object, eg. logger.
            }

            this.disposed = true;
        }
    }


}
