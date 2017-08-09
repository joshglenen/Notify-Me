using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Notify_Me
{
    class Build_Tray
    {
        public NotifyIcon myTray = new NotifyIcon();
        private ContextMenu myMenu = new ContextMenu();
        public MenuItem myItem1 = new MenuItem();
        public MenuItem myItem2 = new MenuItem();
        public MenuItem myItem3 = new MenuItem();

        public Build_Tray()
        {

            //creates the icon and message
            myTray.Icon = new Icon(@"clock.ico");
            myTray.Visible = true;
            myTray.Text = "Notify Me";
            myTray.ShowBalloonTip(500, "Notify Me", "Notify Me is Now On!", ToolTipIcon.Info);

            //creates a list of menu items in context menu
            myMenu.MenuItems.AddRange(new MenuItem[] { myItem1, myItem2, myItem3 });
            myItem1.Index = 0;
            myItem1.Text = "Exit";
            myItem2.Index = 1;
            myItem2.Text = "About";
            myTray.ContextMenu = myMenu;
            myItem3.Index = 2;
            myItem3.Text = "Create";
            myTray.ContextMenu = myMenu;

        } //Creates a simple icon in the system tray
    }
}
