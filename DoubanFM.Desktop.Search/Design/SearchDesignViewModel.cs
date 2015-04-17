using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoubanFM.Desktop.Search.Design
{
    internal class SearchDesignViewModel
    {
        public SearchDesignViewModel()
        {
            if (Infrastructure.Extension.d.IsInDesignMode)
                LoadDesignTimeData();
            else
                return;
        }

        public bool IsSearching { get; set; }

        private void LoadDesignTimeData()
        {
            this.IsSearching = true;
        }
    }
}
