using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Dynamics.Nav.TestUtilities;
using Microsoft.Dynamics.Nav.UserSession;
using Microsoft.Dynamics.Framework.UI.Client;
using System.Globalization;
using newsystemLoadTest.Properties;

namespace newsystemLoadTest
{
    public class NsysTest
    {
        public TestContext TestContext { get; set; }

        protected const int DoppikRoleCenterId = 5162300;

        private static UserContextManager nsysUserContextManager;

        public UserContextManager NsysUserContextManager
        {
            get
            {
                return nsysUserContextManager ?? CreateUserContextManager();
            }
        }

        private UserContextManager CreateUserContextManager()
        {
            nsysUserContextManager = new WindowsUserContextManager(
                    NAVClientService,
                    null,
                    null,
                    DoppikRoleCenterId,
                    UICulture);

            return nsysUserContextManager;
        }
        public string NAVClientService
        {
            get
            {
                return Settings.Default.NAVClientService;
            }
        }

        public string UICulture
        {
            get
            {
                return Settings.Default.UICulture;
            }
        }

        public static void Cleanup()
        {
            if (nsysUserContextManager != null)
            {
                nsysUserContextManager.CloseAllSessions();
            }
        }
        
    }
}

