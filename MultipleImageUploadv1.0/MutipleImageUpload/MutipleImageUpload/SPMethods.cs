using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using System.IO;

namespace MultipleImageUpload
{
    public class SPMethods : IDisposable
    {
        public void UploadDocument(string LibraryName, string UniqueFolderGuid, string strFileColl)
        {
            string fileids = string.Empty;
            try
            {
                using (SPSite Sitecollection = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb Site = Sitecollection.OpenWeb())
                    {
                        SPSecurity.RunWithElevatedPrivileges(delegate()
                        {
                            Boolean replaceExistingFiles = true;
                            SPFolder myLibrary = Site.Folders[LibraryName];
                            SPFolder subfolder;
                            try
                            {
                                subfolder = myLibrary.SubFolders[UniqueFolderGuid];
                                if (subfolder == null)
                                    subfolder = myLibrary.SubFolders.Add(UniqueFolderGuid);
                                else
                                {
                                    subfolder.Delete();
                                    myLibrary.Update();
                                    subfolder = myLibrary.SubFolders.Add(UniqueFolderGuid);
                                }
                            }
                            catch
                            {
                                subfolder = myLibrary.SubFolders.Add(UniqueFolderGuid);
                            }
                            //subfolder.Update();
                            string[] filecollarr = strFileColl.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                            foreach (string file in filecollarr)
                            {
                                if (!string.IsNullOrEmpty(file) && file != " ")
                                {
                                    string strFileName = file.Split('-')[1].Trim();
                                    if (!System.IO.File.Exists(strFileName))
                                        throw new FileNotFoundException("File not found.", strFileName);

                                    String fileName = System.IO.Path.GetFileName(strFileName);
                                    FileStream fileStream = File.OpenRead(strFileName);

                                    // Upload document
                                    //SPFile spfile = myLibrary.Files.Add(fileName, fileStream, hashProperties, replaceExistingFiles);
                                    SPFile spfile = subfolder.Files.Add(fileName, fileStream, replaceExistingFiles);
                                    fileids += spfile.UniqueId + ",";
                                    // Commit 
                                    //SPSecurity.RunWithElevatedPrivileges(delegate()
                                    //{
                                        //myLibrary.Update();
                                    //});
                                }
                            }
                            myLibrary.Update();       
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Dispose()
        {
            
        }
    }
}
