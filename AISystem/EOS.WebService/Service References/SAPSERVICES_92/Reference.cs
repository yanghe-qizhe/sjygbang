﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace EOS.WebService.SAPSERVICES_92 {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://sjyg.com/unmanned2erp/ZFMM092/if", ConfigurationName="SAPSERVICES_92.SI_ZFMM092_OUT")]
    public interface SI_ZFMM092_OUT {
        
        // CODEGEN: 操作 SI_ZFMM092_OUT 以后生成的消息协定不是 RPC，也不是换行文档。
        [System.ServiceModel.OperationContractAttribute(Action="http://sap.com/xi/WebService/soap1.1", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        EOS.WebService.SAPSERVICES_92.SI_ZFMM092_OUTResponse SI_ZFMM092_OUT(EOS.WebService.SAPSERVICES_92.SI_ZFMM092_OUTRequest request);
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.3190.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="urn:sap-com:document:sap:rfc:functions")]
    public partial class ZFMM092 : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string i_EBELNField;
        
        private string i_LIFNRField;
        
        private string i_MATNRField;
        
        private ZFMM092_ITEM[] t_ITEMField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string I_EBELN {
            get {
                return this.i_EBELNField;
            }
            set {
                this.i_EBELNField = value;
                this.RaisePropertyChanged("I_EBELN");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string I_LIFNR {
            get {
                return this.i_LIFNRField;
            }
            set {
                this.i_LIFNRField = value;
                this.RaisePropertyChanged("I_LIFNR");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string I_MATNR {
            get {
                return this.i_MATNRField;
            }
            set {
                this.i_MATNRField = value;
                this.RaisePropertyChanged("I_MATNR");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("item", Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=false)]
        public ZFMM092_ITEM[] T_ITEM {
            get {
                return this.t_ITEMField;
            }
            set {
                this.t_ITEMField = value;
                this.RaisePropertyChanged("T_ITEM");
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
    public partial class ZFMM092_ITEM : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string eBELNField;
        
        private string eBELPField;
        
        private string wERKSField;
        
        private string mATNRField;
        
        private string lIFNRField;
        
        private decimal mENGEField;
        
        private bool mENGEFieldSpecified;
        
        private string aEDATField;
        
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
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=3)]
        public string MATNR {
            get {
                return this.mATNRField;
            }
            set {
                this.mATNRField = value;
                this.RaisePropertyChanged("MATNR");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=4)]
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
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=5)]
        public decimal MENGE {
            get {
                return this.mENGEField;
            }
            set {
                this.mENGEField = value;
                this.RaisePropertyChanged("MENGE");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool MENGESpecified {
            get {
                return this.mENGEFieldSpecified;
            }
            set {
                this.mENGEFieldSpecified = value;
                this.RaisePropertyChanged("MENGESpecified");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=6)]
        public string AEDAT {
            get {
                return this.aEDATField;
            }
            set {
                this.aEDATField = value;
                this.RaisePropertyChanged("AEDAT");
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
    public partial class ZFMM092Response : object, System.ComponentModel.INotifyPropertyChanged {
        
        private ZFMM092_ITEM[] t_ITEMField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("item", Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=false)]
        public ZFMM092_ITEM[] T_ITEM {
            get {
                return this.t_ITEMField;
            }
            set {
                this.t_ITEMField = value;
                this.RaisePropertyChanged("T_ITEM");
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
    public partial class SI_ZFMM092_OUTRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="urn:sap-com:document:sap:rfc:functions", Order=0)]
        public EOS.WebService.SAPSERVICES_92.ZFMM092 ZFMM092;
        
        public SI_ZFMM092_OUTRequest() {
        }
        
        public SI_ZFMM092_OUTRequest(EOS.WebService.SAPSERVICES_92.ZFMM092 ZFMM092) {
            this.ZFMM092 = ZFMM092;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class SI_ZFMM092_OUTResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="ZFMM092.Response", Namespace="urn:sap-com:document:sap:rfc:functions", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute("ZFMM092.Response")]
        public EOS.WebService.SAPSERVICES_92.ZFMM092Response ZFMM092Response;
        
        public SI_ZFMM092_OUTResponse() {
        }
        
        public SI_ZFMM092_OUTResponse(EOS.WebService.SAPSERVICES_92.ZFMM092Response ZFMM092Response) {
            this.ZFMM092Response = ZFMM092Response;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface SI_ZFMM092_OUTChannel : EOS.WebService.SAPSERVICES_92.SI_ZFMM092_OUT, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class SI_ZFMM092_OUTClient : System.ServiceModel.ClientBase<EOS.WebService.SAPSERVICES_92.SI_ZFMM092_OUT>, EOS.WebService.SAPSERVICES_92.SI_ZFMM092_OUT {
        
        public SI_ZFMM092_OUTClient() {
        }
        
        public SI_ZFMM092_OUTClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public SI_ZFMM092_OUTClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public SI_ZFMM092_OUTClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public SI_ZFMM092_OUTClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        EOS.WebService.SAPSERVICES_92.SI_ZFMM092_OUTResponse EOS.WebService.SAPSERVICES_92.SI_ZFMM092_OUT.SI_ZFMM092_OUT(EOS.WebService.SAPSERVICES_92.SI_ZFMM092_OUTRequest request) {
            return base.Channel.SI_ZFMM092_OUT(request);
        }
        
        public EOS.WebService.SAPSERVICES_92.ZFMM092Response SI_ZFMM092_OUT(EOS.WebService.SAPSERVICES_92.ZFMM092 ZFMM092) {
            EOS.WebService.SAPSERVICES_92.SI_ZFMM092_OUTRequest inValue = new EOS.WebService.SAPSERVICES_92.SI_ZFMM092_OUTRequest();
            inValue.ZFMM092 = ZFMM092;
            EOS.WebService.SAPSERVICES_92.SI_ZFMM092_OUTResponse retVal = ((EOS.WebService.SAPSERVICES_92.SI_ZFMM092_OUT)(this)).SI_ZFMM092_OUT(inValue);
            return retVal.ZFMM092Response;
        }
    }
}
