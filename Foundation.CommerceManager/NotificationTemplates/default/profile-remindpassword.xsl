<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="html" />

	<xsl:template match="/">
		<html>
			<head id="Head1">
				<style type="text/css">
          #PasswordReset {}
          h1 {font-size: 20px;}
          h2 {font-size: 18px;}
          h3 {font-size: 16px; padding: 2px 2px 2px 2px}
          .footer {padding: 10px 0 0 0}
        </style>
				<title>
          Password Reset Notification from the Store
        </title>
			</head>
			<body>
        <xsl:call-template name="PasswordResetTemplate"></xsl:call-template>
			</body>
		</html>
	</xsl:template>

	<xsl:template name="PasswordResetTemplate">
    <div id="PasswordReset">
      <h1>Password Reset Notification from the Store</h1>
      <h3>Your password has been reset.</h3>
      <b>Your credentials:</b><br/>
      UserName: <xsl:value-of select="//User"/><br/>
      Password: <xsl:value-of select="//Password"/><br/>
      <div class="Footer">
        Regards,<br/> your Company.
      </div>
    </div>
	</xsl:template>
</xsl:stylesheet>