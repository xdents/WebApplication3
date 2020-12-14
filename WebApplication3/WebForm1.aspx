<%@ Page Language="C#" Debug="true" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="WebApplication3.WebForm1" %>

<%@ Register Assembly="WebApplication3" Namespace="WebApplication3" TagPrefix="cc1" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">


<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <script type="text/javascript" src="Scripts/jquery-1.12.4.js"></script>
    <link href="Content/StyleSheet1.css" rel="stylesheet" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <script type="text/javascript">
        function additem() {
            var dataList = [

                "6211125886667895", "6211125886667892", "6211125886667897"

            ];

            debugger;
            for (var i = 0; i < dataList.length; i++) {

                //先创建好select里面的option元素

                var option = document.createElement("option");

                //转换DOM对象为JQ对象,好用JQ里面提供的方法 给option的value赋值

                $(option).val(dataList[i]);

                //给option的text赋值,这就是你点开下拉框能够看到的东西

                $(option).text(dataList[i]);

                //获取select 下拉框对象,并将option添加进select

                $('#problemTypeId').append(option);

            }
        }
    </script>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <cc1:MyWebControl ID="MyWebControl1" runat="server" Href="www.baidu.com">test</cc1:MyWebControl>
            <select id ="problemTypeId"></select>
            <input id="Button1" type="button" value="button" onclick="additem()" />
            <asp:TextBox ID="a1" runat="server" OnTextChanged="a1_TextChanged"></asp:TextBox>
            <asp:Label runat="server" ID="lbl"></asp:Label>
            <button value="aaaa" title="bbbb" ></button>
            <label title="ccc" ></label>
            <input value="dd" onkeyup />
       </div>
    </form>
</body>
</html>
