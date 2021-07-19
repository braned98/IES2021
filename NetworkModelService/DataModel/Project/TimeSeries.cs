using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using FTN.Common;

namespace FTN.Services.NetworkModelService.DataModel.Core
{
    public class TimeSeries : IdentifiedObject
    {
        private string objectAggregation = String.Empty;
        private string product = String.Empty;
        private string version = String.Empty;
        private List<long> measurementPoints = new List<long>();
        private long marketDocument = 0;
        private long period = 0;

        public TimeSeries(long globalId) : base(globalId)
        {
        }

        public List<long> MeasurementPoints
        {
            get
            {
                return measurementPoints;
            }

            set
            {
                measurementPoints = value;
            }
        }

        public long MarketDocument
        {
            get
            {
                return marketDocument;
            }

            set
            {
                marketDocument = value;
            }
        }

        public long Period
        {
            get
            {
                return period;
            }

            set
            {
                period = value;
            }
        }

        public string ObjectAggregation
        {
            get { return objectAggregation; }
            set { objectAggregation = value; }
        }

        public string Product
        {
            get { return product; }
            set { product = value; }
        }

        public string Version
        {
            get { return version; }
            set { version = value; }
        }

        public override bool Equals(object obj)
        {
            if (base.Equals(obj))
            {
                TimeSeries x = (TimeSeries)obj;
                return (x.marketDocument == this.marketDocument && x.period == this.period &&
                    x.objectAggregation == this.objectAggregation && x.product == this.product &&
                    x.version == this.version &&
                    CompareHelper.CompareLists(x.measurementPoints, this.measurementPoints, true)
                    );
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #region IAccess implementation

        public override bool HasProperty(ModelCode t)
        {
            switch (t)
            {
                case ModelCode.TIMESERIES_MARKETDOC:
                case ModelCode.TIMESERIES_MEASUREPOINTS:
                case ModelCode.TIMESERIES_OBJAGGREG:
                case ModelCode.TIMESERIES_PERIOD:
                case ModelCode.TIMESERIES_PRODUCT:
                case ModelCode.TIMESERIES_VERSION:
                    return true;

                default:
                    return base.HasProperty(t);
            }
        }

        public override void GetProperty(Property prop)
        {
            switch (prop.Id)
            {
                case ModelCode.TIMESERIES_MARKETDOC:
                    prop.SetValue(marketDocument);
                    break;

                case ModelCode.TIMESERIES_MEASUREPOINTS:
                    prop.SetValue(measurementPoints);
                    break;

                case ModelCode.TIMESERIES_OBJAGGREG:
                    prop.SetValue(objectAggregation);
                    break;

                case ModelCode.TIMESERIES_PERIOD:
                    prop.SetValue(period);
                    break;

                case ModelCode.TIMESERIES_PRODUCT:
                    prop.SetValue(product);
                    break;

                case ModelCode.TIMESERIES_VERSION:
                    prop.SetValue(version);
                    break;

                default:
                    base.GetProperty(prop);
                    break;
            }
        }

        public override void SetProperty(Property property)
        {
            switch (property.Id)
            {
                case ModelCode.TIMESERIES_MARKETDOC:
                    marketDocument = property.AsLong();
                    break;

                case ModelCode.TIMESERIES_OBJAGGREG:
                    objectAggregation = property.AsString();
                    break;

                case ModelCode.TIMESERIES_PRODUCT:
                    product = property.AsString();
                    break;

                case ModelCode.TIMESERIES_VERSION:
                    version = property.AsString();
                    break;

                case ModelCode.TIMESERIES_PERIOD:
                    period = property.AsLong();
                    break;

                default:
                    base.SetProperty(property);
                    break;
            }
        }

        #endregion IAccess implementation

        #region IReference implementation

        public override bool IsReferenced
        {
            get
            {
                return measurementPoints.Count > 0 || base.IsReferenced;
            }
        }

        public override void GetReferences(Dictionary<ModelCode, List<long>> references, TypeOfReference refType)
        {
            if (measurementPoints != null && measurementPoints.Count > 0 && (refType == TypeOfReference.Target || refType == TypeOfReference.Both))
            {
                references[ModelCode.TIMESERIES_MEASUREPOINTS] = measurementPoints.GetRange(0, measurementPoints.Count);
            }
            if (marketDocument != 0 && (refType == TypeOfReference.Reference || refType == TypeOfReference.Both))
            {
                references[ModelCode.TIMESERIES_MARKETDOC] = new List<long>();
                references[ModelCode.TIMESERIES_MARKETDOC].Add(marketDocument);
            }
            if (period != 0 && (refType == TypeOfReference.Reference || refType == TypeOfReference.Both))
            {
                references[ModelCode.TIMESERIES_PERIOD] = new List<long>();
                references[ModelCode.TIMESERIES_PERIOD].Add(period);
            }

            base.GetReferences(references, refType);
        }

        public override void AddReference(ModelCode referenceId, long globalId)
        {
            switch (referenceId)
            {
                case ModelCode.MEASUREMENTPOINT_TIMESERIES:
                    measurementPoints.Add(globalId);
                    break;

                default:
                    base.AddReference(referenceId, globalId);
                    break;
            }
        }

        public override void RemoveReference(ModelCode referenceId, long globalId)
        {
            switch (referenceId)
            {
                case ModelCode.MEASUREMENTPOINT_TIMESERIES:

                    if (measurementPoints.Contains(globalId))
                    {
                        measurementPoints.Remove(globalId);
                    }
                    else
                    {
                        CommonTrace.WriteTrace(CommonTrace.TraceWarning, "Entity (GID = 0x{0:x16}) doesn't contain reference 0x{1:x16}.", this.GlobalId, globalId);
                    }

                    break;
               

                default:
                    base.RemoveReference(referenceId, globalId);
                    break;
            }
        }

        #endregion IReference implementation	
    }
}

