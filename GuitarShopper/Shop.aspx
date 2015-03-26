<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Shop.aspx.cs" Inherits="Shop" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="features_items">
        <!--features_items-->
        <h2 class="title text-center">Producten</h2>
        <asp:Repeater runat="server" ID="ProductsRepeater">
            <ItemTemplate>
                <div class="col-sm-4">
                    <div class="product-image-wrapper">
                        <div class="single-products">
                            <div class="productinfo text-center">
                                <a href="ProductDetails.aspx?id=<%# DataBinder.Eval(Container.DataItem, "id") %>">
                                    <img src="<%# DataBinder.Eval(Container.DataItem, "ImageURL") %>" alt="product image" /></a>
                                <h2>&euro; <%# DataBinder.Eval(Container.DataItem, "Price") %></h2>
                                <p><%# DataBinder.Eval(Container.DataItem,"BrandName") %> <%# DataBinder.Eval(Container.DataItem, "Name") %></p>
                                <%--<a id="<%# DataBinder.Eval(Container.DataItem,"Id") %>" href="#" class="btn btn-default add-to-cart"><i class="fa fa-shopping-cart"></i>In winkelkar</a>--%>

                                <asp:LinkButton ID="linkBtn" runat="server" OnCommand="linkBtn_Command" CommandName="OrderLocal" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"Id") %>' CssClass="btn btn-default add-to-cart"><i class="fa fa-shopping-cart"></i>In winkelkar</asp:LinkButton>
                            </div>
                        </div>
                        <div class="choose">
                            <%--<ul class="nav nav-pills nav-justified">
                                <li><a href="#"><i class="fa fa-plus-square"></i>Add to wishlist</a></li>
                            </ul>--%>
                        </div>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>


        <ul class="pagination" runat="server" id="pagination">
            <li runat="server" id="GoBack" data-id="GoBack"><a href="">&laquo;</a></li>
            <asp:Repeater runat="server" ID="PaginationRepeater">
                <ItemTemplate>
                    <li class="<%# DataBinder.Eval(Container.DataItem, "CssClass") %>"><a href=""><%# DataBinder.Eval(Container.DataItem, "PageNr") %></a></li>
                </ItemTemplate>
            </asp:Repeater>
            <li runat="server" id="GoForward" data-id="GoForward"><a href="">&raquo;</a></li>
        </ul>
    </div>


</asp:Content>

