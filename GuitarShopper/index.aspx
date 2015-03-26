<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="index.aspx.cs" Inherits="index" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title>Home | GuitarShopper</title>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="sliderplaceHolder" runat="server">
            
        	<section id="slider"><!--slider-->
		<div class="container">
			<div class="row">
				<div class="col-sm-12">
					<div id="slider-carousel" class="carousel slide" data-ride="carousel">
						<ol class="carousel-indicators">
<%--							<li data-target="#slider-carousel" data-slide-to="0" class="active"></li>
							<li data-target="#slider-carousel" data-slide-to="1"></li>
							<li data-target="#slider-carousel" data-slide-to="2"></li>--%>
						</ol>
						
						<div class="carousel-inner">
							<div class="item active">
								<div class="col-sm-6">
									<h1><span style="color: orange">G</span>uitar-<span style="color: orange">S</span>hopper</h1>
									<h2>Simpelweg de beste !</h2>
									<p>Of toch bijna</p>
									<a href="Shop.aspx" style="background-color: orange" class="btn btn-default get">Shop !</a>
								</div>
								<div class="col-sm-6">
									<img src="images/home/guitar1.jpg" class="girl img-responsive" alt="" />
									<%--<img src="images/home/pricing.png"  class="pricing" alt="" />--%>
								</div>
							</div>
							
						</div>
						
					</div>
					
				</div>
			</div>
		</div>
	</section><!--/slider-->
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="features_items">
        <!--features_items-->
        <h2 class="title text-center">In de kijker</h2>
        <asp:Repeater runat="server" ID="FeaturedItems">
            <ItemTemplate>
                <div class="col-sm-4">
                    <div class="product-image-wrapper">
                        <div class="single-products">
                            <div class="productinfo text-center">
                                <a href="ProductDetails.aspx?id=<%# DataBinder.Eval(Container.DataItem, "id") %>">
                                    <img src="<%# DataBinder.Eval(Container.DataItem, "ImageURL") %>" alt="product image" /></a>
                                <h2>&euro; <%# DataBinder.Eval(Container.DataItem, "Price") %></h2>
                                <p><%# DataBinder.Eval(Container.DataItem, "BrandName") %> <%# DataBinder.Eval(Container.DataItem, "Name") %></p>
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
    </div>
    <!--features_items-->

    <div class="category-tab">
        <!--category-tab-->
        <div class="col-sm-12">
            <ul class="nav nav-tabs">
                <li class="active"><a href="#electric" data-toggle="tab">Elektrische gitaren</a></li>
                <li><a href="#acoustic" data-toggle="tab">Akoestische gitaren</a></li>
                <li><a href="#bass" data-toggle="tab">Basgitaren</a></li>
                <li><a href="#accessories" data-toggle="tab">Accessoires</a></li>
            </ul>
        </div>
        <div class="tab-content">
            <div class="tab-pane fade active in" id="electric">
                <asp:Repeater runat="server" ID="ElekGtrRepeater">
                    <ItemTemplate>
                        <div class="col-sm-3">
                            <div class="product-image-wrapper">
                                <div class="single-products">
                                    <div class="productinfo text-center tabproduct">
                                        <a href="ProductDetails.aspx?id=<%# DataBinder.Eval(Container.DataItem, "id") %>">
                                            <img src="<%# DataBinder.Eval(Container.DataItem, "ImageURL") %>" alt="product image" /></a>
                                        <h2>&euro; <%# DataBinder.Eval(Container.DataItem, "Price") %></h2>
                                        <p><%# DataBinder.Eval(Container.DataItem, "BrandName") %> <%# DataBinder.Eval(Container.DataItem, "Name") %></p>
                                        <asp:LinkButton ID="linkBtn" runat="server" OnCommand="linkBtn_Command" CommandName="OrderLocal" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"Id") %>' CssClass="btn btn-default add-to-cart"><i class="fa fa-shopping-cart"></i>In winkelkar</asp:LinkButton>

                                    </div>

                                </div>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>

            <div class="tab-pane fade" id="acoustic">
                <asp:Repeater runat="server" ID="AcousticRepeater">
                    <ItemTemplate>
                        <div class="col-sm-3">
                            <div class="product-image-wrapper">
                                <div class="single-products">
                                    <div class="productinfo text-center tabproduct">
                                        <a href="ProductDetails.aspx?id=<%# DataBinder.Eval(Container.DataItem, "id") %>">
                                            <img src="<%# DataBinder.Eval(Container.DataItem, "ImageURL") %>" alt="product image" /></a>
                                        <h2>&euro; <%# DataBinder.Eval(Container.DataItem, "Price") %></h2>
                                        <p><%# DataBinder.Eval(Container.DataItem, "BrandName") %> <%# DataBinder.Eval(Container.DataItem, "Name") %></p>
                                        <asp:LinkButton ID="linkBtn" runat="server" OnCommand="linkBtn_Command" CommandName="OrderLocal" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"Id") %>' CssClass="btn btn-default add-to-cart"><i class="fa fa-shopping-cart"></i>In winkelkar</asp:LinkButton>

                                    </div>

                                </div>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>

            <div class="tab-pane fade" id="bass">
                <asp:Repeater runat="server" ID="BassRepeater">
                    <ItemTemplate>
                        <div class="col-sm-3">
                            <div class="product-image-wrapper">
                                <div class="single-products">
                                    <div class="productinfo text-center tabproduct">
                                        <a href="ProductDetails.aspx?id=<%# DataBinder.Eval(Container.DataItem, "id") %>">
                                            <img src="<%# DataBinder.Eval(Container.DataItem, "ImageURL") %>" alt="product image" /></a>
                                        <h2>&euro; <%# DataBinder.Eval(Container.DataItem, "Price") %></h2>
                                        <p><%# DataBinder.Eval(Container.DataItem, "BrandName") %> <%# DataBinder.Eval(Container.DataItem, "Name") %></p>
                                        <asp:LinkButton ID="linkBtn" runat="server" OnCommand="linkBtn_Command" CommandName="OrderLocal" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"Id") %>' CssClass="btn btn-default add-to-cart"><i class="fa fa-shopping-cart"></i>In winkelkar</asp:LinkButton>

                                    </div>

                                </div>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>

            <div class="tab-pane fade" id="accessories">
                <asp:Repeater runat="server" ID="AccessoriesRepeater">
                    <ItemTemplate>
                        <div class="col-sm-3">
                            <div class="product-image-wrapper">
                                <div class="single-products">
                                    <div class="productinfo text-center tabproduct">
                                        <a href="ProductDetails.aspx?id=<%# DataBinder.Eval(Container.DataItem, "id") %>">
                                            <img src="<%# DataBinder.Eval(Container.DataItem, "ImageURL") %>" alt="product image" /></a>
                                        <h2>&euro; <%# DataBinder.Eval(Container.DataItem, "Price") %></h2>
                                        <p><%# DataBinder.Eval(Container.DataItem, "BrandName") %> <%# DataBinder.Eval(Container.DataItem, "Name") %></p>
                                        <asp:LinkButton ID="linkBtn" runat="server" OnCommand="linkBtn_Command" CommandName="OrderLocal" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"Id") %>' CssClass="btn btn-default add-to-cart"><i class="fa fa-shopping-cart"></i>In winkelkar</asp:LinkButton>

                                    </div>

                                </div>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>

        </div>
    </div>
    <!--/category-tab-->

    <div class="recommended_items">
        <!--recommended_items-->
        <h2 class="title text-center">Aanbevolen Items</h2>

        <div id="recommended-item-carousel" class="carousel slide" data-ride="carousel">
            <div class="carousel-inner">
                <div class="item active">
                    <asp:Repeater runat="server" ID="RecommendedRepeater1">
                        <ItemTemplate>
                            <div class="col-sm-4">
                                <div class="product-image-wrapper">
                                    <div class="single-products">
                                        <div class="productinfo text-center alternativeproduct">
                                            <a href="ProductDetails.aspx?id=<%# DataBinder.Eval(Container.DataItem, "id") %>">
                                                <img src="<%# DataBinder.Eval(Container.DataItem, "ImageUrl").ToString().ToLower() %>" alt="" /></a>
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
                <div class="item">
                    <asp:Repeater runat="server" ID="RecommendedRepeater2">
                        <ItemTemplate>
                            <div class="col-sm-4">
                                <div class="product-image-wrapper">
                                    <div class="single-products">
                                        <div class="productinfo text-center alternativeproduct">
                                            <a href="ProductDetails.aspx?id=<%# DataBinder.Eval(Container.DataItem, "id") %>">
                                                <img src="<%# DataBinder.Eval(Container.DataItem, "ImageUrl").ToString().ToLower() %>" alt="" /></a>
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
    <!--/recommended_items-->

</asp:Content>

