﻿<%@ Control Language="C#" AutoEventWireup="true" CodeFile="RequestList.ascx.cs" Inherits="RockWeb.Blocks.Security.BackgroundCheck.RequestList" %>

<asp:UpdatePanel ID="upList" runat="server">
    <ContentTemplate>

        <asp:Panel ID="pnlContent" runat="server">

            <div class="panel panel-block">
                <div class="panel-heading">
                    <h1 class="panel-title"><i class="fa fa-file-text-o"></i> Requests</h1>
                </div>
                <div class="panel-body">

                    <Rock:NotificationBox ID="nbNotice" runat="server" Visible="false" />

                    <div class="grid grid-panel">

                        <Rock:GridFilter ID="fRequest" runat="server">
                            <Rock:RockTextBox ID="tbFirstName" runat="server" Label="First Name" />
                            <Rock:RockTextBox ID="tbLastName" runat="server" Label="Last Name" />
                            <Rock:DateRangePicker ID="drpRequestDates" runat="server" Label="Request Date Range" />
                            <Rock:DateRangePicker ID="drpResponseDates" runat="server" Label="Response Date Range" />
                            <Rock:RockDropDownList ID="ddlRecordFound" runat="server" Label="Record Found">
                                <asp:ListItem Text="" Value=""></asp:ListItem>
                                <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                                <asp:ListItem Text="No" Value="No"></asp:ListItem>
                            </Rock:RockDropDownList>
                        </Rock:GridFilter>
        
                        <Rock:ModalAlert ID="mdGridWarning" runat="server" />
                
                        <Rock:Grid ID="gRequest" runat="server" AllowSorting="true" PersonIdField="PersonId">
                            <Columns>

                                <Rock:RockBoundField DataField="Name" HeaderText="Name" SortExpression="Name" HtmlEncode="false" />
                                <Rock:DateField DataField="RequestDate" HeaderText="Request Date" SortExpression="RequestDate" />
                                <Rock:DateField DataField="ResponseDate" HeaderText="Response Date" SortExpression="ResponseDate" />
                                <Rock:RockBoundField ItemStyle-HorizontalAlign="Center" DataField="RecordFoundLabel" HeaderText="Record Found" SortExpression="RecordFound" HtmlEncode="false" />
                                <Rock:LinkButtonField HeaderText="Response XML" Text="View" OnClick="gRequest_XML" HeaderStyle-CssClass="" ItemStyle-CssClass="" />
                                <asp:HyperLinkField HeaderText="Response Document" DataNavigateUrlFields="ResponseDocumentId" DataNavigateUrlFormatString="~/GetFile.ashx?id={0}" DataTextField="ResponseDocumentText" ItemStyle-HorizontalAlign="Center" />

                            </Columns>
                        </Rock:Grid>
                    </div>

                </div>
            </div>

        </asp:Panel>

        <Rock:ModalDialog ID="dlgResponse" runat="server" Title="Response XML">
            <Content>
                <Rock:RockTextBox ID="tbResponseXml" runat="server" TextMode="MultiLine" Rows="20" ReadOnly="true" ValidateRequestMode="Disabled" />
            </Content>
        </Rock:ModalDialog>

    </ContentTemplate>
</asp:UpdatePanel>