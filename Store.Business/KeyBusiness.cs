﻿using Store.Data.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Business
{
    public class KeyBusiness
    {
        private readonly IUnitOfWork _unitOfWork;
        public KeyBusiness(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
    }
}
