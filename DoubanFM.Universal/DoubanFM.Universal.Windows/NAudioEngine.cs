using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoubanFM.Universal
{
    public class NAudioEngine:BindableBase
    {
        private static NAudioEngine instance;
        private bool isPlaying;

        public bool IsPlaying
        {
            get { return isPlaying; }
            set
            {
                if(value!=isPlaying)
                {
                    isPlaying = value;
                    OnPropertyChanged(() => this.IsPlaying);
                }
            }
        }
    }
}
