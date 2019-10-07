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
			<xsl:value-of select="CustomerName"/>,<br /><br />

			Subscription order has been created.<br /><br />
			We thank you for your business.<br /><br />

			<h1>**SUBSCRIPTION DETAILS</h1>
			<div class="introduction">
				Subscription plan has been created in the system. You will receive another email when the purchase order is created.
			</div>

			<h1>**PLAN SUMMARY</h1>
			<xsl:call-template name="OrderHeader" />
			<xsl:call-template name="PaymentPlanSchedule" />

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


