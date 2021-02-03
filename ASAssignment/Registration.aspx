<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Registration.aspx.cs" Inherits="ASAssignment.Registration" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .auto-style1 {
            margin-left: 41px;
        }
        .auto-style2 {
            margin-left: 99px;
        }
        .auto-style3 {
            margin-left: 79px;
        }
        .auto-style4 {
            margin-left: 124px;
        }
        .auto-style5 {
            margin-left: 74px;
        }
        .auto-style6 {
            margin-left: 73px;
        }
        .auto-style7 {
            margin-left: 75px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <br />
            <br />
            <br />
            <br />
            First Name:&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:TextBox ID="tbFirstName" runat="server" CssClass="auto-style5"></asp:TextBox>
        </div>
        <p>
            Last Name:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <asp:TextBox ID="tbLastName" runat="server" CssClass="auto-style7"></asp:TextBox>
        </p>
        <p>
            Card Number:
            <asp:TextBox ID="tbCardNumber" runat="server" Width="253px" CssClass="auto-style6"></asp:TextBox>
        </p>
        <p>
            Expiration date:<asp:TextBox ID="tbExpDate" runat="server" CssClass="auto-style5"></asp:TextBox>
&nbsp; CVV:<asp:TextBox ID="tbCVV" runat="server" style="margin-left: 19px" Width="54px"></asp:TextBox>
        </p>
        <p>
            Email:
            <asp:TextBox ID="tbEmail" runat="server" Width="167px" TextMode="Email" CssClass="auto-style4"></asp:TextBox>
        </p>
        <p>
            Password:
            <asp:TextBox ID="tbPassword" ClientIDMode="Static" runat="server" Width="163px" TextMode="Password" CssClass="auto-style2"> </asp:TextBox>
            
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Button ID="btn_Check" runat="server" CssClass="auto-style1" OnClick="btn_Check_Click" style="height: 26px" Text="Check Password" />
            <asp:Label ID="lbl_pwdchecker" runat="server"></asp:Label>
        </p>
        <p>
            Confirm password:
            <asp:TextBox ID="tbPasswordCfm" ClientIDMode="Static" runat="server" style="margin-left: 45px" Width="163px" TextMode="Password"></asp:TextBox>
            
        </p>
        <p>
            Date of Birth: <asp:TextBox ID="tbDoB" runat="server" Width="167px" CssClass="auto-style3"></asp:TextBox>
        </p>
        <p>
            &nbsp;</p>
        <p>
            &nbsp;</p>
        <p>
            <asp:Button ID="btn_submit" runat="server" style="margin-left: 401px" Text="Submit" Width="172px" OnClick="btn_submit_Click" />
        &nbsp;&nbsp;&nbsp;
        </p>
        <p>
            &nbsp;<asp:Label ID="lbl_msg" runat="server" ForeColor="Red"></asp:Label>
        </p>
        <p>
            &nbsp;</p>
        <p>
            &nbsp;</p>
        
    </form>
    
</body>
    <script type="text/javascript">


        var pass = document.getElementById("tbPassword")
        pass.addEventListener('keyup', function () {
            validate()    
        })
        function validate() {
            var str = document.getElementById('<%=tbPassword.ClientID %>').value;
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
</html>
