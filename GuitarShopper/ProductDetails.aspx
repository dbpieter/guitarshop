<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ProductDetails.aspx.cs" Inherits="ProductDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title>Detail | GuitarShopper</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="product-details">
        <!--product-details-->
        <div class="col-sm-5">
            <div class="view-product">
                <img id="productImg" src="<%= Product.ImageURL %>" alt="" />
                <%--<h3>ZOOM</h3>--%>
            </div>
            <div id="similar-product" class="carousel slide" data-ride="carousel">

                <!-- Wrapper for slides -->
                <div class="carousel-inner">
                    <div class="item active">
                        <a class="imageLnk" href="#">
                            <img src="<%= Product.ImageURL %>" alt=""></a>
                    </div>
                    <asp:Repeater runat="server" ID="ImagesRepeater" Visible="true" OnItemDataBound="ImagesRepeater_ItemDataBound">
                        <ItemTemplate>
                            <div class="item" id="tzeDiv" runat="server">
                                <a class="imageLnk" href="#">
                                    <img src="<%# DataBinder.Eval(Container.DataItem, "Value") %>" alt=""></a>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>

                <!-- Controls -->
                <a class="left item-control" href="#similar-product" data-slide="prev">
                    <i class="fa fa-angle-left"></i>
                </a>
                <a class="right item-control" href="#similar-product" data-slide="next">
                    <i class="fa fa-angle-right"></i>
                </a>
            </div>

        </div>
        <div class="col-sm-7">
            <div class="product-information">
                <!--/product-information-->
                <img src="images/product-details/new.jpg" class="newarrival" alt="" />
                <h2><%= Product.Name %></h2>
                <%--<p>Web ID: 1089772</p>--%>
                <img src="images/product-details/rating.png" alt="" />
                <span>
                    <span>€ <%= Product.Price %></span>
                    <label>Aantal:</label>
                    <input type="text" value="1" disabled="disabled" />
<%--                    <button type="button" class="btn btn-fefault cart myaddcart">
                        <i class="fa fa-shopping-cart"></i> 
                        Add to cart
                    </button>--%>
                    <asp:LinkButton CssClass="btn btn-default cart mylinkbtn" CommandName="OrderLocal"  runat="server" ID="AddToCartButton" OnCommand="linkBtn_Command"><i class="fa fa-shopping-cart"></i> In winkelkar</asp:LinkButton>

                </span>
                <p><b>Beschikbaarheid:</b> <%= Product.Availablity %></p>
                <p><b>Merk:</b> <%= Product.BrandName %></p>
                <p><b>Kleur:</b> <%= Product.Color %></p>
                <a href="#">
                    <img src="images/product-details/share.png" class="share img-responsive" alt="" /></a>
                <p class="description"><%= Product.Description %></p>

            </div>
            <!--/product-information-->
        </div>
    </div>
    <div class="recommended_items">
        <!--recommended_items-->
        <h2 class="title text-center">Vergelijkbare items</h2>

        <div id="recommended-item-carousel" class="carousel slide" data-ride="carousel">
            <div class="carousel-inner">
                <div class="item active">
                    <asp:Repeater runat="server" ID="SimilarRepeater1">
                        <ItemTemplate>
                            <div class="col-sm-4">
                                <div class="product-image-wrapper">
                                    <div class="single-products">
                                        <div class="productinfo text-center alternativeproduct">
                                            <a href="ProductDetails.aspx?id=<%# DataBinder.Eval(Container.DataItem, "id") %>"><img src="<%# DataBinder.Eval(Container.DataItem, "ImageUrl").ToString().ToLower() %>" alt="" /></a>
                                            <h2>&euro; <%# DataBinder.Eval(Container.DataItem, "Price").ToString().ToLower() %></h2>
                                            <p><%# DataBinder.Eval(Container.DataItem, "BrandName").ToString().ToLower() %> <%# DataBinder.Eval(Container.DataItem, "Name").ToString().ToLower() %></p>
                                <asp:LinkButton ID="linkBtn" runat="server" OnCommand="linkBtn_Command" CommandName="OrderLocal" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"Id") %>' CssClass="btn btn-default add-to-cart"><i class="fa fa-shopping-cart"></i>In winkelkar</asp:LinkButton>
                                        </div>

                                    </div>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
                <div class="item" runat="server" id="similar2">
                    <asp:Repeater runat="server" ID="SimilarRepeater2">
                        <ItemTemplate>
                            <div class="col-sm-4">
                                <div class="product-image-wrapper">
                                    <div class="single-products">
                                        <div class="productinfo text-center alternativeproduct">
                                            <a href="ProductDetails.aspx?id=<%# DataBinder.Eval(Container.DataItem, "id") %>"><img src="<%# DataBinder.Eval(Container.DataItem, "ImageUrl").ToString().ToLower() %>" alt="" /></a>
                                            <h2>&euro; <%# DataBinder.Eval(Container.DataItem, "Price").ToString().ToLower() %></h2>
                                            <p><%# DataBinder.Eval(Container.DataItem, "BrandName").ToString().ToLower() %> <%# DataBinder.Eval(Container.DataItem, "Name").ToString().ToLower() %></p>
                                <asp:LinkButton ID="linkBtn" runat="server" OnCommand="linkBtn_Command" CommandName="OrderLocal" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"Id") %>' CssClass="btn btn-default add-to-cart"><i class="fa fa-shopping-cart"></i>In winkelkar</asp:LinkButton>
                                        </div>

                                    </div>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </div>
            <a class="left recommended-item-control" href="#recommended-item-carousel" data-slide="prev">
                <i class="fa fa-angle-left"></i>
            </a>
            <a class="right recommended-item-control" href="#recommended-item-carousel" data-slide="next">
                <i class="fa fa-angle-right"></i>
            </a>
        </div>


    </div>

        <div class="recommended_items" runat="server" id="RemoteProductsDiv">
        <!--recommended_items-->
        <h2 class="title text-center">Vergelijkbare items van andere shops</h2>

        <div id="remote-item-carousel" class="carousel slide" data-ride="carousel">
            <div class="carousel-inner">
                <div class="item active" runat="server" id="RemoteProducts1">
                    <asp:Repeater runat="server" ID="RemoteRepeater1">
                        <ItemTemplate>
                            <div class="col-sm-4">
                                <div class="product-image-wrapper">
                                    <div class="single-products">
                                        <div class="productinfo text-center alternativeproduct remoteproduct">
                                            <a href="<%# "http://webshop.michieldedeyne.ikdoeict.net/Product.aspx?productID=" + DataBinder.Eval(Container.DataItem,"id") %>"><img src="<%# DataBinder.Eval(Container.DataItem, "picture").ToString().ToLower() %>" alt="" /></a>
                                            <h2>&euro; <%# DataBinder.Eval(Container.DataItem, "price").ToString().ToLower() %></h2>
                                            <p><%# DataBinder.Eval(Container.DataItem, "name").ToString().ToLower() %></p>
                                            <asp:LinkButton ID="linkBtn" runat="server" OnCommand="linkBtn_Command" CommandName="OrderMichiel" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"id") %>' CssClass="btn btn-default add-to-cart"><i class="fa fa-shopping-cart"></i>In winkelkar</asp:LinkButton>
                                        </div>

                                    </div>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
                <div class="item" runat="server" id="RemoteProducts2">
                    <asp:Repeater runat="server" ID="RemoteRepeater2">
                        <ItemTemplate>
                            <div class="col-sm-4">
                                <div class="product-image-wrapper">
                                    <div class="single-products">
                                        <div class="productinfo text-center alternativeproduct remoteproduct">
                                            <a href="<%# "http://gitaarshop.gillesdeblock.ikdoeict.net/GuitarDetail.aspx?id=" + DataBinder.Eval(Container.DataItem,"id") %>"><img src="<%# DataBinder.Eval(Container.DataItem, "picture").ToString().ToLower() %>" alt="" /></a>
                                            <h2>&euro; <%# DataBinder.Eval(Container.DataItem, "price").ToString().ToLower() %></h2>
                                            <p><%# DataBinder.Eval(Container.DataItem, "name").ToString().ToLower() %></p>
                                            <asp:LinkButton ID="linkBtn" runat="server" OnCommand="linkBtn_Command" CommandName="OrderGilles" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"id") %>' CssClass="btn btn-default add-to-cart"><i class="fa fa-shopping-cart"></i>In winkelkar</asp:LinkButton>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </div>
            <a class="left recommended-item-control" href="#remote-item-carousel" data-slide="prev">
                <i class="fa fa-angle-left"></i>
            </a>
            <a class="right recommended-item-control" href="#remote-item-carousel" data-slide="next">
                <i class="fa fa-angle-right"></i>
            </a>
        </div>


    </div>

    
    <!--/product-details-->
</asp:Content>

