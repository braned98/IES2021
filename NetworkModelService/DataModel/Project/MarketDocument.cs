
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
    public class MarketDocument : Document
    {
        private List<long> periods = new List<long>();
        private List<long> timeSeries = new List<long>();
        private long process = 0;

        public MarketDocument(long globalId) : base(globalId)
        {
        }

        public List<long> Periods
        {
            get
            {
                return periods;
            }

            set
            {
                periods = value;
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

        public long Process
        {
            get
            {
                return process;
            }

            set
            {
                process = value;
            }
        }

        

        public override bool Equals(object obj)
        {
            if (base.Equals(obj))
            {
                MarketDocument x = (MarketDocument)obj;
                return (x.process == this.process &&
                    CompareHelper.CompareLists(x.timeSeries, this.timeSeries, true) &&
                    CompareHelper.CompareLists(x.periods, this.periods, true)
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
                case ModelCode.MARKETDOCUMENT_PERIODS:
                case ModelCode.MARKETDOCUMENT_PROCESS:
                case ModelCode.MARKETDOCUMENT_TIMESERIES:
                    return true;

                default:
                    return base.HasProperty(t);
            }
        }

        public override void GetProperty(Property prop)
        {
            switch (prop.Id)
            {
                case ModelCode.MARKETDOCUMENT_PERIODS:
                    prop.SetValue(periods);
                    break;

                case ModelCode.MARKETDOCUMENT_PROCESS:
                    prop.SetValue(process);
                    break;

                case ModelCode.MARKETDOCUMENT_TIMESERIES:
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
                case ModelCode.MARKETDOCUMENT_PROCESS:
                    process = property.AsLong();
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
                return timeSeries.Count > 0 || periods.Count > 0 || base.IsReferenced;
            }
        }

        public override void GetReferences(Dictionary<ModelCode, List<long>> references, TypeOfReference refType)
        {
            if (periods != null && periods.Count > 0 && (refType == TypeOfReference.Target || refType == TypeOfReference.Both))
            {
                references[ModelCode.PERIOD_POINTS] = periods.GetRange(0, periods.Count);
            }
            if (timeSeries != null && timeSeries.Count > 0 && (refType == TypeOfReference.Target || refType == TypeOfReference.Both))
            {
                references[ModelCode.PERIOD_TIMESERS] = timeSeries.GetRange(0, timeSeries.Count);
            }
            if (process != 0 && (refType == TypeOfReference.Reference || refType == TypeOfReference.Both))
            {
                references[ModelCode.MARKETDOCUMENT_PROCESS] = new List<long>();
                references[ModelCode.MARKETDOCUMENT_PROCESS].Add(process);
            }

            base.GetReferences(references, refType);
        }

        public override void AddReference(ModelCode referenceId, long globalId)
        {
            switch (referenceId)
            {
                case ModelCode.PERIOD_MARKETDOC:
                    periods.Add(globalId);
                    break;

                case ModelCode.TIMESERIES_MARKETDOC:
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
                case ModelCode.PERIOD_MARKETDOC:

                    if (periods.Contains(globalId))
                    {
                        periods.Remove(globalId);
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

