using System;
using Rave.api;

namespace Rave.Models.PayPal
{
    public class ChargeRes<T> : RaveResponse<T> where T : ResponseData
    {
        public override T Data { get ;  set ; }
    }
}
