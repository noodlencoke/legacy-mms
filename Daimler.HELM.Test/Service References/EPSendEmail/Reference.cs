﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18408
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Daimler.HELM.Test.EPSendEmail {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://contact.di.webservice.huiyee.com/", ConfigurationName="EPSendEmail.SendEmailSoap")]
    public interface SendEmailSoap {
        
        // CODEGEN: Generating message contract since element name arg0 from namespace  is not marked nillable
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        Daimler.HELM.Test.EPSendEmail.pushResponse push(Daimler.HELM.Test.EPSendEmail.pushRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        System.Threading.Tasks.Task<Daimler.HELM.Test.EPSendEmail.pushResponse> pushAsync(Daimler.HELM.Test.EPSendEmail.pushRequest request);
        
        // CODEGEN: Generating message contract since element name return from namespace  is not marked nillable
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        Daimler.HELM.Test.EPSendEmail.hiResponse hi(Daimler.HELM.Test.EPSendEmail.hiRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        System.Threading.Tasks.Task<Daimler.HELM.Test.EPSendEmail.hiResponse> hiAsync(Daimler.HELM.Test.EPSendEmail.hiRequest request);
        
        // CODEGEN: Generating message contract since element name arg0 from namespace  is not marked nillable
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        Daimler.HELM.Test.EPSendEmail.postxmlResponse postxml(Daimler.HELM.Test.EPSendEmail.postxmlRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        System.Threading.Tasks.Task<Daimler.HELM.Test.EPSendEmail.postxmlResponse> postxmlAsync(Daimler.HELM.Test.EPSendEmail.postxmlRequest request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class pushRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="push", Namespace="http://contact.di.webservice.huiyee.com/", Order=0)]
        public Daimler.HELM.Test.EPSendEmail.pushRequestBody Body;
        
        public pushRequest() {
        }
        
        public pushRequest(Daimler.HELM.Test.EPSendEmail.pushRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="")]
    public partial class pushRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string arg0;
        
        public pushRequestBody() {
        }
        
        public pushRequestBody(string arg0) {
            this.arg0 = arg0;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class pushResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="pushResponse", Namespace="http://contact.di.webservice.huiyee.com/", Order=0)]
        public Daimler.HELM.Test.EPSendEmail.pushResponseBody Body;
        
        public pushResponse() {
        }
        
        public pushResponse(Daimler.HELM.Test.EPSendEmail.pushResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="")]
    public partial class pushResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=0)]
        public int @return;
        
        public pushResponseBody() {
        }
        
        public pushResponseBody(int @return) {
            this.@return = @return;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class hiRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="hi", Namespace="http://contact.di.webservice.huiyee.com/", Order=0)]
        public Daimler.HELM.Test.EPSendEmail.hiRequestBody Body;
        
        public hiRequest() {
        }
        
        public hiRequest(Daimler.HELM.Test.EPSendEmail.hiRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute()]
    public partial class hiRequestBody {
        
        public hiRequestBody() {
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class hiResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="hiResponse", Namespace="http://contact.di.webservice.huiyee.com/", Order=0)]
        public Daimler.HELM.Test.EPSendEmail.hiResponseBody Body;
        
        public hiResponse() {
        }
        
        public hiResponse(Daimler.HELM.Test.EPSendEmail.hiResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="")]
    public partial class hiResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string @return;
        
        public hiResponseBody() {
        }
        
        public hiResponseBody(string @return) {
            this.@return = @return;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class postxmlRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="postxml", Namespace="http://contact.di.webservice.huiyee.com/", Order=0)]
        public Daimler.HELM.Test.EPSendEmail.postxmlRequestBody Body;
        
        public postxmlRequest() {
        }
        
        public postxmlRequest(Daimler.HELM.Test.EPSendEmail.postxmlRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="")]
    public partial class postxmlRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string arg0;
        
        public postxmlRequestBody() {
        }
        
        public postxmlRequestBody(string arg0) {
            this.arg0 = arg0;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class postxmlResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="postxmlResponse", Namespace="http://contact.di.webservice.huiyee.com/", Order=0)]
        public Daimler.HELM.Test.EPSendEmail.postxmlResponseBody Body;
        
        public postxmlResponse() {
        }
        
        public postxmlResponse(Daimler.HELM.Test.EPSendEmail.postxmlResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="")]
    public partial class postxmlResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string @return;
        
        public postxmlResponseBody() {
        }
        
        public postxmlResponseBody(string @return) {
            this.@return = @return;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface SendEmailSoapChannel : Daimler.HELM.Test.EPSendEmail.SendEmailSoap, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class SendEmailSoapClient : System.ServiceModel.ClientBase<Daimler.HELM.Test.EPSendEmail.SendEmailSoap>, Daimler.HELM.Test.EPSendEmail.SendEmailSoap {
        
        public SendEmailSoapClient() {
        }
        
        public SendEmailSoapClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public SendEmailSoapClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public SendEmailSoapClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public SendEmailSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        Daimler.HELM.Test.EPSendEmail.pushResponse Daimler.HELM.Test.EPSendEmail.SendEmailSoap.push(Daimler.HELM.Test.EPSendEmail.pushRequest request) {
            return base.Channel.push(request);
        }
        
        public int push(string arg0) {
            Daimler.HELM.Test.EPSendEmail.pushRequest inValue = new Daimler.HELM.Test.EPSendEmail.pushRequest();
            inValue.Body = new Daimler.HELM.Test.EPSendEmail.pushRequestBody();
            inValue.Body.arg0 = arg0;
            Daimler.HELM.Test.EPSendEmail.pushResponse retVal = ((Daimler.HELM.Test.EPSendEmail.SendEmailSoap)(this)).push(inValue);
            return retVal.Body.@return;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<Daimler.HELM.Test.EPSendEmail.pushResponse> Daimler.HELM.Test.EPSendEmail.SendEmailSoap.pushAsync(Daimler.HELM.Test.EPSendEmail.pushRequest request) {
            return base.Channel.pushAsync(request);
        }
        
        public System.Threading.Tasks.Task<Daimler.HELM.Test.EPSendEmail.pushResponse> pushAsync(string arg0) {
            Daimler.HELM.Test.EPSendEmail.pushRequest inValue = new Daimler.HELM.Test.EPSendEmail.pushRequest();
            inValue.Body = new Daimler.HELM.Test.EPSendEmail.pushRequestBody();
            inValue.Body.arg0 = arg0;
            return ((Daimler.HELM.Test.EPSendEmail.SendEmailSoap)(this)).pushAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        Daimler.HELM.Test.EPSendEmail.hiResponse Daimler.HELM.Test.EPSendEmail.SendEmailSoap.hi(Daimler.HELM.Test.EPSendEmail.hiRequest request) {
            return base.Channel.hi(request);
        }
        
        public string hi() {
            Daimler.HELM.Test.EPSendEmail.hiRequest inValue = new Daimler.HELM.Test.EPSendEmail.hiRequest();
            inValue.Body = new Daimler.HELM.Test.EPSendEmail.hiRequestBody();
            Daimler.HELM.Test.EPSendEmail.hiResponse retVal = ((Daimler.HELM.Test.EPSendEmail.SendEmailSoap)(this)).hi(inValue);
            return retVal.Body.@return;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<Daimler.HELM.Test.EPSendEmail.hiResponse> Daimler.HELM.Test.EPSendEmail.SendEmailSoap.hiAsync(Daimler.HELM.Test.EPSendEmail.hiRequest request) {
            return base.Channel.hiAsync(request);
        }
        
        public System.Threading.Tasks.Task<Daimler.HELM.Test.EPSendEmail.hiResponse> hiAsync() {
            Daimler.HELM.Test.EPSendEmail.hiRequest inValue = new Daimler.HELM.Test.EPSendEmail.hiRequest();
            inValue.Body = new Daimler.HELM.Test.EPSendEmail.hiRequestBody();
            return ((Daimler.HELM.Test.EPSendEmail.SendEmailSoap)(this)).hiAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        Daimler.HELM.Test.EPSendEmail.postxmlResponse Daimler.HELM.Test.EPSendEmail.SendEmailSoap.postxml(Daimler.HELM.Test.EPSendEmail.postxmlRequest request) {
            return base.Channel.postxml(request);
        }
        
        public string postxml(string arg0) {
            Daimler.HELM.Test.EPSendEmail.postxmlRequest inValue = new Daimler.HELM.Test.EPSendEmail.postxmlRequest();
            inValue.Body = new Daimler.HELM.Test.EPSendEmail.postxmlRequestBody();
            inValue.Body.arg0 = arg0;
            Daimler.HELM.Test.EPSendEmail.postxmlResponse retVal = ((Daimler.HELM.Test.EPSendEmail.SendEmailSoap)(this)).postxml(inValue);
            return retVal.Body.@return;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<Daimler.HELM.Test.EPSendEmail.postxmlResponse> Daimler.HELM.Test.EPSendEmail.SendEmailSoap.postxmlAsync(Daimler.HELM.Test.EPSendEmail.postxmlRequest request) {
            return base.Channel.postxmlAsync(request);
        }
        
        public System.Threading.Tasks.Task<Daimler.HELM.Test.EPSendEmail.postxmlResponse> postxmlAsync(string arg0) {
            Daimler.HELM.Test.EPSendEmail.postxmlRequest inValue = new Daimler.HELM.Test.EPSendEmail.postxmlRequest();
            inValue.Body = new Daimler.HELM.Test.EPSendEmail.postxmlRequestBody();
            inValue.Body.arg0 = arg0;
            return ((Daimler.HELM.Test.EPSendEmail.SendEmailSoap)(this)).postxmlAsync(inValue);
        }
    }
}