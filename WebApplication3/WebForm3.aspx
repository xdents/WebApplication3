<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm3.aspx.cs" Inherits="WebApplication3.WebForm3" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <link href="css/common.css" rel="stylesheet" />
    <link href="layui/css/layui.css" rel="stylesheet" />
    <link href="css/sourcedetail.css" rel="stylesheet" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div class="ReportBody">
            <table class="TableClass">
                <tr class="layui-form">
                    <td>
                        <select name="drpTableType" class="layui-select" lay-filter="TempTableType" lay-search style="height:28px">
                            <option disabled="disabled" selected="selected" value="">Please Choose</option>
                            <% if (TableTypeList != null)
                                {
                                    foreach (var TempTableType in TableTypeList)
                                    {
                                        %>
                                        <option value="<%=TempTableType.ProcName %>"><%=TempTableType.TableName %></option>
                                        <%
                                    }
                                } %>
                        </select>
                    </td>
                    <td>
                        <asp:FileUpload ID="FileUpload" runat="server" />
                        <asp:Button ID="BtnUpload" runat="server" OnClick="BtnUpload_Click" OnClientClick="setHiddenField()" class="ButtonClass"  Text="Upload" />
                    </td>
                </tr>
                <tr class="layui-form">
                    <td>
                        <select name="drpPeriod" class="layui-select" lay-filter="TempPeriod" lay-search style="height:28px">
                            <option disabled="disabled" selected="selected" value="">Please Choose</option>
                            <% if (PeriodList != null)
                                {
                                    foreach (var TempPeriod in PeriodList)
                                    {
                                        %>
                                        <option value="<%=TempPeriod.Period %>"><%=TempPeriod.Period %></option>
                                        <%
                                    }
                                } %>
                        </select>
                    </td>
                    <td>
                        <asp:Button ID="BtnRefresh" runat="server" OnClick="BtnRefresh_Click" Text="Search" class="ButtonClass" OnClientClick="setHiddenField()"  />
                    </td>
                </tr>
            </table>
            <table>
                <tr><td><asp:Label ID="LblMsg" runat="server" Text=""></asp:Label></td></tr>
            </table>
            <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" />
            <div class="content">
                <rsweb:ReportViewer ID="ReportViewer1" PageCountMode="Actual" runat="server" Width="100%" Height="100%">
                </rsweb:ReportViewer>
            </div>
        </div>
        <asp:HiddenField ID="hfInfo" runat="server" />
    </form>
    <script src="Scripts/jquery-1.12.4.js"></script>
    <script src="layui/layui.js"></script>
    <script>
        var info = {};
        function setHiddenField() {
            var $info = $('#hfInfo');
            $info.val(JSON.stringify(info));
        }

        layui.use(['form'], function () {
            var form = layui.form;

            form.on('select(TempTableType)', function (data) {
                info.ProcName = data.value;
            });

            form.on('select(TempPeriod)', function (data) {
                info.Period = data.value;
            });
        });
    </script>
</body>
</html>
