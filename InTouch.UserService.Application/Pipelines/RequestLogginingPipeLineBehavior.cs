using Microsoft.Extensions.Logging;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using InTouch.UserService.Core;
using System.Diagnostics;
using Ardalis.Result;
using System;

namespace InTouch.UserService.Application;

internal sealed class RequestLogginingPipeLineBehavior<TRequest, TResponse>(
    ILogger<RequestLogginingPipeLineBehavior<TRequest, TResponse>> logger)
: IPipelineBehavior<TRequest, TResponse>
where TRequest : IRequest<TResponse>
{
    private readonly ILogger<RequestLogginingPipeLineBehavior<TRequest,TResponse>> _logger = logger;
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        string requestCommand = request.GetGenericTypeName();
        _logger.LogInformation
        ($"Выполение запроса {requestCommand}!", requestCommand, DateTime.UtcNow);
        
        var timer = new Stopwatch();
        timer.Start();

        var result  = await next();
        
        timer.Stop();
        
        var timeTaken = timer.Elapsed.TotalSeconds;
        _logger.LogInformation("----- Команда '{CommandName}' выполнена за ({TimeTaken} секунд)", requestCommand, timeTaken);
        
        return result;
    }
}