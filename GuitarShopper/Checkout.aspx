<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPageReduced.master" AutoEventWireup="true" CodeFile="Checkout.aspx.cs" Inherits="Checkout" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section id="cart_items">
        <div class="container" runat="server" id="checkoutFinished" visible="false">
            <div class="step-one" style="margin-top: -40px;">
                <div class="shopper-informations">
                    <div class="row">
                        <div class="col-sm-12 clearfix"  style="margin-bottom:300px;margin-top:50px;">
                            <h3><strong>Bedankt !</strong> Uw bestelling werd doorgegeven !</h3>
                            <a href="index.aspx">Terug naar homepagina</a>
                        </div>
                    </div>
                </div>
                
            </div>
        </div>

        <div class="container" runat="server" id="checkoutUnfinished" visible="true">
            <div class="breadcrumbs">
                <ol class="breadcrumb">
                    <li><a href="#">Home</a></li>
                    <li class="active">Afrekenen</li>
                </ol>
            </div>
            <!--/breadcrums-->

            <div class="step-one" style="margin-top: -40px;">
                <h2 class="heading">Afrekenen</h2>
                
            </div>

            <div class="shopper-informations">
                <div class="row">
                    <div class="col-sm-5 clearfix">
                        <div class="bill-to">
                            <p>Gegevens</p>
                            <div class="form-one">
                                <asp:TextBox type="text" AutoPostBack="false" ID="EmailField" runat="server" CssClass="" placeholder="Email *"></asp:TextBox>
                                <asp:RequiredFieldValidator ValidationGroup="checkout" CssClass="has-error" ControlToValidate="EmailField" Display="Dynamic" Text="Email verplicht !" runat="server" ID="emailValidator"></asp:RequiredFieldValidator>

                                <asp:TextBox type="text" AutoPostBack="false" ID="FirstNameField" runat="server" CssClass="" placeholder="Voornaam *"></asp:TextBox>
                                <asp:TextBox type="text" AutoPostBack="false" ID="LastNameField" runat="server" CssClass="" placeholder="Familienaam *"></asp:TextBox>
                                <asp:TextBox type="text" AutoPostBack="false" ID="AddressField1" runat="server" CssClass="" placeholder="Address 1 *"></asp:TextBox>
                                <asp:TextBox type="text" AutoPostBack="false" ID="AddressField2" runat="server" CssClass="" placeholder="Address 2 *"></asp:TextBox>
                                <span style="color: red;" runat="server" id="errorMsg" visible="false">Vul aub de gegevens gemarkeerd met * in</span>
                            </div>
                            <div class="form-two">
                                <asp:TextBox type="text" AutoPostBack="false" ID="PostalCodeField" runat="server" CssClass="" placeholder="Postcode *"></asp:TextBox>
                                <select>
                                    <option>-- Land --</option>
                                    <option>België</option>
                                    <option>Nederland</option>
                                </select>
                                <input type="text" placeholder="Telefoonnummer">
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-4">
                        <div class="order-message">
                            <p>Opmerkingen</p>
                            <textarea name="message" placeholder="Opmerkingen over de levering en/of de bestelling" rows="16"></textarea>
                        </div>
                    </div>
                </div>
            </div>
            <div class="review-payment">
                <span style="color: red;" runat="server" id="cartError" visible="false">Er zitten geen producten in uw winkelkarretje</span>
                <h2>Overzicht en betaling</h2>
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
                                        <a href="ProductDetails.aspx?id=<%# DataBinder.Eval(Container.DataItem, "id") %>">
                                            <img src="<%# DataBinder.Eval(Container.DataItem, "ImageUrl") %>" alt=""></a>
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
                                            <a class="cart_quantity_up" href="">+ </a>
                                            <input class="cart_quantity_input" type="text" name="quantity" value="1" autocomplete="off" size="2">
                                            <a class="cart_quantity_down" href="">- </a>
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
            <div class="payment-options">
                <span>
                    <label>
                        <input type="checkbox">
                        Overschrijving</label>
                </span>
                <span>
                    <label>
                        <input type="checkbox">
                        Kredietkaart (Visa & MasterCard)</label>
                </span>
                <span>
                    <label>
                        <input type="checkbox">
                        Paypal</label>
                </span>
                <%--<span><a class="btn btn-primary" id="checkoutBtn" href="">Afrekenen</a></span>--%>
                <span>
                    <asp:LinkButton CssClass="btn btn-primary" runat="server" ID="CheckoutButton" OnClick="CheckoutButton_Click">Afrekenen</asp:LinkButton>
                </span>
            </div>
        </div>
    </section>
    <!--/#cart_items-->
</asp:Content>

