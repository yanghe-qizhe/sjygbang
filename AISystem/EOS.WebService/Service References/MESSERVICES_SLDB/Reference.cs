﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace EOS.WebService.MESSERVICES_SLDB {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://cost.soap.ws.sunshine.com/", ConfigurationName="MESSERVICES_SLDB.LoadometerWeighingTransferWS")]
    public interface LoadometerWeighingTransferWS {
        
        // CODEGEN: 操作 executeLoadometerWeighingTransfer 以后生成的消息协定不是 RPC，也不是换行文档。
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(baseRequest))]
        EOS.WebService.MESSERVICES_SLDB.executeLoadometerWeighingTransferResponse1 executeLoadometerWeighingTransfer(EOS.WebService.MESSERVICES_SLDB.executeLoadometerWeighingTransferRequest request);
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.3190.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://cost.soap.ws.sunshine.com/")]
    public partial class executeLoadometerWeighingTransfer : object, System.ComponentModel.INotifyPropertyChanged {
        
        private loadometerWeighingTransferRequest requestField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public loadometerWeighingTransferRequest request {
            get {
                return this.requestField;
            }
            set {
                this.requestField = value;
                this.RaisePropertyChanged("request");
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
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://cost.soap.ws.sunshine.com/")]
    public partial class loadometerWeighingTransferRequest : baseRequest {
        
        private loadometerWeighingTransferLineRequest[] lineRequestListField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("lineRequestList", Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=true, Order=0)]
        public loadometerWeighingTransferLineRequest[] lineRequestList {
            get {
                return this.lineRequestListField;
            }
            set {
                this.lineRequestListField = value;
                this.RaisePropertyChanged("lineRequestList");
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.3190.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://cost.soap.ws.sunshine.com/")]
    public partial class loadometerWeighingTransferLineRequest : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string batchField;
        
        private string dateTimeField;
        
        private string itemField;
        
        private string qtyField;
        
        private string storageLocationField;
        
        private string transferTypeField;
        
        private string vendorField;
        
        private string workShopField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public string batch {
            get {
                return this.batchField;
            }
            set {
                this.batchField = value;
                this.RaisePropertyChanged("batch");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=1)]
        public string dateTime {
            get {
                return this.dateTimeField;
            }
            set {
                this.dateTimeField = value;
                this.RaisePropertyChanged("dateTime");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=2)]
        public string item {
            get {
                return this.itemField;
            }
            set {
                this.itemField = value;
                this.RaisePropertyChanged("item");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=3)]
        public string qty {
            get {
                return this.qtyField;
            }
            set {
                this.qtyField = value;
                this.RaisePropertyChanged("qty");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=4)]
        public string storageLocation {
            get {
                return this.storageLocationField;
            }
            set {
                this.storageLocationField = value;
                this.RaisePropertyChanged("storageLocation");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=5)]
        public string transferType {
            get {
                return this.transferTypeField;
            }
            set {
                this.transferTypeField = value;
                this.RaisePropertyChanged("transferType");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=6)]
        public string vendor {
            get {
                return this.vendorField;
            }
            set {
                this.vendorField = value;
                this.RaisePropertyChanged("vendor");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=7)]
        public string workShop {
            get {
                return this.workShopField;
            }
            set {
                this.workShopField = value;
                this.RaisePropertyChanged("workShop");
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
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://cost.soap.ws.sunshine.com/")]
    public partial class baseResponse : object, System.ComponentModel.INotifyPropertyChanged {
        
        private long codeField;
        
        private string messageField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public long code {
            get {
                return this.codeField;
            }
            set {
                this.codeField = value;
                this.RaisePropertyChanged("code");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=1)]
        public string message {
            get {
                return this.messageField;
            }
            set {
                this.messageField = value;
                this.RaisePropertyChanged("message");
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
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://cost.soap.ws.sunshine.com/")]
    public partial class executeLoadometerWeighingTransferResponse : object, System.ComponentModel.INotifyPropertyChanged {
        
        private baseResponse returnField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public baseResponse @return {
            get {
                return this.returnField;
            }
            set {
                this.returnField = value;
                this.RaisePropertyChanged("return");
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
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(loadometerWeighingTransferRequest))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.3190.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://cost.soap.ws.sunshine.com/")]
    public partial class baseRequest : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string siteField;
        
        private string userIdField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public string site {
            get {
                return this.siteField;
            }
            set {
                this.siteField = value;
                this.RaisePropertyChanged("site");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=1)]
        public string userId {
            get {
                return this.userIdField;
            }
            set {
                this.userIdField = value;
                this.RaisePropertyChanged("userId");
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
    public partial class executeLoadometerWeighingTransferRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://cost.soap.ws.sunshine.com/", Order=0)]
        public EOS.WebService.MESSERVICES_SLDB.executeLoadometerWeighingTransfer executeLoadometerWeighingTransfer;
        
        public executeLoadometerWeighingTransferRequest() {
        }
        
        public executeLoadometerWeighingTransferRequest(EOS.WebService.MESSERVICES_SLDB.executeLoadometerWeighingTransfer executeLoadometerWeighingTransfer) {
            this.executeLoadometerWeighingTransfer = executeLoadometerWeighingTransfer;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class executeLoadometerWeighingTransferResponse1 {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://cost.soap.ws.sunshine.com/", Order=0)]
        public EOS.WebService.MESSERVICES_SLDB.executeLoadometerWeighingTransferResponse executeLoadometerWeighingTransferResponse;
        
        public executeLoadometerWeighingTransferResponse1() {
        }
        
        public executeLoadometerWeighingTransferResponse1(EOS.WebService.MESSERVICES_SLDB.executeLoadometerWeighingTransferResponse executeLoadometerWeighingTransferResponse) {
            this.executeLoadometerWeighingTransferResponse = executeLoadometerWeighingTransferResponse;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface LoadometerWeighingTransferWSChannel : EOS.WebService.MESSERVICES_SLDB.LoadometerWeighingTransferWS, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class LoadometerWeighingTransferWSClient : System.ServiceModel.ClientBase<EOS.WebService.MESSERVICES_SLDB.LoadometerWeighingTransferWS>, EOS.WebService.MESSERVICES_SLDB.LoadometerWeighingTransferWS {
        
        public LoadometerWeighingTransferWSClient() {
        }
        
        public LoadometerWeighingTransferWSClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public LoadometerWeighingTransferWSClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public LoadometerWeighingTransferWSClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public LoadometerWeighingTransferWSClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        EOS.WebService.MESSERVICES_SLDB.executeLoadometerWeighingTransferResponse1 EOS.WebService.MESSERVICES_SLDB.LoadometerWeighingTransferWS.executeLoadometerWeighingTransfer(EOS.WebService.MESSERVICES_SLDB.executeLoadometerWeighingTransferRequest request) {
            return base.Channel.executeLoadometerWeighingTransfer(request);
        }
        
        public EOS.WebService.MESSERVICES_SLDB.executeLoadometerWeighingTransferResponse executeLoadometerWeighingTransfer(EOS.WebService.MESSERVICES_SLDB.executeLoadometerWeighingTransfer executeLoadometerWeighingTransfer1) {
            EOS.WebService.MESSERVICES_SLDB.executeLoadometerWeighingTransferRequest inValue = new EOS.WebService.MESSERVICES_SLDB.executeLoadometerWeighingTransferRequest();
            inValue.executeLoadometerWeighingTransfer = executeLoadometerWeighingTransfer1;
            EOS.WebService.MESSERVICES_SLDB.executeLoadometerWeighingTransferResponse1 retVal = ((EOS.WebService.MESSERVICES_SLDB.LoadometerWeighingTransferWS)(this)).executeLoadometerWeighingTransfer(inValue);
            return retVal.executeLoadometerWeighingTransferResponse;
        }
    }
}
