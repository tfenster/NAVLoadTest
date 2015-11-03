using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace newsystemLoadTest
{
    class NsysUtils
    {
        private static Dictionary<Guid, TimeLog> Timers = new Dictionary<Guid, TimeLog>();

        public static Guid start(string message = null)
        {
            Guid newGuid = Guid.NewGuid();
            TimeLog tl = new TimeLog();
            tl.dateTime = DateTime.Now;
            if (message == null)
                tl.message = "";
            else
                tl.message = message;
            Timers.Add(newGuid, tl);
            return newGuid;
        }

        public static Guid startWithLogging(TestContext tc, string message)
        {
            log(tc, "{0} starts", message);
            return start(message);
        }

        public static TimeSpan stop(Guid guid)
        {
            TimeLog tl = Timers[guid];
            TimeSpan diff = DateTime.Now - tl.dateTime;
            Timers.Remove(guid);
            return diff;
        }

        public static TimeSpan stopWithLogging(Guid guid, TestContext tc)
        {
            TimeLog tl = Timers[guid];
            TimeSpan ts = stop(guid);
            log(tc, "{0} stops", tl.message);
            log(tc, "{0} took {1}", tl.message, ts.ToString());
            return ts;
        }

        public static void log(TestContext tc, string format, params object[] args)
        {
            int last = args.Count() + 1;
            object[] args2 = new object[last];
            Array.Copy(args, args2, args.Count());
            args2[last-1] = DateTime.Now.ToString("HH:mm:ss");
            tc.WriteLine("{" + (last-1) + "}: " + format, args2);
        }

        private class TimeLog
        {
            public string message { get; set; }
            public DateTime dateTime { get; set; }
        }
    }
}
