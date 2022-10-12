//文本框
UE.plugins['formtext'] = function () {
    var me = this, thePlugins = 'formtext';
    me.commands[thePlugins] = {
        execCommand: function (e) {
            var FrmTable = GetQuery('FrmTable')
            UEDialog(this, "/WorkflowModule/FormAttribute/TextAttribute?FrmTable=" + escape(FrmTable), thePlugins, "文本框", 600, 300);
        }
    };
    var popup = new baidu.editor.ui.Popup({
        editor: this,
        content: '',
        className: 'edui-bubble',
        _edittext: function () {
            baidu.editor.plugins[thePlugins].editdom = popup.anchorEl;
            me.execCommand(thePlugins);
            this.hide();
        },
        _delete: function () {
            if (window.confirm('确认删除该控件吗？')) {
                baidu.editor.dom.domUtils.remove(this.anchorEl, false);
            }
            this.hide();
        }
    });
    popup.render();
    me.addListener('mouseover', function (t, evt) {
        evt = evt || window.event;
        var el = evt.target || evt.srcElement;
        var type1 = el.getAttribute('type1');
        if (/input/ig.test(el.tagName) && type1 == "flow_" + thePlugins.replace('form', '')) {
            var html = popup.formatHtml('<nobr>文本框: <span onclick=$$._edittext() class="edui-clickable">编辑</span>&nbsp;&nbsp;<span onclick=$$._delete() class="edui-clickable">删除</span></nobr>');
            popup.getDom('content').innerHTML = html;
            popup.anchorEl = el;
            popup.showAnchor(popup.anchorEl);
        }
    });
};
//文本域
UE.plugins['formtextarea'] = function () {
    var me = this, thePlugins = 'formtextarea';
    me.commands[thePlugins] = {
        execCommand: function () {
            var dialog = new UE.ui.Dialog({
                iframeUrl: this.options.UEDITOR_HOME_URL + 'plugins/dialogs/textarea.aspx',
                name: thePlugins + '_' + (new Date().valueOf()),
                editor: this,
                title: '文本域',
                cssRules: "width:600px;height:300px;",
                buttons: [
				{
				    className: 'edui-okbutton',
				    label: '确定',
				    onclick: function () {
				        dialog.close(true);
				    }
				},
				{
				    className: 'edui-cancelbutton',
				    label: '取消',
				    onclick: function () {
				        dialog.close(false);
				    }
				}]
            });
            dialog.render();
            dialog.open();
        }
    };
    var popup = new baidu.editor.ui.Popup({
        editor: this,
        content: '',
        className: 'edui-bubble',
        _edittext: function () {
            baidu.editor.plugins[thePlugins].editdom = popup.anchorEl;
            me.execCommand(thePlugins);
            this.hide();
        },
        _delete: function () {
            if (window.confirm('确认删除该控件吗？')) {
                baidu.editor.dom.domUtils.remove(this.anchorEl, false);
            }
            this.hide();
        }
    });
    popup.render();
    me.addListener('mouseover', function (t, evt) {
        evt = evt || window.event;
        var el = evt.target || evt.srcElement;
        var type1 = el.getAttribute('type1');
        if (/input/ig.test(el.tagName) && type1 == "flow_" + thePlugins.replace('form', '')) {
            var html = popup.formatHtml('<nobr>文本域: <span onclick=$$._edittext() class="edui-clickable">编辑</span>&nbsp;&nbsp;<span onclick=$$._delete() class="edui-clickable">删除</span></nobr>');
            if (html) {
                popup.getDom('content').innerHTML = html;
                popup.anchorEl = el;
                popup.showAnchor(popup.anchorEl);
            } else {
                popup.hide();
            }
        }
    });
};
//单选按钮组
UE.plugins['formradio'] = function () {
    var me = this, thePlugins = 'formradio';
    me.commands[thePlugins] = {
        execCommand: function (e) {
            var FrmTable = GetQuery('FrmTable')
            UEDialog(this, "/WorkflowModule/FormAttribute/RadioAttribute?FrmTable=" + escape(FrmTable), thePlugins, "单选框", 600, 300);
        }
    };
    var popup = new baidu.editor.ui.Popup({
        editor: this,
        content: '',
        className: 'edui-bubble',
        _edittext: function () {
            baidu.editor.plugins[thePlugins].editdom = popup.anchorEl;
            me.execCommand(thePlugins);
            this.hide();
        },
        _delete: function () {
            if (window.confirm('确认删除该控件吗？')) {
                baidu.editor.dom.domUtils.remove(this.anchorEl, false);
            }
            this.hide();
        }
    });
    popup.render();
    me.addListener('mouseover', function (t, evt) {
        evt = evt || window.event;
        var el = evt.target || evt.srcElement;
        var type1 = el.getAttribute('type1');
        if (/input/ig.test(el.tagName) && type1 == "flow_" + thePlugins.replace('form', '')) {
            //$('.edui-popup-content').hide();
            var html = popup.formatHtml('<nobr>单选框: <span onclick=$$._edittext() class="edui-clickable">编辑</span>&nbsp;&nbsp;<span onclick=$$._delete() class="edui-clickable">删除</span></nobr>');
            if (html) {
                popup.getDom('content').innerHTML = html;
                popup.anchorEl = el;
                popup.showAnchor(popup.anchorEl);
            } else {
                popup.hide();
            }
        }
    });
};
//复选按钮组
UE.plugins['formcheckbox'] = function () {
    var me = this, thePlugins = 'formcheckbox';
    me.commands[thePlugins] = {
        execCommand: function () {
            var FrmTable = GetQuery('FrmTable')
            UEDialog(this, "/WorkflowModule/FormAttribute/CheckboxAttribute?FrmTable=" + escape(FrmTable), thePlugins, "复选框", 600, 300);
        }
    };
    var popup = new baidu.editor.ui.Popup({
        editor: this,
        content: '',
        className: 'edui-bubble',
        _edittext: function () {
            baidu.editor.plugins[thePlugins].editdom = popup.anchorEl;
            me.execCommand(thePlugins);
            this.hide();
        },
        _delete: function () {
            if (window.confirm('确认删除该控件吗？')) {
                baidu.editor.dom.domUtils.remove(this.anchorEl, false);
            }
            this.hide();
        }
    });
    popup.render();
    me.addListener('mouseover', function (t, evt) {
        evt = evt || window.event;
        var el = evt.target || evt.srcElement;
        var type1 = el.getAttribute('type1');
        if (/input/ig.test(el.tagName) && type1 == "flow_" + thePlugins.replace('form', '')) {
            var html = popup.formatHtml('<nobr>复选框: <span onclick=$$._edittext() class="edui-clickable">编辑</span>&nbsp;&nbsp;<span onclick=$$._delete() class="edui-clickable">删除</span></nobr>');
            if (html) {
                popup.getDom('content').innerHTML = html;
                popup.anchorEl = el;
                popup.showAnchor(popup.anchorEl);
            } else {
                popup.hide();
            }
        }
    });
};
//日期时间选择
UE.plugins['formdatetime'] = function () {
    var me = this, thePlugins = 'formdatetime';
    me.commands[thePlugins] = {
        execCommand: function (e) {
            var FrmTable = GetQuery('FrmTable')
            UEDialog(this, "/WorkflowModule/FormAttribute/DateTimeAttribute?FrmTable=" + escape(FrmTable), "formtext", "时间选择", 600, 300);
        }
    };
    var popup = new baidu.editor.ui.Popup({
        editor: this,
        content: '',
        className: 'edui-bubble',
        _edittext: function () {
            baidu.editor.plugins[thePlugins].editdom = popup.anchorEl;
            me.execCommand(thePlugins);
            this.hide();
        },
        _delete: function () {
            if (window.confirm('确认删除该控件吗？')) {
                baidu.editor.dom.domUtils.remove(this.anchorEl, false);
            }
            this.hide();
        }
    });
    popup.render();
    me.addListener('mouseover', function (t, evt) {
        evt = evt || window.event;
        var el = evt.target || evt.srcElement;
        var type1 = el.getAttribute('type1');
        if (/input/ig.test(el.tagName) && type1 == "flow_" + thePlugins.replace('form', '')) {
            var html = popup.formatHtml('<nobr>日期时间选择: <span onclick=$$._edittext() class="edui-clickable">编辑</span>&nbsp;&nbsp;<span onclick=$$._delete() class="edui-clickable">删除</span></nobr>');
            if (html) {
                popup.getDom('content').innerHTML = html;
                popup.anchorEl = el;
                popup.showAnchor(popup.anchorEl);
            } else {
                popup.hide();
            }
        }
    });
};
//隐藏域
UE.plugins['formhidden'] = function () {
    var me = this, thePlugins = 'formhidden';
    me.commands[thePlugins] = {
        execCommand: function (e) {
            var FrmTable = GetQuery('FrmTable')
            UEDialog(this, "/WorkflowModule/FormAttribute/HiddenAttribute?FrmTable=" + escape(FrmTable), "formtext", "隐藏域", 600, 200);
        }
    };
    var popup = new baidu.editor.ui.Popup({
        editor: this,
        content: '',
        className: 'edui-bubble',
        _edittext: function () {
            baidu.editor.plugins[thePlugins].editdom = popup.anchorEl;
            me.execCommand(thePlugins);
            this.hide();
        },
        _delete: function () {
            if (window.confirm('确认删除该控件吗？')) {
                baidu.editor.dom.domUtils.remove(this.anchorEl, false);
            }
            this.hide();
        }
    });
    var popup = new baidu.editor.ui.Popup({
        editor: this,
        content: '',
        className: 'edui-bubble',
        _edittext: function () {
            baidu.editor.plugins[thePlugins].editdom = popup.anchorEl;
            me.execCommand(thePlugins);
            this.hide();
        },
        _delete: function () {
            if (window.confirm('确认删除该控件吗？')) {
                baidu.editor.dom.domUtils.remove(this.anchorEl, false);
            }
            this.hide();
        }
    });
    popup.render();
    me.addListener('mouseover', function (t, evt) {
        evt = evt || window.event;
        var el = evt.target || evt.srcElement;
        var type1 = el.getAttribute('type1');
        if (/input/ig.test(el.tagName) && type1 == "flow_" + thePlugins.replace('form', '')) {
            var html = popup.formatHtml('<nobr>隐藏域: <span onclick=$$._edittext() class="edui-clickable">编辑</span>&nbsp;&nbsp;<span onclick=$$._delete() class="edui-clickable">删除</span></nobr>');
            if (html) {
                popup.getDom('content').innerHTML = html;
                popup.anchorEl = el;
                popup.showAnchor(popup.anchorEl);
            } else {
                popup.hide();
            }
        }
    });
};
//下拉框
UE.plugins['formselect'] = function () {
    var me = this, thePlugins = 'formselect';
    me.commands[thePlugins] = {
        execCommand: function (e) {
            var FrmTable = GetQuery('FrmTable')
            UEDialog(this, "/WorkflowModule/FormAttribute/SelectAttribute?FrmTable=" + escape(FrmTable), thePlugins, "下拉框", 600, 380);
        }
    };
    var popup = new baidu.editor.ui.Popup({
        editor: this,
        content: '',
        className: 'edui-bubble',
        _edittext: function () {
            baidu.editor.plugins[thePlugins].editdom = popup.anchorEl;
            me.execCommand(thePlugins);
            this.hide();
        },
        _delete: function () {
            if (window.confirm('确认删除该控件吗？')) {
                baidu.editor.dom.domUtils.remove(this.anchorEl, false);
            }
            this.hide();
        }
    });
    popup.render();
    me.addListener('mouseover', function (t, evt) {
        evt = evt || window.event;
        var el = evt.target || evt.srcElement;
        var type1 = el.getAttribute('type1');
        if (/select/ig.test(el.tagName) && type1 == "flow_" + thePlugins.replace('form', '')) {
            var html = popup.formatHtml('<nobr>下拉列表框: <span onclick=$$._edittext() class="edui-clickable">编辑</span>&nbsp;&nbsp;<span onclick=$$._delete() class="edui-clickable">删除</span></nobr>');
            if (html) {
                popup.getDom('content').innerHTML = html;
                popup.anchorEl = el;
                popup.showAnchor(popup.anchorEl);
            } else {
                popup.hide();
            }
        }
    });
};
//按钮
UE.plugins['formbutton'] = function () {
    var me = this, thePlugins = 'formbutton';
    me.commands[thePlugins] = {
        execCommand: function (e) {
            var FrmTable = GetQuery('FrmTable')
            UEDialog(this, "/WorkflowModule/FormAttribute/ButtonAttribute?FrmTable=" + escape(FrmTable), thePlugins, "按钮", 600, 300);
        }
    };
    var popup = new baidu.editor.ui.Popup({
        editor: this,
        content: '',
        className: 'edui-bubble',
        _edittext: function () {
            baidu.editor.plugins[thePlugins].editdom = popup.anchorEl;
            me.execCommand(thePlugins);
            this.hide();
        },
        _delete: function () {
            if (window.confirm('确认删除该控件吗？')) {
                baidu.editor.dom.domUtils.remove(this.anchorEl, false);
            }
            this.hide();
        }
    });
    popup.render();
    me.addListener('mouseover', function (t, evt) {
        evt = evt || window.event;
        var el = evt.target || evt.srcElement;
        var type1 = el.getAttribute('type1');
        if (/input/ig.test(el.tagName) && type1 == "flow_" + thePlugins.replace('form', '')) {
            var html = popup.formatHtml('<nobr>按钮: <span onclick=$$._edittext() class="edui-clickable">编辑</span>&nbsp;&nbsp;<span onclick=$$._delete() class="edui-clickable">删除</span></nobr>');
            if (html) {
                popup.getDom('content').innerHTML = html;
                popup.anchorEl = el;
                popup.showAnchor(popup.anchorEl);
            } else {
                popup.hide();
            }
        }
    });
};



//自定义函数弹出窗口
function UEDialog(editor, url, id, title, width, height) {
    var dialog = new UE.ui.Dialog({
        iframeUrl: url,
        name: id + '_' + (new Date().valueOf()),
        editor: editor,
        title: title,
        cssRules: "width:" + width + "px;height:" + height + "px;",
        buttons: [
        {
            className: 'edui-okbutton',
            label: '确定',
            onclick: function () {
                dialog.close(true);
            }
        },
        {
            className: 'edui-cancelbutton',
            label: '取消',
            onclick: function () {
                dialog.close(false);
            }
        }]
    });
    dialog.render();
    dialog.open();
}
