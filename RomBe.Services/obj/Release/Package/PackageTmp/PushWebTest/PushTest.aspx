<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PushTest.aspx.cs" Inherits="Dreamworks.Services.PushWebTest.PushTest" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
       <%-- <h1>Iphone</h1>--%>
        <table style="width: 100%;display:none">
            <tr>
                <td>Device Token</td>
                <td>Type</td>
                <td>Message</td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>
                    <asp:TextBox ID="TxtDeviceTokenIphone" runat="server"></asp:TextBox>
                </td>
                <td>
                    <asp:TextBox ID="TxtTypeIphone" runat="server"></asp:TextBox>
                </td>
                <td>
                    <asp:TextBox ID="TxtMessageIphone" runat="server"></asp:TextBox>
                </td>
                <td>
                    <asp:Button Text="SendPushIphone" runat="server" OnClick="OnClickSendPushIphone" />
                </td>
            </tr>
        </table>
        <h1>Android</h1>
        <table style="width: 100%;">
            <tr>
                <td>Email</td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>
                    <asp:TextBox ID="TxtEmail" runat="server"></asp:TextBox>
                </td>
                <td>
                    <asp:Button Text="SendPushAndroid" runat="server" OnClick="OnClickSendPushAndroid" />
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
