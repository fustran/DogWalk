﻿namespace API.Errores
{
    public class ApiErrorResponse
    {
        public ApiErrorResponse(int statusCode, string mensaje = null)
        {
            StatusCode = statusCode;
            Mensaje = mensaje ?? GetMensajeStatusCode(statusCode);
        }

        public int StatusCode { get; set; }
        public string Mensaje { get; set; }

        private string GetMensajeStatusCode(int statusCode)
        {
            return statusCode switch
            {
                400 => "Se ha realizado una solicitud no válida",
                401 => "No estás autorizado para este recurso",
                404 => "Recurso No encontrado",
                500 => "Error interno del Servidor",
                _ => null
            };
        }

    }
}
