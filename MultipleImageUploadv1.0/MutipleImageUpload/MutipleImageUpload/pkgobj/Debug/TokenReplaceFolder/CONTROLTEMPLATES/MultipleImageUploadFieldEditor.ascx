<%@ Assembly Name="MultipleImageUpload, Version=1.0.0.0, Culture=neutral, PublicKeyToken=e2a6b6e64224d85e" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MultipleImageUploadFieldEditor.ascx.cs"
    Inherits="MultipleImageUpload.MultipleImageUploadFieldEditor" %>

<script src="/MultipleImageUploadScripts/jquery-1.4.2.min.js" type="text/javascript"></script>
<script src="/MultipleImageUploadScripts/jquery.SPServices-0.7.2ALPHA7.js" type="text/javascript"></script>
<script type="text/javascript">
    ExecuteOrDelayUntilScriptLoaded(Initialize, 'sp.js');
    var filecount = 0;

    function Initialize() {
        var hidfiles = document.getElementById('<%= hidFiles.ClientID %>');
        if (hidfiles.value != null && hidfiles.value != "") {
            var filecollhidval = hidfiles.value;
            var filecollctrl = document.getElementById('<%= tblFilestoupload.ClientID %>');
            var filecollarr = filecollhidval.split(",");
            for (var i = 0; i < filecollarr.length; i++) {
                if (filecollarr[i] != null && filecollarr[i] != "") {
                    if (filecollarr[i].split("-")[1] != null && filecollarr[i].split("-")[1] != "") {
                        filecount++;
                        var htmltoappend = "<tr><td valign='top' style='display:none;'>" + filecollarr[i].split("-")[0] + "</td><td width='1%' valign='top'><img src='/_layouts/MultipleImageUpload/Images/fileicon.gif' />" +
                                        "</td>" +
                                        "<td>" +
                                            filecollarr[i].split("-")[1] +
                "</td>" +
                                        "<td width='1%' valign='top'>" +
                                            "<img src='/_layouts/MultipleImageUpload/Images/filedelete.gif' style='cursor:hand;' onClick='javascript:DeleteRow(this)' />" +
                                        "</td>" +
                                    "</tr>";
                        $("#<%= tblFilestoupload.ClientID %>").append(htmltoappend);
                        var shtml = "<img src='" + filecollarr + "' width='50px' />";
                        if (filecount > 0)
                            filecollctrl.style.border = "1px solid #CCC";
                        filecount = filecollarr[i].split("-")[0];
                    }
                }
            }
        }
    }

    function AddFile() {
        var FileToUpload = document.getElementById("fleArtifacts");
        var filecollctrl = document.getElementById('<%= tblFilestoupload.ClientID %>');
        var hidfiles = document.getElementById('<%= hidFiles.ClientID %>');

        var tblid = '<%= tblFilestoupload.ClientID %>';
        var error = document.getElementById('<%= lblerrorMsg.ClientID %>');
        error.style.display = "none";
        error.innerText = "";      
        if (FileToUpload.value == null || FileToUpload.value == "")
            alert("Please select the file to upload...");
        else {
            filecount++;
            var htmltoappend = "<tr><td valign='top' style='display:none;'>" + filecount + "</td><td width='1%' valign='top'><img src='/_layouts/MultipleImageUpload/Images/fileicon.gif' />" +
                                        "</td>" +
                                        "<td>" +
                                            FileToUpload.value +
                "</td>" +
                                        "<td width='1%' valign='top'>" +
                                            "<img src='/_layouts/MultipleImageUpload/Images/filedelete.gif' style='cursor:hand;' onClick='javascript:DeleteRow(this)' />" +
                                        "</td>" +
                                    "</tr>";
            $("#<%= tblFilestoupload.ClientID %>").append(htmltoappend);
            hidfiles.value += filecount + "-" + FileToUpload.value + ",";
            FileToUpload.value = "";
            document.getElementById("spnFileUpload").innerHTML = document.getElementById("spnFileUpload").innerHTML;
            if (filecount > 0)
                filecollctrl.style.border = "1px solid #CCC";
        }
    }

    function DeleteRow(ctrl) {
        var row = ctrl.parentNode.parentNode.rowIndex;
        var hidfiles = document.getElementById('<%= hidFiles.ClientID %>');
        var filecollctrl = document.getElementById('<%= tblFilestoupload.ClientID %>');
        var error = document.getElementById('<%= lblerrorMsg.ClientID %>');
        if (filecollctrl.rows.length > 1) {
            var finalvalue = hidfiles.value;
            hidfiles.value = finalvalue.replace(ctrl.parentNode.parentNode.childNodes[0].innerText + "-" + ctrl.parentNode.parentNode.childNodes[2].innerText + ",", "");
            document.getElementById('<%= tblFilestoupload.ClientID %>').deleteRow(row);
            error.style.display = "none";
            error.innerText = "";
        }
        else {
            error.style.display = "block";
            error.innerText = "There should be one file atleast.";
        }           filecollctrl.style.border = "0px solid #CCC";
    }
</script>
<asp:HiddenField ID="hidUniqueFolderGuid" runat="server" />
<table width="100%" class="ms-propertysheet" border="0" cellSpacing="0" cellPadding="0">
    <tr>
        <td class="ms-descriptiontext" vAlign="top" style="border-top:1px solid #CCC;">
            <table width="100%" border="0" cellSpacing="0" cellPadding="1">
                <tr>
                    <td height="22" class="ms-sectionheader" vAlign="top" style="padding-top: 4px;">
                        <h3 class="ms-standardheader ms-inputformheader">
                            Upload Multiple Images
                        </h3>
                    </td>
                </tr>
                <tr>
                    <td class="ms-descriptiontext ms-inputformdescription">
                        Configure the Image Library name and upload the images.
                    </td>
                </tr>
                <tr>
                    <td>
                        <img width="150" height="19" alt="" src="/_layouts/images/blank.gif"/>
                    </td>
                </tr>
            </table>
        </td>
        <td align="left" class="ms-authoringcontrols ms-inputformcontrols" vAlign="top" style="border-top:1px solid #CCC;">
            <table width="100%" border="0" cellSpacing="0" cellPadding="0">
                <tr>
                    <td width="9"><img width="9" height="7" alt="" src="/_layouts/images/blank.gif"/></td>
                    <td><img width="150" height="7" alt="" src="/_layouts/images/blank.gif"/></td>
                    <td width="10"><img width="10" height="1" alt="" src="/_layouts/images/blank.gif"/></td>
                </tr>

                <tr>      
                    <td />
                    <td class="ms-authoringcontrols">
                        <!-- Main Content -->
                        <table width="100%" class="ms-authoringcontrols" border="0" cellSpacing="0" cellPadding="0">
                            <tr>
                                <td class="ms-authoringcontrols" width="5%" nowrap><span>Library Name:</span></td>
                                <td width="5"><img width="5" height="1" style="display: block;" alt="" src="/_layouts/images/blank.gif"/></td>
                                <td class="ms-authoringcontrols">
                                    <asp:DropDownList ID="ddlPicDocLib" runat="server"></asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <img width="1" height="6" style="display: block;" alt="" src="/_layouts/images/blank.gif"/>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <asp:HiddenField ID="hidFiles" runat="server" />
                                    <span id="spnFileUpload">
                                    <input type="file" id="fleArtifacts" size="34" /></span>
                                    <span class="s4-clust" style="cursor:hand;width: 10px; height: 10px; overflow: hidden; display: inline-block; position: relative;">
                                        <img style="left: 0px !important; top: -128px !important; position: absolute;cursor:hand;" alt="" src="/_layouts/images/fgimg.png" onclick="javascript:AddFile();"/>
                                    </span>
                                    <table width="100%" border="0" cellpadding="2" cellspacing="2" id="tblFilestoupload" runat="server" style="border:0px solid #CCC;">
                                    
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <span style="color:Red;"><asp:Label ID="lblerrorMsg" runat="server" ></asp:Label></span>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td width="10"><img width="10" height="1" alt="" src="/_layouts/images/blank.gif"/></td>       
                </tr>

                <tr>
                    <td />
                    <td><img width="150" height="13" alt="" src="/_layouts/images/blank.gif"/></td>
                    <td />
                </tr>
            </table>
        </td>
    </tr>
</table>


