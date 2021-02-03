<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Registration.aspx.cs" Inherits="AS_Assignment.Registration" %>

<!DOCTYPE html>


<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    
    <title></title>
    <style type="text/css">
        .auto-style1 {
            width: 100%;
        }
        .auto-style2 {
            width: 202px;
        }
    </style>

    <script type="text/javascript">
        function Validate() {
            var str = document.getElementById('<%=tb_pwd.ClientID %>').value;

            if (str.length < 8) {
                document.getElementById("lbl_pwdchecker").innerHTML = "Password length must be at least 8 characters";
                document.getElementById("lbl_pwdchecker").style.color = "Red";
                return ("too_short");
            }
            if (str.search(/[0-9]/) == -1) {
                document.getElementById("lbl_pwdchecker").innerHTML = "Password require at least 1 number";
                document.getElementById("lbl_pwdchecker").style.color = "Red";
                return("no_number")
            }
             if (str.search(/[A-Z]/) == -1) {
                document.getElementById("lbl_pwdchecker").innerHTML = "Password require at least 1 Upper Case Character";
                document.getElementById("lbl_pwdchecker").style.color = "red";
                return ("no_upper")
            }
            if (!/[a-z]/.test(str)) {
                document.getElementById("lbl_pwdchecker").innerHTML = "Password require at least 1 Lower Case Character";
                document.getElementById("lbl_pwdchecker").style.color = "red";
                return ("no_lower")
            }
            if (!format.test(str)) {
                document.getElementById("lbl_pwdchecker").innerHTML = "Password require at least 1 Special Character";
                document.getElementById("lbl_pwdchecker").style.color = "red";
                return ("no_special")
            }

            document.getElementById("lbl_pwdchecker").innerHTML = "Execllent!";
            document.getElementById("lbl_pwdchecker").style.color = "Blue";
        }
    </script>
     <style>
        $font-family: "Roboto";
        $font-size: 14px;

        $color-primary: #ABA194;

        * {
            margin: 0;
            padding: 0;
            box-sizing: border-box;
        }

        body {
            font-family: $font-family;
            font-size: $font-size;
            background-size: 200% 100% !important;
            animation: move 10s ease infinite;
            transform: translate3d(0, 0, 0);
            background: linear-gradient(45deg, #49D49D 10%, #A2C7E5 90%);
            height: 100vh
        }

        .user {
            width: 90%;
            max-width: 340px;
            margin: 10vh auto;
        }

        .user__header {
            text-align: center;
            opacity: 0;
            transform: translate3d(0, 500px, 0);
            animation: arrive 500ms ease-in-out 0.7s forwards;
        }

        .user__title {
            font-size: $font-size;
            margin-bottom: -10px;
            font-weight: 500;
            color: white;
        }

        .form {
            margin-top: 40px;
            border-radius: 6px;
            overflow: hidden;
            opacity: 0;
            transform: translate3d(0, 500px, 0);
            animation: arrive 500ms ease-in-out 0.9s forwards;
        }

        .form--no {
            animation: NO 1s ease-in-out;
            opacity: 1;
            transform: translate3d(0, 0, 0);
        }

        .form__input {
            display: block;
            width: 100%;
            padding: 20px;
            font-family: $font-family;
            -webkit-appearance: none;
            border: 0;
            outline: 0;
            transition: 0.3s;
            &:focus

        {
            background: darken(#fff, 3%);
        }

        }

        .btn {
            display: block;
            width: 100%;
            padding: 20px;
            font-family: $font-family;
            -webkit-appearance: none;
            outline: 0;
            border: 0;
            color: black;
            background: $color-primary;
            transition: 0.3s;
            &:hover

        {
            background: darken($color-primary, 5%);
        }

        }

        @keyframes NO {
            from, to {
                -webkit-transform: translate3d(0, 0, 0);
                transform: translate3d(0, 0, 0);
            }

            10%, 30%, 50%, 70%, 90% {
                -webkit-transform: translate3d(-10px, 0, 0);
                transform: translate3d(-10px, 0, 0);
            }

            20%, 40%, 60%, 80% {
                -webkit-transform: translate3d(10px, 0, 0);
                transform: translate3d(10px, 0, 0);
            }
        }

        @keyframes arrive {
            0% {
                opacity: 0;
                transform: translate3d(0, 50px, 0);
            }

            100% {
                opacity: 1;
                transform: translate3d(0, 0, 0);
            }
        }

        @keyframes move {
            0% {
                background-position: 0 0
            }

            50% {
                background-position: 100% 0
            }

            100% {
                background-position: 0 0
            }
        }
    </style>
</head>
    
<body>
    <div class="user">
    <header class="user__header">
            <img src="https://s3-us-west-2.amazonaws.com/s.cdpn.io/3219/logo.svg" alt="" />
            <h1 class="user__title">Account Registration</h1>
        </header>
    <form id="form1" runat="server" class="form">
        

            
            
                <div class="form__group">
                        <asp:Label ID="Label2" runat="server" Text="Email (UserId)" CssClass="form__input"></asp:Label>
                  
                        <asp:TextBox ID="tb_userid" runat="server"  CssClass="form__input"></asp:TextBox>
                    <asp:Label ID="lbl_email" runat="server" Text=""></asp:Label>
                </div>
                
               
                <div class="form__group">
                        <asp:Label ID="Label6" runat="server" Text="First Name" CssClass="form__input"></asp:Label>
                   
                        <asp:TextBox ID="tb_fname" runat="server"  CssClass="form__input"></asp:TextBox>
                   </div>
               
                    <div class="form__group">
                        <asp:Label ID="Label7" runat="server" Text="Last Name" CssClass="form__input"></asp:Label>
                   
                        <asp:TextBox ID="tb_lname" runat="server"  CssClass="form__input"></asp:TextBox>
                    </div>
                <div class="form__group">
                        <asp:Label ID="Label8" runat="server" Text="Date of Birth" CssClass="form__input"></asp:Label>
                    
                        <asp:TextBox ID="tb_dob" runat="server"  CssClass="form__input"></asp:TextBox>
                   </div>
                    
                     
            <div class="form__group">
            <asp:Label ID="Label3" runat="server" Text="Password" CssClass="form__input"></asp:Label>
                   
                        <asp:TextBox ID="tb_pwd" runat="server"  onkeyup="javascript:Validate()" CssClass="form__input" TextMode="Password"></asp:TextBox>
                        <asp:Label ID="lbl_state" runat="server"></asp:Label>
                        <asp:Label ID="lbl_pwdchecker" runat="server" Text=""></asp:Label>
                 </div>   
                
            <div class="form__group">
                        <asp:Label ID="Label4" runat="server" Text="Confirm Password" CssClass="form__input"></asp:Label>
                    
                        <asp:TextBox ID="tb_cfpwd" runat="server"  CssClass="form__input" TextMode="Password"></asp:TextBox>
                    </div>

                <div class="form__group">
                        <asp:Label ID="Label5" runat="server" Text="Credit Card Number" CssClass="form__input"></asp:Label>
                    
                        <asp:TextBox ID="tb_card" runat="server"  CssClass="form__input"></asp:TextBox>
                 
                </div>
               
        <br />
                <div class="form__group">
                        <asp:Button ID="Button1" runat="server" OnClick="btn_Submit_Click" Text="Submit"  CssClass="btn" />
           </div>
        <br />
        <br />
         <div class="form__group">
                        <asp:Button ID="Button2" runat="server" OnClick="btn_login" Text="Login"  CssClass="btn" />
           </div>
            <asp:Label ID="lbl_error" runat="server" Text=""></asp:Label>
            <br />
     
    </form>
        </div>
</body>
</html>
