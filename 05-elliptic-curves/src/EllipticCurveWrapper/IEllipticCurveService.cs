using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace EllipticCurveWrapper
{
    public interface IEllipticCurveService
    {
        ECPoint BasePointGGet();
        bool IsOnCurveCheck(ECPoint point);
        ECPoint AddECPoints(ECPoint p, ECPoint q);
        ECPoint DoubleECPoints(ECPoint p);
        ECPoint ScalarMult(BigInteger k, ECPoint p);
        string ECPointToString(ECPoint point);
        ECPoint StringToECPoint(string s);
    }
}
