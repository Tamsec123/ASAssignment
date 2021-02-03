<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RecoverAccount.aspx.cs" Inherits="ASAssignment.RecoverAccount" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .auto-style1 {
            margin-left: 75px;
        }
        .auto-style2 {
            margin-left: 145px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            Verification Code:
            <asp:Image ID="Image1" ImageUrl="~/Captcha.aspx" runat="server" Height="80px" Width="226px" />
            <br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Label ID="lblCaptchaMessage" runat="server"></asp:Label>
            <br />
            <br />
            Enter the code:
            <asp:TextBox ID="tbVerification" runat="server" CssClass="auto-style1" Width="217px"></asp:TextBox>
            <br />
            <br />
            <asp:Button ID="btn_submit" runat="server" Text="Submit" OnClick="btn_submit_Click" />
            <asp:Button ID="btn_goToLogin" runat="server" Text="Back To Login" Visible="false" CssClass="auto-style2" OnClick="btn_goToLogin_Click" />
            <br />
            <asp:Label ID="lblError" runat="server"></asp:Label>
        </div>
    </form>
</body>
</html>
