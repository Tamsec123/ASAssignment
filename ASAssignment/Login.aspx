<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="ASAssignment.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .auto-style1 {
            margin-left: 70px;
        }

        .auto-style2 {
            margin-left: 41px;
        }

        .auto-style3 {
            margin-left: 293px;
        }
        .auto-style4 {
            margin-left: 108px;
        }
    </style>
    <script src="https://www.google.com/recaptcha/api.js?render="></script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
        </div>
        <br />
        <br />
        <br />
        <input type="hidden" id="g-recaptcha-response" name="g-recaptcha-response" />
        <asp:Label ID="Label1" runat="server" Text="Email: "></asp:Label>
        <asp:TextBox ID="tb_email" runat="server" CssClass="auto-style1"></asp:TextBox>
        <br />
        <br />
        <asp:Label ID="Label2" runat="server" Text="Password: "></asp:Label>
        <asp:TextBox ID="tb_password" runat="server" TextMode="Password" CssClass="auto-style2"></asp:TextBox>
        <br />
        <br />
        <asp:Button ID="btn_login" runat="server" CssClass="auto-style3" Text="Login" OnClick="btn_login_Click" />
        
        <asp:Button ID="btn_Recover" runat="server" CssClass="auto-style4" Visible="false" Text="Recover account" OnClick="btn_Recover_Click" />
        
        <asp:Button ID="btn_changePassword" runat="server" CssClass="auto-style4" Visible="false" Text="Change Password" OnClick="btn_changePassword_Click" />
        
        <br />
        <asp:Label ID="lblError" runat="server"></asp:Label>
    </form>
    <script>
        grecaptcha.ready(function () {
            grecaptcha.execute('', { action: 'Login' }).then(function (token) {
                document.getElementById("g-recaptcha-response").value = token;
            });
        });
    </script>
</body>
</html>
