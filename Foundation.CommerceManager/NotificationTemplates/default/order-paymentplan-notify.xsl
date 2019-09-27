<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="html" />
	<xsl:include href="order-shared.xsl"/>

	<xsl:template match="/">
		<html>
			<head id="Head1">
				<style type="text/css">
					#PaymentPlan {}
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
				<xsl:apply-templates select="//PaymentPlan"></xsl:apply-templates>
			</body>
		</html>
	</xsl:template>

	<xsl:template match="PaymentPlan">
		<div id="PaymentPlan">
			<h1>Payment Plan Notification from the Store</h1>

			<h1>**PAYMET PLAN SUMMARY</h1>
			<xsl:call-template name="OrderHeader"></xsl:call-template>
			<xsl:call-template name="PaymentPlanSchedule" />
			<div class="OrderForms">
				<h2>Products Purchased:</h2>
				<xsl:apply-templates select="OrderForms/OrderForm"></xsl:apply-templates>
			</div>
			
			<xsl:call-template name="OrderFooter"></xsl:call-template>

			<div class="Footer">
				Regards,<br/> Your Company.
			</div>
		</div>
	</xsl:template>
</xsl:stylesheet>