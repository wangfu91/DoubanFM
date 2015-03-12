using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DoubanFM.Desktop.DwmHelper;

namespace DoubanFM.Desktop.Resource.Controls
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:DoubanFM.Desktop.Resource"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:DoubanFM.Desktop.Resource;assembly=DoubanFM.Desktop.Resource"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:WindowBase/>
    ///
    /// </summary>
    public class WindowBase : Window
    {
        static WindowBase()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WindowBase), new FrameworkPropertyMetadata(typeof(WindowBase)));
        }

        public WindowBase()
        {
            this.ContentRendered += delegate
            {
                //添加窗口阴影
                ShadowWindow.Attach(this);
            };

            if (DWM.IsDwmSupported)
            {
                dwmHelper = new DWM(this);
                dwmHelper.AeroGlassEffectChanged += DwmHelper_AeroGlassEffectChanged;
            }
        }

        protected DWM dwmHelper;

        void DwmHelper_AeroGlassEffectChanged(object sender, EventArgs e)
        {
            if (DWM.IsAeroGlassEffectEnabled)
            {
                dwmHelper.EnableBlurBehindWindow();
            }
            else
            {
                dwmHelper.EnableBlurBehindWindow(false);
            }
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            if (dwmHelper != null && DWM.IsAeroGlassEffectEnabled)
            {
                dwmHelper.EnableBlurBehindWindow();
            }
        }

        /// <summary>
        /// 当前是否正在拖拽窗口
        /// </summary>
        protected bool IsDraging = false;

        /// <summary>
        /// 是否按下了鼠标左键（不要用Mouse.LeftButton去检测）
        /// </summary>
        private bool pressed = false;

        /// <summary>
        /// 鼠标相对于窗口的位置
        /// </summary>
        private Point? mousePosition;

        protected override void OnMouseLeftButtonDown(System.Windows.Input.MouseButtonEventArgs e)
        {
            pressed = true;
            base.OnMouseLeftButtonDown(e);
        }

        protected override void OnMouseRightButtonUp(System.Windows.Input.MouseButtonEventArgs e)
        {
            pressed = false;
            base.OnMouseRightButtonUp(e);
        }

        protected override void OnMouseEnter(System.Windows.Input.MouseEventArgs e)
        {
            mousePosition = e.GetPosition(this);
        }

        protected override void OnMouseLeave(System.Windows.Input.MouseEventArgs e)
        {
            pressed = false;
            mousePosition = null;
            base.OnMouseLeave(e);
        }

        /// <summary>
        /// 支持拖拽窗口
        /// </summary>
        protected override void OnMouseMove(System.Windows.Input.MouseEventArgs e)
        {
            base.OnMouseMove(e);

            //当鼠标没动，但有控件在鼠标下方移动时，仍会触发MouseMove事件，所以要根据鼠标位置来判断是否真的移动了。
            //只有鼠标真的移动了，才可能触发窗口拖动。
            var newPosition = e.GetPosition(this);
            bool moved = mousePosition.HasValue && newPosition != mousePosition.Value;
            mousePosition = newPosition;

            if (!IsDraging && pressed && System.Windows.Input.Mouse.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
            {
                if (moved)
                {
                    IsDraging = true;
                    DragMove();
                    pressed = false;
                    IsDraging = false;
                }
            }
        }

    }
}
