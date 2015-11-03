using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Dynamics.Nav.TestUtilities;
using Microsoft.Dynamics.Nav.UserSession;
using Microsoft.Dynamics.Framework.UI.Client;
using System.Collections.Generic;

namespace newsystemLoadTest
{
    [TestClass]
    public class ExplorerDebitorenpostenTest : ExplorerTest
    {
        private const int ExplorerPageId = 5010440;
        //private const int ExplorerSubPageId = 5010441;
 
        [TestMethod]
        [Timeout(TestTimeout.Infinite)]
        public void ExplorerDebitorenpostenBenutzen()
        {
            TestScenario.Run(NsysUserContextManager, TestContext, RunExplorerDebitorenpostenBenutzen);
        }

        public void RunExplorerDebitorenpostenBenutzen(UserContext userContext)
        {
            // Explorer Debitorenposten öffnen
            openExplorerPage(userContext, ExplorerPageId);

            // Suchen
            search("Debitorennr.", "113203", userContext);
            logCurrentSorting();

            string docNo = getPosten(0).Control("Document No.").StringValue;
            NsysUtils.log(TestContext, "Selected first Debitorenposten Key: {0} Value: {1}", "Document No.", docNo);

            // Filtern
            filter("Amount", ">500", userContext);
            logCurrentSorting();

            // Sortieren
            sortAscending("Amount");
            logCurrentSorting();
            docNo = getPosten(0).Control("Document No.").StringValue;
            NsysUtils.log(TestContext, "Selected first Debitorenposten Key: {0} Value: {1}", "Document No.", docNo);

            sortDescending("Amount");
            logCurrentSorting();
            docNo = getPosten(0).Control("Document No.").StringValue;
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
