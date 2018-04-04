using Microsoft.Deployment.WindowsInstaller;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClosePromptCA
{
    public class Logger
    {
        public static Session session
        {
            get; set;
        }

        public static void Write(string message, params object[] args)
        {
            if (session != null)
            {

                session.Log("CloseApps => " + message, args);
            }
        }
    }
}
