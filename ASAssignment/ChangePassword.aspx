<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs" Inherits="ASAssignment.ChangePassword" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .auto-style2 {
            margin-left: 18px;
        }
        .auto-style3 {
            margin-left: 35px;
        }
        .auto-style4 {
            margin-left: 172px;
        }
        .auto-style5 {
            margin-left: 72px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            
            <br />
            Current Password:
            <asp:TextBox ID="tbCurrPassword" runat="server" TextMode="Password" CssClass="auto-style2"></asp:TextBox>
            <br />
            New Password:
            <asp:TextBox ID="tbNewPassword" runat="server" TextMode="Password" CssClass="auto-style3"></asp:TextBox>
            <br />
            
        <asp:Label ID="lblMessage" runat="server"></asp:Label>
            <br />
            <asp:Button ID="btn_Submit" runat="server" CssClass="auto-style4" OnClick="btn_Submit_Click" Text="Submit" />
            <asp:Button ID="btn_BackToLogin" runat="server" CssClass="auto-style5" OnClick="btn_BackToLogin_Click" Text="Back to Login" />
        </div>
        

    </form>
</body>
</html>
