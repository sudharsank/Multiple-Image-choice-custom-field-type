using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;
using Microsoft.SharePoint.Utilities;
using System.IO;

namespace WRDC_MultipleImageUpload.Features.MultipleImageUpload
{
    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>

    [Guid("7288b324-47a3-4c34-a3be-179d7487c55c")]
    public class MultipleImageUploadEventReceiver : SPFeatureReceiver
    {
        // Uncomment the method below to handle the event raised after a feature has been activated.

        public const string lstName = "MultipleImageUploadScripts";
        public const string lstDesc = "To store scripts for the MultipleImageUpload custom field type.";

        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            try
            {
                using (SPWeb Site = properties.Feature.Parent as SPWeb)
                {
                    SPSecurity.RunWithElevatedPrivileges(delegate()
                    {
                        Site.AllowUnsafeUpdates = true;
                        SPList picturelist = null;
                        picturelist = Site.Lists.TryGetList(lstName);
                        if (picturelist == null)
                        {
                            Site.Lists.Add(lstName, lstDesc, SPListTemplateType.DocumentLibrary);
                            Site.Update();
                            picturelist = Site.Lists.TryGetList(lstName);

                            if (picturelist != null)
                            {
                                picturelist.Hidden = false;
                                picturelist.OnQuickLaunch = false;

                                string path = SPUtility.GetGenericSetupPath("TEMPLATE\\LAYOUTS");
                                path += "\\MultipleImageUpload\\MIUScripts\\";
                                                                

                                string libraryRelativePath = picturelist.RootFolder.ServerRelativeUrl;
                                string libraryPath = Site.Site.MakeFullUrl(libraryRelativePath);
                                using (FileStream fs = new FileStream(path + "jquery-1.4.2.min.js", FileMode.Open))
                                {
                                    SPFile file = Site.Files.Add(libraryPath + "\\jquery-1.4.2.min.js", fs);
                                    file.Update();
                                }

                                using (FileStream fs = new FileStream(path + "jquery.SPServices-0.7.2ALPHA7.js", FileMode.Open))
                                {
                                    SPFile file = Site.Files.Add(libraryPath + "\\jquery.SPServices-0.7.2ALPHA7.js", fs);
                                    file.Update();
                                }
                                picturelist.Update();
                            }
                        }
                        Site.AllowUnsafeUpdates = false;
                    });
                }
            }
            catch (Exception ex)
            {                
                throw ex;
            }
        }


        // Uncomment the method below to handle the event raised before a feature is deactivated.

        //public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        //{
        //}


        // Uncomment the method below to handle the event raised after a feature has been installed.

        //public override void FeatureInstalled(SPFeatureReceiverProperties properties)
        //{
        //}


        // Uncomment the method below to handle the event raised before a feature is uninstalled.

        //public override void FeatureUninstalling(SPFeatureReceiverProperties properties)
        //{
        //}

        // Uncomment the method below to handle the event raised when a feature is upgrading.

        //public override void FeatureUpgrading(SPFeatureReceiverProperties properties, string upgradeActionName, System.Collections.Generic.IDictionary<string, string> parameters)
        //{
        //}
    }
}
