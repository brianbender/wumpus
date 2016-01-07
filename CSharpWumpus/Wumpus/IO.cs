﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wumpus
{
    public interface IO
    {
        void WriteLine(string data);
        void Prompt(string data);
        char ReadChar();
        int readInt();
        void Continue();
    }
}
