using System;
using System.Collections;
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
using System.Timers;

namespace GnomeSort
{

    public class NumDrawElement
    {
       public Grid Grid { get; set; }
       public double Num { get; set; }
        public NumDrawElement(double Num, double size)
        {
            this.Num = Num;
            Grid = new Grid() { Background = Brushes.White, Margin = new Thickness(1), Height = ((450D / 255D) * Num), Width = (450 / size + 1), VerticalAlignment = VerticalAlignment.Bottom };
            Border border = new Border() { BorderBrush = Brushes.Black, BorderThickness = new Thickness(1) };
            Grid.Children.Add(border);
        }
    }

    public class GnomeAlgo
    {
        static int size = 15;
        public List<NumDrawElement> Numbers { get; private set; }
        Random ran;
        StackPanel SPanel;
        int i = 1;

        public GnomeAlgo(StackPanel SPanel)
        {
            Numbers = new List<NumDrawElement>();
            ran = new Random();
            this.SPanel = SPanel;
        }

        public void Generate()
        {
            for(int i = 0; i < (int)size; i++)   Numbers.Add(new NumDrawElement((byte)ran.Next(255), size));
        }

        public void Visualize()
        {
            for (int i = 0; i < Numbers.Count; i++)   SPanel.Children.Add(Numbers[i].Grid);
        }
        static Grid lastGrid;
        public  void NextIterationForGnome()
        {
            SPanel.Dispatcher.Invoke(() =>
            {
                while (i < Numbers.Count)
                {
                    if (i == 0 || Numbers[i - 1].Grid.Height <= Numbers[i].Grid.Height) i++;
                    else
                    {
                        double temp = Numbers[i].Grid.Height;
                        Numbers[i].Grid.Height = Numbers[i - 1].Grid.Height;
                        Numbers[i - 1].Grid.Height = temp;

                        if (lastGrid != null)
                        {
                            lastGrid.Background = Brushes.White;
                        }
                        Numbers[i - 1].Grid.Background = Brushes.Violet;
                        lastGrid = Numbers[i - 1].Grid;
                        i--;
                    }
                    break;
                }
            });
        }
    }

    public class GnomeDraw
    {
        GnomeAlgo Gnome;
        Timer timer;
        StackPanel SPanel;
        public GnomeDraw(StackPanel SPanel)
        {
            this.SPanel = SPanel;
            Gnome = new GnomeAlgo(SPanel);
            timer = new Timer(100);
            timer.Elapsed += Timer_Elapsed;
        }
        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Gnome.NextIterationForGnome();
            SPanel.Dispatcher.Invoke(() => SPanel.UpdateLayout());
        }
        public void Run()
        {
            Gnome.Generate();
            Gnome.Visualize();
            timer.Start();
        }
    }
}
