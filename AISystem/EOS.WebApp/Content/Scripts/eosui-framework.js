/**
* jQuery EOSUI 4.1
* Copyright © EOS 2014
*/
//样式
$(function () {
    readyIndex();
    //    setTimeout(function () {
    //       // Loading(false);
    //    }, 2000);
})
function readyIndex() {
    $(".main_menu li div").click(function () {
        $(".main_menu li div").removeClass('main_menu leftselected');
        $(this).addClass('main_menu leftselected');
    }).hover(function () {
        $(this).addClass("hoverleftselected");
    }, function () {
        $(this).removeClass("hoverleftselected");
    });
}
/**
加载布局
**/
function Loadlayout() {
    if ($('.layout').length > 0) {
        $("#layout").splitter({
            type: "v",
            outline: true,
            minLeft: 150, sizeLeft: 200, maxLeft: 350,
            anchorToWindow: true,
            accessKey: "L"
        });
    }
}
//Tab标签切换
function Tabchange(id) {
    $('.ScrollBar').find('.tabPanel').hide();
    $('.ScrollBar').find("#" + id).show();
    $(".tab_list_top div").removeClass("actived");
    $(".tab_list_top").find("#Tab" + id).addClass("actived"); //添加选中样式  
}
function thisTabchange(e, id) {
    $(e).parent().find('div').removeClass("actived");
    $(e).addClass("actived");
    $('.tabPanel').hide();
    $("#" + id).show();
}
function standTabchange(e, id) {
    $(e).parent().find('div').removeClass("standtabactived");
    $(e).addClass("standtabactived");
    $(e).parent().next().children('div').hide();
    $("#" + id).show();
}
/********
接收地址栏参数
**********/
function GetQuery(key) {
    var search = location.search.slice(1); //得到get方式提交的查询字符串
    var arr = search.split("&");
    for (var i = 0; i < arr.length; i++) {
        var ar = arr[i].split("=");
        if (ar[0] == key) {
            if (unescape(ar[1]) == 'undefined') {
                return "";
            } else {
                return unescape(ar[1]);
            }
        }
    }
    return "";
}
/*
获取动态tab标签当前iframeID
*/
function tabiframeId() {
    var tabs_container = top.$("#tabs_container");
    return "tabs_iframe_" + tabs_container.find('.selected').attr('id').substr(5);
}
//关闭当前tab
function btn_back() {
    top.ThisCloseTab();
}
/*
中间加载对话窗
*/
function Loading(bool, text) {
    var ajaxbg = top.$("#loading_background,#loading");
    if (!!text) {
        top.$("#loading").css("left", (top.$('body').width() - top.$("#loading").width()) / 2);
        top.$("#loading span").html(text);
    } else {
        top.$("#loading").css("left", (top.$('body').width() - top.$("#loading").width()) / 2);
        top.$("#loading span").html("正在拼了命为您加载…");
    }
    if (bool) {
        ajaxbg.show();
    } else {
        ajaxbg.hide();
    }
}
/* 
请求Ajax 带返回值
*/
function getAjax(url, postData, callBack) {
    $.ajax({
        type: 'post',
        dataType: "text",
        url: RootPath() + url,
        data: postData,
        cache: false,
        async: false,
        success: function (data) {
            callBack(data);
            Loading(false);
        },
        error: function (data) {
            alert("error:" + JSON.stringify(data));
            Loading(false);
        }
    });
}
function AjaxJson(url, postData, callBack) {
    try {
        $.ajax({
            url: RootPath() + url,
            type: "post",
            data: postData,
            dataType: "json",
            async: false,
            success: function (data) {
                if (data.Code == "-1") {
                    Loading(false);
                    alertDialog(data.Message, -1);
                } else {
                    Loading(false);
                    callBack(data);
                }
            },
            error: function (data) {
                Loading(false);
                alertDialog(data.responseText, -1);
            }
        });
    } catch (e) {

    }
}
/*
刷新当前页面
*/
function Replace() {
    location.reload();
    return false;
}
/*
href跳转页面
*/
function Urlhref(url) {
    location.href = url;
    return false;
}
/*
iframe同步连接
*/
function iframe_src(iframe, src) {
    //  Loading(true);
    $("#" + iframe).attr('src', RootPath() + src);
    $("#" + iframe).load(function () {
        // Loading(false);
    });
}
/*
grid表格扩展Begin
*/

/**获取表格选择行
**/
function GetJqGridRowIndx(jgrid) {
    return $(jgrid).jqGrid('getGridParam', 'selrow')
}
/**获取JqGrid表格列值
jgrid：ID，code：列字段
**/
function GetJqGridRowValue(jgrid, code) {
    var KeyValue = "";
    var selectedRowIds = $(jgrid).jqGrid("getGridParam", "selarrrow");
    if (selectedRowIds != "") {
        var len = selectedRowIds.length;
        for (var i = 0; i < len; i++) {
            var rowData = $(jgrid).jqGrid('getRowData', selectedRowIds[i]);
            KeyValue += rowData[code] + ",";
        }
        KeyValue = KeyValue.substr(0, KeyValue.length - 1);
    } else {
        var rowData = $(jgrid).jqGrid('getRowData', $(jgrid).jqGrid('getGridParam', 'selrow'));
        KeyValue = rowData[code];
    }
    return KeyValue;
}
/**获取JqGrid表格列值
jgrid：ID，RowIndx:行ID,code：列字段
**/
function GetJqGridValue(jgrid, RowIndx, code) {
    var rowData = $(jgrid).jqGrid('getRowData', RowIndx);
    return rowData[code];
}

/**grid表格扩展end**/


/**
格式化时间显示方式、用法:format="yyyy-MM-dd hh:mm:ss";
*/
formatDate = function (v, format) {
    if (!v) return "";
    var d = v;
    if (typeof v === 'string') {
        if (v.indexOf("/Date(") > -1)
            d = new Date(parseInt(v.replace("/Date(", "").replace(")/", ""), 10));
        else
            d = new Date(Date.parse(v.replace(/-/g, "/").replace("T", " ").split(".")[0])); //.split(".")[0] 用来处理出现毫秒的情况，截取掉.xxx，否则会出错
    }
    var o = {
        "M+": d.getMonth() + 1,  //month
        "d+": d.getDate(),       //day
        "h+": d.getHours(),      //hour
        "m+": d.getMinutes(),    //minute
        "s+": d.getSeconds(),    //second
        "q+": Math.floor((d.getMonth() + 3) / 3),  //quarter
        "S": d.getMilliseconds() //millisecond
    };
    if (/(y+)/.test(format)) {
        format = format.replace(RegExp.$1, (d.getFullYear() + "").substr(4 - RegExp.$1.length));
    }
    for (var k in o) {
        if (new RegExp("(" + k + ")").test(format)) {
            format = format.replace(RegExp.$1, RegExp.$1.length == 1 ? o[k] : ("00" + o[k]).substr(("" + o[k]).length));
        }
    }
    return format;
};
/**
当前时间
*/
function CurrentTime() {
    var date = new Date();
    var year = date.getFullYear();
    var month = date.getMonth() + 1;
    var day = date.getDate();
    var hour = date.getHours();
    var minute = date.getMinutes();
    var second = date.getSeconds();
    return year + '-' + month + '-' + day + ' ' + hour + ':' + minute;
}
String.prototype.replaceAll = function (s1, s2) {
    return this.replace(new RegExp(s1, "gm"), s2);
}

/*
自动获取页面控件值
*/
function GetWebControls(element) {
    var reVal = "";
    $(element).find('input,select,textarea').each(function (r) {
        var id = $(this).attr('id');
        var value = $(this).val();
        var type = $(this).attr('type');
        switch (type) {
            case "checkbox":
                if ($(this).attr("checked")) {
                    reVal += '"' + id + '"' + ':' + '"1",'
                } else {
                    reVal += '"' + id + '"' + ':' + '"0",'
                }
                break;
            default:
                if (value == "") {
                    value = "";
                }
                reVal += '"' + id + '"' + ':' + '"' + $.trim(value) + '",'
                break;
        }
    });
    reVal = reVal.substr(0, reVal.length - 1);
    reVal = reVal.replaceAll("\n", "NNNNNN");
    return jQuery.parseJSON('{' + reVal + '}');
}

/*
自动获取页面控件值
*/
function GetWebControlsForTrue(element) {
    var reVal = "";
    $(element).find('input[DBField=true],select[DBField=true],textarea[DBField=true]').each(function (r) {
        var id = $(this).attr('id');
        var value = $(this).val();
        var type = $(this).attr('type');
        switch (type) {
            case "checkbox":
                if ($(this).attr("checked")) {
                    reVal += '"' + id + '"' + ':' + '"1",'
                } else {
                    reVal += '"' + id + '"' + ':' + '"0",'
                }
                break;
            default:
                if (value == "") {
                    value = "&nbsp;";
                }
                reVal += '"' + id + '"' + ':' + '"' + $.trim(value) + '",'
                break;
        }
    });
    reVal = reVal.substr(0, reVal.length - 1);
    return jQuery.parseJSON('{' + reVal + '}');
}
/*
自动给控件赋值
*/
function SetWebControls(data) {
    try {
        for (var key in data) {
            var id = $('#' + key);
            if (id.attr('id')) {
                var value = $.trim(data[key]).replace("&nbsp;", "").replace(/</g, "<").replace(/>/, ">");
                var type = id.attr('type');
                switch (type) {
                    case "checkbox":
                        if (value == 1) {
                            id.attr("checked", 'checked');
                        } else {
                            id.removeAttr("checked");
                        }
                        break;
                    default:
                        id.val(value);
                        break;
                }
            }
        }
    } catch (e) {
        alert(e)
    }
}
/*
自动给控件赋值、对Lable
*/
function SetWebLable(data) {
    for (var key in data) {
        var id = $('#' + key);
        if (id.attr('id')) {
            var value = $.trim(data[key]).replace("&nbsp;", "");
            id.text(value);
        }
    }
}
/**
* 获取搜索条件：返回JSON
* var parameter = GetParameterJson("搜索区域table的ID");
*/
function GetParameterJson(tableId) {
    var parameter = "";
    $("#" + tableId + " tr").find('td').find('input,select').each(function () {
        var pk_id = $(this).attr('id');
        var pk_value = $("#" + pk_id).val();
        parameter += '"' + pk_id + '"' + ':' + '"' + $.trim(pk_value) + '",'
    });
    parameter = parameter.substr(0, parameter.length - 1);
    return '{' + parameter + '}';
}
/**
* 获取动态table：键、值，返回JSON
* var GetTableData = GetTableDataJson("table的ID");
*/
function GetTableDataJson(tableId) {
    var item_Key_Value = "";
    var index = 1;
    var trjson = "";
    if ($(tableId + " tbody tr").length > 0) {
        $(tableId + " tbody tr").each(function () {
            var tdjson = "";
            $(this).find('td').find('input,select,textarea').each(function () {
                var pk_id = $(this).attr('id');
                var pk_value = "";
                if ($("#" + pk_id).attr('type') == "checkbox") {
                    if ($("#" + pk_id).attr("checked")) {
                        pk_value = "1";
                    } else {
                        pk_value = "0";
                    }
                } else {
                    pk_value = $("#" + pk_id).val();
                }
                var array = new Array();
                array = pk_id.split("➩"); //字符分割
                tdjson += '"' + array[0] + '"' + ':' + '"' + $.trim(pk_value) + '",'
            })
            tdjson = tdjson.substr(0, tdjson.length - 1);
            trjson += '{' + tdjson + '},';
        });
    } else {
        $(tableId + " tr").each(function () {
            var tdjson = "";
            $(this).find('td').find('input,select,textarea').each(function () {
                var pk_id = $(this).attr('id');
                var pk_value = "";
                if ($("#" + pk_id).attr('type') == "checkbox") {
                    if ($("#" + pk_id).attr("checked")) {
                        pk_value = "1";
                    } else {
                        pk_value = "0";
                    }
                } else {
                    pk_value = $("#" + pk_id).val();
                }
                var array = new Array();
                array = pk_id.split("➩"); //字符分割
                tdjson += '"' + array[0] + '"' + ':' + '"' + $.trim(pk_value) + '",'
            })
            tdjson = tdjson.substr(0, tdjson.length - 1);
            trjson += '{' + tdjson + '},';
        });
    }
    trjson = trjson.substr(0, trjson.length - 1);
    if (trjson == '{}') {
        trjson = "";
    }
    return '[' + trjson + ']';
}

/**
获取选中复选框值
值：1,2,3,4
**/
function CheckboxValue() {
    var reVal = '';
    $('[type = checkbox]').each(function () {
        if ($(this).attr("checked")) {
            reVal += $(this).val() + ",";
        }
    });
    reVal = reVal.substr(0, reVal.length - 1);
    return reVal;
}
/**
文本框只允许输入数字
**/
function IsNumber(obj) {
    $("#" + obj).bind("contextmenu", function () {
        return false;
    });
    $("#" + obj).css('ime-mode', 'disabled');
    $("#" + obj).keypress(function (e) {
        if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
            return false;
        }
    });
}
/**
只能输入数字和小数点
**/
function clearNoNum(obj) {
    obj.value = obj.value.replace(/[^\d.]/g, "");  //清除“数字”和“.”以外的字符
    obj.value = obj.value.replace(/^\./g, "");  //验证第一个字符是数字而不是.
    obj.value = obj.value.replace(/\.{2,}/g, "."); //只保留第一个. 清除多余的.
    obj.value = obj.value.replace(".", "$#$").replace(/\./g, "").replace("$#$", ".");
}
/**
只能输入数字和小数点
**/
function IsMoney(obj) {
    $("#" + obj).bind("contextmenu", function () {
        return false;
    });
    $("#" + obj).css('ime-mode', 'disabled');
    $("#" + obj).bind("keydown", function (e) {
        var key = window.event ? e.keyCode : e.which;
        if (isFullStop(key)) {
            return $(this).val().indexOf('.') < 0;
        }
        return (isSpecialKey(key)) || ((isNumber(key) && !e.shiftKey));
    });
    function isNumber(key) {
        return (key >= 48 && key <= 57) || (key <= 105 && key >= 96)
    }
    function isSpecialKey(key) {
        return key == 8 || key == 46 || (key >= 37 && key <= 40) || key == 35 || key == 36 || key == 9 || key == 13
    }
    function isFullStop(key) {
        return key == 190 || key == 110;
    }
}
/**
只能输入数字和小数点 负数
**/
function IsMoneySubtract(obj) {
    $("#" + obj).bind("contextmenu", function () {
        return false;
    });
    $("#" + obj).css('ime-mode', 'disabled');
    $("#" + obj).bind("keydown", function (e) {
        var key = window.event ? e.keyCode : e.which;
        if (isFullStop(key)) {
            return $(this).val().indexOf('.') < 0;
        }
        if (isSubtract(key)) {
            return $(this).val().indexOf('-') < 0;
        }
        return (isSpecialKey(key)) || ((isNumber(key) && !e.shiftKey));
    });
    function isNumber(key) {
        return (key >= 48 && key <= 57) || (key <= 105 && key >= 96)
    }
    function isSpecialKey(key) {
        return key == 8 || key == 46 || (key >= 37 && key <= 40) || key == 35 || key == 36 || key == 9 || key == 13
    }
    function isFullStop(key) {
        return key == 190 || key == 110;
    }
    function isSubtract(key) {
        return key == 109 || key == 189;
    }
}
/**
* 金额格式(保留2位小数)后格式化成金额形式
*/
function FormatCurrency(num) {
    num = num.toString().replace(/\$|\,/g, '');
    if (isNaN(num))
        num = "0";
    sign = (num == (num = Math.abs(num)));
    num = Math.floor(num * 100 + 0.50000000001);
    cents = num % 100;
    num = Math.floor(num / 100).toString();
    if (cents < 10)
        cents = "0" + cents;
    for (var i = 0; i < Math.floor((num.length - (1 + i)) / 3); i++)
        num = num.substring(0, num.length - (4 * i + 3)) + '' +
                num.substring(num.length - (4 * i + 3));
    return (((sign) ? '' : '-') + num + '.' + cents);
}


/** 数字金额大写转换(可以处理整数,小数,负数) */
function ChargeChinese(currencyDigits) {
    // Constants: 
    var MAXIMUM_NUMBER = 99999999999.99;
    // Predefine the radix characters and currency symbols for output: 
    var CN_ZERO = "零";
    var CN_ONE = "壹";
    var CN_TWO = "贰";
    var CN_THREE = "叁";
    var CN_FOUR = "肆";
    var CN_FIVE = "伍";
    var CN_SIX = "陆";
    var CN_SEVEN = "柒";
    var CN_EIGHT = "捌";
    var CN_NINE = "玖";
    var CN_TEN = "拾";
    var CN_HUNDRED = "佰";
    var CN_THOUSAND = "仟";
    var CN_TEN_THOUSAND = "万";
    var CN_HUNDRED_MILLION = "亿";
    var CN_SYMBOL = "人民币";
    var CN_DOLLAR = "元";
    var CN_TEN_CENT = "角";
    var CN_CENT = "分";
    var CN_INTEGER = "整";

    // Variables: 
    var integral;    // Represent integral part of digit number. 
    var decimal;    // Represent decimal part of digit number. 
    var outputCharacters;    // The output result. 
    var parts;
    var digits, radices, bigRadices, decimals;
    var zeroCount;
    var i, p, d;
    var quotient, modulus;

    // Validate input string: 
    currencyDigits = currencyDigits.toString();

    // Normalize the format of input digits: 
    currencyDigits = currencyDigits.replace(/,/g, "");    // Remove comma delimiters. 
    currencyDigits = currencyDigits.replace(/^0+/, "");    // Trim zeros at the beginning. 
    // Assert the number is not greater than the maximum number. 
    if (Number(currencyDigits) > MAXIMUM_NUMBER) {
        alert("金额过大，应小于1000亿元！");
        return "";
    }

    // Process the coversion from currency digits to characters: 
    // Separate integral and decimal parts before processing coversion: 
    parts = currencyDigits.split(".");
    if (parts.length > 1) {
        integral = parts[0];
        decimal = parts[1];
        // Cut down redundant decimal digits that are after the second. 
        decimal = decimal.substr(0, 2);
    }
    else {
        integral = parts[0];
        decimal = "";
    }
    // Prepare the characters corresponding to the digits: 
    digits = new Array(CN_ZERO, CN_ONE, CN_TWO, CN_THREE, CN_FOUR, CN_FIVE, CN_SIX, CN_SEVEN, CN_EIGHT, CN_NINE);
    radices = new Array("", CN_TEN, CN_HUNDRED, CN_THOUSAND);
    bigRadices = new Array("", CN_TEN_THOUSAND, CN_HUNDRED_MILLION);
    decimals = new Array(CN_TEN_CENT, CN_CENT);
    // Start processing: 
    outputCharacters = "";
    // Process integral part if it is larger than 0: 
    if (Number(integral) > 0) {
        zeroCount = 0;
        for (i = 0; i < integral.length; i++) {
            p = integral.length - i - 1;
            d = integral.substr(i, 1);
            quotient = p / 4;
            modulus = p % 4;
            if (d == "0") {
                zeroCount++;
            }
            else {
                if (zeroCount > 0) {
                    outputCharacters += digits[0];
                }
                zeroCount = 0;
                outputCharacters += digits[Number(d)] + radices[modulus];
            }
            if (modulus == 0 && zeroCount < 4) {
                outputCharacters += bigRadices[quotient];
                zeroCount = 0;
            }
        }
        outputCharacters += CN_DOLLAR;
    }
    // Process decimal part if there is: 
    if (decimal != "") {
        for (i = 0; i < decimal.length; i++) {
            d = decimal.substr(i, 1);
            if (d != "0") {
                outputCharacters += digits[Number(d)] + decimals[i];
            }
        }
    }
    // Confirm and return the final output string: 
    if (outputCharacters == "") {
        outputCharacters = CN_ZERO + CN_DOLLAR;
    }
    if (decimal == "") {
        outputCharacters += CN_INTEGER;
    }
    //outputCharacters =  outputCharacters;
    return outputCharacters;
}

//保留小数位
function getFloat(number, n) {
    n = n ? parseInt(n) : 0;
    if (n <= 0) {
        return Math.round(number);
    }
    number = Math.round(number * Math.pow(10, n)) / Math.pow(10, n); //四舍五入
    number = Number(number).toFixed(n); //补足位数
    return number;
};


//保留两位小数    
//功能：将浮点数四舍五入，取小数点后2位   
function ToDecimal(x) {
    var f = parseFloat(x);
    if (isNaN(f)) {
        return 0;
    }
    f = Math.round(x * 100) / 100;
    return f;
}
/**
查找数组中是否存在某个值
arr:数组
val:对象值
**/
function ArrayExists(arr, val) {
    for (var i = 0; i < arr.length; i++) {
        if (arr[i] == val)
            return true;
    }
    return false;
}
/*弹出对话框begin========================================*/
/*关闭对话框*/
function closeDialog() {
    var api = frameElement.api, W = api.opener;
    api.close();
    return true;
}


/*
弹出对话框（带：取消按钮）
*/
function openCancelDialog(url, _id, _title, _width, _height) {
    //  Loading(true);
    top.$.dialog({
        id: _id,
        width: _width,
        height: _height,
        max: false,
        lock: true,
        title: _title,
        resize: false,
        extendDrag: true,
        content: 'url:' + RootPath() + url,
        cancel: true
    });
}

function openMyDialog(url, _id, _title, _width, _height, callBack, callBack1) {
    //  Loading(true);
    top.$.dialog({
        id: _id,
        width: _width,
        height: _height,
        max: false,
        lock: true,
        title: _title,
        resize: false,
        extendDrag: true,
        content: 'url:' + RootPath() + url,
        ok: function () {
            callBack(_id);
            return false;
        },
        cancel: function () {
            callBack1(_id);
            return false;
        }
    });
}
/*
弹出对话框（带：确认按钮、取消按钮）
*/
function openDialog(url, _id, _title, _width, _height, callBack) {
    //  Loading(true);
    top.$.dialog({
        id: _id,
        width: _width,
        height: _height,
        max: false,
        lock: true,
        title: _title,
        resize: false,
        extendDrag: true,
        content: 'url:' + RootPath() + url,
        ok: function () {
            callBack(_id);
            return false;
        },
        cancel: true
    });
}
/*
最大化弹出对话框（带：确认按钮、取消按钮）
*/
function FullopenDialog(url, _id, _title, callBack) {
    //  Loading(true);
    top.$.dialog({
        id: _id,
        lock: true,
        title: _title,
        max: false,
        min: false,
        width: top.$(window).width() - 40,
        height: top.$('body').height() - 100,
        content: 'url:' + RootPath() + url,
        ok: function () {
            callBack(_id);
            return false;
        },
        cancel: true
    })
}
/*
弹出对话框（没按钮）
*/
function Dialog(url, _id, _title, _width, _height) {
    //  Loading(true);
    top.$.dialog({
        id: _id,
        width: _width,
        height: _height,
        max: false,
        lock: true,
        title: _title,
        content: 'url:' + RootPath() + url
    });
}
/*
最大化弹出对话框（没按钮）
*/
function FullDialog(url, _id, _title) {
    //  Loading(true);
    top.$.dialog({
        id: _id,
        lock: true,
        title: _title,
        max: false,
        min: false,
        width: top.$(window).width() - 40,
        height: top.$('body').height() - 100,
        content: 'url:' + RootPath() + url
    })
}
/*
弹出查询
*/
function QueryDialog(url, _id, _title, _width, _height, _callBack) {
    //  Loading(true);
    top.$.dialog({
        id: _id,
        width: _width,
        height: _height,
        max: false,
        lock: true,
        title: _title,
        content: 'url:' + RootPath() + url,
        button: [
            {
                name: '查 询',
                callback: function () {
                    _callBack(_id);
                    return false;
                }
            },
            {
                name: '取 消'
            }
        ]
    });
}
/*
弹出对话框
*/
function MessageDialog(_html, _id, _title, _width, _height, _callBack) {
    top.$.dialog({
        id: _id,
        width: _width,
        height: _height,
        max: false,
        min: false,
        title: _title,
        content: _html,
        ok: function () {
            var data = eval("(" + top.GetParameterJson(_id) + ")"); ;
            _callBack(data);
            return false;
        },
        cancel: true
    });
}
/*
弹出打印网页
*/
function PrintDialog(url, _id, _title, _width, _height, _callBack) {
    //  Loading(true);
    top.$.dialog({
        id: _id,
        width: _width,
        height: _height,
        max: false,
        lock: true,
        title: _title,
        content: 'url:' + RootPath() + url,
        button: [
            {
                name: '打 印',
                callback: function () {
                    _callBack(_id);
                    return false;
                }
            },
            {
                name: '取 消'
            }
        ]
    });
}

/*打开网页 window.open
/*url:          表示请求路径
/*windowname:   定义页名称
/*width:        宽度
/*height:       高度
---------------------------------------------------*/
function OpenWindow(url, title, w, h) {
    var width = w;
    var height = h;
    var left = ($(window).width() - width) / 2;
    var top = ($(window).height() - height) / 2;
    window.open(RootPath() + url, title, 'height=' + height + ', width=' + width + ', top=' + top + ', left=' + left + ', toolbar=no, menubar=no, scrollbars=no, resizable=no, location=no, status=no, titlebar=yes, alwaysRaised=yes');
}

/**
短暂提示
msg: 显示消息
time：停留时间
type：类型 >1：成功，<1：失败，其他：警告
**/
function tipDialog(msg, time, type) {
    var msg = "<div class='ui_alert_tip'>" + msg + "</div>"
    if (type >= 1) {
        top.$.dialog.tips(msg, time, 'succ.png');
    } else if (type == -1) {
        top.$.dialog.tips(msg, time, 'fail.png');
    } else if (type == 0) {
        top.$.dialog.tips(msg, time, 'fail.png');
    } else {
        top.$.dialog.tips(msg, time, 'i.png');
    }
}
/*
警告消息
msg: 显示消息
type：类型 >1：成功，<1：失败，其他：警告
*/
function alertDialog(msg, type) {
    var msg = "<div class='ui_alert'>" + msg + "</div>"
    var icon = "";
    if (type >= 1) {
        icon = "succ.png";
    } else if (type == -1) {
        icon = "fail.png";
    } else {
        icon = "i.png";
    }
    top.$.dialog({
        id: "alertDialog",
        icon: icon,
        content: msg,
        title: "提示",
        ok: function () {
            return true;
        }
    });
}
/*
确认对话框
*/
function confirmDialog(_title, msg, callBack) {
    var msg = "<div class='ui_alert'>" + msg + "</div>"
    top.$.dialog({
        id: "confirmDialog",
        lock: true,
        icon: "hits.png",
        content: msg,
        title: _title,
        ok: function () {
            callBack(true)
            return true;
        },
        cancel: function () {
            callBack(false)
            return true;
        }
    });
}
/*弹出对话框end========================================*/
/***
* 自动关闭弹出内容提示
timeOut : 4000,				//提示层显示的时间
msg : "恭喜你!你已成功操作此插件，谢谢使用！",			//显示的消息
type : "success"//提示类型（1、success 2、error 3、warning）
***/
function TipMsg(msg, timeOut, type) {
    $(".tip_container").remove();
    var bid = parseInt(Math.random() * 100000);
    $("body").prepend('<div id="tip_container' + bid + '" class="container tip_container"><div id="tip' + bid + '" class="mtip"><span id="tsc' + bid + '"></span></div></div>');
    var $this = $(this);
    var $tip_container = $("#tip_container" + bid);
    var $tip = $("#tip" + bid);
    var $tipSpan = $("#tsc" + bid);
    //先清楚定时器
    clearTimeout(window.timer);
    //主体元素绑定事件
    $tip.attr("class", type).addClass("mtip");
    $tipSpan.html(msg);
    $tip_container.slideDown(300);
    //提示层隐藏定时器
    window.timer = setTimeout(function () {
        $tip_container.slideUp(300);
        $(".tip_container").remove();
    }, timeOut);
    //鼠标移到提示层时清除定时器
    $tip_container.live("mouseover", function () {
        clearTimeout(window.timer);
    });
    //鼠标移出提示层时启动定时器
    $tip_container.live("mouseout", function () {
        window.timer = setTimeout(function () {
            $tip_container.slideUp(300);
            $(".tip_container").remove();
        }, timeOut);
    });
    $("#tip_container" + bid).css("left", ($(window).width() - $("#tip_container" + bid).width()) / 2);
}


/*
验证是否为空
*/
function IsNullOrEmpty(str) {
    var isOK = true;
    if (str == undefined || str == "" || str == 'null') {
        isOK = false;
    }
    return isOK;
}

function IsCheckeds(id) {
    var isOK = true;
    if (id == undefined || id == "" || id == 'null' || id == 'undefined') {
        isOK = false;
        tipDialog('您没有选中任何项,请您选中后再操作。', 4, 'warning');
    }
    return isOK;
}

function IsDelData(id) {
    var isOK = true;
    if (id == undefined || id == "" || id == 'null' || id == 'undefined') {
        isOK = false;
        tipDialog('您没有选中任何项,请您选中后再操作。', 4, 'warning');
    }
    return isOK;
}

function IsChecked(id) {
    var isOK = true;
    if (id == undefined || id == "" || id == 'null' || id == 'undefined') {
        isOK = false;
        tipDialog('您没有选中任何项,请您选中后再操作。', 4, 'warning');
    } else if (id.split(",").length > 1) {
        isOK = false;
        tipDialog('很抱歉,一次只能选择一条记录。', 4, 'warning');
    }
    return isOK;
}

/*鼠标右击菜单begin========================================*/
var getOffset = {
    top: function (obj) {
        return obj.offsetTop + (obj.offsetParent ? arguments.callee(obj.offsetParent) : 0)
    },
    left: function (obj) {
        return obj.offsetLeft + (obj.offsetParent ? arguments.callee(obj.offsetParent) : 0)
    }
};

function LoadrightMenu(element) {
    var oMenu = $('.rightMenu');
    $(document).click(function () {
        oMenu.hide();
    });
    $(document).mousedown(function (e) {
        if (3 == e.which) {
            oMenu.hide();
        }
    })
    var aUl = oMenu.find("ul");
    var aLi = oMenu.find("li");
    var showTimer = hideTimer = null;
    var i = 0;
    var maxWidth = maxHeight = 0;
    var aDoc = [document.documentElement.offsetWidth, document.documentElement.offsetHeight];
    oMenu.hide();
    for (i = 0; i < aLi.length; i++) {
        //为含有子菜单的li加上箭头
        aLi[i].getElementsByTagName("ul")[0] && (aLi[i].className = "sub");
        //鼠标移入
        aLi[i].onmouseover = function () {
            var oThis = this;
            var oUl = oThis.getElementsByTagName("ul");
            //鼠标移入样式
            oThis.className += " active";
            //显示子菜单
            if (oUl[0]) {
                clearTimeout(hideTimer);
                showTimer = setTimeout(function () {
                    for (i = 0; i < oThis.parentNode.children.length; i++) {
                        oThis.parentNode.children[i].getElementsByTagName("ul")[0] &&
						(oThis.parentNode.children[i].getElementsByTagName("ul")[0].style.display = "none");
                    }
                    oUl[0].style.display = "block";
                    oUl[0].style.top = oThis.offsetTop + "px";
                    oUl[0].style.left = oThis.offsetWidth + "px";

                    //最大显示范围					
                    maxWidth = aDoc[0] - oUl[0].offsetWidth;
                    maxHeight = aDoc[1] - oUl[0].offsetHeight;

                    //防止溢出
                    maxWidth < getOffset.left(oUl[0]) && (oUl[0].style.left = -oUl[0].clientWidth + "px");
                    maxHeight < getOffset.top(oUl[0]) && (oUl[0].style.top = -oUl[0].clientHeight + oThis.offsetTop + oThis.clientHeight + "px")
                }, 300);
            }
        };
        //鼠标移出	
        aLi[i].onmouseout = function () {
            var oThis = this;
            var oUl = oThis.getElementsByTagName("ul");
            //鼠标移出样式
            oThis.className = oThis.className.replace(/\s?active/, "");

            clearTimeout(showTimer);
            hideTimer = setTimeout(function () {
                for (i = 0; i < oThis.parentNode.children.length; i++) {
                    oThis.parentNode.children[i].getElementsByTagName("ul")[0] &&
					(oThis.parentNode.children[i].getElementsByTagName("ul")[0].style.display = "none");
                }
            }, 300);
        };
    }
    //自定义右键菜单
    $(element).bind("contextmenu", function () {
        var event = event || window.event;
        oMenu.show();
        oMenu.css('top', event.clientY + "px");
        oMenu.css('left', event.clientX + "px");
        //最大显示范围
        maxWidth = aDoc[0] - oMenu.width();
        maxHeight = aDoc[1] - oMenu.height();
        //防止菜单溢出
        if (oMenu.offset().top > maxHeight) {
            oMenu.css('top', maxHeight + "px");
        }
        if (oMenu.offset().left > maxWidth) {
            oMenu.css('left', maxWidth + "px");
        }
        return false;
    }).bind("click", function () {
        oMenu.hide();
    });
}
/*鼠标右击菜单end========================================*/

//自动补全表格
var IndetableRow_autocomplete = 0;
var scrollTopheight = 0;
function autocomplete(Objkey, width, height, data, callBack) {
    if ($('#' + Objkey).attr('readonly') == 'readonly') {
        return false;
    }
    if ($('#' + Objkey).attr('disabled') == 'disabled') {
        return false;
    }
    IndetableRow_autocomplete = 0;
    scrollTopheight = 0;
    var X = $("#" + Objkey).offset().top;
    var Y = $("#" + Objkey).offset().left;
    $("#div_gridshow").html("");
    if ($("#div_gridshow").attr("id") == undefined) {
        $('body').append('<div id="div_gridshow" style="overflow: auto;z-index: 1000;border: 1px solid #A8A8A8;width:' + width + ';height:' + height + ';position: absolute; background-color: #fff; display: none;"></div>');
    } else {
        $("#div_gridshow").height(height);
        $("#div_gridshow").width(width);
    }
    var sbhtml = '<table class="grid" style="width: 100%;">';
    if (data != "") {
        sbhtml += '<tbody>' + data + '</tbody>';
    } else {
        sbhtml += '<tbody><tr><td style="color:red;text-align:center;width:' + width + ';">没有找到您要的相关数据！</td></tr></tbody>';
    }
    sbhtml += '</table>';
    $("#div_gridshow").html(sbhtml);
    $("#div_gridshow").css("left", Y).css("top", X + $('#' + Objkey).height()).show();
    $("#div_gridshow .grid td").css("border-left", "none").css("padding-left", "2px");
    if (data != "") {
        $("#div_gridshow").find('tbody tr').each(function (r) {
            if (r == 0) {
                $(this).addClass('selected');
            }
        });
    }
    $("#div_gridshow").find('tbody tr').click(function () {
        var parameter = "";
        $(this).find('td').each(function (i) {
            parameter += '"' + $(this).attr('id') + '"' + ':' + '"' + $.trim($(this).text()) + '",'
        });
        if ($('#' + Objkey).attr('readonly') == 'readonly') {
            return false;
        }
        if ($('#' + Objkey).attr('disabled') == 'disabled') {
            return false;
        }
        callBack(JSON.parse('{' + parameter.substr(0, parameter.length - 1) + '}'));
        $("#div_gridshow").hide();
    });
    $("#div_gridshow").find('tbody tr').hover(function () {
        $(this).addClass("selected");
    }, function () {
        $(this).removeClass("selected");
    });
    //任意键关闭
    document.onclick = function (e) {
        var e = e ? e : window.event;
        var tar = e.srcElement || e.target;
        if (tar.id != 'div_gridshow') {
            if ($(tar).attr("id") == 'div_gridshow' || $(tar).attr("id") == Objkey) {
                $("#div_gridshow").show();
            } else {
                $("#div_gridshow").hide();
            }
        }
    }
}
//方向键上,方向键下,回车键
function autocompletekeydown(Objkey, callBack) {
    $("#" + Objkey).keydown(function (e) {
        switch (e.keyCode) {
            case 38: // 方向键上
                if (IndetableRow_autocomplete > 0) {
                    IndetableRow_autocomplete--
                    $("#div_gridshow").find('tbody tr').removeClass('selected');
                    $("#div_gridshow").find('tbody tr').each(function (r) {
                        if (r == IndetableRow_autocomplete) {
                            scrollTopheight -= 22;
                            $("#div_gridshow").scrollTop(scrollTopheight);
                            $(this).addClass('selected');
                        }
                    });
                }
                break;
            case 40: // 方向键下
                var tindex = $("#div_gridshow").find('tbody tr').length - 1;
                if (IndetableRow_autocomplete < tindex) {
                    IndetableRow_autocomplete++;
                    $("#div_gridshow").find('tbody tr').removeClass('selected');
                    $("#div_gridshow").find('tbody tr').each(function (r) {
                        if (r == IndetableRow_autocomplete) {
                            scrollTopheight += 22;
                            $("#div_gridshow").scrollTop(scrollTopheight);
                            $(this).addClass('selected');
                        }
                    });
                }
                break;
            case 13:  //回车键
                var parameter = "";
                $("#div_gridshow").find('tbody tr').each(function (r) {
                    if (r == IndetableRow_autocomplete) {
                        $(this).find('td').each(function (i) {
                            parameter += '"' + $(this).attr('id') + '"' + ':' + '"' + $.trim($(this).text()) + '",'
                        });
                    }
                });
                if ($('#' + Objkey).attr('readonly') == 'readonly') {
                    return false;
                }
                if ($('#' + Objkey).attr('disabled') == 'disabled') {
                    return false;
                }
                callBack(JSON.parse('{' + parameter.substr(0, parameter.length - 1) + '}'));
                $("#div_gridshow").hide();
                break;
            default:
                break;
        }
    })
}
/**
树下拉框
Objkey:          ID
width：          宽度
height：         高度
data：           数据
**/
function comboBoxTree(Objkey, height) {
    top.$(".ui_state_highlight").focus();
    var width = $("#" + Objkey).width();
    $("#" + Objkey).css('ime-mode', 'disabled');
    $("#" + Objkey).bind("contextmenu", function () { return false; });
    $("#" + Objkey).keypress(function (e) { return false; });
    if ($('#' + Objkey).attr('readonly') == 'readonly') { return false; }
    if ($('#' + Objkey).attr('disabled') == 'disabled') { return false; }
    var X = $("#" + Objkey).offset().top - 1;
    var Y = $("#" + Objkey).offset().left - 1;
    var comboBoxTree = "comboBoxTree" + Objkey;
    if ($("#" + comboBoxTree).attr("id") == undefined) {
        $('body').append('<div id="' + comboBoxTree + '" style="overflow: auto;border: 1px solid #ccc;border-top:none;width:' + width + 'px;height:' + height + ';position: absolute; background-color: #fff; display: none;"></div>');
    }
    $("#" + comboBoxTree).css("left", Y).css("top", X + $("#" + Objkey).height() + 2).css("z-index", "99").slideDown(100);
    //任意键关闭
    document.onclick = function (e) {
        var e = e ? e : window.event;
        var tar = e.srcElement || e.target;
        if (tar.id != '' + comboBoxTree + '') {
            if ($(tar).hasClass("bbit-tree-ec-icon")) {
                return false;
            }
            if ($(tar).attr("id") == Objkey) {
                return false;
            } else {
                $("#" + comboBoxTree).slideUp(100);
            }
        }
    }
}
/*删除数据
/*url:        表示请求路径
/*parm：      条件参数
--------------------------------------------------*/
function delConfirm(url, parm, msg) {
    confirmDialog("提示", msg, function (r) {
        if (r) {
            Loading(true, "正在删除数据...");
            window.setTimeout(function () {
                AjaxJson(url, parm, function (data) {
                    tipDialog(data.Message, 3, data.Code);
                    if (data.Code > 0) {
                        windowload();
                    }
                });
            }, 200);
        }
    });
}
function delConfig(url, parm, count) {
    if (count == undefined) {
        count = 1;
    }
    confirmDialog("提示", "注：您确定要删除 " + count + " 笔记录？", function (r) {
        if (r) {
            Loading(true, "正在删除数据...");
            window.setTimeout(function () {
                AjaxJson(url, parm, function (data) {
                    tipDialog(data.Message, 3, data.Code);
                    if (data.Code > 0) {
                        windowload();
                    }
                });
            }, 200);
        }
    });
}
/*绑定数据字典下拉框
ControlId:控件ID
Code:分类编码
Memo:默认显示
*/
function BindDropItem(ControlId, Code, Memo) {
    $(ControlId).html("");
    if (IsNullOrEmpty(Memo)) {
        $(ControlId).append("<option value=''>" + Memo + "</option>");
    }
    getAjax('/CommonModule/DataDictionary/BinDingItemsJson', { Code: Code }, function (data) {
        var itemjson = eval("(" + data + ")");
        $.each(itemjson, function (i) {
            $(ControlId).append($("<option></option>").val(itemjson[i].Code).html(itemjson[i].FullName));
        });
        // Loading(false);
    });
}
/*绑定数据字典下拉框
ControlId:控件ID
Code:分类编码
Memo:默认显示
*/
function BindDropItemForID(ControlId, Code, Memo) {
    $(ControlId).html("");
    if (IsNullOrEmpty(Memo)) {
        $(ControlId).append("<option value=''>" + Memo + "</option>");
    }
    getAjax('/CommonModule/DataDictionary/BinDingItemsJson', { Code: Code }, function (data) {
        var itemjson = eval("(" + data + ")");
        $.each(itemjson, function (i) {
            $(ControlId).append($("<option></option>").val(itemjson[i].DataDictionaryDetailId).html(itemjson[i].FullName));
        });
        // Loading(false);
    });
}
/*绑定下拉框
ControlId:控件ID
Memo:默认显示
*/
function JsonBindDrop(ControlId, Memo, DataJson) {
    $(ControlId).html("");
    if (IsNullOrEmpty(Memo)) {
        $(ControlId).append("<option value=''>" + Memo + "</option>");
    }
    var DataJson = eval("(" + DataJson + ")");
    $.each(DataJson, function (i) {
        $(ControlId).append($("<option></option>").val(DataJson[i].Code).html(DataJson[i].FullName));
    });
}
/*
验证对象值不能重复 
id:字段ID、
tablename：表名、
keyfield：主键Id、
Msg：提示消息
*/
function FieldExist(id, tablename, keyfield, Msg) {
    var $ObjFiel = $("#" + id);
    var postData = {
        tablename: tablename,
        fieldname: id,
        fieldvalue: $ObjFiel.val(),
        keyfield: GetQuery('KeyValue'),
        keyvalue: keyfield
    }
    getAjax('/Utility/FieldExist', postData, function (data) {
        if (data.toLocaleLowerCase() == 'false') {
            tipCss("#" + id, Msg + "已存在,请重新输入");
        }
        // Loading(false);
    });
}
/*公共导出Excel，将表格数据取出来。在导出Excel
obj:表格对象ID
fileName：导出Excel文件名
*/
function SetDeriveExcel(obj, fileName) {
    var columnModel = $(obj).jqGrid('getGridParam', 'colModel');
    var dataModel = $(obj).jqGrid("getRowData");
    var footerData = $(obj).jqGrid("footerData");
    var JsonData = {
        JsonColumn: JSON.stringify(columnModel),
        JsonData: JSON.stringify(dataModel),
        JsonFooter: JSON.stringify(footerData),
        FileName: fileName
    }
    getAjax("/Utility/SetDeriveExcel", JsonData, function (data) { })
}
/*
表格视图列名
*/
function columnModelData(jgrid) {
    AjaxJson("/Utility/LoadViewColumn", {}, function (data) {
        $.each(data, function (i) {
            $(jgrid).hideCol(data[i].FieldName);
        });
    });
}
//加载权限按钮
function PartialButton() {
    var ModuleId = GetQuery('ModuleId'); //菜单主键
    var JsonData = "";
    var toolbar_html = "";
    var toolbar_Children_Count = 0;
    AjaxJson("/Utility/LoadButton", { ModuleId: ModuleId }, function (Data) {
        JsonData = Data;
        $.each(Data, function (i) {
            if (Data[i].ParentId == '0' && Data[i].Category == '1') {
                var toolbar_Children_List = Toolbar_Children(Data[i].ButtonId, Data)
                if (toolbar_Children_Count > 0) {
                    toolbar_html += "<a id=\"" + Data[i].Code + "\" ids=\"" + Data[i].ButtonId + "\" class=\"tools_btn dropdown\">";
                    toolbar_html += "<div style=\"float: left;\"><div class=\"icon\"><img src=\"../../Content/Images/Icon16/" + Data[i].Icon + "\" /></div><div class=\"text\">" + Data[i].FullName + "</div></div>";
                    toolbar_html += "<div class=\"dropdown-icon\"><img src=\"../../Content/Images/dropdown-icon.png\" /></div>";
                    toolbar_html += "<div class=\"dropdown-data\"><i></i><span></span>";
                    toolbar_html += "    <ul>";
                    toolbar_html += toolbar_Children_List;
                    toolbar_html += "    </ul>";
                    toolbar_html += "</div>";
                    toolbar_html += "</a>";
                } else {
                    var title = "title=\"" + Data[i].Remark + "\"";
                    if (Data[i].Remark == "") { title = ""; }
                    toolbar_html += "<a id=\"" + Data[i].Code + "\" ids=\"" + Data[i].ButtonId + "\" " + title + " onclick=\"" + Data[i].JsEvent + "\" class=\"tools_btn\"><span><b style=\"background: url('../../Content/Images/Icon16/" + Data[i].Icon + "') 50% 4px no-repeat;\">" + Data[i].FullName + "</b></span></a>";
                }
                if (Data[i].Split == '1') {
                    toolbar_html += "<div class=\"tools_separator\"></div>";
                }
            }
        });
    });
    $('.tools_bar .PartialButton').html(toolbar_html);
    function Toolbar_Children(ButtonId, Data) {
        var _toolbar_Children = "";
        toolbar_Children_Count = 0;
        $.each(Data, function (i) {
            if (Data[i].ParentId == ButtonId) {
                var title = "title=\"" + Data[i].Remark + "\"";
                if (Data[i].Remark == "") { title = ""; }
                _toolbar_Children += "<li id=\"" + Data[i].Code + "\" ids=\"" + Data[i].ButtonId + "\" " + title + " onclick=\"" + Data[i].JsEvent + "\"><img src=\"../../Content/Images/Icon16/" + Data[i].Icon + "\" />" + Data[i].FullName + "</li>";
                toolbar_Children_Count++;
            }
        });
        return _toolbar_Children;
    }
    $(".tools_btn").hover(function () {
        $(this).addClass("tools_btn_hover");
    }, function () {
        $(this).removeClass("tools_btn_hover");
    });
    $(".tools_bar .dropdown").hover(function () {
        var left = $(this).offset().left - ($(this).find('.dropdown-data').width() / 2) + ($(this).width() / 2 + 9);
        $(this).find('.dropdown-data').show().css('top', ($(this).offset().top + 50)).css('left', left);
        $(this).find('.dropdown-icon').addClass('dropdown-icon-hover');
        $(this).addClass('dropdown-selected');
    }, function () {
        if (!$(this).hasClass("_click")) {
            $(this).removeClass('dropdown-selected');
            $(this).find('.dropdown-data').hide();
            $(this).find('.dropdown-icon').removeClass('dropdown-icon-hover');
        }
    });
    $('.tools_bar .dropdown').toggle(function () {
        $(this).addClass('_click');
        var left = $(this).offset().left - ($(this).find('.dropdown-data').width() / 2) + ($(this).width() / 2 + 9);
        $(this).find('.dropdown-data').show().css('top', ($(this).offset().top + 50)).css('left', left);
        $(this).find('.dropdown-icon').addClass('dropdown-icon-hover');
        $(this).addClass('dropdown-selected');
    }, function () {
        $(this).removeClass('dropdown-selected');
        $(this).find('.dropdown-data').hide();
        $(this).find('.dropdown-icon').removeClass('dropdown-icon-hover');
        $('.dropdown').removeClass('_click');
    });
    $(".dropdown-data li").click(function () {
        $('.dropdown').removeClass('dropdown-selected');
        $('.dropdown').find('.dropdown-data').hide();
        $('.dropdown').find('.dropdown-icon').removeClass('dropdown-icon-hover');
        $('.dropdown').removeClass('_click');
    });
    //右击菜单
    var right_toolbar_Count = 0;
    var right_toolbar_html = "";
    right_toolbar_html += '<div class="rightMenu"><ul>';
    $.each(JsonData, function (i) {
        if (JsonData[i].ParentId == '0' && JsonData[i].Category == '2') {
            var title = "title=\"" + JsonData[i].Remark + "\"";
            if (JsonData[i].Remark == "") { title = ""; }
            var righttoolbar_Children_List = right_toolbar_Children(JsonData[i].ButtonId, JsonData)
            if (right_toolbar_Count > 0) {
                right_toolbar_html += "<li id=\"right_" + JsonData[i].Code + "\" ids=\"" + JsonData[i].ButtonId + "\" " + title + " onclick=\"" + JsonData[i].JsEvent + "\" ><img src=\"../../Content/Images/Icon16/" + JsonData[i].Icon + "\" />" + JsonData[i].FullName + righttoolbar_Children_List + "</li>";
            } else {
                right_toolbar_html += "<li id=\"right_" + JsonData[i].Code + "\"  ids=\"" + JsonData[i].ButtonId + "\" " + title + " onclick=\"" + JsonData[i].JsEvent + "\" ><img src=\"../../Content/Images/Icon16/" + JsonData[i].Icon + "\" />" + JsonData[i].FullName + "</li>";
            }
            if (JsonData[i].Split == '1') {
                right_toolbar_html += "<div class=\"m-split\"></div>";
            }
        }
    });
    right_toolbar_html += '</ul></div>';
    function right_toolbar_Children(ButtonId, JsonData) {
        var _right_toolbar = "<ul>";
        right_toolbar_Count = 0;
        $.each(JsonData, function (i) {
            if (JsonData[i].ParentId == ButtonId) {
                var title = "title=\"" + JsonData[i].Remark + "\"";
                if (JsonData[i].Remark == "") { title = ""; }
                _right_toolbar += "<li id=\"right_" + JsonData[i].Code + "\"  ids=\"" + JsonData[i].ButtonId + "\" " + title + " onclick=\"" + JsonData[i].JsEvent + "\" ><img src=\"../../Content/Images/Icon16/" + JsonData[i].Icon + "\" />" + JsonData[i].FullName + "</li>";
                if (JsonData[i].Split == '1') {
                    _right_toolbar += "<div class=\"m-split\"></div>";
                }
                right_toolbar_Count++;
            }
        });
        return _right_toolbar + "</ul>";
    }
    $("body").append(right_toolbar_html);
    if ($('.rightMenu').find('li').length > 0) {
        LoadrightMenu("#grid_List");
    } else {
        $('.rightMenu').remove();
    }
}
//=================动态菜单tab标签========================
function AddTabMenu(tabid, url, name, img, Isclose, IsReplace, IsVisitorModule) {
    $('#overlay_startmenu').hide();
    $('#start_menu_panel').hide();
    $('#start_menu_panel .nicescroll-rails').show();
    $('.nicescroll-rails').hide();
    if (url == "" || url == "#" || url == RootPath()) {
        url = RootPath() + "/Error/Error404";
    }
    var tabs_container = top.$("#tabs_container");
    var ContentPannel = top.$("#ContentPannel");
    if (IsReplace == 'true' || IsReplace == true) {
        top.RemoveDiv(tabid);
    }
    if (top.document.getElementById("tabs_" + tabid) == null) { //如果当前tabid存在直接显示已经打开的tab
        //  Loading(true);
        if (!IsVisitorModule) {
            VisitorModule(tabid, name);
        }
        if (tabs_container.find('li').length >= 10) {
            // Loading(false);
            alertDialog("为保证系统效率,只允许同时运行10个功能窗口,请关闭一些窗口后重试！", 0)
            return false;
        }
        tabs_container.find('li').removeClass('selected');
        ContentPannel.find('iframe').hide();
        if (Isclose != 'false') { //判断是否带关闭tab
            //tabs_container.append("<li id=\"tabs_" + tabid + "\" class='selected' win_close='true'><span title='" + name + "' onclick=\"AddTabMenu('" + tabid + "','" + url + "','" + name + "','true')\"><a><img src='../Content/Images/Icon16/" + img + "' width='16' height='16'>" + name + "</a></span><a class=\"win_close\" title=\"关闭当前窗口\" onclick=\"RemoveDiv('" + tabid + "')\"></a></li>");

            tabs_container.append("<li id=\"tabs_" + tabid + "\" class='selected' win_close='true'><span title='" + name + "' onclick=\"AddTabMenu('" + tabid + "','" + url + "','" + name + "','true')\"><a>" + name + "</a></span><a class=\"win_close\" title=\"关闭当前窗口\" onclick=\"RemoveDiv('" + tabid + "')\"></a></li>");
        } else {
            //tabs_container.append("<li id=\"tabs_" + tabid + "\" class=\"selected\" onclick=\"AddTabMenu('" + tabid + "','" + url + "','" + name + "','false')\"><a><img src='../Content/Images/Icon16/" + img + "' width='16' height='16'>" + name + "</a></li>");
            tabs_container.append("<li id=\"tabs_" + tabid + "\" class=\"selected\" onclick=\"AddTabMenu('" + tabid + "','" + url + "','" + name + "','false')\"><a>" + name + "</a></li>");
        }
        ContentPannel.append("<iframe id=\"tabs_iframe_" + tabid + "\" name=\"tabs_iframe_" + tabid + "\" height=\"100%\" width=\"100%\" src=\"" + url + "\" frameBorder=\"0\"></iframe>");
    } else {
        if (!IsVisitorModule) {
            VisitorModule(tabid, name);
        }
        tabs_container.find('li').removeClass('selected');
        ContentPannel.find('iframe').hide();
        tabs_container.find('#tabs_' + tabid).addClass('selected');
        top.document.getElementById("tabs_iframe_" + tabid).style.display = 'block';
    }
    $('iframe#' + tabiframeId()).load(function () {
        // Loading(false);
    });
    LoadrightMenu(".tab-nav li");
}
//关闭当前tab
function ThisCloseTab() {
    var tabs_container = top.$("#tabs_container");
    top.RemoveDiv(tabs_container.find('.selected').attr('id').substr(5));
}
//全部关闭tab
function AllcloseTab() {
    top.$(".tab-nav").find("[win_close=true]").each(function () {
        RemoveDiv($(this).attr('id').substr(5))
    });
}
//关闭除当前之外的tab
function othercloseTab() {
    var tabs_container = top.$("#tabs_container");
    var id = tabs_container.find('.selected').attr('id').substr(5);
    top.$(".tab-nav").find("[win_close=true]").each(function () {
        if ($(this).attr('id').substr(5) != id) {
            RemoveDiv($(this).attr('id').substr(5))
        }
    });
}
//关闭事件
function RemoveDiv(obj) {
    // Loading(false);
    var tabs_container = top.$("#tabs_container");
    var ContentPannel = top.$("#ContentPannel");
    var ModuleId = tabs_container.find('.selected').attr('id').substr(5); //原来ID
    var ModuleName = tabs_container.find('.selected').find('span').attr('title'); //原来名称
    SetLeave(ModuleId, ModuleName);
    tabs_container.find("#tabs_" + obj).remove();
    ContentPannel.find("#tabs_iframe_" + obj).remove();
    var tablist = tabs_container.find('li');
    var pannellist = ContentPannel.find('iframe');
    if (tablist.length > 0) {
        tablist[tablist.length - 1].className = 'selected';
        pannellist[tablist.length - 1].style.display = 'block';
        var id = tabs_container.find('.selected').attr('id').substr(5);
        VisitorModule(id);
    }
}
//访问模块，写入系统菜单Id
function VisitorModule(ModuleId, ModuleName) {
    top.$("#ModuleId").val(ModuleId);
    getAjax("/Home/SetModuleId", { ModuleId: ModuleId, ModuleName: ModuleName }, function (data) { })
}
//离开模块
function SetLeave(ModuleId, ModuleName) {
    getAjax("/Home/SetLeave", { ModuleId: ModuleId, ModuleName: ModuleName }, function (data) { })
}
//=================动态菜单tab标签========================
//js获取网站根路径(站点及虚拟目录)
function RootPath() {
    var strFullPath = window.document.location.href;
    var strPath = window.document.location.pathname;
    var pos = strFullPath.indexOf(strPath);
    var prePath = strFullPath.substring(0, pos);
    var postPath = strPath.substring(0, strPath.substr(1).indexOf('/') + 1);
    //return (prePath + postPath);如果发布IIS，有虚假目录用用这句
    return (prePath);
}


//自动补全
function BindAutocompleteForCode(txtname, txtid, queryType, width, height, event) {
    var $txtname = $("#" + txtname);
    if (height == "") {
        height = "150";
    }
    $txtname.bind("keyup", function (e) {
        if (e.which != 13 && e.which != 40 && e.which != 38) {
            var keyvalue = $(this).val();
            if (keyvalue.length >= 2) {
                var parm = "keywords=" + escape(keyvalue);
                if (queryType == "Dictionary") {
                    //添加items属性值为职称查找职称字典数据
                    var items = $(this).attr("items");
                    if (items != null && items != "") {
                        parm += "&items=" + escape(items);
                    }
                }
                if (queryType == "ZJCOLUMN") {
                    var coltype = $(this).attr("coltype");  //绑定列类型
                    if (coltype != null && coltype != "") {
                        parm += "&type=" + coltype;
                    }
                }
                if (queryType == "Material") {
                    var istype = $(this).attr("istype");  // 
                    if (istype != null && istype != "") {
                        parm += "&istype=" + istype;
                    }
                }
                if (queryType == "KSUser") {
                    var kstype = $(this).attr("kstype");  // 
                    alert(kstype);
                    if (kstype != null && kstype != "") {
                        parm += "&kstype=" + kstype;
                    }
                }
                if (queryType == "ZJFA") {
                    var material = $(this).attr("material");  //所属物料
                    if (material != null && material != "") {
                        parm += "&material=" + material;
                    }
                }
                DataSource(parm);
            }

            if (keyvalue.length == 0) {
                $("#" + txtid).val("");
            }
        }
    }).focus(function () {
        var parm = "keywords=";
        $(this).select();
        if (queryType == "Dictionary") {
            var items = $(this).attr("items"); //添加items属性值为职称查找职称字典数据
            if (items != null && items != "") {
                parm += "&items=" + items;
            }
        }
        if (queryType == "ZJCOLUMN") {
            var coltype = $(this).attr("coltype");  //绑定列类型
            if (coltype != null && coltype != "") {
                parm += "&type=" + coltype;
            }
        }
        if (queryType == "Material") {
            var istype = $(this).attr("istype");  //绑定列类型
            if (istype != null && istype != "") {
                parm += "&istype=" + istype;
            }
        }
        if (queryType == "KSUser") {
            var kstype = $(this).attr("kstype");  // 
            alert(kstype);
            if (kstype != null && kstype != "") {
                parm += "&kstype=" + kstype;
            }
        }
        if (queryType == "ZJFA") {
            var material = $(this).attr("material");  //所属物料
            if (material != null && material != "") {
                parm += "&material=" + material;
            }
        }
        DataSource(parm);
    });
    //上，下键盘回调
    autocompletekeydown(txtname, function (data) {
        $("#" + txtid).val(data.code)
        $("#" + txtname).val(data.name)
    });
    //获取数据源
    function DataSource(parm) {
        AjaxJson("/AutoData/" + queryType, parm, function (DataJson) {
            var html = "";
            $.each(DataJson, function (i) {
                if (queryType == "Dictionary") {
                    html += "<tr>";
                    html += '<td id="id" style="display: none;">' + DataJson[i].id + '</td>';
                    html += '<td id="name" style="width: 100%;">' +  DataJson[i].name + '</td>';
                    html += "</tr>";
                }
                else if (queryType == "CustomerCode" || queryType == "MaterialCode") {
                    html += "<tr>";
                    html += '<td id="id" style="display: none;">' + DataJson[i].code + '</td>';
                    html += '<td id="name" style="width: 100%;">' + DataJson[i].name + '</td>';
                    html += "</tr>";
                }
                else if (queryType == "Material") {
                    var datatypename = "自建";
                    if (DataJson[i].datatype == "1") {
                        datatypename = "SAP";
                    }
                    if (DataJson[i].datatype == "2") {
                        datatypename = "BI";
                    }
                    html += "<tr>";
                    html += '<td id="id" style="display: none;">' + DataJson[i].id + '</td>';
                    html += '<td id="name" style="width: 60%;">' + DataJson[i].name + '</td>';
                    html += '<td id="materialspec" style="width: 30%;">' + (DataJson[i].materialspec!=null? DataJson[i].materialspec :"") + '</td>';
                    html += '<td id="datatype" style="width: 10%;">' + datatypename + '</td>';
                    html += "</tr>";
                } else if (queryType == "Supply") {
                    var datatypename = "自建";
                    if (DataJson[i].datatype == "1") {
                        datatypename = "SAP";
                    }
                    if (DataJson[i].datatype == "2") {
                        datatypename = "BI";
                    }
                    html += "<tr>";
                    html += '<td id="id" style="display: none;">' + DataJson[i].id + '</td>';
                    html += '<td id="name" style="width: 90%;">' + DataJson[i].name + '</td>';
                    html += '<td id="datatype" style="width:10%;">' + datatypename + '</td>';
                    html += "</tr>";
                }
                else if (queryType == "Company") {
                    var datatypename = "自建";
                    if (DataJson[i].datatype == "1") {
                        datatypename = "SAP";
                    }
                    if (DataJson[i].datatype == "2") {
                        datatypename = "BI";
                    }
                    html += "<tr>";
                    html += '<td id="id" style="display: none;">' + DataJson[i].id + '</td>';
                    html += '<td id="name" style="width: 90%;">' + DataJson[i].name + '</td>';
                    html += '<td id="datatype" style="width:10%;">' + datatypename + '</td>';
                    html += "</tr>";
                }
                else {
                    html += "<tr>";
                    html += '<td id="id" style="display: none;">' + DataJson[i].id + '</td>';
                    html += '<td id="code" style="display: none;">' + DataJson[i].code + '</td>';
                    html += '<td id="name" style="width: 100%;">' + DataJson[i].name + '</td>';
                    html += "</tr>";
                }
            });
            //点击事件回调
            autocomplete(txtname, $txtname.width() + "px", height + "px", html, function (data) {
                $("#" + txtid).val(data.code)
                $("#" + txtname).val(data.name)
            });
        });
    }
}



//自动补全
function BindAutocomplete(txtname, txtid, queryType, width, height, event) {
    var $txtname = $("#" + txtname);
    if (height == "") {
        height = "150";
    }
    $txtname.bind("keyup", function (e) {
        if (e.which != 13 && e.which != 40 && e.which != 38) {
            var keyvalue = $(this).val();
            if (keyvalue.length >= 2) {
                var parm = "keywords=" + escape(keyvalue);
                if (queryType == "Dictionary") {
                    //添加items属性值为职称查找职称字典数据
                    var items = $(this).attr("items");
                    if (items != null && items != "") {
                        parm += "&items=" + escape(items);
                    }
                }
                if (queryType == "ZJCOLUMN") {
                    var coltype = $(this).attr("coltype");  //绑定列类型
                    if (coltype != null && coltype != "") {
                        parm += "&type=" + coltype;
                    }
                }
                if (queryType == "Material") {
                    var istype = $(this).attr("istype");  // 
                    if (istype != null && istype != "") {
                        parm += "&istype=" + istype;
                    }
                }
                if (queryType == "ZJFA") {
                    var material = $(this).attr("material");  //所属物料
                    if (material != null && material != "") {
                        parm += "&material=" + material;
                    }
                }
                DataSource(parm);
            }

            if (keyvalue.length == 0) {
                $("#" + txtid).val("");
            }
        }
    }).focus(function () {
        var parm = "keywords=";
        $(this).select();
        if (queryType == "Dictionary") {
            var items = $(this).attr("items"); //添加items属性值为职称查找职称字典数据
            if (items != null && items != "") {
                parm += "&items=" + items;
            }
        }
        if (queryType == "ZJCOLUMN") {
            var coltype = $(this).attr("coltype");  //绑定列类型
            if (coltype != null && coltype != "") {
                parm += "&type=" + coltype;
            }
        }
        if (queryType == "Material") {
            var istype = $(this).attr("istype");  //绑定列类型
            if (istype != null && istype != "") {
                parm += "&istype=" + istype;
            }
        }
        if (queryType == "ZJFA") {
            var material = $(this).attr("material");  //所属物料
            if (material != null && material != "") {
                parm += "&material=" + material;
            }
        }
        DataSource(parm);
    });
    //上，下键盘回调
    autocompletekeydown(txtname, function (data) {
        $("#" + txtid).val(data.id)
        $("#" + txtname).val(data.name)
    });
    //获取数据源
    function DataSource(parm) {
        AjaxJson("/AutoData/" + queryType, parm, function (DataJson) {
            var html = "";
            $.each(DataJson, function (i) {
                if (queryType == "Dictionary") {
                    html += "<tr>";
                    html += '<td id="id" style="display: none;">' + DataJson[i].id + '</td>';
                    html += '<td id="name" style="width: 100%;">' +  DataJson[i].name + '</td>';
                    html += "</tr>";
                }
                else if (queryType == "CustomerCode" || queryType == "MaterialCode") {
                    html += "<tr>";
                    html += '<td id="id" style="display: none;">' + DataJson[i].code + '</td>';
                    html += '<td id="name" style="width: 100%;">' + DataJson[i].name + '</td>';
                    html += "</tr>";
                }
                else if (queryType == "Material") {
                    var datatypename = "自建";
                    if (DataJson[i].datatype == "1") {
                        datatypename = "SAP";
                    }
                    if (DataJson[i].datatype == "2") {
                        datatypename = "BI";
                    }
                    html += "<tr>";
                    html += '<td id="id" style="display: none;">' + DataJson[i].id + '</td>';
                    html += '<td id="name" style="width: 60%;">' + DataJson[i].name + '</td>';
                    html += '<td id="materialspec" style="width: 30%;">' + (DataJson[i].materialspec!=null?DataJson[i].materialspec:"") + '</td>';
                    html += '<td id="datatype" style="width: 10%;">' + datatypename + '</td>';
                    html += "</tr>";
                } else if (queryType == "Supply") {
                    var datatypename = "自建";
                    if (DataJson[i].datatype == "1") {
                        datatypename = "SAP";
                    }
                    if (DataJson[i].datatype == "2") {
                        datatypename = "BI";
                    }
                    html += "<tr>";
                    html += '<td id="id" style="display: none;">' + DataJson[i].id + '</td>';
                    html += '<td id="name" style="width: 90%;">' + DataJson[i].name + '</td>';
                    html += '<td id="datatype" style="width:10%;">' + datatypename + '</td>';
                    html += "</tr>";
                }
                else if (queryType == "Company") {
                    var datatypename = "自建";
                    if (DataJson[i].datatype == "1") {
                        datatypename = "SAP";
                    }
                    if (DataJson[i].datatype == "2") {
                        datatypename = "BI";
                    }
                    html += "<tr>";
                    html += '<td id="id" style="display: none;">' + DataJson[i].id + '</td>';
                    html += '<td id="name" style="width: 90%;">' + DataJson[i].name + '</td>';
                    html += '<td id="datatype" style="width:10%;">' + datatypename + '</td>';
                    html += "</tr>";
                }
                else {
                    html += "<tr>";
                    html += '<td id="id" style="display: none;">' + DataJson[i].id + '</td>';
                    html += '<td id="name" style="width: 100%;">' + DataJson[i].name + '</td>';
                    html += "</tr>";
                }
            });
            //点击事件回调
            autocomplete(txtname, $txtname.width() + "px", height + "px", html, function (data) {
                $("#" + txtid).val(data.id)
                $("#" + txtname).val(data.name)
            });
        });
    }
}



//自动补全
function BindAutocompleteNew(txtname, txtid, txtspec, queryType, width, height, event) {
    var $txtname = $("#" + txtname);
    if (height == "") {
        height = "150";
    }
    $txtname.bind("keyup", function (e) {
        if (e.which != 13 && e.which != 40 && e.which != 38) {
            var keyvalue = $(this).val();
            if (keyvalue.length >= 2) {
                var parm = "keywords=" + escape(keyvalue);
                if (queryType == "Dictionary") {
                    //添加items属性值为职称查找职称字典数据
                    var items = $(this).attr("items");
                    if (items != null && items != "") {
                        parm += "&items=" + escape(items);
                    }
                }
                if (queryType == "ZJCOLUMN") {
                    var coltype = $(this).attr("coltype");  //绑定列类型
                    if (coltype != null && coltype != "") {
                        parm += "&type=" + coltype;
                    }
                }
                if (queryType == "ZJFA") {
                    var material = $(this).attr("material");  //所属物料
                    if (material != null && material != "") {
                        parm += "&material=" + material;
                    }
                }
                DataSource(parm);
            }

            if (keyvalue.length == 0) {
                $("#" + txtid).val("");
                $("#" + txtspec).val("");
            }
        }
    }).focus(function () {
        var parm = "keywords=";
        $(this).select();
        if (queryType == "Dictionary") {
            var items = $(this).attr("items"); //添加items属性值为职称查找职称字典数据
            if (items != null && items != "") {
                parm += "&items=" + items;
            }
        }
        if (queryType == "ZJCOLUMN") {
            var coltype = $(this).attr("coltype");  //绑定列类型
            if (coltype != null && coltype != "") {
                parm += "&type=" + coltype;
            }
        }
        if (queryType == "ZJFA") {
            var material = $(this).attr("material");  //所属物料
            if (material != null && material != "") {
                parm += "&material=" + material;
            }
        }
        DataSource(parm);
    });
    //上，下键盘回调
    autocompletekeydown(txtname, function (data) {
        $("#" + txtid).val(data.id)
        $("#" + txtname).val(data.name)
        $("#" + txtspec).val(data.materialspec)

    });
    //获取数据源
    function DataSource(parm) {
        AjaxJson("/AutoData/" + queryType, parm, function (DataJson) {
            var html = "";
            $.each(DataJson, function (i) {
                if (queryType == "CustomerCode" || queryType == "MaterialCode") {
                    html += "<tr>";
                    html += '<td id="id" style="display: none;">' + DataJson[i].code + '</td>';
                    html += '<td id="name" style="width: 100%;">' + DataJson[i].name + '</td>';
                    html += "</tr>";
                }
                else if (queryType == "Material") {
                    html += "<tr>";
                    html += '<td id="id" style="display: none;">' + DataJson[i].id + '</td>';
                    html += '<td id="name" style="width: 60%;">' + DataJson[i].name + '</td>';
                    html += '<td id="materialspec" style="width: 40%;">' + DataJson[i].materialspec + '</td>';
                    html += "</tr>";
                }
                else {
                    html += "<tr>";
                    html += '<td id="id" style="display: none;">' + DataJson[i].id + '</td>';
                    html += '<td id="name" style="width: 100%;">' + DataJson[i].name + '</td>';
                    html += "</tr>";
                }
            });
            //点击事件回调
            autocomplete(txtname, $txtname.width() + "px", height + "px", html, function (data) {
                $("#" + txtid).val(data.id)
                $("#" + txtname).val(data.name)
                $("#" + txtspec).val(data.materialspec)
            });
        });
    }
}


function BindJSONAutocomplete(DataJson, txtname, txtid, width, height) {
    var $txtname = $("#" + txtname);
    if (height == "") {
        height = "150";
    }
    $txtname.bind("keyup", function (e) {
        if (e.which != 13 && e.which != 40 && e.which != 38) {
            var keyvalue = $(this).val();
            if (keyvalue.length >= 2) {
                DataSource(DataJson);
            }
            $("#" + txtid).val("");
        }
    }).focus(function () {
        DataSource(DataJson);
    });
    //上，下键盘回调
    autocompletekeydown(txtname, function (data) {
        $("#" + txtid).val(data.id)
        $("#" + txtname).val(data.name)
    });
    //获取数据源
    function DataSource(DataJson) {
        var html = "";
        $.each(DataJson, function (i) {
            html += "<tr>";
            html += '<td id="id" style="display: none;">' + DataJson[i].val + '</td>';
            html += '<td id="name" style="width: 100%;">' + DataJson[i].txt + '</td>';
            html += "</tr>";
        });
        //点击事件回调
        autocomplete(txtname, $txtname.width() + "px", height + "px", html, function (data) {
            $("#" + txtid).val(data.id)
            $("#" + txtname).val(data.name)
        });

    }
}

function BindJSONAutocomplete1(DataJson, txtname, width, height) {
    var $txtname = $("#" + txtname);
    if (height == "") {
        height = "150";
    }
    $txtname.bind("keyup", function (e) {
        if (e.which != 13 && e.which != 40 && e.which != 38) {
            var keyvalue = $(this).val();
            if (keyvalue.length >= 2) {
                DataSource(DataJson);
            }
        }
    }).focus(function () {
        DataSource(DataJson);
    });
    //上，下键盘回调
    autocompletekeydown(txtname, function (data) {
        $("#" + txtname).val(data.name)
    });
    //获取数据源
    function DataSource(DataJson) {
        var html = "";
        $.each(DataJson, function (i) {
            html += "<tr>";
            html += '<td id="id" style="display: none;">' + DataJson[i].val + '</td>';
            html += '<td id="name" style="width: 100%;">' + DataJson[i].txt + '</td>';
            html += "</tr>";
        });
        //点击事件回调
        autocomplete(txtname, $txtname.width() + "px", height + "px", html, function (data) {
            $("#" + txtname).val(data.name)
        });

    }
}


var datajson = "";
function LoadBindDrop(cid, json) {
    $("#" + cid).empty();
    $("#" + cid).append("<option value=''>" + "==请选择==" + "</option>");
    $.each(json, function (i, o) {
        if (o.selected == true) {
            $("#" + cid).append($("<option selected='selected'></option>").val(o.val).html(o.txt));
        } else {
            $("#" + cid).append($("<option></option>").val(o.val).html(o.txt));
        }
    });
}

/*
验证是否为空
*/
function IsNull(val) {
    if (val != null && val != 'null' && val != "" && val != "undefined" && val != undefined) {
        return true;
    } else {
        return false;
    }
}



//浮点数加法运算   
function FloatAdd(arg1, arg2) {
    var r1, r2, m;
    try { r1 = arg1.toString().split(".")[1].length } catch (e) { r1 = 0 }
    try { r2 = arg2.toString().split(".")[1].length } catch (e) { r2 = 0 }
    m = Math.pow(10, Math.max(r1, r2))
    return (arg1 * m + arg2 * m) / m
}

//浮点数减法运算   
function FloatSub(arg1, arg2) {
    var r1, r2, m, n;
    try { r1 = arg1.toString().split(".")[1].length } catch (e) { r1 = 0 }
    try { r2 = arg2.toString().split(".")[1].length } catch (e) { r2 = 0 }
    m = Math.pow(10, Math.max(r1, r2));
    //动态控制精度长度   
    n = (r1 >= r2) ? r1 : r2;
    return ((arg1 * m - arg2 * m) / m).toFixed(n);
}

//浮点数乘法运算   
function FloatMul(arg1, arg2) {
    var m = 0, s1 = arg1.toString(), s2 = arg2.toString();
    try { m += s1.split(".")[1].length } catch (e) { }
    try { m += s2.split(".")[1].length } catch (e) { }
    return Number(s1.replace(".", "")) * Number(s2.replace(".", "")) / Math.pow(10, m)
}

//浮点数除法运算   
function FloatDiv(arg1, arg2) {
    var t1 = 0, t2 = 0, r1, r2;
    try { t1 = arg1.toString().split(".")[1].length } catch (e) { }
    try { t2 = arg2.toString().split(".")[1].length } catch (e) { }
    with (Math) {
        r1 = Number(arg1.toString().replace(".", ""))
        r2 = Number(arg2.toString().replace(".", ""))
        return (r1 / r2) * pow(10, t2 - t1);
    }
}





Date.prototype.pattern = function (fmt) {
    var o = {
        "M+": this.getMonth() + 1, //月份        
        "d+": this.getDate(), //日        
        "h+": this.getHours() % 12 == 0 ? 12 : this.getHours() % 12, //小时        
        "H+": this.getHours(), //小时        
        "m+": this.getMinutes(), //分        
        "s+": this.getSeconds(), //秒        
        "q+": Math.floor((this.getMonth() + 3) / 3), //季度        
        "S": this.getMilliseconds() //毫秒        
    };
    var week = {
        "0": "/u65e5",
        "1": "/u4e00",
        "2": "/u4e8c",
        "3": "/u4e09",
        "4": "/u56db",
        "5": "/u4e94",
        "6": "/u516d"
    };
    if (/(y+)/.test(fmt)) {
        fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    }
    if (/(E+)/.test(fmt)) {
        fmt = fmt.replace(RegExp.$1, ((RegExp.$1.length > 1) ? (RegExp.$1.length > 2 ? "/u661f/u671f" : "/u5468") : "") + week[this.getDay() + ""]);
    }
    for (var k in o) {
        if (new RegExp("(" + k + ")").test(fmt)) {
            fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
        }
    }
    return fmt;
}

function ClearValue() {
    $(".form-find tr td").find("input").each(function () {
        var type = $(this).attr('type');
        var pk_id = $(this).attr('id');
        if (type != "button") {
            if (pk_id == "bdate") {
                $("#bdate").val((new Date()).pattern("yyyy-MM-dd") + " 00:00:00");
            } else if (pk_id == "edate") {
                $("#edate").val((new Date()).pattern("yyyy-MM-dd") + " 23:59:59");
            }
            else if (pk_id.toLocaleLowerCase() == "type" || pk_id.toLocaleLowerCase() == "isdel" || pk_id.toLocaleLowerCase() == "isuse" || pk_id.toLocaleLowerCase() == "status") {
            }
            else {
                $("#" + pk_id).val("");
            }
        }
    });
}

function BindDeptTree(txtname, txtid, width, height) {
    var $txtid = $("#" + txtid);
    var $txtname = $("#" + txtname);
    if (width == "") {
        width = $txtname.width();
    }
    if (height == "") {
        height = "150";
    }
    $txtname.focus(function () {
        var objId = this.id;
        comboBoxTree(objId, "200px");
        var itemtree = {
            onnodeclick: function (item) {
                $("#" + txtid).val(item.id);
                $('#' + txtname).val(item.text);
            },
            url: "/CommonModule/Department/TreeJson"
        };
        $("#comboBoxTree" + objId).treeview(itemtree);
        //移除Id上级元素
        $("#comboBoxTree" + objId + "_" + GetQuery('KeyValue').replace(/-/g, '_')).parent('li').remove();
    }).keyup(function () {
        $("#" + txtid).val("");
    })
}

function BindCompTree(txtname, txtid, width, height) {
    var $txtid = $("#" + txtid);
    var $txtname = $("#" + txtname);
    if (width == "") {
        width = $txtname.width();
    }
    if (height == "") {
        height = "150";
    }
    var comboBoxTreeId = "comboBoxTree" + txtname;
    $txtname.focus(function () {
        var objId = this.id;
        comboBoxTree(objId, "150px");
        var itemtree = {
            onnodeclick: function (item) {
                $("#" + txtid).val(item.id);
                $('#' + txtname).val(item.text);
                $("#" + comboBoxTree).hide();
            },
            url: "/CommonModule/Company/TreeJson"
        };
        $("#comboBoxTree" + objId).treeview(itemtree);
    }).keyup(function () {
        $("#" + txtid).val("");
    });
}

//树型下拉选择   
function BindComBoxTree(txtname, txtid, height, url) {
    if (height == "") {
        height = "200";
    }
    $("#" + txtname).focus(function () {
        var objId = this.id;
        comboBoxTree(objId, height + "px");
        var itemtree = {
            onnodeclick: function (item) {
                $("#" + txtid).val(item.value);
                $('#' + txtname).val(item.text);
                var comboBoxTree = "comboBoxTree" + objId;
                $("#" + comboBoxTree).hide();
            },
            url: url
        };
        $("#comboBoxTree" + objId).treeview(itemtree);
    })
}

function SelectDept(txtname, txtid, title) {
    if (title == "") {
        title = "选取单位信息";
    }
    var url = "/Utility/SelectDept";
    openMyDialog(url, "Form11111", title, 800, 380,
             function (iframe) {
                 top.frames[iframe].SelectText(txtname, txtid);
             }, function (iframe) {
                 top.frames[iframe].ClearText(txtname, txtid);
             });
}
function SelectDept1(txtname, txtid, txtpname, txtpid, title) {
    if (title == "") {
        title = "选取单位信息";
    }
    var url = "/Utility/SelectDept";
    openMyDialog(url, "Form11111", title, 800, 380,
             function (iframe) {
                 top.frames[iframe].SelectText1(txtname, txtid, txtpname, txtpid);
             }, function (iframe) {
                 top.frames[iframe].ClearText1(txtname, txtid, txtpname, txtpid);
             });
}

function SelectMaterialType(txtname, txtid, title) {
    if (title == "") {
        title = "选取物料分类信息";
    }
    var url = "/Utility/SelectMaterialType";
    openMyDialog(url, "Form11111", title, 800, 380,
             function (iframe) {
                 top.frames[iframe].SelectText(txtname, txtid);
             }, function (iframe) {
                 top.frames[iframe].ClearText(txtname, txtid);
             });
}

function SelectMaterial(txtname, txtid, title) {
    if (title == "") {
        title = "选取物料信息";
    }
    var url = "/Utility/SelectMaterial?type=1";
    openMyDialog(url, "Form11111", title, 800, 380,
             function (iframe) {
                 top.frames[iframe].SelectText(txtname, txtid);
             }, function (iframe) {
                 top.frames[iframe].ClearText(txtname, txtid);
             });
}

function SelectMaterialSpec(txtname, txtid, txtcode, title) {
    if (title == "") {
        title = "选取物料信息";
    }
    var url = "/Utility/SelectMaterial?type=1";
    openMyDialog(url, "Form11111", title, 800, 380,
             function (iframe) {
                 top.frames[iframe].SelectText1(txtname, txtid, txtcode);
             }, function (iframe) {
                 top.frames[iframe].ClearText1(txtname, txtid, txtcode);
             });
}

function SelectMaterialClass(txtname, txtid, txtcode, txtclass, title) {
    if (title == "") {
        title = "选取物料信息";
    }
    var url = "/Utility/SelectMaterial?type=1";
    openMyDialog(url, "Form11111", title, 800, 380,
             function (iframe) {
                 top.frames[iframe].SelectText2(txtname, txtid, txtcode, txtclass);
             }, function (iframe) {
                 top.frames[iframe].ClearText2(txtname, txtid, txtcode, txtclass);
             });
}




function BHGMaterialAutocomplete(txtname) {
    var $txtname = $("#" + txtname);
    var width = "";
    if (width == "") {
        width = $txtname.width();
    }
    var height = "";
    if (height == "") {
        height = "80";
    }
    $txtname.bind("keyup", function (e) {
        if (e.which != 13 && e.which != 40 && e.which != 38) {
            DataSource();
        }
    }).focus(function () {
        $(this).select();
        DataSource();
    });
    //上，下键盘回调
    autocompletekeydown(txtname, function (data) {
        $txtname.val(data.name);
    });
    //获取数据源
    function DataSource() {
        AjaxJson("/AutoData/BHGMaterial", { keywords: $txtname.val() }, function (DataJson) {
            var html = "";
            $.each(DataJson, function (i) {
                html += "<tr>";
                html += '<td id="id" style="display: none;">' + DataJson[i].id + '</td>';
                html += '<td id="name" style="width: 100%;">' + DataJson[i].name + '</td>';
                html += "</tr>";
            });
            //点击事件回调
            autocomplete(txtname, width + "px", height + "px", html, function (data) {
                $txtname.val(data.name);
            });
        });
    }
}