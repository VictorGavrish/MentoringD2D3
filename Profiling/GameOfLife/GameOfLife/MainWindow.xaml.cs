namespace GameOfLife
{
    using System;
    using System.Windows;
    using System.Windows.Threading;

    public partial class MainWindow : Window
    {
        private readonly Grid mainGrid;

        private readonly DispatcherTimer timer; // Generation timer

        private int genCounter;

        private AdWindow[] adWindow;

        public MainWindow()
        {
            this.InitializeComponent();
            this.mainGrid = new Grid(this.MainCanvas);

            this.timer = new DispatcherTimer();
            this.timer.Tick += this.OnTimer;
            this.timer.Interval = TimeSpan.FromMilliseconds(200);
        }

        private void StartAd()
        {
            {
                this.adWindow = new AdWindow[2];
                for (var i = 0; i < 2; i++)
                {
                    if (this.adWindow[i] == null)
                    {
                        this.adWindow[i] = new AdWindow(this);
                        this.adWindow[i].Closed += this.AdWindowOnClosed;
                        this.adWindow[i].Top = this.Top + 330 * i + 70;
                        this.adWindow[i].Left = this.Left + 240;
                        this.adWindow[i].Show();
                    }
                }
            }
        }

        private void AdWindowOnClosed(object sender, EventArgs eventArgs)
        {
            for (var i = 0; i < 2; i++)
            {
                this.adWindow[i].Closed -= this.AdWindowOnClosed;
                this.adWindow[i] = null;
            }
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