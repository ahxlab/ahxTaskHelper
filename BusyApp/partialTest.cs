using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logging4net;

namespace BusyApp
{
    public partial class TestPartial
    {
        public void TestCallCaller()
        {
            TestCall();
        }
        partial void TestCall();
    }

    public partial class TestPartial
    {
        partial void TestCall()
        {
            Log.TR_OUT(this);
        }
    }
}
