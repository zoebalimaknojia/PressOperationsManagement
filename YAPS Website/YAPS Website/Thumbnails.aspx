<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Thumbnails.aspx.cs" Inherits="YAPS_Website.Wedding" EnableEventValidation="false" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


    


    <asp:ListView ID="ListView1" runat="server" GroupItemCount="3" ViewStateMode="Inherit">
        <GroupTemplate>
            <tr id="itemPlaceholderContainer" runat="server">
                <td id="itemPlaceholder" runat="server">
                </td>
            </tr>
        </GroupTemplate>
        <LayoutTemplate>
            <table id="Table2" runat="server">
                <tr id="Tr1" runat="server">
                    <td id="Td3" runat="server">
                        <table id="groupPlaceholderContainer" runat="server" border="5" style="">
                            <tr id="groupPlaceholder" runat="server">
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr id="Tr2" runat="server">
                    <td id="Td4" runat="server" style="">
                        <asp:DataPager ID="DataPager1" runat="server" PageSize="12">
                            <Fields>
                                <asp:NextPreviousPagerField ButtonType="Button" ShowFirstPageButton="True" ShowNextPageButton="False"
                                    ShowPreviousPageButton="False" />
                                <asp:NumericPagerField />
                                <asp:NextPreviousPagerField ButtonType="Button" ShowLastPageButton="True" ShowNextPageButton="False"
                                    ShowPreviousPageButton="False" />
                            </Fields>
                        </asp:DataPager>
                    </td>
                </tr>
            </table>
        </LayoutTemplate>
        <ItemTemplate>
            <td id="Td2" runat="server" style="">
                <div id="list">
                    <li>
                        <div class="clsThumbholder">
                            <div id="content_wrapper">
                                <div id="content">
                                    <div class="zitem">
                                        <asp:HyperLink NavigateUrl='<%# Bind("ImageSource") %>' ID="dip" ToolTip='<%# Bind("ImageName") %>'
                                            runat="server" CssClass="lightbox" rel="group1">
                                            <asp:Image ID="Image1" ImageUrl='<%# Bind("ImageSource") %>' Width="220" Height="200"
                                                runat="server" Style="float: left" /><br />
                                            <div class="caption">
                                                <asp:Label ID="abcc" runat="server" Text='<%# Bind("ImageName") %>' /><br />
                                            </div>
                                        </asp:HyperLink></div></div></div></div></li></div></td></ItemTemplate></asp:ListView><asp:Label ID="smsstatus" runat="server" Text="smsstatu" Visible="False"></asp:Label><br />
    <asp:Label ID="mobilelabel" runat="server" Text="Mobile Number :"></asp:Label><asp:TextBox
        ID="mobileno" runat="server" Height="22px" MaxLength="10"></asp:TextBox><asp:RequiredFieldValidator
            ID="RequiredFieldValidator1" runat="server" ControlToValidate="mobileno" ErrorMessage="Mobile Number">*</asp:RequiredFieldValidator><asp:Label
                ID="Label1" runat="server" Text="Design ID :">
            </asp:Label><asp:TextBox ID="messagebody" runat="server" MaxLength="160"></asp:TextBox><asp:RequiredFieldValidator
                ID="RequiredFieldValidator2" runat="server" ControlToValidate="messagebody" ErrorMessage="Message">*</asp:RequiredFieldValidator>
                
                
                
                <asp:Button 
        ID="Button1" runat="server" Text="Send" onclick="Button1_Click1" /><asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="mobileno"
        ErrorMessage="Invalid Contact No." ValidationExpression="[0-9]*"></asp:RegularExpressionValidator><br />
    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="mobileno"
        ErrorMessage="Enter 10 Digit No." ValidationExpression=".{10}.*"></asp:RegularExpressionValidator><asp:ValidationSummary
            HeaderText="You must enter a value in following fields" DisplayMode="List" ID="ValidationSummary1"
            runat="server" />
</asp:Content>
