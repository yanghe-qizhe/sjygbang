﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace EOS.WebService.SAPSERVICES_96 {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://sjyg.com/unmanned2erp/ZFMM096/if", ConfigurationName="SAPSERVICES_96.SI_ZFMM096_OUT")]
    public interface SI_ZFMM096_OUT {
        
        // CODEGEN: 操作 SI_ZFMM096_OUT 以后生成的消息协定不是 RPC，也不是换行文档。
        [System.ServiceModel.OperationContractAttribute(Action="http://sap.com/xi/WebService/soap1.1", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        EOS.WebService.SAPSERVICES_96.SI_ZFMM096_OUTResponse SI_ZFMM096_OUT(EOS.WebService.SAPSERVICES_96.SI_ZFMM096_OUTRequest request);
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.3190.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="urn:sap-com:document:sap:rfc:functions")]
    public partial class ZFMM096 : object, System.ComponentModel.INotifyPropertyChanged {
        
        private ZSFMM096[] t_OUTTABField;
        
        private ZTFMM096[] t_RETURNField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("item", Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=false)]
        public ZSFMM096[] T_OUTTAB {
            get {
                return this.t_OUTTABField;
            }
            set {
                this.t_OUTTABField = value;
                this.RaisePropertyChanged("T_OUTTAB");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("item", Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=false)]
        public ZTFMM096[] T_RETURN {
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
    public partial class ZSFMM096 : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string aUFNRField;
        
        private string wERKSField;
        
        private string pHAS0Field;
        
        private string pHAS2Field;
        
        private string pHAS3Field;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public string AUFNR {
            get {
                return this.aUFNRField;
            }
            set {
                this.aUFNRField = value;
                this.RaisePropertyChanged("AUFNR");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=1)]
        public string WERKS {
            get {
                return this.wERKSField;
            }
            set {
                this.wERKSField = value;
                this.RaisePropertyChanged("WERKS");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=2)]
        public string PHAS0 {
            get {
                return this.pHAS0Field;
            }
            set {
                this.pHAS0Field = value;
                this.RaisePropertyChanged("PHAS0");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=3)]
        public string PHAS2 {
            get {
                return this.pHAS2Field;
            }
            set {
                this.pHAS2Field = value;
                this.RaisePropertyChanged("PHAS2");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=4)]
        public string PHAS3 {
            get {
                return this.pHAS3Field;
            }
            set {
                this.pHAS3Field = value;
                this.RaisePropertyChanged("PHAS3");
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
    public partial class ZTFMM096 : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string eBELNField;
        
        private string eBELPField;
        
        private string lOEKZField;
        
        private string eLIKZField;
        
        private string zSLBZField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public string EBELN {
            get {
                return this.eBELNField;
            }
            set {
                this.eBELNField = value;
                this.RaisePropertyChanged("EBELN");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=1)]
        public string EBELP {
            get {
                return this.eBELPField;
            }
            set {
                this.eBELPField = value;
                this.RaisePropertyChanged("EBELP");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=2)]
        public string LOEKZ {
            get {
                return this.lOEKZField;
            }
            set {
                this.lOEKZField = value;
                this.RaisePropertyChanged("LOEKZ");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=3)]
        public string ELIKZ {
            get {
                return this.eLIKZField;
            }
            set {
                this.eLIKZField = value;
                this.RaisePropertyChanged("ELIKZ");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=4)]
        public string ZSLBZ {
            get {
                return this.zSLBZField;
            }
            set {
                this.zSLBZField = value;
                this.RaisePropertyChanged("ZSLBZ");
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
    public partial class ZFMM096Response : object, System.ComponentModel.INotifyPropertyChanged {
        
        private ZSFMM096[] t_OUTTABField;
        
        private ZTFMM096[] t_RETURNField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("item", Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=false)]
        public ZSFMM096[] T_OUTTAB {
            get {
                return this.t_OUTTABField;
            }
            set {
                this.t_OUTTABField = value;
                this.RaisePropertyChanged("T_OUTTAB");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("item", Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=false)]
        public ZTFMM096[] T_RETURN {
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
    public partial class SI_ZFMM096_OUTRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="urn:sap-com:document:sap:rfc:functions", Order=0)]
        public EOS.WebService.SAPSERVICES_96.ZFMM096 ZFMM096;
        
        public SI_ZFMM096_OUTRequest() {
        }
        
        public SI_ZFMM096_OUTRequest(EOS.WebService.SAPSERVICES_96.ZFMM096 ZFMM096) {
            this.ZFMM096 = ZFMM096;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class SI_ZFMM096_OUTResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="ZFMM096.Response", Namespace="urn:sap-com:document:sap:rfc:functions", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute("ZFMM096.Response")]
        public EOS.WebService.SAPSERVICES_96.ZFMM096Response ZFMM096Response;
        
        public SI_ZFMM096_OUTResponse() {
        }
        
        public SI_ZFMM096_OUTResponse(EOS.WebService.SAPSERVICES_96.ZFMM096Response ZFMM096Response) {
            this.ZFMM096Response = ZFMM096Response;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface SI_ZFMM096_OUTChannel : EOS.WebService.SAPSERVICES_96.SI_ZFMM096_OUT, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class SI_ZFMM096_OUTClient : System.ServiceModel.ClientBase<EOS.WebService.SAPSERVICES_96.SI_ZFMM096_OUT>, EOS.WebService.SAPSERVICES_96.SI_ZFMM096_OUT {
        
        public SI_ZFMM096_OUTClient() {
        }
        
        public SI_ZFMM096_OUTClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public SI_ZFMM096_OUTClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public SI_ZFMM096_OUTClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public SI_ZFMM096_OUTClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        EOS.WebService.SAPSERVICES_96.SI_ZFMM096_OUTResponse EOS.WebService.SAPSERVICES_96.SI_ZFMM096_OUT.SI_ZFMM096_OUT(EOS.WebService.SAPSERVICES_96.SI_ZFMM096_OUTRequest request) {
            return base.Channel.SI_ZFMM096_OUT(request);
        }
        
        public EOS.WebService.SAPSERVICES_96.ZFMM096Response SI_ZFMM096_OUT(EOS.WebService.SAPSERVICES_96.ZFMM096 ZFMM096) {
            EOS.WebService.SAPSERVICES_96.SI_ZFMM096_OUTRequest inValue = new EOS.WebService.SAPSERVICES_96.SI_ZFMM096_OUTRequest();
            inValue.ZFMM096 = ZFMM096;
            EOS.WebService.SAPSERVICES_96.SI_ZFMM096_OUTResponse retVal = ((EOS.WebService.SAPSERVICES_96.SI_ZFMM096_OUT)(this)).SI_ZFMM096_OUT(inValue);
            return retVal.ZFMM096Response;
        }
    }
}
