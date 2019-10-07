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
					.footer {padding: 5px 0 0 0}
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
			<h1>Sale/Order Notification from the Store</h1>

			<h1>**ORDER SUMMARY</h1>
			<xsl:call-template name="OrderHeader"></xsl:call-template>
			<div class="OrderForms">
				<h2>Products Purchased:</h2>
				<xsl:apply-templates select="OrderForms/OrderForm"></xsl:apply-templates>
			</div>
			
			<xsl:call-template name="OrderFooter"></xsl:call-template>

			<div class="Footer">
				Regards,<br/> your Company.
			</div>
		</div>
	</xsl:template>
</xsl:stylesheet>