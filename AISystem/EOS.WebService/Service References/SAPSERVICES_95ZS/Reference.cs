//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace EOS.WebService.SAPSERVICES_95ZS {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://sjyg.com/unmanned2erp/ZFMM095/if", ConfigurationName="SAPSERVICES_95ZS.SI_ZFMM095")]
    public interface SI_ZFMM095 {
        
        // CODEGEN: 操作 SI_ZFMM095 以后生成的消息协定不是 RPC，也不是换行文档。
        [System.ServiceModel.OperationContractAttribute(Action="http://sap.com/xi/WebService/soap1.1", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        EOS.WebService.SAPSERVICES_95ZS.SI_ZFMM095Response SI_ZFMM095(EOS.WebService.SAPSERVICES_95ZS.SI_ZFMM095Request request);
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.3190.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="urn:sap-com:document:sap:rfc:functions")]
    public partial class ZFMM095 : object, System.ComponentModel.INotifyPropertyChanged {
        
        private ZTFMM095[] t_RETURNField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("item", Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=false)]
        public ZTFMM095[] T_RETURN {
            get {
                return this.t_RETURNField;
            }
            set {
                this.t_RETURNField = value;
                this.RaisePropertyChanged("T_RETURN");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.3190.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:sap-com:document:sap:rfc:functions")]
    public partial class ZTFMM095 : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string lIFNRField;
        
        private string nAME1Field;
        
        private string mTEXTField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public string LIFNR {
            get {
                return this.lIFNRField;
            }
            set {
                this.lIFNRField = value;
                this.RaisePropertyChanged("LIFNR");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=1)]
        public string NAME1 {
            get {
                return this.nAME1Field;
            }
            set {
                this.nAME1Field = value;
                this.RaisePropertyChanged("NAME1");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=2)]
        public string MTEXT {
            get {
                return this.mTEXTField;
            }
            set {
                this.mTEXTField = value;
                this.RaisePropertyChanged("MTEXT");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.3190.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="urn:sap-com:document:sap:rfc:functions")]
    public partial class ZFMM095Response : object, System.ComponentModel.INotifyPropertyChanged {
        
        private ZTFMM095[] t_RETURNField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("item", Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=false)]
        public ZTFMM095[] T_RETURN {
            get {
                return this.t_RETURNField;
            }
            set {
                this.t_RETURNField = value;
                this.RaisePropertyChanged("T_RETURN");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class SI_ZFMM095Request {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="urn:sap-com:document:sap:rfc:functions", Order=0)]
        public EOS.WebService.SAPSERVICES_95ZS.ZFMM095 ZFMM095;
        
        public SI_ZFMM095Request() {
        }
        
        public SI_ZFMM095Request(EOS.WebService.SAPSERVICES_95ZS.ZFMM095 ZFMM095) {
            this.ZFMM095 = ZFMM095;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class SI_ZFMM095Response {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="ZFMM095.Response", Namespace="urn:sap-com:document:sap:rfc:functions", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute("ZFMM095.Response")]
        public EOS.WebService.SAPSERVICES_95ZS.ZFMM095Response ZFMM095Response;
        
        public SI_ZFMM095Response() {
        }
        
        public SI_ZFMM095Response(EOS.WebService.SAPSERVICES_95ZS.ZFMM095Response ZFMM095Response) {
            this.ZFMM095Response = ZFMM095Response;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface SI_ZFMM095Channel : EOS.WebService.SAPSERVICES_95ZS.SI_ZFMM095, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class SI_ZFMM095Client : System.ServiceModel.ClientBase<EOS.WebService.SAPSERVICES_95ZS.SI_ZFMM095>, EOS.WebService.SAPSERVICES_95ZS.SI_ZFMM095 {
        
        public SI_ZFMM095Client() {
        }
        
        public SI_ZFMM095Client(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public SI_ZFMM095Client(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public SI_ZFMM095Client(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public SI_ZFMM095Client(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        EOS.WebService.SAPSERVICES_95ZS.SI_ZFMM095Response EOS.WebService.SAPSERVICES_95ZS.SI_ZFMM095.SI_ZFMM095(EOS.WebService.SAPSERVICES_95ZS.SI_ZFMM095Request request) {
            return base.Channel.SI_ZFMM095(request);
        }
        
        public EOS.WebService.SAPSERVICES_95ZS.ZFMM095Response SI_ZFMM095(EOS.WebService.SAPSERVICES_95ZS.ZFMM095 ZFMM095) {
            EOS.WebService.SAPSERVICES_95ZS.SI_ZFMM095Request inValue = new EOS.WebService.SAPSERVICES_95ZS.SI_ZFMM095Request();
            inValue.ZFMM095 = ZFMM095;
            EOS.WebService.SAPSERVICES_95ZS.SI_ZFMM095Response retVal = ((EOS.WebService.SAPSERVICES_95ZS.SI_ZFMM095)(this)).SI_ZFMM095(inValue);
            return retVal.ZFMM095Response;
        }
    }
}
