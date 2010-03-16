using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GARITS
{
    class Permissions
    {
        private ArrayList components;

        public Permissions()
        {
            components = new ArrayList();
        }

        public void AddComponent(Object item, string permission)
        {
           bool found = false;
           foreach (Permission p in components)
            {
                if (p.getPermission() == permission)
                {
                    found = true;
                    p.setComponent(item);
                }
            }
           if (!found)
               components.Add(new Permission(-1, permission, false)); //Add the new permission in the array list so that we can set it up
        }

        public void updateComponents(int roleID)
        {
            foreach (Permission p in components)
            {
                if ((p.getRoleID() == roleID) || (roleID == 0))   //Are we looking at a permission relevant to our role? If not, if its the super user, enable anyway
                {
                    try
                    {
                        if (p.getComponent().GetType() == typeof(System.Windows.Forms.ToolStripMenuItem))
                            ((ToolStripMenuItem)p.getComponent()).Enabled = true;
                    }
                    catch (NullReferenceException)
                    {
                        throw new Exception("Caught NullReferenceException in updateComponents. Someone added a permission for a component that doesn't exist");
                    }
                }
            }
        }

        public void disableAll()
        {
            foreach (Permission p in components)
            {
                    try
                    {
                        if (p.getComponent().GetType() == typeof(System.Windows.Forms.ToolStripMenuItem))
                            ((ToolStripMenuItem)p.getComponent()).Enabled = false;
                    }
                    catch (NullReferenceException)
                    {
                        throw new Exception("Caught NullReferenceException in updateComponents. Someone added a permission for a component that doesn't exist");
                    }
            }
        }

        public void readPermissions(Database db)
        {
            components = db.getPermissions();
            foreach (Permission p in components)
            {
                Console.WriteLine(p.getRoleID() + " " + p.getPermission() + " " + p.getAccess());
            }
        }
    }
}
