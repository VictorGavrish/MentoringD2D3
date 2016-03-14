namespace GameOfLife
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Threading;

    public partial class MainWindow : Window
    {
        private readonly Grid mainGrid;

        private readonly DispatcherTimer timer; // Generation timer

        private int genCounter;

        private readonly AdWindow[] adWindows;

        public MainWindow()
        {
            this.InitializeComponent();
            this.mainGrid = new Grid(this.MainCanvas);

            this.timer = new DispatcherTimer();
            this.timer.Tick += this.OnTimer;
            this.timer.Interval = TimeSpan.FromMilliseconds(200);
            this.adWindows = new AdWindow[2];
        }

        private void StartAd()
        {
            for (var i = 0; i < 2; i++)
            {
                if (this.adWindows[i] == null)
                {
                    this.adWindows[i] = new AdWindow(this);
                    this.adWindows[i].Closed += this.AdWindowOnClosed;
                    this.adWindows[i].Top = this.Top + 330 * i + 70;
                    this.adWindows[i].Left = this.Left + 240;
                    this.adWindows[i].Show();
                }
            }
        }

        private void AdWindowOnClosed(object sender, EventArgs eventArgs)
        {
            // clean-up: unsubscribe to event handlers and remove from array so the object can be GCed
            var adWindow = (AdWindow)sender;
            adWindow.Closed -= this.AdWindowOnClosed;
            var index = Array.IndexOf(this.adWindows, adWindow);
            this.adWindows[index] = null;
        }

        private void ButtonStartStop_OnClick(object sender, EventArgs e)
        {
            if (!this.timer.IsEnabled)
            {
                this.timer.Start();
                this.ButtonStart.Content = "Stop";
                this.StartAd();
            }
            else
            {
                this.timer.Stop();
                this.ButtonStart.Content = "Start";
            }
        }

        private void OnTimer(object sender, EventArgs e)
        {
            this.mainGrid.Update();
            this.genCounter++;
            this.LblGenCount.Content = "Generations: " + this.genCounter;
        }

        private void ButtonClear_OnClick(object sender, RoutedEventArgs e)
        {
            this.mainGrid.Clear();
        }
    }
}