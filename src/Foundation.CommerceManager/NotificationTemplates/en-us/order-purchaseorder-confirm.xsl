<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="html" />
	<xsl:include href="order-shared.xsl"/>

	<xsl:template match="/">
		<html>
			<head id="Head1">
				<style type="text/css">
					#PurchaseOrder {}
					h1 {font-size: 20px;}
					h2 {font-size: 18px;}
					h3 {font-size: 16px; background-color: #cccccc; padding: 2px 2px 2px 2px}
					.introduction {padding: 5px 0 0 0}
				</style>
				<title>
					Order Notification
				</title>
			</head>
			<body>
				<xsl:apply-templates select="//PurchaseOrder"></xsl:apply-templates>
			</body>
		</html>
	</xsl:template>

	<xsl:template match="PurchaseOrder">
		<div id="PurchaseOrder">
			<xsl:value-of select="CustomerName"/>,<br /><br />

			Your order with YOUR COMPANY has been completed.<br /><br />
			We thank you for your business.<br /><br />

			<h1>**Thanks for ordering</h1>
			<div class="introduction">
				We thank you for your business
			</div>

			<h1>**YOUR ORDER SUMMARY</h1>
			<xsl:call-template name="OrderHeader"></xsl:call-template>

			<div class="OrderForms">
				<h2>Products Purchased:</h2>
				<xsl:apply-templates select="OrderForms/OrderForm"></xsl:apply-templates>
			</div>

			<xsl:call-template name="OrderFooter"></xsl:call-template>

			<h1>**SUPPORT AND ASSISTANCE</h1>

			<div class="Footer">
				If you need further assistance or have any other questions, feel free to contact us as follows:<br /><br />
				<strong>Support e-mail:</strong> <a href="mailto:support@yourcompany.com">support@yourcompany.com</a> <br /><br />
				For more information on Mediachase and our products and services, please visit
				<a href="http://www.yourcompany.com">http://www.yourcompany.com</a>. <br /><br />
				Regards,<br/> Your Company.
			</div>
		</div>
	</xsl:template>
</xsl:stylesheet>


