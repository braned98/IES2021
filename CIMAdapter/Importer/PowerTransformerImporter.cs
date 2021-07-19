using System;
using System.Collections.Generic;
using CIM.Model;
using FTN.Common;
using FTN.ESI.SIMES.CIM.CIMAdapter.Manager;

namespace FTN.ESI.SIMES.CIM.CIMAdapter.Importer
{
    /// <summary>
    /// PowerTransformerImporter
    /// </summary>
    public class PowerTransformerImporter
    {
        /// <summary> Singleton </summary>
        private static PowerTransformerImporter ptImporter = null;
        private static object singletoneLock = new object();

        private ConcreteModel concreteModel;
        private Delta delta;
        private ImportHelper importHelper;
        private TransformAndLoadReport report;


        #region Properties
        public static PowerTransformerImporter Instance
        {
            get
            {
                if (ptImporter == null)
                {
                    lock (singletoneLock)
                    {
                        if (ptImporter == null)
                        {
                            ptImporter = new PowerTransformerImporter();
                            ptImporter.Reset();
                        }
                    }
                }
                return ptImporter;
            }
        }

        public Delta NMSDelta
        {
            get
            {
                return delta;
            }
        }
        #endregion Properties


        public void Reset()
        {
            concreteModel = null;
            delta = new Delta();
            importHelper = new ImportHelper();
            report = null;
        }

        public TransformAndLoadReport CreateNMSDelta(ConcreteModel cimConcreteModel)
        {
            LogManager.Log("Importing PowerTransformer Elements...", LogLevel.Info);
            report = new TransformAndLoadReport();
            concreteModel = cimConcreteModel;
            delta.ClearDeltaOperations();

            if ((concreteModel != null) && (concreteModel.ModelMap != null))
            {
                try
                {
                    // convert into DMS elements
                    ConvertModelAndPopulateDelta();
                }
                catch (Exception ex)
                {
                    string message = string.Format("{0} - ERROR in data import - {1}", DateTime.Now, ex.Message);
                    LogManager.Log(message);
                    report.Report.AppendLine(ex.Message);
                    report.Success = false;
                }
            }
            LogManager.Log("Importing PowerTransformer Elements - END.", LogLevel.Info);
            return report;
        }

        /// <summary>
        /// Method performs conversion of network elements from CIM based concrete model into DMS model.
        /// </summary>
        private void ConvertModelAndPopulateDelta()
        {
            LogManager.Log("Loading elements and creating delta...", LogLevel.Info);

            //// import all concrete model types (DMSType enum)
            ImportProcess();
            ImportMarketDocument();
            ImportPeriods();
            ImportPoints();
            ImportTimeSeries();
            ImportMeasurementPoints();
            

            LogManager.Log("Loading elements and creating delta completed.", LogLevel.Info);
        }

        #region Import
        private void ImportPoints()
        {
            SortedDictionary<string, object> cimPoints = concreteModel.GetAllObjectsOfType("FTN.Point");
            if (cimPoints != null)
            {
                foreach (KeyValuePair<string, object> cimPointPair in cimPoints)
                {
                    FTN.Point cimPoint = cimPointPair.Value as FTN.Point;

                    ResourceDescription rd = CreatePointResourceDescription(cimPoint);
                    if (rd != null)
                    {
                        delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
                        report.Report.Append("Point ID = ").Append(cimPoint.ID).Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
                    }
                    else
                    {
                        report.Report.Append("Point ID = ").Append(cimPoint.ID).AppendLine(" FAILED to be converted");
                    }
                }
                report.Report.AppendLine();
            }
        }

        private ResourceDescription CreatePointResourceDescription(FTN.Point cimPoint)
        {
            ResourceDescription rd = null;
            if (cimPoint != null)
            {
                long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.POINT, importHelper.CheckOutIndexForDMSType(DMSType.POINT));
                rd = new ResourceDescription(gid);
                importHelper.DefineIDMapping(cimPoint.ID, gid);

                ////populate ResourceDescription
                PowerTransformerConverter.PopulatePointResourceProperties(cimPoint, rd, importHelper, report);
            }
            return rd;
        }

        private void ImportPeriods()
        {
            SortedDictionary<string, object> cimPeriods = concreteModel.GetAllObjectsOfType("FTN.Period");
            if (cimPeriods != null)
            {
                foreach (KeyValuePair<string, object> cimPeriodPair in cimPeriods)
                {
                    FTN.Period cimPeriod = cimPeriodPair.Value as FTN.Period;

                    ResourceDescription rd = CreatePeriodResourceDescription(cimPeriod);
                    if (rd != null)
                    {
                        delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
                        report.Report.Append("Period ID = ").Append(cimPeriod.ID).Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
                    }
                    else
                    {
                        report.Report.Append("Period ID = ").Append(cimPeriod.ID).AppendLine(" FAILED to be converted");
                    }
                }
                report.Report.AppendLine();
            }
        }

        private ResourceDescription CreatePeriodResourceDescription(FTN.Period cimPeriod)
        {
            ResourceDescription rd = null;
            if (cimPeriod != null)
            {
                long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.PERIOD, importHelper.CheckOutIndexForDMSType(DMSType.PERIOD));
                rd = new ResourceDescription(gid);
                importHelper.DefineIDMapping(cimPeriod.ID, gid);

                ////populate ResourceDescription
                PowerTransformerConverter.PopulatePeriodResourceProperties(cimPeriod, rd, importHelper, report);
            }
            return rd;
        }

        private void ImportTimeSeries()
        {
            SortedDictionary<string, object> cimTimeSeries = concreteModel.GetAllObjectsOfType("FTN.TimeSeries");
            if (cimTimeSeries != null)
            {
                foreach (KeyValuePair<string, object> cimTimeSerPair in cimTimeSeries)
                {
                    FTN.TimeSeries cimTimeSer = cimTimeSerPair.Value as FTN.TimeSeries;

                    ResourceDescription rd = CreateTimeSeriesResourceDescription(cimTimeSer);
                    if (rd != null)
                    {
                        delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
                        report.Report.Append("TimeSeries ID = ").Append(cimTimeSer.ID).Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
                    }
                    else
                    {
                        report.Report.Append("TimeSeries ID = ").Append(cimTimeSer.ID).AppendLine(" FAILED to be converted");
                    }
                }
                report.Report.AppendLine();
            }
        }

        private ResourceDescription CreateTimeSeriesResourceDescription(FTN.TimeSeries cimTimeSer)
        {
            ResourceDescription rd = null;
            if (cimTimeSer != null)
            {
                long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.TIMESERIES, importHelper.CheckOutIndexForDMSType(DMSType.TIMESERIES));
                rd = new ResourceDescription(gid);
                importHelper.DefineIDMapping(cimTimeSer.ID, gid);

                ////populate ResourceDescription
                PowerTransformerConverter.PopulateTimeSeriesProperties(cimTimeSer, rd, importHelper, report);
            }
            return rd;
        }

        private void ImportProcess()
        {
            SortedDictionary<string, object> Process = concreteModel.GetAllObjectsOfType("FTN.Process");
            if (Process != null)
            {
                foreach (KeyValuePair<string, object> cimProcessPair in Process)
                {
                    FTN.Process cimProc = cimProcessPair.Value as FTN.Process;

                    ResourceDescription rd = CreateProcessResourceDescription(cimProc);
                    if (rd != null)
                    {
                        delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
                        report.Report.Append("Process ID = ").Append(cimProc.ID).Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
                    }
                    else
                    {
                        report.Report.Append("Process ID = ").Append(cimProc.ID).AppendLine(" FAILED to be converted");
                    }
                }
                report.Report.AppendLine();
            }
        }

        private ResourceDescription CreateProcessResourceDescription(FTN.Process cimProc)
        {
            ResourceDescription rd = null;
            if (cimProc != null)
            {
                long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.PROCESS, importHelper.CheckOutIndexForDMSType(DMSType.PROCESS));
                rd = new ResourceDescription(gid);
                importHelper.DefineIDMapping(cimProc.ID, gid);

                ////populate ResourceDescription
                PowerTransformerConverter.PopulateProcessProperties(cimProc, rd, importHelper, report);
            }
            return rd;
        }

        private void ImportMeasurementPoints()
        {
            SortedDictionary<string, object> cimMeasurementPoints = concreteModel.GetAllObjectsOfType("FTN.MeasurementPoint");
            if (cimMeasurementPoints != null)
            {
                foreach (KeyValuePair<string, object> cimMeasurementPointPair in cimMeasurementPoints)
                {
                    FTN.MeasurementPoint cimMeasurementPoint = cimMeasurementPointPair.Value as FTN.MeasurementPoint;

                    ResourceDescription rd = CreateMeasurementPointResourceDescription(cimMeasurementPoint);
                    if (rd != null)
                    {
                        delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
                        report.Report.Append("MeasurementPoint ID = ").Append(cimMeasurementPoint.ID).Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
                    }
                    else
                    {
                        report.Report.Append("MeasurementPoint ID = ").Append(cimMeasurementPoint.ID).AppendLine(" FAILED to be converted");
                    }
                }
                report.Report.AppendLine();
            }
        }

        private ResourceDescription CreateMeasurementPointResourceDescription(FTN.MeasurementPoint cimMeasurementPoint)
        {
            ResourceDescription rd = null;
            if (cimMeasurementPoint != null)
            {
                long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.MEASUREMENTPOINT, importHelper.CheckOutIndexForDMSType(DMSType.MEASUREMENTPOINT));
                rd = new ResourceDescription(gid);
                importHelper.DefineIDMapping(cimMeasurementPoint.ID, gid);

                ////populate ResourceDescription
                PowerTransformerConverter.PopulateMeasurementPointProperties(cimMeasurementPoint, rd, importHelper, report);
            }
            return rd;
        }

        private void ImportMarketDocument()
        {
            SortedDictionary<string, object> cimMarketDocuments = concreteModel.GetAllObjectsOfType("FTN.MarketDocument");
            if (cimMarketDocuments != null)
            {
                foreach (KeyValuePair<string, object> cimMarketDocumentPair in cimMarketDocuments)
                {
                    FTN.MarketDocument cimMarketDocument = cimMarketDocumentPair.Value as FTN.MarketDocument;

                    ResourceDescription rd = CreateMarketDocumentResourceDescription(cimMarketDocument);
                    if (rd != null)
                    {
                        delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
                        report.Report.Append("MarketDocument ID = ").Append(cimMarketDocument.ID).Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
                    }
                    else
                    {
                        report.Report.Append("MarketDocument ID = ").Append(cimMarketDocument.ID).AppendLine(" FAILED to be converted");
                    }
                }
                report.Report.AppendLine();
            }
        }

        private ResourceDescription CreateMarketDocumentResourceDescription(FTN.MarketDocument cimMarketDocument)
        {
            ResourceDescription rd = null;
            if (cimMarketDocument != null)
            {
                long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.MARKETDOCUMENT, importHelper.CheckOutIndexForDMSType(DMSType.MARKETDOCUMENT));
                rd = new ResourceDescription(gid);
                importHelper.DefineIDMapping(cimMarketDocument.ID, gid);

                ////populate ResourceDescription
                PowerTransformerConverter.PopulateMarketDocumentProperties(cimMarketDocument, rd, importHelper, report);
            }
            return rd;
        }
        #endregion Import
    }
}

