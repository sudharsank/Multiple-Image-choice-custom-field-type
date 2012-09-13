<%@ Control Language="C#" Debug="true" %>
<%@ Assembly Name="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c"
    Namespace="Microsoft.SharePoint.WebControls" %>

<style type="text/css">
    .TableEdit
    {
        border:1px solid #CCC;
        padding:0px;        
    }
</style>
<SharePoint:RenderingTemplate ID="MultipleImageUploadField" runat="server">
    <Template>
        <input type="hidden" id="hidCheckedImage" runat="server" />
        <asp:Table ID="tblImage" runat="server" Width="100%" CssClass="TableEdit">
        
        </asp:Table>
    </Template>
</SharePoint:RenderingTemplate>

<SharePoint:RenderingTemplate ID="MultipleImageUploadFieldDisplay" runat="server">
	<Template>        
		<asp:Table ID="tblImage" runat="server" Width="100%" CssClass="TableEdit">
        
        </asp:Table>
    </Template>
</SharePoint:RenderingTemplate>
