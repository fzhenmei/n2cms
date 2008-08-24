<%@ Page Language="C#" MasterPageFile="~/Layouts/Top+SubMenu.Master" AutoEventWireup="true" CodeBehind="Container.aspx.cs" Inherits="N2.Templates.News.UI.Container" Title="" %>
<asp:Content ContentPlaceHolderID="PostContent" runat="server">
    <n2:ItemDataSource id="idsNews" runat="server" />
    <asp:Repeater runat="server" DataSourceID="idsNews">
        <HeaderTemplate><div class="list"></HeaderTemplate>
        <ItemTemplate>
            <div class="item i<%# Container.ItemIndex %> a<%# Container.ItemIndex % 2 %>">
                <span class="date"><%# Eval("Published") %></span>
                <a href='<%# Eval("Url") %>'><%# Eval("Title") %></a>
                <p><%# Eval("Introduction") %></p>
            </div>
        </ItemTemplate>
        <FooterTemplate></div></FooterTemplate>
    </asp:Repeater>
</asp:Content>
