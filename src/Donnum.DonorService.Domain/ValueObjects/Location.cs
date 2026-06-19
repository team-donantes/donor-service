using System;
using Donnum.DonorService.Domain.Exceptions;

namespace Donnum.DonorService.Domain.ValueObjects;

public class Location
{
    public decimal Latitude { get; private set; }
    public decimal Longitude { get; private set; }
    
    private Location() { } // EF Core constructor

    public Location(decimal latitude, decimal longitude)
    {
        if (latitude < -90m || latitude > 90m) 
            throw new DomainException("Latitude inválida");
        if (longitude < -180m || longitude > 180m) 
            throw new DomainException("Longitude inválida");

        Latitude = latitude;
        Longitude = longitude;
    }
}
