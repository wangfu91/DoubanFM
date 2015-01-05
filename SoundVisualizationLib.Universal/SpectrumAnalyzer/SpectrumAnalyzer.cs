using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Shapes;

namespace SoundVisualizationLib.Universal
{
    [TemplatePart(Name = "PART_SpectrumCanvas", Type = typeof(Canvas))]
    public class SpectrumAnalyzer : Control
    {
        #region Fields
        private readonly DispatcherTimer animationTimer;
        private Canvas spectrumCanvas;
        private ISpectrumPlayer soundPlayer;
        private readonly List<Shape> barShapes = new List<Shape>();
        private readonly List<Shape> peakShapes = new List<Shape>();
        private double[] barHeights;
        private double[] peakHeights;
        private float[] channelData = new float[2048];
        private float[] channelPeakData;
        private double bandWidth = 1.0;
        private double barWidth = 1;
        private int maximumFrequencyIndex = 2047;
        private int minimumFrequencyIndex;
        private int[] barIndexMax;
        private int[] barLogScaleIndexMax;
        #endregion

        #region Constants
        private const int scaleFactorLinear = 9;
        private const int scaleFactorSqr = 2;
        private const double minDBValue = -90;
        private const double maxDBValue = 0;
        private const double dbScale = (maxDBValue - minDBValue);
        private const int defaultUpdateInterval = 25;
        #endregion

        #region Dependency Properties
        #region MaximumFrequency
        /// <summary>
        /// Identifies the <see cref="MaximumFrequency" /> dependency property. 
        /// </summary>
        public static readonly DependencyProperty MaximumFrequencyProperty =
            DependencyProperty.Register("MaximumFrequency",
            typeof(int),
            typeof(SpectrumAnalyzer),
            new PropertyMetadata(20000, OnMaximumFrequencyChanged));


        private static void OnMaximumFrequencyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            SpectrumAnalyzer spectrumAnalyzer = o as SpectrumAnalyzer;
            if (spectrumAnalyzer != null)
                spectrumAnalyzer.OnMaximumFrequencyChanged((int)e.OldValue, (int)e.NewValue);
        }


        /// <summary>
        /// Called after the <see cref="MaximumFrequency"/> value has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of <see cref="MaximumFrequency"/></param>
        /// <param name="newValue">The new value of <see cref="MaximumFrequency"/></param>
        protected virtual void OnMaximumFrequencyChanged(int oldValue, int newValue)
        {
            UpdateBarLayout();
        }

        /// <summary>
        /// Gets or sets the maximum display frequency (right side) for the spectrum analyzer.
        /// </summary>
        /// <remarks>In usual practice, this value should be somewhere between 0 and half of the maximum sample rate. If using
        /// the maximum sample rate, this would be roughly 22000.</remarks>
        public int MaximumFrequency
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get
            {
                return (int)GetValue(MaximumFrequencyProperty);
            }
            set
            {
                SetValue(MaximumFrequencyProperty, value);
            }
        }
        #endregion

        #region Minimum Frequency
        /// <summary>
        /// Identifies the <see cref="MinimumFrequency" /> dependency property. 
        /// </summary>
        public static readonly DependencyProperty MinimumFrequencyProperty =
            DependencyProperty.Register("MinimumFrequency",
            typeof(int),
            typeof(SpectrumAnalyzer),
            new PropertyMetadata(20, OnMinimumFrequencyChanged));


        private static void OnMinimumFrequencyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            SpectrumAnalyzer spectrumAnalyzer = o as SpectrumAnalyzer;
            if (spectrumAnalyzer != null)
                spectrumAnalyzer.OnMinimumFrequencyChanged((int)e.OldValue, (int)e.NewValue);
        }


        /// <summary>
        /// Called after the <see cref="MinimumFrequency"/> value has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of <see cref="MinimumFrequency"/></param>
        /// <param name="newValue">The new value of <see cref="MinimumFrequency"/></param>
        protected virtual void OnMinimumFrequencyChanged(int oldValue, int newValue)
        {
            UpdateBarLayout();
        }

        /// <summary>
        /// Gets or sets the minimum display frequency (left side) for the spectrum analyzer.
        /// </summary>
        public int MinimumFrequency
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get
            {
                return (int)GetValue(MinimumFrequencyProperty);
            }
            set
            {
                SetValue(MinimumFrequencyProperty, value);
            }
        }

        #endregion

        #region BarCount
        /// <summary>
        /// Identifies the <see cref="BarCount" /> dependency property. 
        /// </summary>
        public static readonly DependencyProperty BarCountProperty =
            DependencyProperty.Register("BarCount",
            typeof(int),
            typeof(SpectrumAnalyzer),
            new PropertyMetadata(32, OnBarCountChanged));


        private static void OnBarCountChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            SpectrumAnalyzer spectrumAnalyzer = o as SpectrumAnalyzer;
            if (spectrumAnalyzer != null)
                spectrumAnalyzer.OnBarCountChanged((int)e.OldValue, (int)e.NewValue);
        }


        /// <summary>
        /// Called after the <see cref="BarCount"/> value has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of <see cref="BarCount"/></param>
        /// <param name="newValue">The new value of <see cref="BarCount"/></param>
        protected virtual void OnBarCountChanged(int oldValue, int newValue)
        {
            UpdateBarLayout();
        }

        /// <summary>
        /// Gets or sets the number of bars to show on the sprectrum analyzer.
        /// </summary>
        /// <remarks>A bar's width can be a minimum of 1 pixel. If the BarSpacing and BarCount property result
        /// in the bars being wider than the chart itself, the BarCount will automatically scale down.</remarks>
        public int BarCount
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get
            {
                return (int)GetValue(BarCountProperty);
            }
            set
            {
                SetValue(BarCountProperty, value);
            }
        }
        #endregion

        #region BarSpacing
        /// <summary>
        /// Identifies the <see cref="BarSpacing" /> dependency property. 
        /// </summary>
        public static readonly DependencyProperty BarSpacingProperty =
            DependencyProperty.Register("BarSpacing",
            typeof(double), typeof(SpectrumAnalyzer),
            new PropertyMetadata(5.0d, OnBarSpacingChanged));

        private static void OnBarSpacingChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            SpectrumAnalyzer spectrumAnalyzer = o as SpectrumAnalyzer;
            if (spectrumAnalyzer != null)
                spectrumAnalyzer.OnBarSpacingChanged((double)e.OldValue, (double)e.NewValue);
        }

        /// <summary>
        /// Called after the <see cref="BarSpacing"/> value has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of <see cref="BarSpacing"/></param>
        /// <param name="newValue">The new value of <see cref="BarSpacing"/></param>
        protected virtual void OnBarSpacingChanged(double oldValue, double newValue)
        {
            UpdateBarLayout();
        }

        /// <summary>
        /// Gets or sets the spacing between the bars.
        /// </summary>
        public double BarSpacing
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get
            {
                return (double)GetValue(BarSpacingProperty);
            }
            set
            {
                SetValue(BarSpacingProperty, value);
            }
        }
        #endregion

        #region PeakFallDelay
        /// <summary>
        /// Identifies the <see cref="PeakFallDelay" /> dependency property. 
        /// </summary>
        public static readonly DependencyProperty PeakFallDelayProperty =
            DependencyProperty.Register("PeakFallDelay",
            typeof(int),
            typeof(SpectrumAnalyzer),
            new PropertyMetadata(10, OnPeakFallDelayChanged));

        private static void OnPeakFallDelayChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            SpectrumAnalyzer spectrumAnalyzer = o as SpectrumAnalyzer;
            if (spectrumAnalyzer != null)
                spectrumAnalyzer.OnPeakFallDelayChanged((int)e.OldValue, (int)e.NewValue);
        }

        /// <summary>
        /// Called after the <see cref="PeakFallDelay"/> value has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of <see cref="PeakFallDelay"/></param>
        /// <param name="newValue">The new value of <see cref="PeakFallDelay"/></param>
        protected virtual void OnPeakFallDelayChanged(int oldValue, int newValue)
        {

        }

        /// <summary>
        /// Gets or sets the delay factor for the peaks falling.
        /// </summary>
        /// <remarks>
        /// The delay is relative to the refresh rate of the chart.
        /// </remarks>
        public int PeakFallDelay
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get
            {
                return (int)GetValue(PeakFallDelayProperty);
            }
            set
            {
                SetValue(PeakFallDelayProperty, value);
            }
        }
        #endregion

        #region IsFrequencyScaleLinear
        /// <summary>
        /// Identifies the <see cref="IsFrequencyScaleLinear" /> dependency property. 
        /// </summary>
        public static readonly DependencyProperty IsFrequencyScaleLinearProperty =
            DependencyProperty.Register("IsFrequencyScaleLinear",
            typeof(bool),
            typeof(SpectrumAnalyzer),
            new PropertyMetadata(false, OnIsFrequencyScaleLinearChanged));

        private static void OnIsFrequencyScaleLinearChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            SpectrumAnalyzer spectrumAnalyzer = o as SpectrumAnalyzer;
            if (spectrumAnalyzer != null)
                spectrumAnalyzer.OnIsFrequencyScaleLinearChanged((bool)e.OldValue, (bool)e.NewValue);
        }

        /// <summary>
        /// Called after the <see cref="IsFrequencyScaleLinear"/> value has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of <see cref="IsFrequencyScaleLinear"/></param>
        /// <param name="newValue">The new value of <see cref="IsFrequencyScaleLinear"/></param>
        protected virtual void OnIsFrequencyScaleLinearChanged(bool oldValue, bool newValue)
        {
            UpdateBarLayout();
        }

        /// <summary>
        /// Gets or sets a value indicating whether the bars are layed out on a linear scale horizontally.
        /// </summary>
        /// <remarks>
        /// If true, the bars will represent frequency buckets on a linear scale (making them all
        /// have equal band widths on the frequency scale). Otherwise, the bars will be layed out
        /// on a logrithmic scale, with each bar having a larger bandwidth than the one previous.
        /// </remarks>
        public bool IsFrequencyScaleLinear
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get
            {
                return (bool)GetValue(IsFrequencyScaleLinearProperty);
            }
            set
            {
                SetValue(IsFrequencyScaleLinearProperty, value);
            }
        }
        #endregion

        #region BarHeightScaling
        /// <summary>
        /// Identifies the <see cref="BarHeightScaling" /> dependency property. 
        /// </summary>
        public static readonly DependencyProperty BarHeightScalingProperty =
            DependencyProperty.Register("BarHeightScaling",
            typeof(BarHeightScalingStyles),
            typeof(SpectrumAnalyzer),
            new PropertyMetadata(BarHeightScalingStyles.Decibel, OnBarHeightScalingChanged));

        private static void OnBarHeightScalingChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            SpectrumAnalyzer spectrumAnalyzer = o as SpectrumAnalyzer;
            if (spectrumAnalyzer != null)
                spectrumAnalyzer.OnBarHeightScalingChanged((BarHeightScalingStyles)e.OldValue, (BarHeightScalingStyles)e.NewValue);
        }

        /// <summary>
        /// Called after the <see cref="BarHeightScaling"/> value has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of <see cref="BarHeightScaling"/></param>
        /// <param name="newValue">The new value of <see cref="BarHeightScaling"/></param>
        protected virtual void OnBarHeightScalingChanged(BarHeightScalingStyles oldValue, BarHeightScalingStyles newValue)
        {

        }

        /// <summary>
        /// Gets or sets a value indicating to what scale the bar heights are drawn.
        /// </summary>
        public BarHeightScalingStyles BarHeightScaling
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get
            {
                return (BarHeightScalingStyles)GetValue(BarHeightScalingProperty);
            }
            set
            {
                SetValue(BarHeightScalingProperty, value);
            }
        }
        #endregion

        #region AveragePeaks
        /// <summary>
        /// Identifies the <see cref="AveragePeaks" /> dependency property. 
        /// </summary>
        public static readonly DependencyProperty AveragePeaksProperty =
            DependencyProperty.Register("AveragePeaks",
            typeof(bool),
            typeof(SpectrumAnalyzer),
            new PropertyMetadata(false, OnAveragePeaksChanged));

        private static void OnAveragePeaksChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            SpectrumAnalyzer spectrumAnalyzer = o as SpectrumAnalyzer;
            if (spectrumAnalyzer != null)
                spectrumAnalyzer.OnAveragePeaksChanged((bool)e.OldValue, (bool)e.NewValue);
        }

        /// <summary>
        /// Called after the <see cref="AveragePeaks"/> value has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of <see cref="AveragePeaks"/></param>
        /// <param name="newValue">The new value of <see cref="AveragePeaks"/></param>
        protected virtual void OnAveragePeaksChanged(bool oldValue, bool newValue)
        {

        }

        /// <summary>
        /// Gets or sets a value indicating whether each bar's peak 
        /// value will be averaged with the previous bar's peak.
        /// This creates a smoothing effect on the bars.
        /// </summary>
        public bool AveragePeaks
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get
            {
                return (bool)GetValue(AveragePeaksProperty);
            }
            set
            {
                SetValue(AveragePeaksProperty, value);
            }
        }
        #endregion

        #region BarStyle
        /// <summary>
        /// Identifies the <see cref="BarStyle" /> dependency property. 
        /// </summary>
        public static readonly DependencyProperty BarStyleProperty =
            DependencyProperty.Register("BarStyle",
            typeof(Style),
            typeof(SpectrumAnalyzer),
            new PropertyMetadata(null, OnBarStyleChanged));

        private static void OnBarStyleChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            SpectrumAnalyzer spectrumAnalyzer = o as SpectrumAnalyzer;
            if (spectrumAnalyzer != null)
                spectrumAnalyzer.OnBarStyleChanged((Style)e.OldValue, (Style)e.NewValue);
        }

        /// <summary>
        /// Called after the <see cref="BarStyle"/> value has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of <see cref="BarStyle"/></param>
        /// <param name="newValue">The new value of <see cref="BarStyle"/></param>
        protected virtual void OnBarStyleChanged(Style oldValue, Style newValue)
        {
            UpdateBarLayout();
        }

        /// <summary>
        /// Gets or sets a style with which to draw the bars on the spectrum analyzer.
        /// </summary>
        public Style BarStyle
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get
            {
                return (Style)GetValue(BarStyleProperty);
            }
            set
            {
                SetValue(BarStyleProperty, value);
            }
        }
        #endregion

        #region PeakStyle
        /// <summary>
        /// Identifies the <see cref="PeakStyle" /> dependency property. 
        /// </summary>
        public static readonly DependencyProperty PeakStyleProperty =
            DependencyProperty.Register("PeakStyle",
            typeof(Style),
            typeof(SpectrumAnalyzer),
            new PropertyMetadata(null, OnPeakStyleChanged));

        private static void OnPeakStyleChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            SpectrumAnalyzer spectrumAnalyzer = o as SpectrumAnalyzer;
            if (spectrumAnalyzer != null)
                spectrumAnalyzer.OnPeakStyleChanged((Style)e.OldValue, (Style)e.NewValue);
        }


        /// <summary>
        /// Called after the <see cref="PeakStyle"/> value has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of <see cref="PeakStyle"/></param>
        /// <param name="newValue">The new value of <see cref="PeakStyle"/></param>
        protected virtual void OnPeakStyleChanged(Style oldValue, Style newValue)
        {
            UpdateBarLayout();
        }

        /// <summary>
        /// Gets or sets a style with which to draw the falling peaks on the spectrum analyzer.
        /// </summary>
        public Style PeakStyle
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get
            {
                return (Style)GetValue(PeakStyleProperty);
            }
            set
            {
                SetValue(PeakStyleProperty, value);
            }
        }
        #endregion

        #region ActualBarWidth
        /// <summary>
        /// Identifies the <see cref="ActualBarWidth" /> dependency property. 
        /// </summary>
        public static readonly DependencyProperty ActualBarWidthProperty =
            DependencyProperty.Register("ActualBarWidth",
            typeof(double),
            typeof(SpectrumAnalyzer),
            new PropertyMetadata(0.0d, OnActualBarWidthChanged));

        private static void OnActualBarWidthChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            SpectrumAnalyzer spectrumAnalyzer = o as SpectrumAnalyzer;
            if (spectrumAnalyzer != null)
                spectrumAnalyzer.OnActualBarWidthChanged((double)e.OldValue, (double)e.NewValue);
        }

        /// <summary>
        /// Called after the <see cref="ActualBarWidth"/> value has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of <see cref="ActualBarWidth"/></param>
        /// <param name="newValue">The new value of <see cref="ActualBarWidth"/></param>
        protected virtual void OnActualBarWidthChanged(double oldValue, double newValue)
        {

        }

        /// <summary>
        /// Gets the actual width that the bars will be drawn at.
        /// </summary>
        public double ActualBarWidth
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get
            {
                return (double)GetValue(ActualBarWidthProperty);
            }
            protected set
            {
                SetValue(ActualBarWidthProperty, value);
            }
        }
        #endregion

        #region RefreshRate
        /// <summary>
        /// Identifies the <see cref="RefreshInterval" /> dependency property. 
        /// </summary>
        public static readonly DependencyProperty RefreshIntervalProperty =
            DependencyProperty.Register("RefreshInterval",
            typeof(int),
            typeof(SpectrumAnalyzer),
            new PropertyMetadata(defaultUpdateInterval, OnRefreshIntervalChanged));

        private static void OnRefreshIntervalChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            SpectrumAnalyzer spectrumAnalyzer = o as SpectrumAnalyzer;
            if (spectrumAnalyzer != null)
                spectrumAnalyzer.OnRefreshIntervalChanged((int)e.OldValue, (int)e.NewValue);
        }

        /// <summary>
        /// Called after the <see cref="RefreshInterval"/> value has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of <see cref="RefreshInterval"/></param>
        /// <param name="newValue">The new value of <see cref="RefreshInterval"/></param>
        protected virtual void OnRefreshIntervalChanged(int oldValue, int newValue)
        {
            animationTimer.Interval = TimeSpan.FromMilliseconds(newValue);
        }

        /// <summary>
        /// Gets or sets the refresh interval, in milliseconds, of the Spectrum Analyzer.
        /// </summary>
        /// <remarks>
        /// The valid range of the interval is 10 milliseconds to 1000 milliseconds.
        /// </remarks>
        public int RefreshInterval
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get
            {
                return (int)GetValue(RefreshIntervalProperty);
            }
            set
            {
                SetValue(RefreshIntervalProperty, value);
            }
        }
        #endregion

        #region FFTComplexity
        /// <summary>
        /// Identifies the <see cref="FFTComplexity" /> dependency property. 
        /// </summary>
        public static readonly DependencyProperty FFTComplexityProperty =
            DependencyProperty.Register("FFTComplexity",
            typeof(FFTDataSize),
            typeof(SpectrumAnalyzer),
            new PropertyMetadata(FFTDataSize.FFT2048, OnFFTComplexityChanged));

        private static void OnFFTComplexityChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            SpectrumAnalyzer spectrumAnalyzer = o as SpectrumAnalyzer;
            if (spectrumAnalyzer != null)
                spectrumAnalyzer.OnFFTComplexityChanged((FFTDataSize)e.OldValue, (FFTDataSize)e.NewValue);
        }

        /// <summary>
        /// Called after the <see cref="FFTComplexity"/> value has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of <see cref="FFTComplexity"/></param>
        /// <param name="newValue">The new value of <see cref="FFTComplexity"/></param>
        protected virtual void OnFFTComplexityChanged(FFTDataSize oldValue, FFTDataSize newValue)
        {
            channelData = new float[((int)newValue / 2)];
        }

        /// <summary>
        /// Gets or sets the complexity of FFT results the Spectrum Analyzer expects. Larger values
        /// will be more accurate at converting time domain data to frequency data, but slower.
        /// </summary>
        public FFTDataSize FFTComplexity
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get
            {
                return (FFTDataSize)GetValue(FFTComplexityProperty);
            }
            set
            {
                SetValue(FFTComplexityProperty, value);
            }
        }
        #endregion
        #endregion

        #region Template Overrides
        /// <summary>
        /// When overridden in a derived class, is invoked whenever application code
        /// or internal processes call System.Windows.FrameworkElement.ApplyTemplate().
        /// </summary>
        protected override void OnApplyTemplate()
        {
            spectrumCanvas = GetTemplateChild("PART_SpectrumCanvas") as Canvas;
            if (spectrumCanvas != null)
            {
                spectrumCanvas.SizeChanged += spectrumCanvas_SizeChanged;
                UpdateBarLayout();
            }
        }

        #endregion

        #region Constructors
        static SpectrumAnalyzer()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpectrumAnalyzer"/> class.
        /// </summary>
        public SpectrumAnalyzer()
        {
            animationTimer = new DispatcherTimer()
            {
                Interval = TimeSpan.FromMilliseconds(defaultUpdateInterval),
            };
            animationTimer.Tick += animationTimer_Tick;

            var obj = XamlReader.Load(
                @"  <ResourceDictionary
                        xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"" 
                        xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
                        xmlns:local=""using:SoundVisualizationLib.Universal"">
                    
                        <!-- Region: SpectrumAnalyzer -->
                        <LinearGradientBrush x:Key=""SpectrumBarBrush""
                                             EndPoint=""0.5,0""
                                             StartPoint=""0.5,1"">
                            <GradientStop Color=""#3c993b""
                                          Offset=""0"" />
                            <GradientStop Color=""#43d33a""
                                          Offset=""1"" />
                        </LinearGradientBrush>
                        <SolidColorBrush x:Key=""SpectrumPeakBrush""
                                         Color=""#43d33a"" />
                        <Style TargetType=""local:SpectrumAnalyzer"">
                            <Setter Property=""BarStyle"">
                                <Setter.Value>
                                    <Style TargetType=""Rectangle"">
                                        <Setter Property=""Fill""
                                                Value=""{StaticResource SpectrumBarBrush}"" />
                                    </Style>
                                </Setter.Value>
                            </Setter>
                            <Setter Property=""PeakStyle"">
                                <Setter.Value>
                                    <Style TargetType=""Rectangle"">
                                        <Setter Property=""Fill""
                                                Value=""{StaticResource SpectrumPeakBrush}"" />
                                    </Style>
                                </Setter.Value>
                            </Setter>
                            <Setter Property=""Template"">
                                <Setter.Value>
                                    <ControlTemplate TargetType=""local:SpectrumAnalyzer"" >
                                        <Border Background=""{TemplateBinding Background}""
                                                BorderBrush=""{TemplateBinding BorderBrush}""
                                                BorderThickness=""{TemplateBinding BorderThickness}"">
                                            <Canvas Name=""PART_SpectrumCanvas""
                                                    HorizontalAlignment=""Stretch""
                                                    VerticalAlignment=""Stretch""
                                                    />
                                        </Border>
                                    </ControlTemplate>
                                </Setter.Value>
                            </Setter>
                        </Style>
                        <!-- EndRegion: SpectrumAnalyzer -->
                    
                    </ResourceDictionary>
                    "
                );

            var resourceDictionary = obj as ResourceDictionary;
            var style = resourceDictionary.Values.First() as Style;
            var setter = style.Setters.Last() as Setter;
            var controlTemplate = setter.Value as ControlTemplate;
            this.Template = controlTemplate;
        }

        #endregion

        #region Public Methods
        /// <summary>
        /// Register a sound player from which the spectrum analyzer
        /// can get the necessary playback data.
        /// </summary>
        /// <param name="soundPlayer">A sound player that provides spectrum data through the ISpectrumPlayer interface methods.</param>
        public void RegisterSoundPlayer(ISpectrumPlayer soundPlayer)
        {
            this.soundPlayer = soundPlayer;
            soundPlayer.PropertyChanged += soundPlayer_PropertyChanged;
            UpdateBarLayout();
            animationTimer.Start();
        }
        #endregion

        #region Private Drawing Methods
        private void UpdateSpectrum()
        {
            if (soundPlayer == null || spectrumCanvas == null || spectrumCanvas.RenderSize.Width < 1 || spectrumCanvas.RenderSize.Height < 1)
                return;

            if (soundPlayer.IsPlaying && !soundPlayer.GetFFTData(channelData))
                return;

            UpdateSpectrumShapes();
        }


        private void UpdateSpectrumShapes()
        {
            bool allZero = true;
            double fftBucketHeight = 0f;
            double barHeight = 0f;
            double lastPeakHeight = 0f;
            double peakYPos = 0f;
            double height = spectrumCanvas.RenderSize.Height;
            int barIndex = 0;
            double peakDotHeight = Math.Max(barWidth / 2.0f, 1);
            double barHeightScale = (height - peakDotHeight);

            for (int i = minimumFrequencyIndex; i <= maximumFrequencyIndex; i++)
            {
                // If we're paused, keep drawing, but set the current height to 0 so the peaks fall.
                if (!soundPlayer.IsPlaying)
                {
                    barHeight = 0f;
                }
                else // Draw the maximum value for the bar's band
                {
                    switch (BarHeightScaling)
                    {
                        case BarHeightScalingStyles.Decibel:
                            double dbValue = 20 * Math.Log10((double)channelData[i]);
                            fftBucketHeight = ((dbValue - minDBValue) / dbScale) * barHeightScale;
                            break;
                        case BarHeightScalingStyles.Linear:
                            fftBucketHeight = (channelData[i] * scaleFactorLinear) * barHeightScale;
                            break;
                        case BarHeightScalingStyles.Sqrt:
                            fftBucketHeight = (((Math.Sqrt((double)channelData[i])) * scaleFactorSqr) * barHeightScale);
                            break;
                    }

                    if (barHeight < fftBucketHeight)
                        barHeight = fftBucketHeight;
                    if (barHeight < 0f)
                        barHeight = 0f;
                }

                // If this is the last FFT bucket in the bar's group, draw the bar.
                int currentIndexMax = IsFrequencyScaleLinear ? barIndexMax[barIndex] : barLogScaleIndexMax[barIndex];
                if (i == currentIndexMax)
                {
                    // Peaks can't surpass the height of the control.
                    if (barHeight > height)
                        barHeight = height;

                    if (AveragePeaks && barIndex > 0)
                        barHeight = (lastPeakHeight + barHeight) / 2;

                    peakYPos = barHeight;

                    if (channelPeakData[barIndex] < peakYPos)
                        channelPeakData[barIndex] = (float)peakYPos;
                    else
                        channelPeakData[barIndex] = (float)(peakYPos + (PeakFallDelay * channelPeakData[barIndex])) / ((float)(PeakFallDelay + 1));

                    double xCoord = BarSpacing + (barWidth * barIndex) + (BarSpacing * barIndex) + 1;

                    barShapes[barIndex].Margin = new Thickness(xCoord, (height - 1) - barHeight, 0, 0);
                    barShapes[barIndex].Height = barHeight;
                    peakShapes[barIndex].Margin = new Thickness(xCoord, (height - 1) - channelPeakData[barIndex] - peakDotHeight, 0, 0);
                    peakShapes[barIndex].Height = peakDotHeight;

                    if (channelPeakData[barIndex] > 0.05)
                        allZero = false;

                    lastPeakHeight = barHeight;
                    barHeight = 0f;
                    barIndex++;
                }
            }

            if (allZero && !soundPlayer.IsPlaying)
                animationTimer.Stop();
        }

        private void UpdateBarLayout()
        {
            if (soundPlayer == null || spectrumCanvas == null)
                return;

            barWidth = Math.Max(((double)(spectrumCanvas.RenderSize.Width - (BarSpacing * (BarCount + 1))) / (double)BarCount), 1);
            maximumFrequencyIndex = Math.Min(soundPlayer.GetFFTFrequencyIndex(MaximumFrequency) + 1, 2047);
            minimumFrequencyIndex = Math.Min(soundPlayer.GetFFTFrequencyIndex(MinimumFrequency), 2047);
            bandWidth = Math.Max(((double)(maximumFrequencyIndex - minimumFrequencyIndex)) / spectrumCanvas.RenderSize.Width, 1.0);

            int actualBarCount;
            if (barWidth >= 1.0d)
                actualBarCount = BarCount;
            else
                actualBarCount = Math.Max((int)((spectrumCanvas.RenderSize.Width - BarSpacing) / (barWidth + BarSpacing)), 1);
            channelPeakData = new float[actualBarCount];

            int indexCount = maximumFrequencyIndex - minimumFrequencyIndex;
            int linearIndexBucketSize = (int)Math.Round((double)indexCount / (double)actualBarCount, 0);
            List<int> maxIndexList = new List<int>();
            List<int> maxLogScaleIndexList = new List<int>();
            double maxLog = Math.Log(actualBarCount, actualBarCount);
            for (int i = 1; i < actualBarCount; i++)
            {
                maxIndexList.Add(minimumFrequencyIndex + (i * linearIndexBucketSize));
                int logIndex = (int)((maxLog - Math.Log((actualBarCount + 1) - i, (actualBarCount + 1))) * indexCount) + minimumFrequencyIndex;
                maxLogScaleIndexList.Add(logIndex);
            }
            maxIndexList.Add(maximumFrequencyIndex);
            maxLogScaleIndexList.Add(maximumFrequencyIndex);
            barIndexMax = maxIndexList.ToArray();
            barLogScaleIndexMax = maxLogScaleIndexList.ToArray();

            barHeights = new double[actualBarCount];
            peakHeights = new double[actualBarCount];

            spectrumCanvas.Children.Clear();
            barShapes.Clear();
            peakShapes.Clear();

            double height = spectrumCanvas.RenderSize.Height;
            double peakDotHeight = Math.Max(barWidth / 2.0f, 1);
            for (int i = 0; i < actualBarCount; i++)
            {
                double xCoord = BarSpacing + (barWidth * i) + (BarSpacing * i) + 1;
                Rectangle barRectangle = new Rectangle()
                {
                    Margin = new Thickness(xCoord, height, 0, 0),
                    Width = barWidth,
                    Height = 0,
                    Style = BarStyle
                };
                barShapes.Add(barRectangle);
                Rectangle peakRectangle = new Rectangle()
                {
                    Margin = new Thickness(xCoord, height - peakDotHeight, 0, 0),
                    Width = barWidth,
                    Height = peakDotHeight,
                    Style = PeakStyle
                };
                peakShapes.Add(peakRectangle);
            }

            foreach (Shape shape in barShapes)
                spectrumCanvas.Children.Add(shape);
            foreach (Shape shape in peakShapes)
                spectrumCanvas.Children.Add(shape);

            ActualBarWidth = barWidth;
        }
        #endregion

        #region Event Handlers
        private void soundPlayer_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "IsPlaying":
                    if (soundPlayer.IsPlaying && !animationTimer.IsEnabled)
                        animationTimer.Start();
                    break;
            }
        }

        private void animationTimer_Tick(object sender, object e)
        {
            UpdateSpectrum();
        }

        private void spectrumCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateBarLayout();
        }
        #endregion

    }

}
