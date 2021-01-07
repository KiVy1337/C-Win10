using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfMVVM.Models;

namespace WpfMVVM.ViewModel {
	public class VMContainer {
		private static readonly object CreationLock = new object();
        public AddViewModel AddVM;
        public UpdateViewModel UpdateVM;
        public ExportViewModel ExportVM;

        private static VMContainer _instance;

        /// Gets the single instance of the Container.
        public static VMContainer GetContainer() {
            if (_instance == null) {
                lock (CreationLock) {
                    if (_instance == null) {
                        _instance = new VMContainer();
                    }
                }
            }
            return _instance;
        }

        /// Initializes a new instance of the Container class.
        private VMContainer() {
            AddVM = new AddViewModel();
            UpdateVM = new UpdateViewModel();
            ExportVM = new ExportViewModel();
        }
    }
}
