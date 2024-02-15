using Application.Interfaces;
using Domain.Entities;
using Microsoft.Extensions.Options;

namespace WorkerProcesamientoDePagos
{
    public class Worker : BackgroundService
    {
    
        private readonly ILogger<Worker> _logger;
        private readonly IMessageService _messageService;
        private readonly IContextMgmt _contextMgmt;

        public Worker(ILogger<Worker> logger, IMessageService messageSenderService, IContextMgmt contextMgmt)
        {
            _messageService = messageSenderService;
            _logger = logger;
            _contextMgmt = contextMgmt;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    //Leer el msj
                    var solicitud = await _messageService.ConsumeAsync();
                    _logger.LogInformation("Solicitud Recibida: {IdSolicitud}",solicitud.IdSolicitud);

                    //Verifica Aprobacion por decimal
                    if(solicitud.Monto % 1 == 0)
                    {
                        //Publica en cola de gestion de aprobacion
                        _logger.LogInformation("Inicio publicar solicitud: {IdSolicitud}", solicitud.IdSolicitud);
                        _messageService.SendMessage(solicitud);
                        _logger.LogInformation("Fin publicar solicitud: {IdSolicitud}", solicitud.IdSolicitud);
                    }
                    else
                    {
                        //inserta solicitud rechazado
                        _logger.LogInformation("Inicio insertar solicitud: {IdSolicitud}", solicitud.IdSolicitud);
                        solicitud.Estado = "Rechazada";
                        solicitud.Fecha = DateTime.Now;
                        solicitud.Observacion = "El valor del monto es decimal";
                        _contextMgmt.InsertSolicitud(solicitud);
                        _logger.LogInformation("Fin insertar solicitud: {IdSolicitud}", solicitud.IdSolicitud);
                    }
                    
                }
                catch (Exception ex)
                {
                    _logger.LogError("Error: {message}", ex.Message);
                }
            }
        }
    }
}