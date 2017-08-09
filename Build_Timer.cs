using System;
using System.Diagnostics;
using System.Speech.Synthesis;
using System.Windows.Forms;

namespace Notify_Me
{
    class Build_Timer
    {
        public Timer timer = new Timer();
        public string mySpeech;
        public string myTitle;

        private int myHours; //These can only be changed when timer is created
        private bool[] myDays = new bool[7];
        private bool once;
         
        public Build_Timer(int Interval, bool Once = true, int MyHours = 0, bool[] MyDays = null, string MySpeech = "Hello",string MyTitle = "Default")
        {
            mySpeech = MySpeech;                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    
            myTitle = MyTitle;
            myHours = MyHours;
            myDays = MyDays;
            once = Once;

            timer.Tick += new EventHandler(Timer_Tick);
            if(Interval<=0)
            {
                Interval = SetNextInterval();
            }
            ConfirmInterval(true, Interval);

        } //Takes date timer first goes off and initializes timer

        public static int MilliSecondsLeftTilTheHour()
        {
            int interval;
            int minutesRemaining = 59 - DateTime.Now.Minute;
            int secondsRemaining = 59 - DateTime.Now.Second;
            interval = ((minutesRemaining * 60) + secondsRemaining) * 1000;
            if (interval == 0) //Next Hour when hour reached
            {
                interval = 60 * 60 * 1000;
            }
            return interval;
        } //returns an integer in miliseconds left until the next hour

        public void ConfirmInterval(bool On = false, int interval = 0)
        {

            Debug.WriteLine("The set interval is " + interval);

            if (!On||interval==0) // default
            {
                timer.Enabled = false;
                timer.Dispose();
            }

            else
            {
                timer.Enabled = true;
                timer.Interval = interval*1000;
            }

        } // Sets up or disables a timer to occur at regular interval in seconds 

        private void Timer_Tick(object sender, EventArgs e)
        {

            Debug.WriteLine("Timer Ticked");
            SpeakNow(mySpeech);

            //
            // TODO: Create notification
            //

            ConfirmInterval(!once, SetNextInterval());

        } //Creates notification and resets timer TODO

        public int SetNextInterval()
        {
            Debug.WriteLine("OoOoOoOoO New instance of setnextinterval begins");
            int Index = (int)DateTime.Now.DayOfWeek;
            Debug.WriteLine("Doy of week" + Index.ToString());
            int DaysToWait = Index;
            while (!myDays[Index])
            {
                Index++;

            }
            DaysToWait = Index - DaysToWait;
            Debug.WriteLine("Day to wait" + DaysToWait.ToString());
            
            int HoursToWait = myHours - DateTime.Now.Hour;
            if (HoursToWait < 0)
            {
                HoursToWait += 24;
            }
            else if (HoursToWait > 0)
            {
                HoursToWait--;
            }
            Debug.WriteLine("hour to wait" + HoursToWait.ToString());

            int min = 59 - DateTime.Now.Minute;
            int sec = 59 - DateTime.Now.Second;
            TimeSpan Span = new TimeSpan(DaysToWait, HoursToWait, min, sec);
            int newInterval = (int)Span.TotalSeconds;
            Debug.WriteLine("span" + Span.ToString());

            return newInterval;

        } //Determines the next interval up to the day of the week only

        private void SpeakNow(string String)
        {
            SpeechSynthesizer synthesizer = new SpeechSynthesizer();
            synthesizer.Volume = 100;  // 0...100
            synthesizer.SelectVoiceByHints(VoiceGender.Female);
            synthesizer.Rate = -2;     // -10...10
            synthesizer.SpeakAsync(String);

        } //Synthesizes string into audio
    }
}
