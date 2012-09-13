using System;
using System.Threading;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using Microsoft.SharePoint.WebPartPages;

namespace MultipleImageUpload
{
    public partial class MultipleImageUploadFieldEditor : UserControl, IFieldEditor
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // To check the User Control post back
            if (!IsPostBack)
            {
                BindDropDown();
            }
        }

        private string Documentname
        {
            get
            {
                return (string)ViewState["Documentname"];

            }
            set
            {
                ViewState["Documentname"] = value;
            }
        }

        private string FileCollection
        {
            get
            {
                return (string)ViewState["FileCollection"];

            }
            set
            {
                ViewState["FileCollection"] = value;
            }
        }

        private string UniqueFolderGuid
        {
            get
            {
                return (string)ViewState["UniqueFolderGuid"];

            }
            set
            {
                ViewState["UniqueFolderGuid"] = value;
            }
        }

        private void BindDropDown()
        {
            using (SPSite site = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb web = site.OpenWeb())
                {
                    ddlPicDocLib.Items.Clear();
                    ddlPicDocLib.Enabled = true;
                    foreach (SPList list in web.Lists)
                    {
                        if (list.BaseTemplate == SPListTemplateType.PictureLibrary)
                        {
                            ddlPicDocLib.Items.Add(list.Title.ToString());                            
                        }
                    }
                }
            }

            if (string.IsNullOrEmpty(Documentname))
            {
                ddlPicDocLib.SelectedIndex = 0;
            }
            else
            {                
                try
                {
                    ddlPicDocLib.Items.FindByText(Documentname).Selected = true;
                    ddlPicDocLib.Enabled = false;
                }
                catch (Exception ex)
                {

                    ddlPicDocLib.SelectedIndex = 0;
                }
            }
        }

        public bool DisplayAsNewSection
        {
            get { return true; }
        }

        public void InitializeWithField(SPField field)
        {
            // To check the page post back
            if (!Page.IsPostBack && field != null)
            {
                Documentname = ((MultipleImageUploadFieldType)field).LibraryName;
                FileCollection = ((MultipleImageUploadFieldType)field).FileCollection;
                UniqueFolderGuid = ((MultipleImageUploadFieldType)field).UniqueFolderGuid;
                hidFiles.Value = FileCollection;
                hidUniqueFolderGuid.Value = UniqueFolderGuid;
                BindDropDown();                
            }
        }

        public void OnSaveChange(SPField field, bool isNewField)
        {
            /*This is perhaps the most tricky part in implementing custom field type properties. 
			 * The field param passed in to this method is a different object instance to the actual field being edited.
			 * This is why we'll need to set the value to be saved into the LocalThreadStorage, and retrieve it back out
			 * in the FieldType class and update the field with the custom setting properties. For more info see
			 * http://msdn.microsoft.com/en-us/library/cc889345(office.12).aspx#CreatingWSS3CustomFields_StoringFieldSetting */

            //TODO: Handle case where location is not specified or not valid
            if(ddlPicDocLib.Items.Count <= 0)
            {
                lblerrorMsg.Text = "Please create a Picture Library.";
                return;
            }
            Thread.SetData(Thread.GetNamedDataSlot(MultipleImageUploadCustomProperties.LibraryName), ddlPicDocLib.SelectedItem.Text);
            Thread.SetData(Thread.GetNamedDataSlot(MultipleImageUploadCustomProperties.FileCollection), hidFiles.Value);
            if (hidUniqueFolderGuid.Value != null && !string.IsNullOrEmpty(hidUniqueFolderGuid.Value))
                Thread.SetData(Thread.GetNamedDataSlot(MultipleImageUploadCustomProperties.UniqueFolderGuid), hidUniqueFolderGuid.Value);
            else
                Thread.SetData(Thread.GetNamedDataSlot(MultipleImageUploadCustomProperties.UniqueFolderGuid), Guid.NewGuid().ToString());
            if (hidFiles.Value == null || hidFiles.Value == "")
            {
                lblerrorMsg.Text = "Please browse atleast one picture.";
                return;
            }
        }
    }
}
