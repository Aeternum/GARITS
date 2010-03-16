using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.IO;

namespace GARITS
{
    class Utils
    {
        private string settings = "settings.ini";

        private ArrayList options; //Stores option key
        private ArrayList values; //Stores option value
        
        public Utils()
        {
            getSettings();
        }

        private void getSettings()
        {
            StreamReader SR;
            string S;
            try
            {
                SR = File.OpenText(settings);
                S = SR.ReadLine();
            }
            catch (Exception ex)
            {
                throw new Exception("Error reading the settings.ini file.\nError: " + ex.Message);
            }
            options = new ArrayList();
            values = new ArrayList();
            while (S != null)
            {
                if (!((S.StartsWith("#")) || (S.Equals("")))) //ignore empty and blank lines
                {
                    if (S.Split('=').Length == 2)
                    {
                        options.Add(S.Split('=')[0]);
                        values.Add(S.Split('=')[1]);
                    }
                    else
                    {
                        Exception myExcp = new Exception("Error reading the settings.ini file. Check for unmatched ='s signs.");
                        throw myExcp;
                    }
                }
                S = SR.ReadLine();
            }
            SR.Close();
        }

        public string getDBConnectionString()
        {
            //Server=" + svr + ";Database=" + db + ";Uid=" + user + ";Pwd=" + pass + ";"
            int index = options.IndexOf("dbserver");
            if (index == -1)
                throw new Exception("Error reading the settings.ini file. Unable to find 'dbserver' directive.");
            string dbserver = (string)values[index];
            index = options.IndexOf("dbname");
            if (index == -1)
                throw new Exception("Error reading the settings.ini file. Unable to find 'dbname' directive.");
            string dbname = (string)values[index];
            index = options.IndexOf("dbuser");
            if (index == -1)
                throw new Exception("Error reading the settings.ini file. Unable to find 'dbuser' directive.");
            string dbuser = (string)values[index];
            index = options.IndexOf("dbpassword");
            if (index == -1)
                throw new Exception("Error reading the settings.ini file. Unable to find 'dbpassword' directive.");
            string dbpassword = (string)values[index];
            if ((dbserver.Length == 0) || (dbname.Length == 0) || (dbuser.Length == 0))
                throw new Exception("Error reading the settings.ini file. Database hostname, name, or username missing.");
            //Now we have all of our info, lets contruct the db connection string
            //We use a ternary return, as passing a connection string with an empty password breaks things
            return dbpassword.Length > 0 ? "Server=" + dbserver + ";Database=" + dbname + ";Uid=" + dbuser + ";Pwd=" + dbpassword + ";" : "Server=" + dbserver + ";Database=" + dbname + ";Uid=" + dbuser + ";";
        }
    }
}
