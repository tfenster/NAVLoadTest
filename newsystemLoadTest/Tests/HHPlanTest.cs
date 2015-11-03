using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Dynamics.Nav.TestUtilities;
using Microsoft.Dynamics.Nav.UserSession;

namespace newsystemLoadTest
{
    [TestClass]
    public class HHPlanTest : NsysTest
    {
        private const int BerichtsdefinitionenPageId = 5010603;
        private const int BerichtsdefinitionPageId = 5010604;

        [TestMethod]
        [Timeout(TestTimeout.Infinite)]
        public void HHPlanDatenBerechnen()
        {
            TestScenario.Run(NsysUserContextManager, TestContext, RunHHPlanDatenBerechnen);
        }

        public void RunHHPlanDatenBerechnen(UserContext userContext)
        {
            // Berichtsdefinitionen öffnen
            var berichtsdefinitionenPage = userContext.EnsurePage(BerichtsdefinitionenPageId, TestScenario.OpenPage(TestContext, userContext, BerichtsdefinitionenPageId));

            // Filtern
            string filter = "THH022016";
            berichtsdefinitionenPage.ExecuteQuickFilter("Code", filter);

            // Zeile auswählen
            var definition = berichtsdefinitionenPage.Repeater().DefaultViewport[0];
            definition.Control("Code").Activate();

            // Berichtsdefinition öffnen
            var berichtsdefinitionPage = userContext.EnsurePage(BerichtsdefinitionPageId, berichtsdefinitionenPage.Action("Ansicht").InvokeCatchForm());

            // Berechnen
            Guid guid = NsysUtils.startWithLogging(TestContext, "HHPlan berechnen für " + filter);
            var yesNoDialog = berichtsdefinitionPage.Action("Daten berechnen").InvokeCatchDialog();
            if (yesNoDialog == null)
                TestContext.WriteLine("No warning about pre-existing data");
            else
            {
                var progDialog = userContext.CatchDialog(yesNoDialog.Action("Ja").Invoke);
                if (progDialog == null)
                    TestContext.WriteLine("no progress dialog");
            }
            NsysUtils.stopWithLogging(guid, TestContext);

            // Aufräumen
            TestScenario.ClosePage(TestContext, userContext, berichtsdefinitionPage);
            TestScenario.ClosePage(TestContext, userContext, berichtsdefinitionenPage);
        }

        [ClassCleanup]
        public static void CleanupClass()
        {
            NsysTest.Cleanup();
        }
    }
}
