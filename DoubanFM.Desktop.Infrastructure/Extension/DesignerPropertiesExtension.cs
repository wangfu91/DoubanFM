using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace DoubanFM.Desktop.Infrastructure.Extension
{
	//Great question here: http://stackoverflow.com/questions/9076419/design-time-only-background-color-in-wpf
	//			 and here: http://stackoverflow.com/questions/5183801/black-background-for-xaml-editor
	//Great post here: http://mnajder.blogspot.com/2011/09/custom-design-time-attributes-in.html


	/// <summary>
	/// Add design-time support in designer for background property.
	/// </summary>
	public static class d
	{
		//static bool? isInDesignMode;

		public static bool IsInDesignMode
		{
			get
			{
				return (bool)DesignerProperties.IsInDesignModeProperty
							.GetMetadata(typeof(DependencyObject)).DefaultValue;
			}
		}


		// Using a DependencyProperty as the backing store for Background.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty BackgroundProperty =
			DependencyProperty.RegisterAttached("Background", typeof(Brush), typeof(d), new PropertyMetadata(new PropertyChangedCallback(BackgroundChanged)));

		public static System.Windows.Media.Brush GetBackground(DependencyObject dependencyObject)
		{
			return (System.Windows.Media.Brush)dependencyObject.GetValue(BackgroundProperty);
		}
		public static void SetBackground(DependencyObject dependencyObject, System.Windows.Media.Brush value)
		{
			dependencyObject.SetValue(BackgroundProperty, value);
		}

		private static void BackgroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (!IsInDesignMode)
				return;

			d.GetType().GetProperty("Background").SetValue(d, e.NewValue, null);
		}
	}
}
