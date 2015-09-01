using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nugr.contract.calculator
{
    public interface ICalculator
    {
        long[] Calculate(long from, long to);
    }
}
