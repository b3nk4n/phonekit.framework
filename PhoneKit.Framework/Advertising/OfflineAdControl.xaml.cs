using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace PhoneKit.Framework.Advertising
{
    public partial class OfflineAdControl : UserControl
    {
        /// <summary>
        /// The number of adverts.
        /// </summary>
        private int _advertsCount = 0;

        /// <summary>
        /// The index of the currently active image.
        /// </summary>
        private int _currentActiveImageIndex = -1;

        /// <summary>
        /// The timer to do adverts switching.
        /// </summary>
        private DispatcherTimer _timer;

        /// <summary>
        /// Creates an OfflineAdControl.
        /// </summary>
        public OfflineAdControl()
        {
            InitializeComponent();

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(10);
            _timer.Tick += _timer_Tick;

            // the tap/click event
            this.Tap += (s, e) => 
                {
                    var currentImage = (FrameworkElement)LayoutRoot.Children[_currentActiveImageIndex];
                    var currentAdvert = (AdvertData)currentImage.Tag;
                    currentAdvert.ExecuteAction();
                };
        }

        /// <summary>
        /// The timer tick event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        void _timer_Tick(object sender, EventArgs e)
        {
            SwitchToNextAdvert();
        }

        /// <summary>
        /// Adds an advert to the control.
        /// </summary>
        /// <param name="advert">The adverts data.</param>
        public void AddAdvert(AdvertData advert)
        {
            Image image = new Image();
            image.Name = "Banner" + _advertsCount;
            image.Source = new BitmapImage(advert.ImageUri);
            image.Stretch = System.Windows.Media.Stretch.Fill;
            image.Visibility = System.Windows.Visibility.Collapsed;
            image.Tag = advert;
            LayoutRoot.Children.Add(image);
            _advertsCount++;
        }

        /// <summary>
        /// Starts to show and rotate the adverts.
        /// </summary>
        public void Start()
        {
            if (_advertsCount == 0)
                return;

            SwitchToNextAdvert();

            if (_advertsCount > 1)
                _timer.Start();
        }

        /// <summary>
        /// Stops the adverts rotation.
        /// </summary>
        public void Stop()
        {
            if (_timer.IsEnabled)
            {
                _timer.Stop();
            }
        }

        /// <summary>
        /// Switches to the next advert
        /// </summary>
        private void SwitchToNextAdvert()
        {
            // deactivate current
            if (_currentActiveImageIndex != -1)
                LayoutRoot.Children[_currentActiveImageIndex].Visibility = System.Windows.Visibility.Collapsed;

            // select next
            _currentActiveImageIndex++;
            if (_currentActiveImageIndex == _advertsCount)
                _currentActiveImageIndex = 0;
            LayoutRoot.Children[_currentActiveImageIndex].Visibility = System.Windows.Visibility.Visible;
        }

        /// <summary>
        /// Gets or sets the switching interval.
        /// </summary>
        public TimeSpan Interval
        {
            get
            {
                return _timer.Interval;
            }
            set
            {
                _timer.Interval = value;
            }
        }

        /// <summary>
        /// Gets the number of adverts.
        /// </summary>
        public int AdvertsCount
        {
            get
            {
                return _advertsCount;
            }
        }
    }
}
