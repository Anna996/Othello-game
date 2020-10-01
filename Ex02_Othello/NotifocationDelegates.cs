using System;
using System.Collections.Generic;
using System.Text;

namespace Ex05_GraphicOthello
{
    public delegate void NotifocationDelegate();

    public delegate void NotifocationDelegate<T>(T i_Param1);

    public delegate void NotifocationDelegate<T1, T2>(T1 i_Param1, T1 i_Param2, T2 i_Param3);
}
