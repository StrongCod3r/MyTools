using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using WpfAppBar;


namespace MyTools.Widgets
{
    public abstract class WidgetBase : Border, IDisposable
    {
        private bool disposed;

        public AppBarWindow MainToolbar { get; internal set; }
        public new string Name { get; set; }
        public bool IsDisposed
        {
            get
            {
                return this.disposed;
            }
        }
        public new bool IsInitialized { get; }


        internal WidgetBase BaseInitialize(AppBarWindow mainToolbar)
        {
            if (disposed)
                throw new ObjectDisposedException(Name);

            this.MainToolbar = mainToolbar ?? throw new ArgumentNullException(nameof(mainToolbar));

            this.Initialize();
            return this;
        }

        protected abstract void Initialize();

        public void Dispose()
        {
            this.disposed = true;
        }
    }
}
