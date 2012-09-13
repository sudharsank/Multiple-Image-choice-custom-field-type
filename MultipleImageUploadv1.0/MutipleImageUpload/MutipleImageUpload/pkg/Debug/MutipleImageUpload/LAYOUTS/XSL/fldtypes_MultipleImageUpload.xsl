<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet xmlns:x="http://www.w3.org/2001/XMLSchema"
        xmlns:d="http://schemas.microsoft.com/sharepoint/dsp"
        version="1.0"
        exclude-result-prefixes="xsl msxsl ddwrt"
        xmlns:ddwrt="http://schemas.microsoft.com/WebParts/v2/DataView/runtime"
        xmlns:asp="http://schemas.microsoft.com/ASPNET/20"
        xmlns:__designer="http://schemas.microsoft.com/WebParts/v2/DataView/designer"
        xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
        xmlns:msxsl="urn:schemas-microsoft-com:xslt"
        xmlns:SharePoint="Microsoft.SharePoint.WebControls"
        xmlns:ddwrt2="urn:frontpage:internal">

  <xsl:template match="FieldRef[@FieldType='MultipleImageUpload']" mode="header" ddwrt:dvt_mode="header">
    <th class="ms-vh2" nowrap="nowrap" scope="col" onmouseover="OnChildColumn(this)">
      <xsl:call-template name="dvt_headerfield">
        <xsl:with-param name="fieldname">
          <xsl:value-of select="@Name" />
        </xsl:with-param>
        <xsl:with-param name="fieldtitle">
          <xsl:value-of select="@DisplayName" />
        </xsl:with-param>
        <xsl:with-param name="displayname">
          <xsl:value-of select="@DisplayName" />
        </xsl:with-param>
        <xsl:with-param name="fieldtype">
          <xsl:value-of select="@FieldType" />
        </xsl:with-param>
      </xsl:call-template>
    </th>
  </xsl:template>
  <xsl:template name="split">
    <xsl:param name="picurls"/>
    <xsl:variable name="first_picurl" select="substring-before($picurls,',')"/>
    <xsl:variable name="rest_picurl" select="substring-after($picurls,',')"/>
    <xsl:if test="$first_picurl">
      <span style="display:inline;padding-left:2px;">
        <img onfocus="OnLink(this)" src="{$first_picurl}" width="50px" height="50px" />
      </span>
    </xsl:if>
    <xsl:if test='$rest_picurl'>
      <xsl:call-template name='split'>
        <xsl:with-param name='picurls' select='$rest_picurl'/>
      </xsl:call-template>
    </xsl:if>
  </xsl:template>
  <xsl:template match="FieldRef[@FieldType='MultipleImageUpload']" mode="body">
    <xsl:param name="thisNode" select="." />
    <xsl:param name="fieldValue" select="$thisNode/@*[name()=current()/@Name]"/>
    <xsl:choose>
      <xsl:when test="$fieldValue=''">
      </xsl:when>
      <xsl:otherwise>
        <xsl:call-template name="split">
          <xsl:with-param name="picurls" select="$thisNode/@*[name()=current()/@Name]"/>
        </xsl:call-template>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
</xsl:stylesheet>
