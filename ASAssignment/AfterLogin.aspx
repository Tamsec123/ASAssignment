<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AfterLogin.aspx.cs" Inherits="ASAssignment.AfterLogin" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .auto-style1 {
            margin-left: 369px;
        }
        .auto-style2 {
            margin-left: 65px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
        </div>
        <asp:Label ID="lbl_message" runat="server"></asp:Label>
        <br />
        <br />
        <br />
        <br />
        <asp:Button ID="btn_Logout" OnClick="LogMeOut" runat="server" CssClass="auto-style1" Text="Logout" Width="70px" />
        <asp:Button ID="btn_ChangePassword" runat="server" CssClass="auto-style2" OnClick="btn_ChangePassword_Click" Text="Change Password" />
    </form>
</body>
</html>
