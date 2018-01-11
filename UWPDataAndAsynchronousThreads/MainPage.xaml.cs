using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UWPDataAndAsynchronousThreads
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void btn1_Click(object sender, RoutedEventArgs e)
        {
            var primeList = new List<int>();

            int potentialPrimeNum = 0;
            int maxNumTested = 500000000;

            // **** Demo a new thread completely separate from the UI thread ***
            //await Task.Run(() =>
            //{
            //    for (int counter = 0; counter < maxNumTested; ++counter)
            //    {
            //        if (IsPrime(potentialPrimeNum))
            //        {
            //            primeList.Add(potentialPrimeNum);
            //        }
            //    }
            //});

            //result.Text = $"All primes up to {maxNumTested} found!"; // must update UI on the UI thread, not the thread we just created.

            // *** Demo accessing the UI thread from the new thread ***
            await Task.Run( async () =>
            {
                for (int counter = 0; counter < maxNumTested; ++counter)
                {
                    if (IsPrime(potentialPrimeNum))
                    {
                        primeList.Add(potentialPrimeNum);
                    }
                }

                // how to access the UI thread 
                await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => {
                    result.Text = $"All primes up to {maxNumTested} found!"; // must update UI on the UI thread, not the thread we just created.
                });
            });            
        } // End btn1_Click()

        private bool IsPrime(int x)
        {
            if (x <= 0)
            {
                return false;
            }

            if (x <= 3) // We've already taken care of cases <= 0
            {
                return true;
            }

            if (x % 2 == 0 || x % 3 == 0) // if x is divisible by 2 or 3
            {
                return false;
            }

            int i = 5;
            while (i * i < x)
            {
                // if x is divisible by 5 or 7.
                // in subsequent iterations, if x is divisible by 5 or 7 plus a multiple of 6
                if (x % i == 0 || x % (i + 2) == 0) 
                {
                    return false;
                }

                i += 6;
            }

            return true; // if code reaches this point, x is prime
        } // End IsPrime()
    } // End parital class MainPage
} // End namespace
