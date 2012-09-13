using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace MultipleImageUpload
{
    public class MultipleImageUploadFieldValue
    {
        public const char InterValueDelimiter = ',';
        public const char InterPropertyDelimiter = '|';

        //public string Name = string.Empty;
        //public bool Checked = false;
        public string URL = string.Empty;

        public override string ToString()
        {
            return string.Concat(
                //this.Name.Trim(), InterPropertyDelimiter,
                //this.Checked, InterPropertyDelimiter,
                this.URL);
        }

        public static List<MultipleImageUploadFieldValue> FromFieldValue(string fieldValue)
        {
            var allValues = new List<MultipleImageUploadFieldValue>();
            var splittedValue = "";
            var splittedValues = new string[] { };
            if(fieldValue.Contains(";#"))
                splittedValue = fieldValue.Split(new string[] { ";#" }, StringSplitOptions.RemoveEmptyEntries)[0];
            if(!string.IsNullOrEmpty(splittedValue))
                splittedValues = splittedValue.Split(new char[] { InterValueDelimiter }, StringSplitOptions.RemoveEmptyEntries);
            else splittedValues = fieldValue.Split(new char[] { InterValueDelimiter }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string val in splittedValues)
            {
                var valueVals = val.Split(new char[] { InterPropertyDelimiter }, StringSplitOptions.RemoveEmptyEntries);
                allValues.Add(new MultipleImageUploadFieldValue
                {
                    //Name = valueVals.Length > 0 ? valueVals[0].Trim() : string.Empty,
                    //Checked = valueVals.Length > 1 && !string.IsNullOrEmpty(valueVals[1]) ?
                    //    bool.Parse(valueVals[1]) : false,
                    //URL = valueVals.Length > 2 ? valueVals[2].Trim() : string.Empty,
                    URL = valueVals.Length > 0 ? valueVals[0].Trim() : string.Empty
                });
            }

            return allValues;
        }

        public static string ToFieldValue(List<MultipleImageUploadFieldValue> listOfValues)
        {
            return string.Concat(
                (from value in listOfValues
                 select value.ToString() + InterValueDelimiter).ToArray());
        }
    }
}
