using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Threading;

namespace MultipleImageUpload
{
    public class MultipleImageUploadFieldType : SPFieldMultiColumn
    {
        public MultipleImageUploadFieldType(SPFieldCollection fields, string fieldName) : base(fields, fieldName) { }

        public MultipleImageUploadFieldType(SPFieldCollection fields, string typeName, string displayName) : base(fields, typeName, displayName) { }


        public override BaseFieldControl FieldRenderingControl
        {
            get
            {
                BaseFieldControl fieldControl = new MultipleImageUploadFieldControl(this);
                fieldControl.FieldName = InternalName;
                return fieldControl;
            }
        }

        public override void OnAdded(SPAddFieldOptions op)
        {
            /*We will need to update the field again after it is added to save the custom setting properties. For more
             * info see http://msdn.microsoft.com/en-us/library/cc889345(office.12).aspx#CreatingWSS3CustomFields_StoringFieldSetting */

            base.OnAdded(op);
            this.Update();
        }

        public override void Update()
        {
            var LibraryNameToValueFromThreadData = Thread.GetData(Thread.GetNamedDataSlot(MultipleImageUploadCustomProperties.LibraryName));
            var FileCollectionToValueFromThreadData = Thread.GetData(Thread.GetNamedDataSlot(MultipleImageUploadCustomProperties.FileCollection));
            var UniqueFolderGuidToValueFromThreadData = Thread.GetData(Thread.GetNamedDataSlot(MultipleImageUploadCustomProperties.UniqueFolderGuid));

            if (LibraryNameToValueFromThreadData != null)
            {
                this.LibraryName = (string)LibraryNameToValueFromThreadData;
            }

            if (FileCollectionToValueFromThreadData != null)
            {
                this.FileCollection = (string)FileCollectionToValueFromThreadData;
            }

            if (UniqueFolderGuidToValueFromThreadData != null)
            {
                this.UniqueFolderGuid = (string)UniqueFolderGuidToValueFromThreadData;
            }

            using (SPMethods methods = new SPMethods())
            {
                methods.UploadDocument(this.LibraryName, this.UniqueFolderGuid, this.FileCollection);
            }

            base.Update();
        }

        public override string GetValidatedString(object value)
        {
            if ((this.Required == true) && (value == null))
            { throw new SPFieldValidationException("This is a required field."); }

            return base.GetValidatedString(value);
        } 

        public string LibraryName
        {
            get
            {
                return (string)base.GetCustomProperty(MultipleImageUploadCustomProperties.LibraryName);
            }
            set
            {
                base.SetCustomProperty(MultipleImageUploadCustomProperties.LibraryName, value);
            }
        }

        public string FileCollection
        {
            get
            {
                return (string)base.GetCustomProperty(MultipleImageUploadCustomProperties.FileCollection);
            }
            set
            {
                base.SetCustomProperty(MultipleImageUploadCustomProperties.FileCollection, value);
            }
        }

        public string UniqueFolderGuid
        {
            get
            {
                return (string)base.GetCustomProperty(MultipleImageUploadCustomProperties.UniqueFolderGuid);
            }
            set
            {
                base.SetCustomProperty(MultipleImageUploadCustomProperties.UniqueFolderGuid, value);
            }
        }
    }
}
