namespace FTN.ESI.SIMES.CIM.CIMAdapter.Importer
{
    using FTN.Common;

    /// <summary>
    /// PowerTransformerConverter has methods for populating
    /// ResourceDescription objects using PowerTransformerCIMProfile_Labs objects.
    /// </summary>
    public static class PowerTransformerConverter
    {

        #region Populate ResourceDescription
        public static void PopulateIdentifiedObjectProperties(FTN.IdentifiedObject cimIdentifiedObject, ResourceDescription rd)
        {
            if ((cimIdentifiedObject != null) && (rd != null))
            {
                if (cimIdentifiedObject.MRIDHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.IDOBJ_MRID, cimIdentifiedObject.MRID));
                }
                if (cimIdentifiedObject.NameHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.IDOBJ_NAME, cimIdentifiedObject.Name));
                }
                if (cimIdentifiedObject.AliasNameHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.IDOBJ_ALIASNAME, cimIdentifiedObject.AliasName));
                }
            }
        }

        public static void PopulatePointResourceProperties(FTN.Point cimPoint, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if ((cimPoint != null) && (rd != null))
            {
                PowerTransformerConverter.PopulateIdentifiedObjectProperties(cimPoint, rd);

                if (cimPoint.PositionHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.POINT_POSITION, cimPoint.Position));
                }
                if (cimPoint.BidQuantityHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.POINT_BIDQUANT, cimPoint.BidQuantity));
                }
                if (cimPoint.QuantityHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.POINT_BIDQUANT, cimPoint.Quantity));
                }
                if (cimPoint.PeriodHasValue)
                {
                    long gid = importHelper.GetMappedGID(cimPoint.Period.ID);
                    if (gid < 0)
                    {
                        report.Report.Append("WARNING: Convert ").Append(cimPoint.GetType().ToString()).Append(" rdfID = \"").Append(cimPoint.ID);
                        report.Report.Append("\" - Failed to set reference to Period: rdfID \"").Append(cimPoint.Period.ID).AppendLine(" \" is not mapped to GID!");
                    }
                    rd.AddProperty(new Property(ModelCode.POINT_PERIOD, gid));
                }
            }
        }

        public static void PopulatePeriodResourceProperties(FTN.Period cimPeriod, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if ((cimPeriod != null) && (rd != null))
            {
                PowerTransformerConverter.PopulateIdentifiedObjectProperties(cimPeriod, rd);


                if (cimPeriod.ResolutionHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.PERIOD_RESOLUTION, cimPeriod.Resolution));
                }
                if (cimPeriod.MarketDocumentHasValue)
                {
                    long gid = importHelper.GetMappedGID(cimPeriod.MarketDocument.ID);
                    if (gid < 0)
                    {
                        report.Report.Append("WARNING: Convert ").Append(cimPeriod.GetType().ToString()).Append(" rdfID = \"").Append(cimPeriod.ID);
                        report.Report.Append("\" - Failed to set reference to MarketDocument: rdfID \"").Append(cimPeriod.MarketDocument.ID).AppendLine(" \" is not mapped to GID!");
                    }
                    rd.AddProperty(new Property(ModelCode.PERIOD_MARKETDOC, gid));
                }
            }
        }

        public static void PopulateDocumentProperties(FTN.Document cimDocument, ResourceDescription rd)
        {
            if ((cimDocument != null) && (rd != null))
            {
                PowerTransformerConverter.PopulateIdentifiedObjectProperties(cimDocument, rd);

                if (cimDocument.CreatedDateTimeHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.DOCUMENT_CREATEDDT, cimDocument.CreatedDateTime));
                }
                if (cimDocument.LastModifiedDateTimeHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.DOCUMENT_LASTMODDT, cimDocument.LastModifiedDateTime));
                }
                if (cimDocument.RevisionNumberHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.DOCUMENT_REVISIONNUM, cimDocument.RevisionNumber));
                }
                if (cimDocument.SubjectHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.DOCUMENT_SUBJECT, cimDocument.Subject));
                }
                if (cimDocument.TitleHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.DOCUMENT_TITLE, cimDocument.Title));
                }
                if (cimDocument.TypeHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.DOCUMENT_TYPE, cimDocument.Type));
                }
            }
        }

        public static void PopulateProcessProperties(FTN.Process cimProcess, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if ((cimProcess != null) && (rd != null))
            {
                PowerTransformerConverter.PopulateIdentifiedObjectProperties(cimProcess, rd);

                if (cimProcess.ClassificationTypeHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.PROCESS_CLASSTYPE, cimProcess.ClassificationType));
                }
                if (cimProcess.ProcessTypeHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.PROCESS_PROCTYPE, cimProcess.ProcessType));
                }
            }
        }

        public static void PopulateMarketDocumentProperties(FTN.MarketDocument cimMarketDocument, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if ((cimMarketDocument != null) && (rd != null))
            {
                PowerTransformerConverter.PopulateDocumentProperties(cimMarketDocument, rd);

                if (cimMarketDocument.ProcessHasValue)
                {
                    long gid = importHelper.GetMappedGID(cimMarketDocument.Process.ID);
                    if (gid < 0)
                    {
                        report.Report.Append("WARNING: Convert ").Append(cimMarketDocument.GetType().ToString()).Append(" rdfID = \"").Append(cimMarketDocument.ID);
                        report.Report.Append("\" - Failed to set reference to Process: rdfID \"").Append(cimMarketDocument.Process.ID).AppendLine(" \" is not mapped to GID!");
                    }
                    rd.AddProperty(new Property(ModelCode.MARKETDOCUMENT_PROCESS, gid));
                }
            }
        }

        public static void PopulateTimeSeriesProperties(FTN.TimeSeries cimTimeSeries, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if ((cimTimeSeries != null) && (rd != null))
            {

                if (cimTimeSeries.ObjectAggregationHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.TIMESERIES_OBJAGGREG, cimTimeSeries.ObjectAggregation));
                }
                if (cimTimeSeries.ProductHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.TIMESERIES_PRODUCT, cimTimeSeries.Product));
                }
                if (cimTimeSeries.VersionHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.TIMESERIES_VERSION, cimTimeSeries.Version));
                }
                if (cimTimeSeries.MarketDocumentHasValue)
                {
                    long gid = importHelper.GetMappedGID(cimTimeSeries.MarketDocument.ID);
                    if (gid < 0)
                    {
                        report.Report.Append("WARNING: Convert ").Append(cimTimeSeries.GetType().ToString()).Append(" rdfID = \"").Append(cimTimeSeries.ID);
                        report.Report.Append("\" - Failed to set reference to MarketDocument: rdfID \"").Append(cimTimeSeries.MarketDocument.ID).AppendLine(" \" is not mapped to GID!");
                    }
                    rd.AddProperty(new Property(ModelCode.TIMESERIES_MARKETDOC, gid));
                }
            }
        }


        public static void PopulateMeasurementPointProperties(FTN.MeasurementPoint cimMeasurementPoint, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if ((cimMeasurementPoint != null) && (rd != null))
            {
                PowerTransformerConverter.PopulateIdentifiedObjectProperties(cimMeasurementPoint, rd);


                if (cimMeasurementPoint.TimeSeriesHasValue)
                {
                    long gid = importHelper.GetMappedGID(cimMeasurementPoint.TimeSeries.ID);
                    if (gid < 0)
                    {
                        report.Report.Append("WARNING: Convert ").Append(cimMeasurementPoint.GetType().ToString()).Append(" rdfID = \"").Append(cimMeasurementPoint.ID);
                        report.Report.Append("\" - Failed to set reference to TimeSeries: rdfID \"").Append(cimMeasurementPoint.TimeSeries.ID).AppendLine(" \" is not mapped to GID!");
                    }
                    rd.AddProperty(new Property(ModelCode.MEASUREMENTPOINT_TIMESERIES, gid));
                }
            }
        }


    }
    #endregion Populate ResourceDescription

}

