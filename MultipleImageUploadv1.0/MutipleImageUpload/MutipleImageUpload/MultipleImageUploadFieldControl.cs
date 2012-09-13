using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;

using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Web.UI.HtmlControls;

namespace MultipleImageUpload
{
    public class MultipleImageUploadFieldControl : BaseFieldControl
    {
        private MultipleImageUploadFieldType ParentField;
        // Common controls
        private Table TableImage;

        // Controls on the Edit Section
        private HtmlInputHidden hiddenCheckedImage;

        private Dictionary<string, ControlSet> tableControlsList = new Dictionary<string, ControlSet>();

        public MultipleImageUploadFieldControl(MultipleImageUploadFieldType parentField)
        { this.ParentField = parentField; }

        protected override string DefaultTemplateName
        {
            get
            {
                return base.ControlMode == SPControlMode.Display ? this.DisplayTemplateName : "MultipleImageUploadField";
            }
        }

        public override string DisplayTemplateName
        {
            get { return "MultipleImageUploadFieldDisplay"; }
            set { throw new NotSupportedException(); }
        }

        protected override void CreateChildControls()
        {
            //If the field we are working on is null then exit and do nothing
            if (base.Field == null)
            {
                return;
            }
            
            base.CreateChildControls();

            //Now instantiate the control instance variables with the controls defined in the rendering templates.
            InstantiateMemberControls();

            var selectedValues = MultipleImageUploadFieldValue.FromFieldValue(string.Empty + this.ItemFieldValue);
            
            if (base.ControlMode == SPControlMode.Display)
            {
                SetupDisplayTemplateControls(selectedValues);
            }
            else
            {
                SetupEditTemplateControls(selectedValues);
            }
        }

        private void SetupDisplayTemplateControls(List<MultipleImageUploadFieldValue> selectedValues)
        {
            string strLibraryName = this.ParentField.LibraryName;
            string strUniqueFolderID = this.ParentField.UniqueFolderGuid;
            bool editMode = (this.ControlMode == SPControlMode.Edit);
            bool newMode = (this.ControlMode == SPControlMode.New);
            try
            {
                using (SPSite SiteCollection = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb site = SiteCollection.OpenWeb())
                    {
                        SPList targetList = site.Lists.TryGetList(strLibraryName);
                        SPFolder RootFolder = targetList.RootFolder;
                        SPFolder subfolder = null;
                        try
                        {
                            subfolder = RootFolder.SubFolders[strUniqueFolderID];
                            if (subfolder != null)
                            {
                                SPFileCollection files = subfolder.Files;
                                int idIndex = 1;
                                TableRow valueRow = new TableRow();
                                TableImage.Rows.Add(valueRow);
                                int cellcount = 1;
                                foreach (SPFile file in files)
                                {
                                    var formattedValue = file.ServerRelativeUrl.ToLower().ToString();
                                    var selectedValue = selectedValues.FirstOrDefault(val => val.URL.ToLower() == formattedValue) ?? new MultipleImageUploadFieldValue();

                                    if (!string.IsNullOrEmpty(selectedValue.URL))
                                    {
                                        if (cellcount == 4)
                                        {
                                            cellcount = 1;
                                            valueRow = new TableRow();
                                            TableImage.Rows.Add(valueRow);
                                        }
                                        if (cellcount <= 3)
                                        {
                                            TableCell imageCell = new TableCell();
                                            imageCell.Width = new Unit(15, UnitType.Percentage);
                                            imageCell.VerticalAlign = VerticalAlign.Top;
                                            imageCell.HorizontalAlign = HorizontalAlign.Left;
                                            valueRow.Cells.Add(imageCell);

                                            // Creating the people editor and populating with the selected field
                                            Image image = new Image();
                                            image.ID = "img" + file.UniqueId;
                                            image.ImageUrl = file.ServerRelativeUrl;
                                            image.Width = new Unit(100, UnitType.Pixel);
                                            image.Height = new Unit(100, UnitType.Pixel);
                                            imageCell.Controls.Add(image);

                                            HtmlGenericControl divImageName = new HtmlGenericControl();
                                            divImageName.InnerText = file.Name.ToString();
                                            imageCell.Controls.Add(divImageName);

                                            tableControlsList.Add(formattedValue, new ControlSet
                                            {
                                                Image = image,
                                                div = divImageName
                                            });
                                            cellcount++;
                                        }
                                        idIndex++;
                                    }
                                }
                            }
                        }
                        catch
                        {

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void SetupEditTemplateControls(List<MultipleImageUploadFieldValue> selectedValues)
        {
            string strLibraryName = this.ParentField.LibraryName;
            string strUniqueFolderID = this.ParentField.UniqueFolderGuid;
            bool editMode = (this.ControlMode == SPControlMode.Edit);
            bool newMode = (this.ControlMode == SPControlMode.New);
            try
            {
                using (SPSite SiteCollection = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb site = SiteCollection.OpenWeb())
                    {
                        SPList targetList = site.Lists.TryGetList(strLibraryName);
                        SPFolder RootFolder = targetList.RootFolder;
                        SPFolder subfolder = null;
                        try
                        {
                            subfolder = RootFolder.SubFolders[strUniqueFolderID];
                            if (subfolder != null)
                            {
                                SPFileCollection files = subfolder.Files;
                                int idIndex = 1;
                                TableRow valueRow = new TableRow();
                                TableImage.Rows.Add(valueRow);
                                int cellcount = 1;
                                foreach (SPFile file in files)
                                {                                    
                                    var formattedValue = file.ServerRelativeUrl.ToLower().ToString();
                                    var selectedValue = selectedValues.FirstOrDefault(val => val.URL.ToLower() == formattedValue) ?? new MultipleImageUploadFieldValue();
                                    if (cellcount == 4)
                                    {
                                        cellcount = 1;
                                        valueRow = new TableRow();
                                        TableImage.Rows.Add(valueRow);
                                    }
                                    if (cellcount <= 3)
                                    {
                                        TableCell checkCell = new TableCell();
                                        checkCell.Width = new Unit(3, UnitType.Percentage);
                                        checkCell.VerticalAlign = VerticalAlign.Top;
                                        checkCell.HorizontalAlign = HorizontalAlign.Left;
                                        valueRow.Cells.Add(checkCell);

                                        // Creating a checkbox to check the default option
                                        CheckBox chkSelected = new CheckBox();
                                        chkSelected.ID = "chkSelected" + idIndex.ToString();
                                        //chkSelected.Attributes["DisplayName"] = formattedValue;
                                        chkSelected.Enabled = editMode || newMode;
                                        checkCell.Controls.Add(chkSelected);

                                        chkSelected.Checked = (!string.IsNullOrEmpty(selectedValue.URL) ? true : false);//.Checked;

                                        TableCell imageCell = new TableCell();
                                        imageCell.Width = new Unit(10, UnitType.Percentage);
                                        imageCell.VerticalAlign = VerticalAlign.Top;
                                        imageCell.HorizontalAlign = HorizontalAlign.Left;                                        
                                        valueRow.Cells.Add(imageCell);

                                        // Creating the people editor and populating with the selected field
                                        Image image = new Image();
                                        image.ID = "img" + file.UniqueId;
                                        image.ImageUrl = file.ServerRelativeUrl;
                                        image.Width = new Unit(100, UnitType.Pixel);
                                        image.Height = new Unit(100, UnitType.Pixel);
                                        imageCell.Controls.Add(image);

                                        HtmlGenericControl divImageName = new HtmlGenericControl();
                                        divImageName.InnerText = file.Name.ToString();
                                        imageCell.Controls.Add(divImageName);

                                        tableControlsList.Add(formattedValue, new ControlSet
                                        {
                                            CheckBox = chkSelected,
                                            Image = image,
                                            div = divImageName
                                        });
                                        cellcount++;
                                    }
                                    
                                    // Adding to control searchable collection                                    
                                    idIndex++;
                                }
                            }
                        }
                        catch
                        {
                            
                        }
                    }
                }
            }
            catch (Exception ex)
            {                
                throw ex;
            }
        }

        private string GetFieldControlValue()
        {
            var allValues = new List<MultipleImageUploadFieldValue>();
            foreach (var valueName in tableControlsList.Keys)
            {
                if (tableControlsList[valueName].CheckBox.Checked)
                {
                    allValues.Add(new MultipleImageUploadFieldValue
                    {
                        //Name = valueName,
                        //Checked = tableControlsList[valueName].CheckBox.Checked,
                        URL = tableControlsList[valueName].Image.ImageUrl
                    });
                }
            }

            return MultipleImageUploadFieldValue.ToFieldValue(allValues);
        }

        //get/set value for custom field 
        public override object Value
        {
            get
            {
                return GetFieldControlValue();
            }
            set { }
        }

        //Update field value with user input & check field validation
        public override void UpdateFieldValueInItem()
        {
            this.EnsureChildControls();
            try
            {
                this.Value = GetFieldControlValue();
                this.ItemFieldValue = GetFieldControlValue();
            }

            catch (Exception ex)
            {
                this.IsValid = false;
                this.ErrorMessage = "* " + ex.Message;
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (this.ControlMode == SPControlMode.Edit ||
                this.ControlMode == SPControlMode.New)
            { /* Code to run in input mode */ }
        }
        
        private void InstantiateMemberControls()
        {
            // Common control for Edit and Display
            TableImage = (Table)base.TemplateContainer.FindControl("tblImage");

            hiddenCheckedImage = (HtmlInputHidden)base.TemplateContainer.FindControl("hidCheckedImage");
        }               
    }
}
