using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Daimler.HELM.PublicService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ISendSMS" in both code and config file together.
    [ServiceContract]
    public interface ISendSMS
    {
        [OperationContract]
        string GetData(string xml);

        [OperationContract]
        string SendSingleSMS(string accessToken, string content);

        [OperationContract]
        string GetAccessToken(string dataSource, string accountId, string securityKey);
    }
}
