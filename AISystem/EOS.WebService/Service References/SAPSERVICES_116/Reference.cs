//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace EOS.WebService.SAPSERVICES_116 {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://sjyg.com/oa2erp/ZFMM116/if", ConfigurationName="SAPSERVICES_116.SI_ZFMM116_OUT")]
    public interface SI_ZFMM116_OUT {
        
        // CODEGEN: 操作 SI_ZFMM116_OUT 以后生成的消息协定不是 RPC，也不是换行文档。
        [System.ServiceModel.OperationContractAttribute(Action="http://sap.com/xi/WebService/soap1.1", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        EOS.WebService.SAPSERVICES_116.SI_ZFMM116_OUTResponse SI_ZFMM116_OUT(EOS.WebService.SAPSERVICES_116.SI_ZFMM116_OUTRequest request);
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.3190.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="urn:sap-com:document:sap:rfc:functions")]
    public partial class ZFMM116 : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string i_EBELNField;
        
        private string i_LIFNRField;
        
        private string i_MATNRField;
        
        private string i_PROCSTATField;
        
        private string i_WERKSField;
        
        private string i_ZAEDATField;
        
        private string i_ZBUDATField;
        
        private ZSMM116[] t_OUTTABField;
        
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
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string I_PROCSTAT {
            get {
                return this.i_PROCSTATField;
            }
            set {
                this.i_PROCSTATField = value;
                this.RaisePropertyChanged("I_PROCSTAT");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string I_WERKS {
            get {
                return this.i_WERKSField;
            }
            set {
                this.i_WERKSField = value;
                this.RaisePropertyChanged("I_WERKS");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string I_ZAEDAT {
            get {
                return this.i_ZAEDATField;
            }
            set {
                this.i_ZAEDATField = value;
                this.RaisePropertyChanged("I_ZAEDAT");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string I_ZBUDAT {
            get {
                return this.i_ZBUDATField;
            }
            set {
                this.i_ZBUDATField = value;
                this.RaisePropertyChanged("I_ZBUDAT");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("item", Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=false)]
        public ZSMM116[] T_OUTTAB {
            get {
                return this.t_OUTTABField;
            }
            set {
                this.t_OUTTABField = value;
                this.RaisePropertyChanged("T_OUTTAB");
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
    public partial class ZSMM116 : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string zBUDATField;
        
        private string zAEDATField;
        
        private string pROCSTATField;
        
        private string eBELNField;
        
        private string mATNRField;
        
        private string lIFNRField;
        
        private string eBELPField;
        
        private string bSARTField;
        
        private string nAME1Field;
        
        private string wERKSField;
        
        private string eKORGField;
        
        private string eKGRPField;
        
        private string tXZ01Field;
        
        private decimal mENGEField;
        
        private bool mENGEFieldSpecified;
        
        private string mEINSField;
        
        private decimal nETPRField;
        
        private bool nETPRFieldSpecified;
        
        private decimal pEINHField;
        
        private bool pEINHFieldSpecified;
        
        private string bPRMEField;
        
        private string uEBTKField;
        
        private decimal uEBTOField;
        
        private bool uEBTOFieldSpecified;
        
        private string lOEKZField;
        
        private string mTARTField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public string ZBUDAT {
            get {
                return this.zBUDATField;
            }
            set {
                this.zBUDATField = value;
                this.RaisePropertyChanged("ZBUDAT");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=1)]
        public string ZAEDAT {
            get {
                return this.zAEDATField;
            }
            set {
                this.zAEDATField = value;
                this.RaisePropertyChanged("ZAEDAT");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=2)]
        public string PROCSTAT {
            get {
                return this.pROCSTATField;
            }
            set {
                this.pROCSTATField = value;
                this.RaisePropertyChanged("PROCSTAT");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=3)]
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
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=4)]
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
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=5)]
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
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=6)]
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
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=7)]
        public string BSART {
            get {
                return this.bSARTField;
            }
            set {
                this.bSARTField = value;
                this.RaisePropertyChanged("BSART");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=8)]
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
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=9)]
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
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=10)]
        public string EKORG {
            get {
                return this.eKORGField;
            }
            set {
                this.eKORGField = value;
                this.RaisePropertyChanged("EKORG");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=11)]
        public string EKGRP {
            get {
                return this.eKGRPField;
            }
            set {
                this.eKGRPField = value;
                this.RaisePropertyChanged("EKGRP");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=12)]
        public string TXZ01 {
            get {
                return this.tXZ01Field;
            }
            set {
                this.tXZ01Field = value;
                this.RaisePropertyChanged("TXZ01");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=13)]
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
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=14)]
        public string MEINS {
            get {
                return this.mEINSField;
            }
            set {
                this.mEINSField = value;
                this.RaisePropertyChanged("MEINS");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=15)]
        public decimal NETPR {
            get {
                return this.nETPRField;
            }
            set {
                this.nETPRField = value;
                this.RaisePropertyChanged("NETPR");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool NETPRSpecified {
            get {
                return this.nETPRFieldSpecified;
            }
            set {
                this.nETPRFieldSpecified = value;
                this.RaisePropertyChanged("NETPRSpecified");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=16)]
        public decimal PEINH {
            get {
                return this.pEINHField;
            }
            set {
                this.pEINHField = value;
                this.RaisePropertyChanged("PEINH");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool PEINHSpecified {
            get {
                return this.pEINHFieldSpecified;
            }
            set {
                this.pEINHFieldSpecified = value;
                this.RaisePropertyChanged("PEINHSpecified");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=17)]
        public string BPRME {
            get {
                return this.bPRMEField;
            }
            set {
                this.bPRMEField = value;
                this.RaisePropertyChanged("BPRME");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=18)]
        public string UEBTK {
            get {
                return this.uEBTKField;
            }
            set {
                this.uEBTKField = value;
                this.RaisePropertyChanged("UEBTK");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=19)]
        public decimal UEBTO {
            get {
                return this.uEBTOField;
            }
            set {
                this.uEBTOField = value;
                this.RaisePropertyChanged("UEBTO");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool UEBTOSpecified {
            get {
                return this.uEBTOFieldSpecified;
            }
            set {
                this.uEBTOFieldSpecified = value;
                this.RaisePropertyChanged("UEBTOSpecified");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=20)]
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
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=21)]
        public string MTART {
            get {
                return this.mTARTField;
            }
            set {
                this.mTARTField = value;
                this.RaisePropertyChanged("MTART");
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
    public partial class ZFMM116Response : object, System.ComponentModel.INotifyPropertyChanged {
        
        private ZSMM116[] t_OUTTABField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("item", Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=false)]
        public ZSMM116[] T_OUTTAB {
            get {
                return this.t_OUTTABField;
            }
            set {
                this.t_OUTTABField = value;
                this.RaisePropertyChanged("T_OUTTAB");
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
    public partial class SI_ZFMM116_OUTRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="urn:sap-com:document:sap:rfc:functions", Order=0)]
        public EOS.WebService.SAPSERVICES_116.ZFMM116 ZFMM116;
        
        public SI_ZFMM116_OUTRequest() {
        }
        
        public SI_ZFMM116_OUTRequest(EOS.WebService.SAPSERVICES_116.ZFMM116 ZFMM116) {
            this.ZFMM116 = ZFMM116;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class SI_ZFMM116_OUTResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="ZFMM116.Response", Namespace="urn:sap-com:document:sap:rfc:functions", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute("ZFMM116.Response")]
        public EOS.WebService.SAPSERVICES_116.ZFMM116Response ZFMM116Response;
        
        public SI_ZFMM116_OUTResponse() {
        }
        
        public SI_ZFMM116_OUTResponse(EOS.WebService.SAPSERVICES_116.ZFMM116Response ZFMM116Response) {
            this.ZFMM116Response = ZFMM116Response;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface SI_ZFMM116_OUTChannel : EOS.WebService.SAPSERVICES_116.SI_ZFMM116_OUT, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class SI_ZFMM116_OUTClient : System.ServiceModel.ClientBase<EOS.WebService.SAPSERVICES_116.SI_ZFMM116_OUT>, EOS.WebService.SAPSERVICES_116.SI_ZFMM116_OUT {
        
        public SI_ZFMM116_OUTClient() {
        }
        
        public SI_ZFMM116_OUTClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public SI_ZFMM116_OUTClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public SI_ZFMM116_OUTClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public SI_ZFMM116_OUTClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        EOS.WebService.SAPSERVICES_116.SI_ZFMM116_OUTResponse EOS.WebService.SAPSERVICES_116.SI_ZFMM116_OUT.SI_ZFMM116_OUT(EOS.WebService.SAPSERVICES_116.SI_ZFMM116_OUTRequest request) {
            return base.Channel.SI_ZFMM116_OUT(request);
        }
        
        public EOS.WebService.SAPSERVICES_116.ZFMM116Response SI_ZFMM116_OUT(EOS.WebService.SAPSERVICES_116.ZFMM116 ZFMM116) {
            EOS.WebService.SAPSERVICES_116.SI_ZFMM116_OUTRequest inValue = new EOS.WebService.SAPSERVICES_116.SI_ZFMM116_OUTRequest();
            inValue.ZFMM116 = ZFMM116;
            EOS.WebService.SAPSERVICES_116.SI_ZFMM116_OUTResponse retVal = ((EOS.WebService.SAPSERVICES_116.SI_ZFMM116_OUT)(this)).SI_ZFMM116_OUT(inValue);
            return retVal.ZFMM116Response;
        }
    }
}
