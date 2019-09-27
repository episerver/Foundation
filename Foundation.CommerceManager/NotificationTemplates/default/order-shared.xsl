<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:ms="urn:schemas-microsoft-com:xslt">
	<xsl:output method="html" />

	<xsl:template name="PaymentPlanSchedule">
		<div class="schedule">
			Payment Schedule: starting&#160;<xsl:value-of select="ms:format-date(StartDate, 'MMM dd, yyyy')"/>&#160;every&#160;<xsl:value-of select="CycleLength"/>&#160;<xsl:call-template name="PlanCycle" />&#160;for&#160;<xsl:value-of select="MaxCyclesCount"/>&#160;<xsl:call-template name="PlanCycle" />&#160;till&#160;<xsl:value-of select="ms:format-date(EndDate, 'MMM dd, yyyy')"/>
		</div>
	</xsl:template>

	<xsl:template name="PlanCycle">
		<xsl:value-of select="CycleMode"/>(s)
	</xsl:template>
	

	<xsl:template name="OrderHeader">
		Order Number: <xsl:value-of select="TrackingNumber"/><br/>
		Status: <xsl:value-of select="Status"/><br/>
		Name: <xsl:value-of select="CustomerName"/><br/>
		Email: <a>
			<xsl:attribute name="href">
				mailto:<xsl:value-of select="//OrderAddresses/OrderAddress[Name=//OrderForms/OrderForm/BillingAddressId]/Email"/>
			</xsl:attribute>
			<xsl:value-of select="//OrderAddresses/OrderAddress[Name=//OrderForms/OrderForm/BillingAddressId]/Email"/>
		</a>
	</xsl:template>

	<xsl:template name="OrderFooter">
		<h3>Order Summary</h3>
		<div class="OrderSummary">
			Sub Total: <xsl:value-of select="BillingCurrency"/>&#160;<xsl:value-of select="format-number(SubTotal, '###,###.00')"/><br/>
			Handling Total: <xsl:value-of select="BillingCurrency"/>&#160;<xsl:value-of select="format-number(HandlingTotal, '###,###.00')"/><br/>
			Shipping Total: <xsl:value-of select="BillingCurrency"/>&#160;<xsl:value-of select="format-number(ShippingTotal, '###,###.00')"/><br/>
			Total Tax: <xsl:value-of select="BillingCurrency"/>&#160;<xsl:value-of select="format-number(TaxTotal, '###,###.00')"/><br/>
			TOTAL: <xsl:value-of select="BillingCurrency"/>&#160;<xsl:value-of select="format-number(Total, '###,###.00')"/><br/>
		</div>
	</xsl:template>

	<xsl:template match="OrderForm">
		<div class="OrderForm">
			<div class="OrderForms">
				<h3>Line Items</h3>
				<xsl:apply-templates select="LineItems/LineItem"></xsl:apply-templates>
				<h3>Payments</h3>
				<xsl:apply-templates select="Payments/Payment"></xsl:apply-templates>
			</div>
			<div class="OrderSummary">
				<!--
				Sub Total: <xsl:value-of select="SubTotal"/><br/>
				Handling Total: <xsl:value-of select="HandlingTotal"/><br/>
				Shipping Total: <xsl:value-of select="ShippingTotal"/><br/>
				Total Tax: <xsl:value-of select="TaxTotal"/><br/>
				Discount: <xsl:value-of select="DiscoutnAmount"/><br/>
				TOTAL: <xsl:value-of select="Total"/><br/>
				-->
			</div>
		</div>
	</xsl:template>

	<xsl:template match="LineItem">
		<div class="LineItem">
			<xsl:value-of select="format-number(Quantity, '###,###.##')"/>&#160;<xsl:value-of select="DisplayName"/> - <xsl:value-of select="//BillingCurrency"/>&#160;<xsl:value-of select="format-number(ListPrice, '###,###.00')"/> each
		</div>
	</xsl:template>

	<xsl:template match="Payment">
		<div class="Payment">
			Payment Method: <xsl:value-of select="PaymentMethodName"/><br/>
			Amount: <xsl:value-of select="//BillingCurrency"/>&#160;<xsl:value-of select="format-number(Amount, '###,###.00')"/>
		</div>
	</xsl:template>


</xsl:stylesheet>


