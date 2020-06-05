using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;

namespace MyCoronas
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer Timer = new DispatcherTimer();
        const int PersonMax = 100_000;

        Person[] Personen = new Person[PersonMax];

        Polyline MyPolylineGenesen = new Polyline();
        Polyline MyPolylineInfiziert = new Polyline();


        DateTime Day = DateTime.Now;
        DateTime DayStart;


        public MainWindow()
        {
            InitializeComponent();

            for (int i = 0; i < PersonMax; i++)
            {
                Personen[i] = new Person();

            }

            for (int i = 0; i < 100; i++)
            {
                Personen[i].Infizieren(DateTime.Now);

            }

            DayStart = Day;

          
            MyPolylineGenesen.Points.Add(new Point(0, YPointForPersons(0)));
            MyPolylineGenesen.Stroke = Brushes.Black;

        
            MyPolylineInfiziert.Points.Add(new Point(0, YPointForPersons(0)));
            MyPolylineInfiziert.Stroke = Brushes.Red;


            Timer.Interval = TimeSpan.FromMilliseconds(10);
            Timer.Tick += Timer_Tick;
            

        }

        int InfizierteAlt = 0;
        int GeneseneAlt = 0;

        private void Timer_Tick(object sender, EventArgs e)
        {

        

            double daySinceStart = (Day - DayStart).TotalDays;
            int iDaySinceStart = (int)daySinceStart;

            int infizierte;
            int neuInfizierte;
            int genesene;
            int neuGenesene;

            (infizierte,genesene) = GoMeetings(Day);

            neuInfizierte = infizierte - InfizierteAlt;
            InfizierteAlt = infizierte;
            neuGenesene = genesene - GeneseneAlt;
            GeneseneAlt = genesene;

            MyPolylineGenesen.Points.Add(new Point(iDaySinceStart*10 , YPointForPersons(genesene)));

            MyPolylineInfiziert.Points.Add(new Point(iDaySinceStart*10, YPointForPersons(infizierte)));


            Day = Day + TimeSpan.FromDays(1);


            if (MyPolylineGenesen.Points.Last().X >= MyCanvas.ActualWidth)
            {
      
                Timer.Stop();
            }
                
            if (MyPolylineGenesen.Points.Last().Y <= 0.0)
            {
           
                Timer.Stop();
            }
           

        }

        private (int infizierte, int genesene)  GoMeetings(DateTime dt)
        {
            int infiziertCount = 0;
            int genesenCount = 0;

            for (int i = 0; i < PersonMax; i++)
            {
                // jede Person trifft 10 andere Personen
                for (int k = 0; k < 10; k++)
                {
                    int pMeet = Person.Rnd.Next(PersonMax);
                    if (!Personen[pMeet].Genesen(Day)&& !Personen[pMeet].Infiziert && Personen[i].Infiziert )
                    {
                        // Infizieren mit Wahrscheinlichkeit 2%
                        int rnd = Person.Rnd.Next(100);
                        if (rnd<10)
                        {
                            Personen[pMeet].Infizieren(dt);
                         
                        }                    
                    }
                }
                if (Personen[i].Genesen(dt))
                {
                    genesenCount++;
                }
                if (Personen[i].Infiziert)
                {
                    infiziertCount++;
                }
            }
            return (infiziertCount, genesenCount);
        }

        private void ButtonStart_Click(object sender, RoutedEventArgs e)
        {
            MyCanvas.Children.Add(MyPolylineGenesen);
            MyCanvas.Children.Add(MyPolylineInfiziert);

            Timer.Start();
        }

        double YPointForPersons(int num)
        {
            // High/100000 = x/1
            // x =High/100000

            double Y0 = MyCanvas.ActualHeight / PersonMax;

            return MyCanvas.ActualHeight - (num * Y0);

        }

        private void ButtonStop_Click(object sender, RoutedEventArgs e)
        {
            Timer.Stop();
        }
    }
}
