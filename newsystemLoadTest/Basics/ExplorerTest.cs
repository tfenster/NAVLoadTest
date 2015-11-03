using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Dynamics.Nav.TestUtilities;
using Microsoft.Dynamics.Nav.UserSession;
using Microsoft.Dynamics.Framework.UI.Client;
using System.Collections.Generic;

namespace newsystemLoadTest
{
    public class ExplorerTest : NsysTest
    {
        private ClientLogicalForm explorerPage = null;

        protected ClientLogicalForm openExplorerPage(UserContext userContext, int explorerPageId)
        {
            if (explorerPage == null)
            {
                explorerPage = userContext.EnsurePage(explorerPageId, TestScenario.OpenPage(TestContext, userContext, explorerPageId));
            }
            return explorerPage;
        }

        protected void closeExplorerPage(UserContext userContext)
        {
            TestScenario.ClosePage(TestContext, userContext, explorerPage);
        }

        public void search(string searchField, string searchValue, UserContext userContext)
        {
            Guid guid = NsysUtils.startWithLogging(TestContext, "searching for " + searchValue + " in field " + searchField);

            //userContext.EnsurePage(ExplorerPageId, explorerPage);
            explorerPage.Control(searchField).Activate();
            TestScenario.SaveValueWithDelay(explorerPage.Control(searchField), searchValue);

            explorerPage.Action("Suchen").Invoke();
            NsysUtils.log(TestContext, "found {0} hits when searching for {1} in field {2}", getNoOfPosten(), searchValue, searchField);

            NsysUtils.stopWithLogging(guid, TestContext);
        }

        public void filter(string filterField, string filterValue, UserContext userContext)
        {
            Guid guid = NsysUtils.startWithLogging(TestContext, "filtering for " + filterValue + " in field " + filterField);

            ClientRepeaterColumnControl filterColumn = explorerPage.Repeater().Column(filterField);
            TestScenario.ApplyColumnFilter(TestContext, userContext, filterColumn, filterValue);
            NsysUtils.log(TestContext, "found {0} hits when filtering for {1} in field {2}", getNoOfPosten(), filterValue, filterField);

            NsysUtils.stopWithLogging(guid, TestContext);
        }

        public void sortAscending(string sortField)
        {
            Guid guid = NsysUtils.startWithLogging(TestContext, "sorting ascending by " + sortField);

            var column = explorerPage.Repeater().Column(sortField);
            column.Action("Aufsteigend").Invoke();

            NsysUtils.stopWithLogging(guid, TestContext);
        }

        public void sortDescending(string sortField)
        {
            Guid guid = NsysUtils.startWithLogging(TestContext, "sorting descending by " + sortField);

            var column = explorerPage.Repeater().Column(sortField);
            column.Action("Absteigend").Invoke();

            NsysUtils.stopWithLogging(guid, TestContext);
        }

        public void logCurrentSorting()
        {
            NsysUtils.log(TestContext, "sort by: {0}", explorerPage.Control("Sortierung:").StringValue);
        }

        public ClientRepeaterRowControl getPosten(int row)
        {
            return explorerPage.Repeater().DefaultViewport[row];
        }

        public ClientRepeaterRowControl getRandomPosten()
        {
            int rowToSelect = SafeRandom.GetRandomNext(getNoOfPosten());
            return getPosten(rowToSelect);
        }

        public int getNoOfPosten()
        {
            return explorerPage.Repeater().DefaultViewport.Count;
        }

    }
}
