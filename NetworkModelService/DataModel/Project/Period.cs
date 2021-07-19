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
    public class Period : IdentifiedObject
    {
        private string resolution = String.Empty;
        private List<long> points = new List<long>();
        private List<long> timeSeries = new List<long>();
        private long marketDocument = 0;

        public Period(long globalId) : base(globalId)
        {
        }

        public List<long> Points
        {
            get
            {
                return points;
            }

            set
            {
                points = value;
            }
        }

        public List<long> TimeSeries
        {
            get
            {
                return timeSeries;
            }

            set
            {
                timeSeries = value;
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

        public string Resolution
        {
            get { return resolution; }
            set { resolution = value; }
        }
        

        public override bool Equals(object obj)
        {
            if (base.Equals(obj))
            {
                Period x = (Period)obj;
                return (x.marketDocument == this.marketDocument && x.resolution == this.resolution && 
                    CompareHelper.CompareLists(x.timeSeries, this.timeSeries, true) &&
                    CompareHelper.CompareLists(x.points, this.points, true)
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
                case ModelCode.PERIOD_MARKETDOC:
                case ModelCode.PERIOD_POINTS:
                case ModelCode.PERIOD_RESOLUTION:
                case ModelCode.PERIOD_TIMESERS:
                    return true;

                default:
                    return base.HasProperty(t);
            }
        }

        public override void GetProperty(Property prop)
        {
            switch (prop.Id)
            {
                case ModelCode.PERIOD_MARKETDOC:
                    prop.SetValue(marketDocument);
                    break;

                case ModelCode.PERIOD_POINTS:
                    prop.SetValue(points);
                    break;

                case ModelCode.PERIOD_RESOLUTION:
                    prop.SetValue(resolution);
                    break;
                    

                case ModelCode.PERIOD_TIMESERS:
                    prop.SetValue(timeSeries);
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
                case ModelCode.PERIOD_MARKETDOC:
                    marketDocument = property.AsLong();
                    break;

                case ModelCode.PERIOD_RESOLUTION:
                    resolution = property.AsString();
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
                return points.Count > 0 || timeSeries.Count > 0 || base.IsReferenced;
            }
        }

        public override void GetReferences(Dictionary<ModelCode, List<long>> references, TypeOfReference refType)
        {
            if (points != null && points.Count > 0 && (refType == TypeOfReference.Target || refType == TypeOfReference.Both))
            {
                references[ModelCode.PERIOD_POINTS] = points.GetRange(0, points.Count);
            }
            if (timeSeries != null && timeSeries.Count > 0 && (refType == TypeOfReference.Target || refType == TypeOfReference.Both))
            {
                references[ModelCode.PERIOD_TIMESERS] = timeSeries.GetRange(0, timeSeries.Count);
            }
            if (marketDocument != 0 && (refType == TypeOfReference.Reference || refType == TypeOfReference.Both))
            {
                references[ModelCode.PERIOD_MARKETDOC] = new List<long>();
                references[ModelCode.PERIOD_MARKETDOC].Add(marketDocument);
            }

            base.GetReferences(references, refType);
        }

        public override void AddReference(ModelCode referenceId, long globalId)
        {
            switch (referenceId)
            {
                case ModelCode.POINT_PERIOD:
                    points.Add(globalId);
                    break;

                case ModelCode.TIMESERIES_PERIOD:
                    timeSeries.Add(globalId);
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
                case ModelCode.POINT_PERIOD:

                    if (points.Contains(globalId))
                    {
                        points.Remove(globalId);
                    }
                    else
                    {
                        CommonTrace.WriteTrace(CommonTrace.TraceWarning, "Entity (GID = 0x{0:x16}) doesn't contain reference 0x{1:x16}.", this.GlobalId, globalId);
                    }

                    break;

                case ModelCode.TIMESERIES_PERIOD:

                    if (timeSeries.Contains(globalId))
                    {
                        timeSeries.Remove(globalId);
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

