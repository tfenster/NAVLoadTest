using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Dynamics.Nav.TestUtilities;
using Microsoft.Dynamics.Nav.UserSession;
using Microsoft.Dynamics.Framework.UI.Client;
using System.Collections.Generic;

namespace newsystemLoadTest
{
    [TestClass]
    public class ExplorerDebitorenpostenOffeneRechnung : ExplorerTest
    {
        private const int ExplorerPageId = 5010440;
        //private const int ExplorerSubPageId = 5010441;

        [TestMethod]
        [Timeout(TestTimeout.Infinite)]
        public void ExplorerDebitorenpostenORBenutzen()
        {
            TestScenario.Run(NsysUserContextManager, TestContext, RunExplorerDebitorenpostenORBenutzen);
        }

        public void RunExplorerDebitorenpostenORBenutzen(UserContext userContext)
        {
            // Explorer Debitorenposten öffnen
            openExplorerPage(userContext, ExplorerPageId);

            // Suchen
            fillsearchfield("Belegart", "Rechnung", userContext);
            search("Offen", "Ja", userContext);
            logCurrentSorting();

            string docNo = getPosten(0).Control("Belegnr.").StringValue;
            NsysUtils.log(TestContext, "Selected first Debitorenposten Key: {0} Value: {1}", "Document No.", docNo);

            // Filtern
            filter("Betrag", ">500", userContext);
            logCurrentSorting();

            // Sortieren
            sortAscending("Betrag");
            logCurrentSorting();
            docNo = getPosten(0).Control("Belegnr.").StringValue;
            NsysUtils.log(TestContext, "Selected first Debitorenposten Key: {0} Value: {1}", "Document No.", docNo);

            sortDescending("Betrag");
            logCurrentSorting();
            docNo = getPosten(0).Control("Belegnr.").StringValue;
            NsysUtils.log(TestContext, "Selected first Debitorenposten Key: {0} Value: {1}", "Document No.", docNo);

            // Aufräumen
            closeExplorerPage(userContext);
        }

        [ClassCleanup]
        public static void CleanupClass()
        {
            NsysTest.Cleanup();
        }

    }
}
