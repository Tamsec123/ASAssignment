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
        .auto-style6 {
            margin-left: 12px;
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
            
        <asp:Label ID="lbl_pwdchecker" runat="server"></asp:Label>
            <br />
            Confirm Password:&nbsp;
            <asp:TextBox ID="tbCfmNewPassword" runat="server" TextMode="Password" CssClass="auto-style6"></asp:TextBox>
            <br />
            
        <asp:Label ID="lblMessage" runat="server"></asp:Label>
            <br />
            <asp:Button ID="btn_Submit" runat="server" CssClass="auto-style4" OnClick="btn_Submit_Click" Text="Submit" />
            <asp:Button ID="btn_BackToLogin" runat="server" CssClass="auto-style5" OnClick="btn_BackToLogin_Click" Text="Back to Login" />
        </div>
        

    </form>
    <script type="text/javascript">


        var pass = document.getElementById("tbNewPassword")
        pass.addEventListener('keyup', function () {
            validate()    
        })
        function validate() {
            var str = document.getElementById('<%=tbNewPassword.ClientID %>').value;
            if (str.length < 8) {
                document.getElementById("lbl_pwdchecker").innerHTML = "Password length must be at least 8 characters";
                document.getElementById("lbl_pwdchecker").style.color = "Red";
                return ("too_short");
            }
            else if (str.search(/[0-9]/) == -1) {
                document.getElementById("lbl_pwdchecker").innerHTML = "Password requires at least 1 number";
                document.getElementById("lbl_pwdchecker").style.color = "Red";
                return ("no_number");
            }
            else if (str.search(/[A-Z]/) == -1) {
                document.getElementById("lbl_pwdchecker").innerHTML = "Password requires at least 1 uppercase letter";
                document.getElementById("lbl_pwdchecker").style.color = "Red";
                return ("no_upper");
            }
            else if (str.search(/[a-z]/) == -1) {
                document.getElementById("lbl_pwdchecker").innerHTML = "Password requires at least 1 lowercase letter";
                document.getElementById("lbl_pwdchecker").style.color = "Red";
                return ("no_lower");
            }
            else if (str.search(/[!@#$%^&*()_+\-=\[\]{};':"\\|,.<>\/?*$]/) == -1) {
                /*alternative str.search(/[^A-Za-z0-9]/) */
                document.getElementById("lbl_pwdchecker").innerHTML = "Password requires at least 1 special character";
                document.getElementById("lbl_pwdchecker").style.color = "Red";
                return ("no_special");
            }

            document.getElementById("lbl_pwdchecker").innerHTML = "Excellent!";
            document.getElementById("lbl_pwdchecker").style.color = "Green";
        }
    </script>
</body>
</html>
