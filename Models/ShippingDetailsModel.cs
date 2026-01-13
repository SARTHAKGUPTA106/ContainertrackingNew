using System;

namespace ContainerTracking.Models
{
    public class ShippingDetailsModel
    {
        public string? Mode { get; set; }
        public string? ReferenceNo { get; set; }
        public string? TrackingNumber { get; set; }
        public string? Carrier { get; set; }
        public string? ContainerNumbers { get; set; }
        public DateTime? FactoryDispatchDate { get; set; }
        public string? OriginPOL { get; set; }
        public string? Destination { get; set; }
        public string? PortOfDeparture { get; set; }
        public string? CurrentPosition { get; set; }
        public string? PortOfDestination { get; set; }
        public string? Status { get; set; }
        public DateTime? ShippingTime { get; set; }
        public DateTime? ETA { get; set; }
        public DateTime? ActualTime { get; set; }
        public DateTime? ArrivalDate { get; set; }
        public DateTime? DispatchDate { get; set; }

        public int? ModeID { get; set; }
        public int? StatusID { get; set; }

    }
}
