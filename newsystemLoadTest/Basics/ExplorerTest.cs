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
        private Dictionary<string, ClientRepeaterColumnControl> explorerColumns = null;

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

            var column = getExplorerColumns()[filterField];
            ClientLogicalForm filters = column.Action("Filter...").InvokeCatchDialog();
            TestScenario.SaveValueWithDelay(filters.Control(filterField), filterValue);
            filters.Action("OK").Invoke();
            NsysUtils.log(TestContext, "found {0} hits when filtering for {1} in field {2}", getNoOfPosten(), filterValue, filterField);

            NsysUtils.stopWithLogging(guid, TestContext);
        }

        public void sortAscending(string sortField)
        {
            Guid guid = NsysUtils.startWithLogging(TestContext, "sorting ascending by " + sortField);

            var column = getExplorerColumns()[sortField];
            column.Action("Ascending").Invoke();

            NsysUtils.stopWithLogging(guid, TestContext);
        }

        public void sortDescending(string sortField)
        {
            Guid guid = NsysUtils.startWithLogging(TestContext, "sorting descending by " + sortField);

            var column = getExplorerColumns()[sortField];
            column.Action("Descending").Invoke();

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

        private Dictionary<string, ClientRepeaterColumnControl> getExplorerColumns()
        {
            if (explorerColumns == null)
            {
                explorerColumns = new Dictionary<string, ClientRepeaterColumnControl>();

                foreach (ClientRepeaterColumnControl column in explorerPage.Repeater().Columns)
                {
                    if (!explorerColumns.ContainsKey(column.Caption))
                        explorerColumns.Add(column.Caption, column);
                }
            }

            return explorerColumns;
        }
        
    }
}
