using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using FTN.Common;



namespace FTN.Services.NetworkModelService.DataModel.Core
{
    public class Point : IdentifiedObject
    {
        private long period = 0;
        private float bidQuantity = 0;
        private int position = 0;
        private float quantity = 0;

        public Point(long globalId)
            : base(globalId)
        {
        }

        public float BidQuantity
        {
            get
            {
                return bidQuantity;
            }

            set
            {
                bidQuantity = value;
            }
        }

        public float Quantity
        {
            get
            {
                return quantity;
            }

            set
            {
                quantity = value;
            }
        }

        public int Position
        {
            get
            {
                return position;
            }

            set
            {
                position = value;
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

        public override bool Equals(object obj)
        {
            if (base.Equals(obj))
            {
                Point x = (Point)obj;
                return (x.bidQuantity == this.bidQuantity && x.period == this.period && x.position == this.position && x.quantity == this.quantity);
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

        public override bool HasProperty(ModelCode property)
        {
            switch (property)
            {
                case ModelCode.POINT_BIDQUANT:
                case ModelCode.POINT_PERIOD:
                case ModelCode.POINT_QUANTITY:
                case ModelCode.POINT_POSITION:
                    return true;

                default:
                    return base.HasProperty(property);
            }
        }

        public override void GetProperty(Property property)
        {
            switch (property.Id)
            {
                case ModelCode.POINT_BIDQUANT:
                    property.SetValue(bidQuantity);
                    break;

                case ModelCode.POINT_PERIOD:
                    property.SetValue(period);
                    break;

                case ModelCode.POINT_QUANTITY:
                    property.SetValue(quantity);
                    break;

                case ModelCode.POINT_POSITION:
                    property.SetValue(position);
                    break;

                default:
                    base.GetProperty(property);
                    break;
            }
        }

        public override void SetProperty(Property property)
        {
            switch (property.Id)
            {
                case ModelCode.POINT_BIDQUANT:
                    bidQuantity = property.AsFloat();
                    break;

                case ModelCode.POINT_PERIOD:
                    period = property.AsReference();
                    break;

                case ModelCode.POINT_QUANTITY:
                    quantity = property.AsFloat();
                    break;

                case ModelCode.POINT_POSITION:
                    position = property.AsInt();
                    break;

                default:
                    base.SetProperty(property);
                    break;
            }
        }

        #endregion IAccess implementation

        #region IReference implementation

        public override void GetReferences(Dictionary<ModelCode, List<long>> references, TypeOfReference refType)
        {
            if (period != 0 && (refType == TypeOfReference.Reference || refType == TypeOfReference.Both))
            {
                references[ModelCode.POINT_PERIOD] = new List<long>();
                references[ModelCode.POINT_PERIOD].Add(period);
            }

            base.GetReferences(references, refType);
        }

        #endregion IReference implementation		
    }
}
