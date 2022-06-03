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

namespace MatchGame
{
    /// Interaction logic for MainWindow.xaml

    using System.Windows.Threading;

    public partial class MainWindow : Window
    {
        DispatcherTimer timer = new DispatcherTimer();
        int tenthsOfSecondsElapsed;
        int matchesFound;

        public MainWindow()
        {
            InitializeComponent();
            
            timer.Interval = TimeSpan.FromSeconds(.1);
            timer.Tick += Timer_Tick;
            
            SetUpGame();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            tenthsOfSecondsElapsed++;
            timeTextBlock.Text = (tenthsOfSecondsElapsed / 10F).ToString("0.0s");

            if(matchesFound == 8)
            {
                timer.Stop();
                timeTextBlock.Text = timeTextBlock.Text + " - Play again?";
            }
        }

        private void SetUpGame() //happens while game starts
        {
            List<string> animalEmoji = new List<string>() //list of available animals as emojis
            {
               "🐙","🐙","🐟","🐟",
               "🐅","🐅","🐈","🐈",
               "🦎","🦎","🐍","🐍",
               "🦢","🦢","🦔","🦔"
            };

            Random random = new Random();

            foreach (TextBlock textBlock in mainGrid.Children.OfType<TextBlock>())
            {
                if(textBlock.Name != "timeTextBlock")
                {
                    int index = random.Next(animalEmoji.Count);
                    string nextEmoji = animalEmoji[index];
                    textBlock.Text = nextEmoji;
                    animalEmoji.RemoveAt(index);
                }
            }
            timer.Start();
            tenthsOfSecondsElapsed = 0;
            matchesFound = 0;
        }

        TextBlock lastTextBlockClicked;
        bool findingMatch = false;
        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e) //click on any animal
        {
            TextBlock textBlock = sender as TextBlock;
            if(findingMatch == false)                               //choosing first animal
            {
                //textBlock.Visibility = Visibility.Hidden;
                textBlock.Background = Brushes.LightGray;
                lastTextBlockClicked = textBlock;
                findingMatch = true;
            }
            else if(textBlock.Text == lastTextBlockClicked.Text)    // match
            {
                textBlock.Visibility = Visibility.Hidden;
                lastTextBlockClicked.Visibility = Visibility.Hidden;
                findingMatch = false;
                matchesFound++;
            }
            else                                                    // not a match
            {
                lastTextBlockClicked.Visibility = Visibility.Visible; 
                findingMatch = false;
            }
        }

        private void TimeTextBlock_MouseDown(object sender, MouseButtonEventArgs e) //
        {
            if(matchesFound == 8)
            {
                SetUpGame(); //### - doesn't reload after new game
            }
        }
    }
}
