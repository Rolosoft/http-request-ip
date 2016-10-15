<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Rsft.HttpRequestIp.Test.WebApp.Default" Title="Rsft.HttpRequestIp Integration Test" Trace="true" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        table{ border-collapse: collapse;}
        table caption{ font-weight: bold;font-size: 1.2em;}
        table tr td,table tr th{ padding: 3px;}
        table tr td{ text-align: left;}
        table tr th{ text-align: right;}
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table>
            <caption>Http Request Details</caption>
            <tr>
                <th scope="row">Best Guess IP</th>
                <td><%=Rsft.HttpRequestIp.Getter.Get().BestGuessIp %></td>
            </tr>
            <tr>
                <th scope="row">IsProxied</th>
                <td><%=Rsft.HttpRequestIp.Getter.Get().IsProxied %></td>
            </tr>
            <tr>
                <th scope="row">HttpForwardedForHeader</th>
                <td><%=Rsft.HttpRequestIp.Getter.Get().ServerVariables.HttpForwardedForHeader %></td>
            </tr>
            <tr>
                <th scope="row">HttpForwardedHeader</th>
                <td><%=Rsft.HttpRequestIp.Getter.Get().ServerVariables.HttpForwardedHeader %></td>
            </tr>
            <tr>
                <th scope="row">HttpViaHeader</th>
                <td><%=Rsft.HttpRequestIp.Getter.Get().ServerVariables.HttpViaHeader %></td>
            </tr>
            <tr>
                <th scope="row">HttpXForwardedForHeader</th>
                <td><%=Rsft.HttpRequestIp.Getter.Get().ServerVariables.HttpXForwardedForHeader %></td>
            </tr>
            <tr>
                <th scope="row">RemoteAddressHeader</th>
                <td><%=Rsft.HttpRequestIp.Getter.Get().ServerVariables.RemoteAddressHeader %></td>
            </tr>
            <tr>
                <th scope="row">CF_IPCOUNTRY</th>
                <td><%=Rsft.HttpRequestIp.Getter.Get().ServerVariables.IpCountry %></td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
