using System.Diagnostics;
using System.Windows;
using System.Speech.Synthesis;
using System.Windows.Forms;
using System;
using System.Windows.Controls;
using System.Collections.Generic;

namespace Notify_Me
{
    public partial class MainWindow : Window
    {
        Build_Tray Tray = new Build_Tray(); //Global tray
        static EventLog log = new EventLog("System"); //change of time log
        List<Build_Timer> myTimerList = new List<Build_Timer>(); //need to figure out how to remove timer from list when disposed
        

        public MainWindow()
        {
            InitializeComponent();
            Hide();

            Left = System.Windows.SystemParameters.WorkArea.Width - Width; //xaml code
            Top = System.Windows.SystemParameters.WorkArea.Height - Height;
            myDatePicker.SelectedDate = DateTime.Now;

            if (Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName).Length > 1)
            {
                Close();
                return;
            } //prevents multiple instances

            Tray.myItem1.Click += new EventHandler(ExitClicked); //On click events for tray
            Tray.myItem2.Click += new EventHandler(AboutClicked);
            Tray.myItem3.Click += new EventHandler(CreateNotification);

            log.EntryWritten += new EntryWrittenEventHandler(log_EntryWritten); //Change of time log
            log.EnableRaisingEvents = true;
           }

        //System Tray
        private void AboutClicked(object sender, EventArgs e)
        {
            Tray.myTray.ShowBalloonTip(1000, "About Notify Me", "Easily Make a notification without the hassel of Windows Settings. All settings are removed on uninstall.", ToolTipIcon.Info);

        } //Info on program
        private void ExitClicked(object sender, EventArgs e)
        {
            Tray.myTray.Visible = false;
            Close();
        } //Exit Option

        //Keep Program on track
        private void log_EntryWritten(object sender, EntryWrittenEventArgs e)
        {
            if (e.Entry.InstanceId == 1 && e.Entry.EntryType == EventLogEntryType.Information)
                foreach(Build_Timer Item in myTimerList)
                {
                    Item.ConfirmInterval(true, Item.SetNextInterval());
                }
        } //Adjusts for when a user changes the time
        
        //Form Interior Events
        private void EveryDay_Checked(object sender, RoutedEventArgs e)
        {
            if(EveryDay.IsChecked==true)
            {
                Monday.IsChecked = true;
                Tuesday.IsChecked = true;
                Wednesday.IsChecked = true;
                Thursday.IsChecked = true;
                Friday.IsChecked = true;
                Saturday.IsChecked = true;
                Sunday.IsChecked = true;
            }
            else
            {
                Monday.IsChecked = false;
                Tuesday.IsChecked = false;
                Wednesday.IsChecked = false;
                Thursday.IsChecked = false;
                Friday.IsChecked = false;
                Saturday.IsChecked = false;
                Sunday.IsChecked = false;
            }
        } //Enables everyday checkbox to select every day

        //Notification Creation Events
        private void CreateNotification(object sender, EventArgs e)
        {
            Show();

            //
            // Moved to CreateNotification_Click
            //

        } //Creates new notification TODO: create new temporary form for each notification
        private void CreateNotification_Click(object sender, RoutedEventArgs e)
        {
            #region Check_for_missing_info
            if
            (Monday.IsChecked.Value == false &&
            Tuesday.IsChecked.Value == false &&
            Wednesday.IsChecked.Value == false &&
            Thursday.IsChecked.Value == false &&
            Friday.IsChecked.Value == false &&
            Saturday.IsChecked.Value == false &&
            Sunday.IsChecked.Value == false)
            {
                Tray.myTray.ShowBalloonTip(1000, "Error", "You need to either select once OR select at least one day", ToolTipIcon.Error);
            }
            #endregion    

            else
            { 
                Hide();

                //Got date, now build parameters
                String LocalTitle = XamlTitle.Text;
                String LocalMessage = XamlMessage.Text;
                bool LocalOnce = CheckBox_Once.IsChecked.Value;
                int Interval = 0;

                DateTime DateChecked = myDatePicker.SelectedDate.Value;
                if (DateChecked.CompareTo(DateTime.Now) <= 0)
                {
                    Interval = 0;
                }
                else
                {
                    TimeSpan Span = DateChecked - DateTime.Now - DateChecked.TimeOfDay; //TODO: only lets 24 days max, can user thread.timer or maybe windows task scheduler. Either way changes this program a shittonne.
                    Interval = (int)Span.TotalSeconds;
                }

                int LocalHours = 0;
                if (AMorPM.SelectedIndex == 1)
                {
                    LocalHours += 12;
                }
                LocalHours += 1 + HourSelect.SelectedIndex;

                bool[] LocalDays = new bool[7];
                LocalDays[1] = Monday.IsChecked.Value; //primitive but functional dayofweek array
                LocalDays[2] = Tuesday.IsChecked.Value;
                LocalDays[3] = Wednesday.IsChecked.Value;
                LocalDays[4] = Thursday.IsChecked.Value;
                LocalDays[5] = Friday.IsChecked.Value;
                LocalDays[6] = Saturday.IsChecked.Value;
                LocalDays[0] = Sunday.IsChecked.Value;

                //Got parameters, now build notification
                Build_Timer myTimer = new Build_Timer(Interval, LocalOnce, LocalHours, LocalDays, LocalMessage, LocalTitle);

                myTimerList.Add(myTimer);
            
        }}

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
           
            myTimerList.RemoveAt(myTimerList.Count - 1);
            foreach(Build_Timer unit in myTimerList)
            {
                Debug.WriteLine("Still Have This Guy: " + unit.myTitle);
            }
            Hide();
        }  // Removes Build_Timer from end of list and hide window. TODO: Implement Separate forms
    }
}
