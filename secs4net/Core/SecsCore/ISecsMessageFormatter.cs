using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Secs4Net
{
    public interface ISecsMessageFormatter
    {
        string Format(SecsMessage msg);
    }
}
