//创建报表
function MyCreateReport(report, date, Parm_Key_Value, Width, Height, bol) {
    var reporturl = "/ReportManager/grf/" + report;
    var dateurl = "/ReportManager/data/" + date + "?Parm_Key_Value=" + Parm_Key_Value;
    CreatePrintViewerEx(Width, Height, reporturl, dateurl, bol, "");
}

function NewMyCreateReport(report, date, Parm_Key_Value, Width, Height, bol) {
    var reporturl = "", dateurl = "";
    if (Parm_Key_Value != "" && Parm_Key_Value != null) {
        reporturl = report;
        dateurl = date + "?Parm_Key_Value=" + Parm_Key_Value;
    } else {
        reporturl = report;
        dateurl = date + "?Parm_Key_Value=" + Parm_Key_Value;
    }
    CreatePrintViewerEx(Width, Height, reporturl, dateurl, bol, "");
}
function MyCreateDisplayReport(report, date, Parm_Key_Value, Width, Height, bol) {
    var reporturl = "", dateurl = "";
    if (Parm_Key_Value != "" && Parm_Key_Value != null) {
        reporturl = report;
        dateurl = date + "?Parm_Key_Value=" + Parm_Key_Value;
    } else {
        reporturl = report;
        dateurl = date + "?Parm_Key_Value=" + Parm_Key_Value;
    };
    CreateDisplayViewerEx(Width, Height, reporturl, dateurl, bol, "");
}

function NewMyCreateDisplayReport(report, date, Parm_Key_Value, Width, Height, bol) {

    var reporturl = "", dateurl = "";
    if (Parm_Key_Value != "" && Parm_Key_Value != null) {
        reporturl = report;
        dateurl = date + "?Parm_Key_Value=" + Parm_Key_Value;
    } else {
        reporturl = report;
        dateurl = date + "?Parm_Key_Value=" + Parm_Key_Value;
    }
    CreateDisplayViewerEx(Width, Height, reporturl, dateurl, bol, "");
}
function MyCreateReportInfo(report, Width, Height, bol) {
    var reporturl = "/ExampleModule/ReportManager/grf/" + report;
    CreatePrintViewerEx(Width, Height, reporturl, "", bol, "");
}
//初始化工具栏:1、预览 2、打印 3、导出
function UpdateToolbar(type) {
    if (ReportViewer.RemoveToolbarControl == null) {
        return false;
    }
    if (type == "1") {
        //Preview
        //        ReportViewer.RemoveToolbarControl(2);
        //        ReportViewer.RemoveToolbarControl(3);
        ReportViewer.RemoveToolbarControl(4);
        ReportViewer.RemoveToolbarControl(5);
        ReportViewer.RemoveToolbarControl(6);
        ReportViewer.RemoveToolbarControl(8);
        ReportViewer.RemoveToolbarControl(9);
        ReportViewer.RemoveToolbarControl(10);
        ReportViewer.RemoveToolbarControl(22);
        ReportViewer.RemoveToolbarControl(60);
        ReportViewer.RemoveToolbarControl(61);

    }
    else if (type == "2") {
        //Print
        ReportViewer.RemoveToolbarControl(4);
        //        ReportViewer.RemoveToolbarControl(5);
        ReportViewer.RemoveToolbarControl(6);
        ReportViewer.RemoveToolbarControl(8);
        ReportViewer.RemoveToolbarControl(9);
        ReportViewer.RemoveToolbarControl(10);
        ReportViewer.RemoveToolbarControl(22);
        ReportViewer.RemoveToolbarControl(60);
        ReportViewer.RemoveToolbarControl(61);

    }
    else if (type == "3") {
        //ExportXls
        ReportViewer.RemoveToolbarControl(2);
        ReportViewer.RemoveToolbarControl(3);
        ReportViewer.RemoveToolbarControl(4);
        ReportViewer.RemoveToolbarControl(5);
        ReportViewer.RemoveToolbarControl(6);
        ReportViewer.RemoveToolbarControl(8);
        ReportViewer.RemoveToolbarControl(9);
        ReportViewer.RemoveToolbarControl(10);
        ReportViewer.RemoveToolbarControl(22);
        ReportViewer.RemoveToolbarControl(61);

    }
    //最后更新显示
    ReportViewer.UpdateToolbar();
    //开启报表生成进度条显示
    ReportViewer.Report.ShowProgressUI = true;
}

//报表参数设置
//例：var Parameter="参数名，参数名|参数类型，参数类型|参数值,参数值"
function SetParameter(Parameter) {
    var strArr = new Array();
    if (Parameter != "") {
        strArr = unescape(Parameter).split("|");
        var ArrName = strArr.length >= 3 ? strArr[0].split(",") : "";
        var ArrType = strArr.length >= 3 ? strArr[1].split(",") : "";
        var ArrVal = strArr.length >= 3 ? strArr[2].split(",") : "";
        $.each(ArrName, function (n, value) {
            if (ReportViewer.Report == null) {
                return false;
            }
            if (ArrType[n] == "String") {
                if (ReportViewer.Report.ParameterByName(value) != null) {
                    ReportViewer.Report.ParameterByName(value).AsString = ArrVal[n];
                }
            }
            else if (ArrType[n] == "Boolean") {
                if (ReportViewer.Report.ParameterByName(value) != null) {
                    ReportViewer.Report.ParameterByName(value).AsBoolean = ArrVal[n];
                }
            }
            else if (ArrType[n] == "DateTime") {
                if (ReportViewer.Report.ParameterByName(value) != null) {
                    ReportViewer.Report.ParameterByName(value).AsDateTime = ArrVal[n];
                }
            }
            else if (ArrType[n] == "Float") {
                if (ReportViewer.Report.ParameterByName(value) != null) {
                    ReportViewer.Report.ParameterByName(value).AsFloat = ArrVal[n];
                }
            }
            else if (ArrType[n] == "Integer") {
                if (ReportViewer.Report.ParameterByName(value) != null) {
                    ReportViewer.Report.ParameterByName(value).AsInteger = ArrVal[n];
                }
            }
        });
    }
}

/*==================报表导出(Begin)============================*/
//导出
//Report报名对象,ReportName导出报表名称
function btnExportXls_OnClick() {
    var FileName = ExportPath + unescape(ReportName) + ".xls";
    //要改变导出默认选项参数，响应IGridppReport.ExportBegin 事件，在事件函数中设置选项参数属性，具体参考例子03.Export
    ReportViewer.Report.ExportDirect(1, FileName, true, true); //gretXLS = 1, 
}
//打印
function btnPrint_OnClick() {
    ReportViewer.Report.Print(true);
}
//预览
function btnPreview_OnClick() {
    ReportViewer.Report.PrintPreview(true);
}
//工具栏Excel导出事件邦定
function ReportImport() {
    ReportViewer.Report.OnExportBegin = OnExportBegin;
    ReportViewer.Report.OnExportEnd = OnExportEnd;
    //启动运行
    ReportViewer.Start();
}

function OnExportBegin(OptionObject) {
    //ExportBegin 事件在将报表导出之前会触发到，无论是调用 ExportDirect 与 Export 方法，
    //还是从打印预览窗口等地方执行导出，都会触发到 ExportBegin 事件。
    //通常在 ExportBegin 事件中设置导出选项参数，改变默认导出行为
    var Report = ReportViewer.Report;
    OptionObject.AbortOpenFile = true;  //导出后不用关联程序打开导出文件，如导出Excel文件之后不用Excel打开
    OptionObject.AbortShowOptionDlg = false;  //导出之前不显示导出选项设置对话框

    //根据导出类型设置其特有的选项参数，有关选项参数的具体信息清参考帮助文档。
    //IGRExportOption是导出选项的基类，其它具体导出选项的接口名称都以IGRE2为前缀
    if (OptionObject.ExportType == 1) //gretXLS	
    {
        Report.DetailGrid.ColumnTitle.RepeatStyle = 1; //grrsNone
        if (ReportName == "") {
            ReportName = "导出报表";
        }
        OptionObject.FileName = unescape(ReportName) + ".xls"; //指定导出文件的完整路径与文件名称
        var E2XLSOption = OptionObject.AsE2XLSOption;  //AsE2XLSOption是必须的
        E2XLSOption.OnlyExportDetailGrid = true;
        E2XLSOption.SupressEmptyLines = true;
        E2XLSOption.ColumnAsDetailGrid = true;
        E2XLSOption.OnlyExportPureText = true;
        E2XLSOption.SameAsPrint = true;
        E2XLSOption.ExportPageHeaderFooter = false;
        E2XLSOption.ExportPageBreak = false;
    }
}
function OnExportEnd(OptionObject) {
    var Report = ReportViewer.Report;
    Report.DetailGrid.ColumnTitle.RepeatStyle = 2; //grrsOnPage
}

