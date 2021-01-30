﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs" Inherits="AS_Assignment.Password" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
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
            <h1 class="user__title">Change Password</h1>
        </header>
    <form id="form1" runat="server" class="form">
        
        
             <div class="form__group">

             <asp:Label ID="lb_name" runat="server" Font-Bold="True" Text="" CssClass="form__input"></asp:Label>
            <asp:Label ID="Error" runat="server" Font-Bold="True" Text="" CssClass="form__input"></asp:Label>
                 </div>
            
         <div class="form__group">
                        <asp:Label ID="Label2" runat="server" Text="Old Password" CssClass="form__input"></asp:Label>
                    
                        <asp:TextBox ID="tb_oldpass" runat="server" CssClass="form__input" TextMode="Password"></asp:TextBox>
                </div>
               
                <div class="form__group">
                        <asp:Label ID="Label6" runat="server" Text="Password" CssClass="form__input"></asp:Label>
                    
                        <asp:TextBox ID="tb_newpass" runat="server"  CssClass="form__input" TextMode="Password"></asp:TextBox>
                   
                </div>

                 <div class="form__group">
                        <asp:Label ID="Label3" runat="server" Text="Confirm Password" CssClass="form__input"></asp:Label>
                    
                        <asp:TextBox ID="tb_confirmpass" runat="server" CssClass="form__input" TextMode="Password"></asp:TextBox>
                    </div>

                 <div class="form__group">
                    
                        <asp:Button ID="Button1" runat="server" OnClick="Submit"  Text="Change Password" CssClass="btn" />
                    </div>


                
            <asp:Label ID="lb_error" runat="server" Text=""></asp:Label>
            <asp:Label ID="lbl_pwdchecker" runat="server" Text=""></asp:Label>
            <asp:Label ID="lbl_state" runat="server" Text=""></asp:Label>
    </form>
        </div>
</body>
</html>
