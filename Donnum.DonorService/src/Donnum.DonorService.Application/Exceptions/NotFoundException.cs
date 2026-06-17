namespace Donnum.DonorService.Application.Exceptions;

public class NotFoundException(string resourceName, object key)
    : Exception($"El recurso '{resourceName}' con identificador '{key}' no fue encontrado.");
