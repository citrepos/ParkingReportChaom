using System;
using System.IO;
using System.Reflection;
using CrystalDecisions.CrystalReports.Engine;
using ParkingManagementReport.Common;

namespace ParkingManagementReport.Utilities
{
    internal class CrystalReportHelper
    {
        internal static void SetGenericReportFormulaFields(
            ReportDocument reportDocument,
            bool useSecondaryCompanyName = false,
            bool useCombinedAddress = false)
        {
            string reportName = AppGlobalVariables.Printings.Header.Trim();
            string address1 = AppGlobalVariables.Printings.Address1.Trim();
            string address2 = AppGlobalVariables.Printings.Address2.Trim();
            string taxId = AppGlobalVariables.Printings.Tax1.Trim();
            string telephone = AppGlobalVariables.Printings.Telephone.Trim();
            string companyName = useSecondaryCompanyName
                ? AppGlobalVariables.Printings.Company2.Trim()
                : AppGlobalVariables.Printings.Company1.Trim();
            try
            {
                reportDocument.DataDefinition.FormulaFields["ReportName"].Text = $@"'{reportName}'";
            }
            catch { }
            try
            {
                reportDocument.DataDefinition.FormulaFields["CompanyName"].Text = $@"'{companyName}'";
            }
            catch { }
            try
            {
                reportDocument.DataDefinition.FormulaFields["Address1"].Text = $@"'{address1}'";
            }
            catch { }
            try
            {
                reportDocument.DataDefinition.FormulaFields["Address2"].Text = $@"'{address2}'";
            }
            catch { }
            try
            {
                reportDocument.DataDefinition.FormulaFields["TaxID"].Text = $@"'{taxId}'";
            }
            catch { }
            try
            {
                reportDocument.DataDefinition.FormulaFields["tel"].Text = $@"'{telephone}'";
            }
            catch { }
            try
            {

            }
            catch { }
        }

        internal static string GetFullReportFilePath(int selectedReportId)
        {
            string path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            path = path.Replace("\\bin\\Debug", "");

            string fullPath = "";

            switch (selectedReportId)
            {
                case 1:
                    if (Configs.Reports.ReportNoRunning)
                    {
                        if (Configs.IsVillage && Configs.Use2Camera)
                            fullPath = Path.Combine(path, "CrystalReports\\Report1_1NoRunning.rpt");
                        else if ((Configs.IsVillage || Configs.VisitorFillDetail) && (selectedReportId == 1 || selectedReportId == 91))
                            fullPath = Path.Combine(path, "CrystalReports\\Report1_2NoRunning.rpt");
                        else
                        {
                            if (Configs.Reports.UseReport1_3)
                                fullPath = Path.Combine(path, "CrystalReports\\Report1_3NoRunning.rpt");
                            else if (Configs.Reports.UseReport1_4)
                                fullPath = Path.Combine(path, "CrystalReports\\Report1_4NoRunning.rpt");
                            else if (Configs.Reports.UseReport1_5)
                                fullPath = Path.Combine(path, "CrystalReports\\Report1_5NoRunning.rpt");
                            else if (Configs.Reports.UseReport1logo)
                                fullPath = Path.Combine(path, "CrystalReports\\Report1logoNoRunning.rpt");
                            else if (Configs.Reports.UseReport1_6)
                                fullPath = Path.Combine(path, "CrystalReports\\Report1_6NoRunning.rpt");
                            else if (Configs.Reports.UseReport1_7)
                                fullPath = Path.Combine(path, "CrystalReports\\Report1_7NoRunning.rpt");
                            else if (Configs.Reports.UseReport1_8)
                                fullPath = Path.Combine(path, "CrystalReports\\Report1_8NoRunning.rpt");
                            else
                                fullPath = Path.Combine(path, "CrystalReports\\Report1NoRunning.rpt");
                        }
                    }
                    else
                    {
                        if (Configs.IsVillage && Configs.Use2Camera)
                            fullPath = Path.Combine(path, "CrystalReports\\Report1_1.rpt");
                        else if ((Configs.IsVillage || Configs.VisitorFillDetail) && (selectedReportId == 1 || selectedReportId == 91))
                            fullPath = Path.Combine(path, "CrystalReports\\Report1_2.rpt");
                        else
                        {
                            if (Configs.Reports.UseReport1_3)
                                fullPath = Path.Combine(path, "CrystalReports\\Report1_3.rpt");
                            else if (Configs.Reports.UseReport1_4)
                                fullPath = Path.Combine(path, "CrystalReports\\Report1_4.rpt");
                            else if (Configs.Reports.UseReport1_5)
                                fullPath = Path.Combine(path, "CrystalReports\\Report1_5.rpt");
                            else if (Configs.Reports.UseReport1logo)
                                fullPath = Path.Combine(path, "CrystalReports\\Report1logo.rpt");
                            else if (Configs.Reports.UseReport1_6)
                                fullPath = Path.Combine(path, "CrystalReports\\Report1_6.rpt");
                            else if (Configs.Reports.UseReport1_7)
                                fullPath = Path.Combine(path, "CrystalReports\\Report1_7.rpt");
                            else if (Configs.Reports.UseReport1_8)
                                fullPath = Path.Combine(path, "CrystalReports\\Report1_8.rpt");
                            else
                                fullPath = Path.Combine(path, "CrystalReports\\Report1.rpt");
                        }
                    }
                    break;
                case 6:
                    if (Configs.Reports.ReportNoRunning)
                    {
                        if (Configs.Reports.UseReport6)
                        {
                            if (Configs.Reports.UseReport1_6)
                                fullPath = Path.Combine(path, "CrystalReports\\Report6plus1_6NoRunning.rpt");
                            else
                                fullPath = Path.Combine(path, "CrystalReports\\Report6NoRunning.rpt");
                        }
                        else
                        {
                            if (Configs.Reports.UseReport6)
                            {
                                if (Configs.Reports.UseReport1_6)
                                    fullPath = Path.Combine(path, "CrystalReports\\Report6plus1_6.rpt");
                                else
                                    fullPath = Path.Combine(path, "CrystalReports\\Report6.rpt");
                            }
                        }
                    }
                    break;
                default:
                    break;
            }

            return fullPath;
        }
    }
}
