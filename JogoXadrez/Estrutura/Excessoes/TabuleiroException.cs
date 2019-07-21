using System;
using System.Collections.Generic;
using System.Text;

namespace Excessoes
{
    public class TabuleiroException : Exception
    {
        public TabuleiroException(string mensagem) : base(mensagem)
        {

        }
    }
}
