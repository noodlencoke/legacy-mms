﻿using Daimler.HELM.HubService.Contract;
using Daimler.HELM.HubService.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daimler.HELM.RepeatSendMessage
{
    public class Program
    {
        static void Main(string[] args)
        {
            CommonResult comResult = SMSHandler.RepeatSendSMS();
            if (!comResult.IsOK)
            { 
                
            }

        }
    }
}
