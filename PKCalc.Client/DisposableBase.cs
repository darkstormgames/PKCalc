namespace PKCalc.Client
{
    public abstract class DisposableBase : IDisposable
    {
        protected bool _disposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    Cleanup();
                }
                _disposed = true;
            }
        }

        /// <summary>
        /// Dispose all members in this function
        /// </summary>
        protected abstract void Cleanup();
    }
}
