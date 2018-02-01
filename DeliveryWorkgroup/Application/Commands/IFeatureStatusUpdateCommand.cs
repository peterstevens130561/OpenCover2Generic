namespace DeliveryWorkgroup.Application.Commands
{
    internal interface IFeatureStatusUpdateCommand
    {
        double AvailabilityFraction { get; set; }
        int ResourceUniqueId { get; set; }
        int TaskUniqueId { get; set; }
        double WorkedFraction { get; set; }
    }
}