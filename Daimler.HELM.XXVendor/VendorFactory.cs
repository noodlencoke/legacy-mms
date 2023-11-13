using Daimler.HELM.MessageInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daimler.HELM.MessageInterface.Impl
{
    public static class VendorFactory
    {
        public static ISMSInterface GetSMSInstance(string vendorName)
        {
            if (vendorName == "DHST")
            {
                return new DhstWebServiceVendor();
            }
            else if (vendorName == "DHSTHttp")
            {
                return new DhstVendor();
            }
            else if (vendorName == "MOCK")
            {
                return new MockVendor();
            }
            else
            {
                return null;
            }
        }

        public static IMMSInterface GetMMSInterface(string vendorName)
        {
            if (vendorName == "DHST")
            { 
                return new DhstWebServiceVendor();
            }
            else if (vendorName == "DHSTHttp")
            {
                return new DhstVendor();
            }
            else if (vendorName == "MOCK")
            {
                return new MockVendor();
            }
            else
            {
                return null;
            }
        }

        public static IEmailInterface GetEmailInterface(string vendorName)
        {
            if (vendorName == "SendCloud")
            {
                return new SendCloundVendor();
            }
            else if (vendorName == "MOCK")
            {
                return new MockVendor();
            }
            else
            {
                return null;
            }

        }
    }
}
