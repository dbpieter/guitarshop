<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPageReduced.master" AutoEventWireup="true" CodeFile="Cart.aspx.cs" Inherits="Cart" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <title> Winkelkarretje | GuitarShopper</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
	<section id="cart_items">
		<div class="container">
			<div class="breadcrumbs">
				<ol class="breadcrumb">
				  <li><a href="#">Home</a></li>
				  <li class="active">Winkelkarretje</li>
				</ol>
			</div>
			<div class="table-responsive cart_info">
				<table class="table table-condensed">
					<thead>
						<tr class="cart_menu">
							<td class="image">Product</td>
							<td class="description"></td>
							<td class="price">Prijs</td>
							<td class="quantity">Aantal</td>
							<td class="total">Totaal</td>
							<td></td>
						</tr>
					</thead>
					<tbody>
                        <asp:Repeater runat="server" ID="cartRowRepeater">
                            <ItemTemplate>
                                <tr>
							        <td class="cart_product">
								        <a href="ProductDetails.aspx?id=<%# DataBinder.Eval(Container.DataItem, "id") %>"><img src="<%# DataBinder.Eval(Container.DataItem, "ImageUrl") %>" alt=""></a>
							        </td>
							        <td class="cart_description">
								        <h4><a href="ProductDetails.aspx?id=<%# DataBinder.Eval(Container.DataItem, "id") %>"><%# DataBinder.Eval(Container.DataItem, "Name") %></a></h4>
								        <p><%# DataBinder.Eval(Container.DataItem, "BrandName") %></p>
							        </td>
							        <td class="cart_price">
								        <p>&euro; <%# DataBinder.Eval(Container.DataItem, "Price") %></p>
							        </td>
							        <td class="cart_quantity">
								        <div class="cart_quantity_button">
									        <a class="cart_quantity_up" href=""> + </a>
									        <input class="cart_quantity_input" type="text" name="quantity" value="1" autocomplete="off" size="2">
									        <a class="cart_quantity_down" href=""> - </a>
								        </div>
							        </td>
							        <td class="cart_total">
								        <p class="cart_total_price">&euro; <%# DataBinder.Eval(Container.DataItem, "Price") %></p>
							        </td>
							        <td class="cart_delete">

                                        <asp:LinkButton OnCommand="DeleteFromCartBtn_Command" CssClass="cart_quantity_delete" runat="server" ID="DeleteFromCartBtn" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' CommandName="DeleteFromCart">
                                            <i class="fa fa-times"></i>
                                        </asp:LinkButton>
							        </td>
						        </tr>
                            </ItemTemplate>
                        </asp:Repeater>
					</tbody>
				</table>
			</div>
		</div>
	</section> <!--/#cart_items-->

    <section id="do_action">
		<div class="container">
			<div class="heading">
				<h3>Wat zou u willen doen ?</h3>
                <p>Kies of u al dan niet een kortingscode of een cadeaubon heeft. U kan ook de verzendkosten laten berekenen</p>
			</div>
			<div class="row">
				<div class="col-sm-6">
					<div class="chose_area">
						<ul class="user_option">
							<li>
								<input type="checkbox">
								<label>Gebruik kortingscode</label>
							</li>
							<li>
								<input type="checkbox">
								<label>Aankoopbon</label>
							</li>
							<li>
								<input type="checkbox">
								<label>Bereken verzendkosten</label>
							</li>
						</ul>
						<ul class="user_info">
							<li class="single_field">
								<label>Land:</label>
								<select>
									<option>Belgie</option>
                                    <option>Nederland</option>
                                    <option>Frankrijk</option>
								</select>
								
							</li>
							<li class="single_field">
								<label>Regio</label>
								<select>
									<option>Oost-Vlaanderen</option>
                                    <option>West-Vlaanderen</option>
								</select>
							
							</li>
							<li class="single_field zip-field">
								<label>Postcode:</label>
								<input type="text">
							</li>
						</ul>
						<a class="btn btn-default update" href="">Bereken</a>
						<a class="btn btn-default check_out" href="checkout.aspx">Afrekenen</a>
					</div>
				</div>
				<div class="col-sm-6">
					<div class="total_area">
						<ul>
							<li>Subtotaal<span>&euro; <%= TotalPrice %></span></li>
							<li>Taksen<span>&euro; 0</span></li>
							<li>Verzendkosten<span>&euro; 0</span></li>
							<li>Totaal <span>&euro; <%= TotalPrice %></span></li>
						</ul>
							<a class="btn btn-default update" href="#">Update</a>
							<a class="btn btn-default check_out" href="checkout.aspx">Afrekenen</a>
					</div>
				</div>
			</div>
		</div>
	</section><!--/#do_action-->
</asp:Content>

