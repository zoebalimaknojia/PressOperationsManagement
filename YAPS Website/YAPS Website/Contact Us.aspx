<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Contact Us.aspx.cs" Inherits="YAPS_Website.Contact_Us" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <p align="left">
            <font size="4" face="Comic Sans Ms" color="Cream"><b>Yamuna Art Printers</b><br />
            </font><font size="4" face="Comic Sans Ms" color="red">12, First Floor, Near Jalaram
                Mandir Laxmi Estate,<br />
                Opposite Gujarat Samachar,<br />
                Karelibaug Vadodara, Gujarat - 390018.<br />
                Tel : 0265-2460144<br />
            </font>
        </p>
    </div>
    <center>
        <br />
        <center>
            <form name="ContactSubmit" method="post" action="Contact Us.aspx">
            <table class="clsmain_cutcontact">
                <tr>
                    <td style="width: 111px" height="30">
                        <asp:Label ID="Label1" runat="server" Font-Size="Large" Font-Names="Comic Sans Ms"
                            Text="Name :*"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="ClientName" runat="server" Width="250"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Name"
                            Text="*" ControlToValidate="ClientName"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ControlToValidate="ClientName"
                            ErrorMessage="Invalid Name" ValidationExpression="[a-zA-z\s]*"></asp:RegularExpressionValidator>
                    </td>
                </tr>
                <tr>
                    <td style="width: 111px" height="30">
                        <asp:Label ID="Label2" runat="server" Font-Size="Large" Font-Names="Comic Sans Ms"
                            Text="Email :*"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="ClientEmail" runat="server" Width="180"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Email Address"
                            Text="*" ControlToValidate="ClientEmail"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="ClientEmail"
                            ErrorMessage="Enter Proper Email Address" ValidationExpression="((\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)*([,])*)*"></asp:RegularExpressionValidator>
                        <br />
                    </td>
                </tr>
                <tr>
                    <td style="width: 111px" height="30">
                        <asp:Label ID="Label3" runat="server" Font-Size="Large" Font-Names="Comic Sans Ms"
                            Text="Contact No :*"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="ClientContactNo" Width="180" runat="server" MaxLength="12"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="ClientContactNo"
                            ErrorMessage="Contact No.">*</asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="ClientContactNo"
                            ErrorMessage="Invalid Contact No." ValidationExpression="[0-9]*"></asp:RegularExpressionValidator>
                    </td>
                </tr>
                <tr>
                    <td style="width: 120px" height="30">
                        <asp:Label ID="Label4" runat="server" Font-Size="Large" Font-Names="Comic Sans Ms"
                            Text="Feedback :*"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="ClientComment" runat="server" TextMode="MultiLine" Height="100"
                            Width="300"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Feedback"
                            ControlToValidate="ClientComment">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" height="100">
                        <asp:Button ID="Button1" runat="server" class="mybutton" Text="Submit" OnClick="Button1_Click"
                            EnableViewState="False" />&nbsp;&nbsp;&nbsp;<input id="Reset1" class="mybutton" type="reset"
                                value="Reset" />
                    </td>
                </tr>
                <asp:Label ID="MailStatus" runat="server" Text=""></asp:Label>
            </table>
            <br />
            <asp:ValidationSummary ID="ValidationSummary1" HeaderText="You must enter a value in following fields"
                DisplayMode="List" runat="server" />
            </form>
        </center>
        <br />
        <br />
        <br />
    </center>
</asp:Content>
